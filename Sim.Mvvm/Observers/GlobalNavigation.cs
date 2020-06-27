using System;
using System.Windows;
using System.Windows.Navigation;

namespace Sim.Mvvm.Observers
{

    public class GlobalNavigation : GlobalNotifyProperty
    {
        #region Declaracoes
        private static string _pagina;
        private static string _parametro;
        private static string _submodulo;
        private static string _modulo;
        private static Uri _navegar;
        private static string _urimodulo;
        private static string _urisubmodulo;
        private static NavigationService _navservice;
        private static Visibility _browseback;
        #endregion

        #region Propriedades
        /// <summary>
        /// 
        /// </summary>
        public static string Pagina { get { return _pagina; } set { _pagina = value; OnGlobalPropertyChanged("Pagina"); } }
        /// <summary>
        /// 
        /// </summary>
        public static string Parametro { get { return _parametro; } set { _parametro = value; OnGlobalPropertyChanged("Parametro"); } }
        /// <summary>
        /// 
        /// </summary>
        public static string Modulo { get { return _modulo; } set { _modulo = value; OnGlobalPropertyChanged("Modulo"); } }
        /// <summary>
        /// 
        /// </summary>
        public static string SubModulo { get { return _submodulo; } set { _submodulo = value; OnGlobalPropertyChanged("SubModulo"); } }
        /// <summary>
        /// 
        /// </summary>
        public static string UriModulo { get { return _urimodulo; } set { _urimodulo = value; OnGlobalPropertyChanged("UriModulo"); } }
        /// <summary>
        /// 
        /// </summary>
        public static string UriSubModulo { get { return _urisubmodulo; } set { _urisubmodulo = value; OnGlobalPropertyChanged("UriSubModulo"); } }
        /// <summary>
        /// 
        /// </summary>
        public static Uri Navegar { get { return _navegar; } set { _navegar = value; OnGlobalPropertyChanged("Navegar"); } }
        /// <summary>
        /// 
        /// </summary>
        public static Visibility BrowseBack { get { return _browseback; } set { _browseback = value; OnGlobalPropertyChanged("BrowseBack"); } }

        public static NavigationService NavService
        {
            get { return _navservice; }
            set
            {
                _navservice = value;
                OnGlobalPropertyChanged("NavService");
            }
        }
        #endregion

        #region Funcoes
        public static void Reiniciar()
        {
            BrowseBack = Visibility.Collapsed;
            Pagina = string.Empty;
            Parametro = string.Empty;
            SubModulo = string.Empty;
            Modulo = string.Empty;
        }

        public static void ReiniciarNavigador()
        {
            Navegar = null;
            GC.Collect();
        }
        #endregion
    }
}
