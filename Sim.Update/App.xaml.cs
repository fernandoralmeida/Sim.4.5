using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sim.Update
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dictionaries = Current.Resources.MergedDictionaries;

            var ThemeMode = new ResourceDictionary { Source = new Uri(@"Temas/ModernUI.Light.xaml", UriKind.RelativeOrAbsolute) };
            var ThemeBase = new ResourceDictionary { Source = new Uri(@"Temas/ModernUI.xaml", UriKind.RelativeOrAbsolute) };

            //Application.Current.Resources.MergedDictionaries.Clear();


            dictionaries.Clear();

            

            Current.Resources["AccentColor"] = System.Windows.Media.Color.FromRgb(0x1b, 0xa1, 0xe2);
            Current.Resources["Accent"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0x1b, 0xa1, 0xe2));
            dictionaries.Add(ThemeBase);
            dictionaries.Add(ThemeMode);
        }
    }
}
