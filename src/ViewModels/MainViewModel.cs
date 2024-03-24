﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.CampusDetector.Core.Campuses;
using Mde.CampusDetector.Core.Campuses.Models;
using Mde.CampusDetector.Core.Campuses.Services;
using System.Collections.ObjectModel;

namespace Mde.CampusDetector.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ICampusService campusService;

        public const string NoPermissionTitle = "Location unavailable";
        public const string NoPermissionMessage = "To allow distance measurement, please allow the app to request your locations information";
        public const string YouAreCloseMessage = "You are close to {0}";
        public const string ErrorTitle = "Error";

        public MainViewModel(ICampusService campusService)
        {
            this.campusService = campusService;

            AppearingCommand = new Command(OnAppearing);
            DisappearingCommand = new Command(OnDisappearing);
        }

        public Command AppearingCommand { get; }
        public Command DisappearingCommand { get; }

        public Location LastLocation { get; set; }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set {
                SetProperty(ref isLoading, value);
            }
        }

        public bool IsCampusSelected => SelectedCampus != null;

        private Campus selectedCampus;
        public Campus SelectedCampus
        {
            get { return selectedCampus; }
            set
            {
                SetProperty(ref selectedCampus, value);
                OnPropertyChanged(nameof(IsCampusSelected));

                HandleLocation();
            }
        }

        private double selectedCampusDistance;
        public double SelectedCampusDistance
        {
            get { return selectedCampusDistance; }
            set
            {
                SetProperty(ref selectedCampusDistance, value);
            }
        }

        private ObservableCollection<Campus> campuses;
        public ObservableCollection<Campus> Campuses
        {
            get { return campuses; }
            set {
                SetProperty(ref campuses, value);
            }
        }

        public virtual void HandleLocation()
        {
            if (selectedCampus != null && LastLocation != null)
            {
                double lastDistance = SelectedCampusDistance;

                SelectedCampusDistance = LastLocation.CalculateDistance(
                        selectedCampus.Latitude, selectedCampus.Longitude, DistanceUnits.Kilometers);

                if(selectedCampusDistance <= AppConstants.Ranges.CloseRange && lastDistance > AppConstants.Ranges.CloseRange)
                {
                    Toast.Make(string.Format(YouAreCloseMessage, selectedCampus.Name)).Show();
                }
            }
        }

        private void OnLocationChanged(object sender, GeolocationLocationChangedEventArgs e)
        {
            LastLocation = e.Location;
            HandleLocation();
        }

        private async void OnAppearing()
        {
            try
            {
                IsLoading = true;

                //load campuses
                var campuses = await campusService.GetAllCampuses();
                Campuses = new ObservableCollection<Campus>(campuses);

                //check if location permission is granted
                bool hasPermission = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted;
                if (!hasPermission)
                {
                    hasPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted;
                }

                if (hasPermission)
                {
                    try
                    {
                        //start listening to location changes
                        Geolocation.LocationChanged += OnLocationChanged;
                        var request = new GeolocationListeningRequest(GeolocationAccuracy.Medium);
                        await Geolocation.StartListeningForegroundAsync(request);
                    }
                    catch (Exception)
                    {
                        hasPermission = false;
                    }
                }
                if(!hasPermission)
                {
                    await Application.Current.MainPage.DisplayAlert(NoPermissionTitle, NoPermissionMessage, "I understand");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(ErrorTitle, ex.Message, "Ok");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnDisappearing()
        {
            Geolocation.StopListeningForeground();
            Geolocation.LocationChanged -= OnLocationChanged;
        }

    }
}
