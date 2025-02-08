using System.Globalization;
using SupplierRequestsApp.Domain.Models;

namespace SupplierRequestsApp.Util.Converters
{
    public class ShelfDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Shelf shelf)
            {
                string idStr = shelf.Id.ToString();
                string shortId = idStr.Length >= 8 ? idStr.Substring(0, 8) : idStr;
                return $"ID: {shortId}..., Свободно ячеек: {shelf.FreeSlots}";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Shelf shelf)
            {
                return shelf;
            }

            return value;
        }
    }
}