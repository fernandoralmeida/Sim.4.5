using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.IO;

using System.Windows.Navigation;

namespace Sim.Update.ViewModel
{
    using Helpers;

    class Main : NotifyPropertyChanged
    {
        #region Declarations
        NavigationService ns;
        private string _titulo;

        #endregion

        #region Properties

        public string Titulo
        {
            get { return _titulo; }
            set
            {
                _titulo = value;
                OnPropertyChanged("Titulo");
            }
        }

        #endregion

        #region Commands

        #endregion

        #region Constructor
        public Main()
        {
            Tema();
            ns = Navegador.NavService;
            GlobalPropertyChanged += NotifyPropertyChanged_GlobalPropertyChanged;
            Navegador.Pagina = "TESTE";
            ns.Navigate(new Uri(@"View/Starter.xaml", UriKind.Relative));            
        }

        #endregion

        #region Methods

        private void NotifyPropertyChanged_GlobalPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Pagina")
                Titulo = Navegador.Pagina;
        }

        private void Tema()
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;

            var ThemeMode = new ResourceDictionary { Source = new Uri(@"../Temas/ModernUI.Light.xaml", UriKind.RelativeOrAbsolute) };
            var ThemeBase = new ResourceDictionary { Source = new Uri(@"../Temas/ModernUI.xaml", UriKind.RelativeOrAbsolute) };

            //Application.Current.Resources.MergedDictionaries.Clear();
            dictionaries.Clear();
            dictionaries.Add(ThemeBase);
            dictionaries.Add(ThemeMode);
        }
        #endregion
    }
}
