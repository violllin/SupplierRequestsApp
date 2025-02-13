using System.Globalization;
using SupplierRequestsApp.Domain.Models;

namespace SupplierRequestsApp.Util.Converters
{
    public class ShelfDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Shelf shelf) return value;
            var idStr = shelf.Id.ToString();
            var shortId = idStr.Length >= 8 ? idStr.Substring(0, 8) : idStr;
            return $"ID: {shortId}..., Свободно ячеек: {shelf.FreeSlots}";
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