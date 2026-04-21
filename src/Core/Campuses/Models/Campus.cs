namespace Mde.CampusDetector.Core.Campuses.Models
{
    public class Campus
    {
        public double Latitude { get; set; } = 0;

        public double Longitude { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public string PhotoUrl { get; set; } = string.Empty;

        public bool IsInRange(Location center, double range)
        {
            return center.CalculateDistance(Latitude, Longitude, DistanceUnits.Kilometers) <= range;
        }
    }
}
