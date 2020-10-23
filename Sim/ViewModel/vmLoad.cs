using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace Sim.ViewModel
{

    using Model;
    using Account;
    using Controls.ViewModels;

    class vmLoad : VMBase
    {

        #region Constructor

        public vmLoad()
        {
            ViewMessageBox = Visibility.Collapsed;

            CheckUpdadte();
        }
        #endregion

        #region Functions

        private async void CheckUpdadte()
        {
            string _url = @"\\servidor\Sim\sim_update.xml";
            //string _url = @"E:\Sim\sim_update.xml";

            var t = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(2000);

                try
                {
                    if (Functions.Updater.HasUpdate(_url))
                    {
                        System.Threading.Thread.Sleep(2000);
                        new Functions.Processos().ExecuteProcess("Sim.Updater");
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(2000);
                        Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                            new Action(delegate
                            {
                                mStarted.SimStarted = false;
                                Logged.Autenticado = false;
                                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                            }));
                    }
                }
                catch 
                {
                    //SyncMessageBox(ex.Message, DialogBoxColor.Red);
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                        new Action(delegate
                        {
                            mStarted.SimStarted = false;
                            Logged.Autenticado = false;
                            Application.Current.MainWindow.WindowState = WindowState.Maximized;
                        }));
                }

            });
            await t;          
        }

        #endregion
    }
}
