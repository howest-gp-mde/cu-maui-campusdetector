namespace Mde.CampusDetector.Core.Campuses.Models
{
    public class Campus
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public bool IsInRange(Location center, double range)
        {
            return center.CalculateDistance(Latitude, Longitude, DistanceUnits.Kilometers) <= range;
        }
    }
}
