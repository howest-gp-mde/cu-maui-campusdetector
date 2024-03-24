using Mde.CampusDetector.Core.Alerts;
using Mde.CampusDetector.Core.AppPermissions;
using Mde.CampusDetector.Core.Campuses.Models;
using Mde.CampusDetector.Core.Campuses.Services;
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

    }
}