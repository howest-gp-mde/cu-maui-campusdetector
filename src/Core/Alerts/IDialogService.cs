namespace Mde.CampusDetector.Core.Alerts
{
    public interface IDialogService
    {
        Task ShowAlert(string title, string message, string cancel);
        Task ShowToast(string message);
    }
}
