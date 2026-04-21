namespace Mde.CampusDetector.Core.Alerts
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string title, string message, string cancel);
        Task ShowToastAsync(string message);
    }
}
