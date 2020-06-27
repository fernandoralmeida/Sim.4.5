using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;

namespace Sim.Update.Helpers
{
    class Navegador : NotifyPropertyChanged
    {
        #region Declaracoes
        private static string _pagina;
        private static NavigationService _navservice;
        #endregion

        #region Propriedades
        /// <summary>
        /// 
        /// </summary>
        public static string Pagina { get { return _pagina; } set { _pagina = value; OnGlobalPropertyChanged("Pagina"); } }
        /// <summary>
        /// 
        /// </summary>
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
    }
}
