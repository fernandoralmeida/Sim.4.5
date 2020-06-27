using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sim
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Current.Resources.MergedDictionaries.Clear();
            new UI.Presentation.ThemeManager().ApplyTheme(System.Windows.Media.Color.FromRgb(0x1b, 0xa1, 0xe2), "Light");
        }
    }
}
