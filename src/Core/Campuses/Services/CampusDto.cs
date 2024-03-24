namespace Mde.CampusDetector.Core.Campuses.Services
{
    public class CampusDto
    {
        public int Id { get; set; }

        public double[] Coordinates { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
    }
}