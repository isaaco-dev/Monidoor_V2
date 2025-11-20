using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MoniraceWPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isSelectionViewVisible = true;
        private string _topAppUrl = "about:blank";
        private string _bottomAppUrl = "about:blank";
        private string _selectionStatusText;

        public ObservableCollection<AppModel> AppList { get; set; }
        public AppModel SelectedAppTop => AppList.FirstOrDefault(a => a.IsSelectedTop);
        public AppModel SelectedAppBottom => AppList.FirstOrDefault(a => a.IsSelectedBottom);
        public bool CanLaunch => SelectedAppTop != null && SelectedAppBottom != null;

        public ICommand SelectAppTopCommand { get; }
        public ICommand SelectAppBottomCommand { get; }
        public ICommand LaunchSplitViewCommand { get; }
        public ICommand ShowSelectionCommand { get; }

        public bool IsSelectionViewVisible
        {
            get => _isSelectionViewVisible;
            set 
            { 
                _isSelectionViewVisible = value; 
                OnPropertyChanged(nameof(IsSelectionViewVisible)); 
                OnPropertyChanged(nameof(IsSplitViewVisible)); 
            }
        }
        public bool IsSplitViewVisible => !_isSelectionViewVisible;

        public string TopAppUrl
        {
            get => _topAppUrl;
            set { _topAppUrl = value; OnPropertyChanged(nameof(TopAppUrl)); }
        }

        public string BottomAppUrl
        {
            get => _bottomAppUrl;
            set { _bottomAppUrl = value; OnPropertyChanged(nameof(BottomAppUrl)); }
        }

        public string SelectionStatusText
        {
            get => _selectionStatusText;
            set { _selectionStatusText = value; OnPropertyChanged(nameof(SelectionStatusText)); }
        }

        public MainViewModel()
        {
            LoadAppList();
            UpdateSelectionStatus();
            
            SelectAppTopCommand = new RelayCommand(SelectAppTop);
            SelectAppBottomCommand = new RelayCommand(SelectAppBottom);
            LaunchSplitViewCommand = new RelayCommand(_ => LaunchSplitView(), () => CanLaunch); // <--- Correzione qui
            ShowSelectionCommand = new RelayCommand(_ => ShowSelection()); // <--- Correzione qui
        }

        private void LoadAppList()
        {
            AppList = new ObservableCollection<AppModel>
            {
                // Replica esatta del tuo array JS
                new AppModel { Name = "MYCRON", Url = "https://www.youtube.com/embed/BOH0ezDm3r0?autoplay=0" }, 
                new AppModel { Name = "ALFANO", Url = "https://www.youtube.com/embed/BOH0ezDm3r0?autoplay=0" },
                new AppModel { Name = "UNIPRO", Url = "https://www.youtube.com/embed/BOH0ezDm3r0?autoplay=0" },
                new AppModel { Name = "STARLANE", Url = "https://www.youtube.com/embed/BOH0ezDm3r0?autoplay=0" },
                new AppModel { Name = "CHROME", Url = "data:text/html" }, // URL fittizio gestito da GetAppUrl
                new AppModel { Name = "YOUTUBE", Url = "https://www.youtube.com/embed/BOH0ezDm3r0?autoplay=0" } 
            };
        }

        private void SelectAppTop(object parameter)
        {
            var app = parameter as AppModel;
            if (app == null || app.IsDisabledTop) return;

            // Logica: deseleziona tutti gli altri, poi togli/metti la selezione sull'app corrente
            foreach (var a in AppList.Where(a => a.Name != app.Name))
            {
                a.IsSelectedTop = false;
            }
            app.IsSelectedTop = !app.IsSelectedTop;
            
            UpdateAppExclusivity();
            UpdateSelectionStatus();
            OnPropertyChanged(nameof(CanLaunch));
            (LaunchSplitViewCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void SelectAppBottom(object parameter)
        {
            var app = parameter as AppModel;
            if (app == null || app.IsDisabledBottom) return;

            // Logica: deseleziona tutti gli altri, poi togli/metti la selezione sull'app corrente
            foreach (var a in AppList.Where(a => a.Name != app.Name))
            {
                a.IsSelectedBottom = false;
            }
            app.IsSelectedBottom = !app.IsSelectedBottom;
            
            UpdateAppExclusivity();
            UpdateSelectionStatus();
            OnPropertyChanged(nameof(CanLaunch));
            (LaunchSplitViewCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void UpdateAppExclusivity()
        {
            // Forziamo il refresh delle proprietà IsDisabledTop/Bottom su tutti gli elementi
            // usando il nuovo metodo pubblico sicuro.
            foreach (var app in AppList)
            {
                app.RefreshExclusivity();
            }
        }
        
        private void UpdateSelectionStatus()
        {
            if (SelectedAppTop != null && SelectedAppBottom != null)
            {
                SelectionStatusText = "GO! La tua COMBINAZIONE è pronta 🚥";
            }
            else if (SelectedAppTop != null)
            {
                SelectionStatusText = "Slot BOTTOM: Scegli i tuoi DATI DI ANALISI 📈";
            }
            else
            {
                SelectionStatusText = "Slot TOP: Scegli il tuo SCHERMO PRINCIPALE 🥇";
            }
        }

        private string GetAppUrl(AppModel app)
        {
            if (app.Name == "CHROME")
            {
                // Replica l'HTML interno per la simulazione "LIVE RESULTS"
                return "data:text/html,<html><body style='background-color: #fff; display:flex; justify-content:center; align-items:center; flex-direction: column; font-family: sans-serif; font-size: 20px; margin: 0; color: #4285F4; padding: 20px;'><h1>LIVE RESULTS</h1><div style='font-size: 14px; color: #555; margin-top: 10px;'>Simulazione Risultati Live / Google Search</div></body></html>";
            }
            
            string finalUrl = app.Url;
            // CORREZIONE AUTOPLAY: Sostituisce autoplay=0 con autoplay=1
            if (finalUrl.Contains("youtube.com/embed"))
            {
                finalUrl = finalUrl.Replace("autoplay=0", "autoplay=1");
            }
            return finalUrl;
        }

        private void LaunchSplitView()
        {
            if (!CanLaunch) return;

            TopAppUrl = GetAppUrl(SelectedAppTop);
            BottomAppUrl = GetAppUrl(SelectedAppBottom);

            IsSelectionViewVisible = false;
        }

        private void ShowSelection()
        {
            // Resetta lo stato di selezione
            foreach (var app in AppList)
            {
                app.IsSelectedTop = false;
                app.IsSelectedBottom = false;
            }

            // Resetta gli URL per stoppare la riproduzione
            TopAppUrl = "about:blank";
            BottomAppUrl = "about:blank";
            
            UpdateAppExclusivity();
            UpdateSelectionStatus();
            OnPropertyChanged(nameof(CanLaunch));

            IsSelectionViewVisible = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}