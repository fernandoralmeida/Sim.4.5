using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sim.UI.Observers
{

    public class ThemeObserver : NotifyPropertyChanged
    {
        public static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };

        public static void OnGlobalPropertyChanged(string propertyName)
        {
            GlobalPropertyChanged(
                typeof(ThemeObserver),
                new PropertyChangedEventArgs(propertyName));
        }

        private static bool _themesource;
        public static bool ThemeSource
        {
            get { return _themesource; }
            set
            {
                _themesource = value;
                OnGlobalPropertyChanged("ThemeSource");
            }
        }

    }
}
