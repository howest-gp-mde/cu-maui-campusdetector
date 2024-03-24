using Mde.CampusDetector.Core.Campuses;
using System.Globalization;

namespace Mde.CampusDetector.Converters
{
    public class DistanceToStringConverter : IValueConverter
    {
        
        public DistanceToStringConverter()
        {
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double)
            {
                double distanceKilometer = (double) value;
                if (distanceKilometer < AppConstants.Ranges.CloseRange)
                {
                    return $"{(distanceKilometer * 1000):N0}";
                }
                else if (distanceKilometer < AppConstants.Ranges.MediumRange)
                {
                    return $"{distanceKilometer:N1}";
                }
                else
                {
                    return $"{distanceKilometer:N0}";
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
