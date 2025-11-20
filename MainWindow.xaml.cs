using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace MoniraceWPF
{
    // Aggiungere un converter helper per la TextDecoration (opzionale ma consigliato per la fedeltà grafica)
    public class StatusPenConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var vm = ((Window)Application.Current.MainWindow).DataContext as MainViewModel;
            if (vm == null) return Brushes.White;

            if (value.ToString().Contains("GO!")) return vm.AppList.Select(a => (SolidColorBrush)Application.Current.Resources["MoniraceBlue"]).FirstOrDefault();
            if (value.ToString().Contains("BOTTOM:")) return vm.AppList.Select(a => (SolidColorBrush)Application.Current.Resources["MoniraceRed"]).FirstOrDefault();

            return Brushes.White;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Aggiungere StatusPenConverter alle risorse se non fatto in XAML
            if (!Application.Current.Resources.Contains("StatusPenConverter"))
            {
                 Application.Current.Resources.Add("StatusPenConverter", new StatusPenConverter());
            }

            InitializeComponent();
            
            // Imposta il DataContext al ViewModel
            DataContext = new MainViewModel();

            // L'inizializzazione di WebView2 (EnsureCoreWebView2Async) non è strettamente necessaria qui 
            // se si usa solo l'attributo Source.
        }
    }
}