using Mde.CampusDetector.Core.Alerts;
using Mde.CampusDetector.Core.AppPermissions;
using Mde.CampusDetector.Core.Campuses;
using Mde.CampusDetector.Core.Campuses.Models;
using Mde.CampusDetector.Core.Campuses.Services;
using Mde.CampusDetector.ViewModels;
using Moq;

namespace Mde.CampusDetector.UnitTests
{
    public class MainViewModelTests
    {
        private Mock<ICampusService> _mockCampusService;
        private Mock<IGeolocation> _mockGeoLocation;
        private Mock<IDialogService> _mockDialogService;
        private Mock<IHandlePermissions> _mockPermissionsHandler;

        public MainViewModelTests()
        {
            _mockCampusService = new Mock<ICampusService>(MockBehavior.Strict);
            _mockGeoLocation = new Mock<IGeolocation>(MockBehavior.Strict);
            _mockDialogService = new Mock<IDialogService>(MockBehavior.Strict);
            _mockPermissionsHandler = new Mock<IHandlePermissions>(MockBehavior.Strict);

            //default setups for Strict mockbehaviour:

            //always return empty collection (unless unit test overrides)
            _mockCampusService.Setup(mock => mock.GetAllCampuses())
                .Returns(() => Task.FromResult((IEnumerable<Campus>)[]));

            //always start listening (unless unit test overrides)
            _mockGeoLocation.Setup(mock => mock
                    .StartListeningForegroundAsync(It.IsAny<GeolocationListeningRequest>()))
                    .Returns(() => Task.FromResult(true));

            //always grant location permission (unless unit test overrides)
            _mockPermissionsHandler.Setup(mock => mock
                    .RequestIfNotGrantedAsync<Permissions.LocationWhenInUse>())
                    .Returns(() => Task.FromResult(PermissionStatus.Granted));

            //default setup of ShowAlert (unless unit test overrides)
            _mockDialogService.Setup(mock => mock
                    .ShowToast(It.IsAny<string>()))
                    .Returns(() => Task.CompletedTask);

            //default setup of ShowAlert (unless unit test overrides)
            _mockDialogService.Setup(mock => mock
                    .ShowAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(() => Task.CompletedTask);
        }

        [Fact]
        public void AppearingCommand_WhenDone_IsLoadingFalse()
        {
            // Arrange
            var viewModel = new MainViewModel(_mockCampusService.Object,
                                              _mockDialogService.Object,
                                              _mockGeoLocation.Object,
                                              _mockPermissionsHandler.Object);

            //act   
            viewModel.AppearingCommand.Execute(null);

            //assert
            Assert.False(viewModel.IsLoading);
        }


        [Fact]
        public void AppearingCommand_DuringLoadingCampuses_IsLoadingTrue()
        {
            // Arrange
            var viewModel = new MainViewModel(_mockCampusService.Object,
                                              _mockDialogService.Object,
                                              _mockGeoLocation.Object,
                                              _mockPermissionsHandler.Object);
            bool isLoading = false;

            _mockCampusService.Setup(mock => mock.GetAllCampuses())
                .Callback(() => isLoading = viewModel.IsLoading)
                .Returns(() => Task.FromResult((IEnumerable<Campus>)[]));

            //act   
            viewModel.AppearingCommand.Execute(null);

            //assert
            Assert.True(isLoading);
        }


        [Theory]
        [InlineData(PermissionStatus.Limited)]
        [InlineData(PermissionStatus.Denied)]
        [InlineData(PermissionStatus.Disabled)]
        [InlineData(PermissionStatus.Restricted)]
        public void AppearingCommand_NoLocationPermission_DisplayAlert(PermissionStatus notAllowedStatus)
        {
            // Arrange
            var viewModel = new MainViewModel(_mockCampusService.Object,
                                              _mockDialogService.Object,
                                              _mockGeoLocation.Object,
                                              _mockPermissionsHandler.Object);

            _mockPermissionsHandler.Setup(mock => mock
                    .CheckAsync<Permissions.LocationWhenInUse>())
                    .ReturnsAsync(notAllowedStatus);

            _mockPermissionsHandler.Setup(mock => mock
                    .RequestAsync<Permissions.LocationWhenInUse>())
                    .ReturnsAsync(notAllowedStatus);

            _mockPermissionsHandler.Setup(mock => mock
                    .RequestIfNotGrantedAsync<Permissions.LocationWhenInUse>())
                    .ReturnsAsync(notAllowedStatus);

            //act   
            viewModel.AppearingCommand.Execute(null);

            // assert
            _mockDialogService.Verify(mock => mock.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                                      Times.Once);
        }

        [Fact]
        public void HandleLocation_ComingInRange_DisplaysToast()
        {
            // Arrange
            var lastLocationMock = new Mock<Location>();
            double currentDistanceFromCampus = AppConstants.Ranges.CloseRange + 1;

            var campus = new Campus
            {
                Name = "Test Campus",
                Latitude = 0,
                Longitude = 0,
                PhotoUrl = string.Empty
            };

            var viewModel = new MainViewModel(_mockCampusService.Object,
                                              _mockDialogService.Object,
                                              _mockGeoLocation.Object,
                                              _mockPermissionsHandler.Object);

            viewModel.LastLocation = lastLocationMock.Object;
            viewModel.SelectedCampus = campus;
            viewModel.SelectedCampusDistance = currentDistanceFromCampus;

            var expectedMessage = string.Format(MainViewModel.YouAreCloseMessage, campus.Name);

            // act
            viewModel.HandleLocation();

            // assert
            _mockDialogService.Verify(mock => mock.ShowToast(expectedMessage), Times.Once);
        }


        [Fact]
        public void SelectedCampus_Changed_HandleLocation()
        {
            // Arrange
            var newCampus = new Campus
            {
                Name = "Test Campus",
                Latitude = 0,
                Longitude = 0,
                PhotoUrl = string.Empty
            };

            // wrap a REAL ViewModel in a mock, so we can verify the HandleLocation method call
            var mockViewModel = new Mock<MainViewModel>(() =>
                    new MainViewModel(_mockCampusService.Object,
                                      _mockDialogService.Object,
                                      _mockGeoLocation.Object,
                                      _mockPermissionsHandler.Object));

            var viewModel = mockViewModel.Object;

            // Act   
            viewModel.SelectedCampus = newCampus;

            // assert
            mockViewModel.Verify(mock => mock.HandleLocation(), Times.Once);
        }


    }
}