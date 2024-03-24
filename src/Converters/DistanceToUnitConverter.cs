using Mde.CampusDetector.Core.Campuses;
using Mde.CampusDetector.Core.Campuses.Services;
using System.Globalization;

namespace Mde.CampusDetector.Converters
{
    public class DistanceToUnitConverter : IValueConverter
    {
        public DistanceToUnitConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double distanceKilometer = (double)value;
                if (distanceKilometer < AppConstants.Ranges.CloseRange)
                {
                    return "m";
                }
                else
                {
                    return "km";
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
