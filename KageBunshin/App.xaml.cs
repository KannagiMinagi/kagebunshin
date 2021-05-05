using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;
using System.Globalization;

namespace KageBunshin
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    /// 
    public partial class App : Application
    {
    }

    public class CBoolNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is bool && (bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is bool && (bool)value);
        }

    }

    public class PitchModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isMode1 = false;
            if(value is string && value.ToString() == "Mode1") isMode1 = true;
            return isMode1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isMode1 = false;
            if (value is string && value.ToString() == "Mode1") isMode1 = true;
            return isMode1;
        }

    }



}
