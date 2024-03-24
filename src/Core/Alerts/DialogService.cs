using CommunityToolkit.Maui.Alerts;

namespace Mde.CampusDetector.Core.Alerts
{
    public class DialogService : IDialogService
    {
        public Task ShowAlert(string title, string message, string cancel)
        { 
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task ShowToast(string message)
        {
            return Toast.Make(message).Show();
        }
    }
}
