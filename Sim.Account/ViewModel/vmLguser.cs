using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Sim.Account.ViewModel
{

    using Model;
    using Mvvm.Commands;
    using Controls.ViewModels;

    class vmLguser : VMBase
    {
        #region Delcarations
        private NavigationService ns;

        private int _focus;

        private string _getid = string.Empty;

        #endregion

        #region Properties
        public int FocusedElements
        {
            get { return _focus; }
            set
            {
                _focus = value;
                RaisePropertyChanged("FocusedElements");
            }
        }

        public string GetID
        {
            get { return _getid; }
            set
            {
                _getid = value;
                RaisePropertyChanged("GetID");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandGetID => new RelayCommand(p => 
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;
            var tbox = (TextBox)p;
            AsyncUser(tbox.Text);
        });
        #endregion

        #region Constructor

        public vmLguser()
        {
            ns = Mvvm.Observers.GlobalNavigation.NavService;

            StartProgress = false;
            BlackBox = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;

            if (Logged.Autenticado == true)
                Logged.Autenticado = false;

            FocusedElements = 0;
        }

        #endregion

        #region Functions
        private async void AsyncUser(string _gid)
        {
            try
            {
                var login = new mData();

                if (_gid == null || _gid == string.Empty)
                {
                    BlackBox = Visibility.Collapsed;
                    StartProgress = false;
                    throw new Exception("Informe seu Identificador!");
                }

                var t = Task.Run(() =>
                {
                    Log.Nome = login.Operador(_gid); 
                    Log.ID = _gid;
                });

                await t;

                if (t.IsCompleted)
                {
                    BlackBox = Visibility.Collapsed;
                    StartProgress = false;
                    ns.Navigate(new Uri(Properties.Resources.Account_Login_PSW, UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                    BlackBox = Visibility.Collapsed;
                    StartProgress = false;
                    AsyncMessageBox(ex.Message, DialogBoxColor.Orange, false);                    
                }));
            }
        }
        #endregion
    }
}
