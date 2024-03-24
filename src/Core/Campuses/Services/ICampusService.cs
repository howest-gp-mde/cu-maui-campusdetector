using Mde.CampusDetector.Core.Campuses.Models;

namespace Mde.CampusDetector.Core.Campuses.Services
{
    public interface ICampusService
    {
        Task<IEnumerable<Campus>> GetAllCampuses();

    }
}
