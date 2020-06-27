using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Navigation;


namespace Sim.Update.ViewModel
{
    using Common;
    using Helpers;


    class Starter
    {
        #region Declarations
        NavigationService ns;
        #endregion

        #region Constructor
        public Starter()
        {
            ns = Navegador.NavService;
            CheckUpdadte();
        }
        #endregion

        #region Functions

        private async void CheckUpdadte()
        {
            var _uri = new Uri(@"View/Installer.xaml", UriKind.RelativeOrAbsolute);

            var t = Task.Run(() =>
            {
                new Functions.Processos().StopProcess("Sim.App");
                System.Threading.Thread.Sleep(4000);

                if (File.Exists(Files.Sim_Exe_Path) && File.Exists(Files.Sim_Xml_Path))
                {
                    Navegador.Pagina = "Sim.Updater";
                    _uri = (new Uri(@"View/Updater.xaml", UriKind.RelativeOrAbsolute));
                }
                else
                {

                    Navegador.Pagina = "Sim.Installer";
                    _uri = (new Uri(@"View/Installer.xaml", UriKind.RelativeOrAbsolute));
                }
            });

            await t;
            ns.Navigate(_uri);
        }

        #endregion
    }
}
