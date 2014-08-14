using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TaiwanPP.Tile.Helpers
{
    public class brandvisconvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (System.Convert.ToInt32(parameter))
            {
                case 0:
                    return System.Convert.ToInt32(value) == 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                case 1:
                    return System.Convert.ToInt32(value) == 1 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            return System.Windows.Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class changevisconvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)    //參數一下二上零不動
        {
            if (double.IsNaN((double)value)) return System.Windows.Visibility.Collapsed;
            double v = System.Convert.ToDouble(value);
            switch ((string)parameter)
            {
                case "0":
                    if (v == 0) return System.Windows.Visibility.Visible;
                    break;
                case "1":
                    if (v < 0) return System.Windows.Visibility.Visible;
                    break;
                case "2":
                    if (v > 0) return System.Windows.Visibility.Visible;
                    break;
            }
            return System.Windows.Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class colorconvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (double.IsNaN((double)value)) return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            if ((double)value > 0)
            {
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            }
            else if ((double)value < 0)
            {
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 70, 222, 70));    //light green
            }
            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class priceconvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (double.IsNaN((double)value)) return "--.-";
            return Math.Abs(Math.Round(System.Convert.ToDouble(value), 2));
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class dateconvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (DateTime)value == DateTime.MinValue ? "----/--/--" : value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class kindvisconvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToInt32(value) == System.Convert.ToInt32(parameter) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
