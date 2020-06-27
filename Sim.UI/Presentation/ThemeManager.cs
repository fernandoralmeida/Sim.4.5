using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Sim.UI.Presentation
{
    using Model;
    using Observers;

    public class ThemeManager : NotifyPropertyChanged
    {
        #region Declarations
        public const string KeyAccentColor = "AccentColor";
        public const string KeyAccent = "Accent";

        private static ThemeManager current = new ThemeManager();
        ObservableCollection<mTheme> gthemes = new ObservableCollection<mTheme>();

        #endregion

        #region Properties

        private ObservableCollection<mTheme> GetThemes
        {
            get
            {
                return gthemes;
            }
            set
            {
                gthemes = value;
                OnPropertyChanged("GetThemes");
            }
        }

        #endregion

        #region Methods

        public void ApplyTheme(Color accentcolor, string theme)
        {
            if (accentcolor != null)
            {
                Application.Current.Resources[KeyAccentColor] = accentcolor;
                Application.Current.Resources[KeyAccent] = new SolidColorBrush(accentcolor);
            }

            if (theme != null)
            {
                //var oldThemeDict = GetThemeDictionary();
                var dictionaries = Application.Current.Resources.MergedDictionaries;

                ResourceDictionary ThemeMode = new ResourceDictionary { Source = new Uri(string.Format(@"/Sim.UI;component/Themes/Modern/ModernUI.{0}.xaml", theme), UriKind.RelativeOrAbsolute) };
                ResourceDictionary ThemeBase = new ResourceDictionary { Source = new Uri(@"/Sim.UI;component/Themes/Modern/ModernUI.xaml", UriKind.RelativeOrAbsolute) };

                //Application.Current.Resources.MergedDictionaries.Clear();
                dictionaries.Clear();           
                dictionaries.Add(ThemeBase);
                dictionaries.Add(ThemeMode);

                ThemeObserver.ThemeSource = true;
                //OnPropertyChanged("ThemeSource");
            }
        }

        #endregion

        #region Constructors

        public ThemeManager()
        {
        }

        #endregion

        #region Functions
        private ResourceDictionary GetThemeDictionary()
        {
            // determine the current theme by looking at the app resources and return the first dictionary having the resource key 'WindowBackground' defined.
            return (from dict in Application.Current.Resources.MergedDictionaries
                    where dict.Contains("WindowBackground")
                    select dict).FirstOrDefault();
        }

        public ObservableCollection<mTheme> ListThemes()
        {
            ObservableCollection<mTheme> _theme = new ObservableCollection<mTheme>();
            _theme.Clear();
            _theme.Add(new mTheme() { Name = "Light", Descricao = "LIGHT", Imagem = new Uri("/Sim.UI;component/Backgrounds/light.jpg", UriKind.Relative) }); ;
            _theme.Add(new mTheme() { Name = "Dark", Descricao = "DARK", Imagem = new Uri("/Sim.UI;component/Backgrounds/dark.jpg", UriKind.Relative) });
            _theme.Add(new mTheme() { Name = "BingImage", Descricao = "BING", Imagem = Images.BingImage.UriBing()});
            //_theme.Add(new mTheme() { Name = "BoraBora", Descricao = "BORA BORA", Imagem = new Uri("/Sim.Resources;component/Wallpaper/bora_bora.jpg", UriKind.Relative) });
            //_theme.Add(new mTheme() { Name = "GreenGrass", Descricao = "GREEN GRASS", Imagem = new Uri("/Sim.Resources;component/Wallpaper/water_drops_on_grass.jpg", UriKind.Relative) });
            //_theme.Add(new mTheme() { Name = "SnowFlakes", Descricao = "SNOW FLAKES", Imagem = new Uri("/Sim.Resources;component/Wallpaper/snow_flakes.jpg", UriKind.Relative) });
            //_theme.Add(new mTheme() { Name = "SmileSnow", Descricao = "SMILE SNOW", Imagem = new Uri("/Sim.Resources;component/Wallpaper/smile_on_snow.jpg", UriKind.Relative) });
            //_theme.Add(new mTheme() { Name = "WaterGlass", Descricao = "WATER DROPS", Imagem = new Uri("/Sim.Resources;component/Wallpaper/water_drops.jpg", UriKind.Relative) });
            //_theme.Add(new mTheme() { Name = "DarkGrass", Descricao = "DARK GRASS", Imagem = new Uri("/Sim.Resources;component/Wallpaper/dark_grass.jpg", UriKind.Relative) });
            //_theme.Add(new mTheme() { Name = "Aventador", Descricao = "AVENTADOR", Imagem = new Uri("/Sim.Resources;component/Wallpaper/aventador.jpg", UriKind.Relative) });
            return _theme;
        }
        #endregion
    }
}
