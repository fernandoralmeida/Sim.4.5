using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading.Tasks;

namespace Sim.Account.ViewModel
{
    using Model;
    using Mvvm.Commands;
    using Controls.ViewModels;

    class vmLgpsw : VMBase
    {
        #region Delcarations
        private NavigationService ns;

        private int _focus;

        private string _getname = string.Empty;

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

        public string GetName
        {
            get { return _getname; }
            set
            {
                _getname = value;
                RaisePropertyChanged("GetName");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandLogin => new RelayCommand(p => 
        {
            AsyncLogin(p);
        });

        public ICommand CommandVoltar => new RelayCommand(p => 
        {
            if (ns.CanGoBack)
                ns.GoBack();
        });

        #endregion

        #region Constructor

        public vmLgpsw()
        {
            ns = Mvvm.Observers.GlobalNavigation.NavService;

            StartProgress = false;
            BlackBox = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;

            if (Logged.Autenticado == true)
                Logged.Autenticado = false;

            GetName = Log.Nome;
            FocusedElements = 1;
        }

        #endregion

        #region Functions

        private async void AsyncLogin(object _gpw)
        {
            var login = new mData();
            StartProgress = true;
            BlackBox = Visibility.Visible;

            try
            {
                var objetos = (object)_gpw;
                var pswd = (PasswordBox)objetos;

                if (pswd.Password == null || pswd.Password == string.Empty)
                {
                    StartProgress = false;
                    BlackBox = Visibility.Collapsed;                    
                    throw new Exception("Digite sua senha!");
                }

                var t = Task.Run(() =>
                {
                    var user = login.AutenticarUsuario(Log.ID, pswd.Password);

                    if (user != null)
                    {
                        Logged.Indice = user.Indice;
                        Logged.Identificador = user.Identificador;
                        Logged.Color = user.Color;
                        Logged.Thema = user.Thema;
                        Logged.Acesso = user.Conta.Conta;
                        Logged.Conta = user.Conta.ContaAcesso;
                        Logged.Modulos = user.Modulos;
                        Logged.Submodulos = user.SubModulos;
                        Logged.Nome = user.Nome;
                        Logged.Sexo = user.Sexo.ToUpper();
                        Logged.Cadastro = user.Cadastro;
                        Logged.Atualizado = user.Atualizado;
                        Logged.Ativo = user.Ativo;
                        Logged.Email = user.Email;
                        Logged.Registro = user.Registro;
                        Logged.Autenticado = user.Autenticado;
                    }
                    else
                        throw new Exception("Senha incorreta!");

                });

                await t;

                if (t.IsCompleted)
                {
                    StartProgress = false;
                    BlackBox = Visibility.Collapsed;
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
