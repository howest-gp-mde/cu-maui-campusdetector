namespace Mde.CampusDetector.Core.Campuses.Services
{
    public class CampusDto
    {
        public int Id { get; set; }

        public double[] Coordinates { get; set; } = new double[2];

        public string Description { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;
    }
}