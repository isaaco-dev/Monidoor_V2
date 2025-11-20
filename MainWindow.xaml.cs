using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System;
using System.Globalization;

namespace MoniraceWPF
{
    // Converter migliorato: non dipende più dall'istanza della Window
    public class StatusPenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value as string;
            if (string.IsNullOrEmpty(status)) return Brushes.White;

            // Recupera le risorse in modo sicuro
            var blueBrush = Application.Current.Resources["MoniraceBlue"] as SolidColorBrush ?? Brushes.Cyan;
            var redBrush = Application.Current.Resources["MoniraceRed"] as SolidColorBrush ?? Brushes.Red;

            if (status.Contains("GO!")) return blueBrush;
            if (status.Contains("BOTTOM:")) return redBrush;

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Il DataContext è ora impostato qui, ma le risorse globali (come il converter) 
            // dovrebbero idealmente stare in App.xaml per pulizia.
            DataContext = new MainViewModel();
        }
    }
}