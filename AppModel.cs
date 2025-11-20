using System.ComponentModel;

namespace MoniraceWPF
{
    public class AppModel : INotifyPropertyChanged
    {
        private string _name;
        private string _url;
        private bool _isSelectedTop;
        private bool _isSelectedBottom;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Url
        {
            get => _url;
            set { _url = value; OnPropertyChanged(nameof(Url)); }
        }

        public bool IsSelectedTop
        {
            get => _isSelectedTop;
            set
            {
                _isSelectedTop = value;
                OnPropertyChanged(nameof(IsSelectedTop));
                OnPropertyChanged(nameof(IsDisabledTop));
            }
        }

        public bool IsSelectedBottom
        {
            get => _isSelectedBottom;
            set
            {
                _isSelectedBottom = value;
                OnPropertyChanged(nameof(IsSelectedBottom));
                OnPropertyChanged(nameof(IsDisabledBottom));
            }
        }

        // Proprietà per la disabilitazione (logica di esclusività: disabilitato se selezionato nell'altro slot)
        public bool IsDisabledTop => IsSelectedBottom;
        public bool IsDisabledBottom => IsSelectedTop;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}