namespace Mde.CampusDetector.Core.AppPermissions
{
    public interface IHandlePermissions
    {
        Task<PermissionStatus> CheckAsync<T>() where T : Permissions.BasePermission, new();
        Task<PermissionStatus> RequestAsync<T>() where T : Permissions.BasePermission, new();
        Task<PermissionStatus> RequestIfNotGrantedAsync<T>() where T : Permissions.BasePermission, new();
    }
}
