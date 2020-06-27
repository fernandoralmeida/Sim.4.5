using System.Windows;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Sim.ViewModel
{
    using UI.Model;
    using UI.Presentation;
    using Mvvm.Observers;
    using Mvvm.Commands;

    class vmToolsTheme : NotifyProperty
    {
        #region Declarations
        private NavigationService ns;
        private ThemeManager tm = new ThemeManager();
        private PaletteColors pc = new PaletteColors();
        private ObservableCollection<mTheme> _themes = new ObservableCollection<mTheme>();
        private ObservableCollection<mColor> _colorthemes = new ObservableCollection<mColor>();
        private Color selectedAccentColor;
        private string _selectedthememode;
        private string _themename;
        #endregion

        #region Properties

        public string ThemeName
        {
            get { return _themename; }
            set
            {
                _themename = value;
                RaisePropertyChanged("ThemeName");
            }
        }

        public string SelectedThemeMode
        {
            get { return _selectedthememode; }
            set
            {
                _selectedthememode = value;
                RaisePropertyChanged("SelectedThemeMode");
            }
        }

        public Color SelectedAccentColor
        {
            get { return this.selectedAccentColor; }
            set
            {
                if (this.selectedAccentColor != value)
                {
                    this.selectedAccentColor = value;
                    RaisePropertyChanged("SelectedAccentColor");
                }
            }
        }

        public ObservableCollection<mTheme> ThemeMode
        {
            get { return _themes; }
            set
            {
                _themes = value;
                RaisePropertyChanged("ThemeMode");
            }
        }

        public ObservableCollection<mColor> ColorThemes
        {
            get { return _colorthemes; }
            set
            {
                _colorthemes = value;
                RaisePropertyChanged("ColorThemes");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandApplyTheme => new RelayCommand(p => 
        {
            AsyncAccountTheme();
            if (ns.CanGoBack)
                ns.GoBack();

        });

        public ICommand CommandGoBack => new RelayCommand(p =>
        {
            CheckTheme();
            if (ns.CanGoBack)
                ns.GoBack();
        });

        #endregion

        #region Constructor

        public vmToolsTheme()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "PERSONALIZAR";
            GlobalNavigation.BrowseBack = Visibility.Visible;
            this.PropertyChanged += VmTheme_PropertyChanged;
            ColorThemes = pc.ListColors();
            ThemeMode = tm.ListThemes();
            CheckTheme();
        }

        #endregion

        #region Methods

        private void VmTheme_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAccentColor" || e.PropertyName == "SelectedThemeMode")
            {                
                tm.ApplyTheme(SelectedAccentColor, SelectedThemeMode);
                SelectedAccentColor = (Color)Application.Current.Resources["AccentColor"];
            }
        }

        private void CheckTheme()
        {
            try
            {
                Color paleta = new Color();
                paleta = (Color)ColorConverter.ConvertFromString(Account.Logged.Color);
                SelectedThemeMode = Account.Logged.Thema;
                SelectedAccentColor = paleta;
            }
            catch
            {
                SelectedAccentColor = Colors.CornflowerBlue;
                SelectedThemeMode = "Light";
            }
        }

        #endregion

        #region Functions
        private async void AsyncAccountTheme()
        {

            var opc = new Account.Model.mOpcoes
            {
                Identificador = Account.Logged.Identificador,
                Thema = SelectedThemeMode.ToString(),
                Color = SelectedAccentColor.ToString()
            };

            Account.Logged.Color = opc.Color;
            Account.Logged.Thema = opc.Thema;

            var t = Task.Run(() => {

                if (Account.Logged.Identificador.ToLower() != "System".ToLower())
                    new Account.Model.mData().GravarOpcoes(opc);
            });

            await t;
        }

        #endregion
    }
}
