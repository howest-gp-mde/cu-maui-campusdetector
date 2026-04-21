using CommunityToolkit.Maui.Alerts;

namespace Mde.CampusDetector.Core.Alerts
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string title, string message, string cancel)
        { 
            return Application.Current?.Windows[0].Page?.DisplayAlertAsync(title, message, cancel) ?? Task.CompletedTask;
        }

        public Task ShowToastAsync(string message)
        {
            return Toast.Make(message).Show();
        }
    }
}
