using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Portarias.ViewModel
{
    using Mvvm.Observers;

    class vmPageObserver : GlobalNotifyProperty
    {
        private static object _searchcontext;
        private static object _reportcontext;
        /// <summary>
        /// Get or Set Current ViewContent for MainWindow
        /// </summary>
        public static object SearchContext
        {
            get { return _searchcontext; }
            set
            {
                _searchcontext = value;
                OnGlobalPropertyChanged("SearchContext");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static object ReportContext
        {
            get { return _reportcontext; }
            set
            {
                _reportcontext = value;
                OnGlobalPropertyChanged("ReportContext");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void ReportClear()
        {
            ReportContext = null;
            GC.Collect();
        }
        /// <summary>
        /// Anula o controle
        /// </summary>
        public static void SearchClear()
        {
            SearchContext = null;
            GC.Collect();
        }
    }
}
