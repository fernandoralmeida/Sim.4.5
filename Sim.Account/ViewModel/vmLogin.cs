using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;

namespace Sim.Account.ViewModel
{
    using Model;
    using Mvvm.Commands;
    using Controls.ViewModels;

    class vmLogin : VMBase
    {
        #region Delcarations
        private Uri _img = new Uri("/Sim.Account;component/Imagens/UserAccountsIcon.png", UriKind.RelativeOrAbsolute);

        private int _focus;

        private string _getid = string.Empty;
        private string _getname = string.Empty;

        private Visibility _viewid;
        private Visibility _viewpw;
        private Visibility _viewretun;
        
        private ICommand _commandlogin;
        private ICommand _commandvoltar;
        private ICommand _commandcancel;
        private ICommand _commandgetid;
        private ICommand _commandmsgok;

        #endregion

        #region Properties
        public Uri LoginImage
        {
            get { return _img; }
            set
            {
                _img = value;
                RaisePropertyChanged("LoginImage");
            }
        }

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

        public string GetID
        {
            get { return _getid; }
            set
            {
                _getid = value;
                RaisePropertyChanged("GetID");
            }
        }

        public Visibility ViewID
        {
            get { return _viewid; }
            set
            {
                _viewid = value;
                RaisePropertyChanged("ViewID");
            }
        }

        public Visibility ViewPW
        {
            get { return _viewpw; }
            set
            {
                _viewpw = value;
                RaisePropertyChanged("ViewPW");
            }
        }

        public Visibility ViewReturn
        {
            get { return _viewretun; }
            set
            {
                _viewretun = value;
                RaisePropertyChanged("ViewReturn");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandGetID
        {
            get
            {
                if (_commandgetid == null)
                    _commandgetid = new DelegateCommand(ExecuteCommandGetID, null);
                return _commandgetid;
            }
        }


        public ICommand CommandLogin
        {
            get
            {
                if (_commandlogin == null)
                    _commandlogin = new DelegateCommand(ExecuteLoginCommand, null);

                return _commandlogin;
            }
        }

        public ICommand CommandVoltar
        {
            get
            {
                if (_commandvoltar == null)
                    _commandvoltar = new DelegateCommand(ExecuteCommandVoltar, null);
                return _commandvoltar;
            }
        }

        public ICommand CommandCancel
        {
            get
            {
                if (_commandcancel == null)
                    _commandcancel = new DelegateCommand(ExecuteCancelCommand, null);

                return _commandcancel;
            }
        }

        public ICommand CommandMsgOk
        {
            get
            {
                if (_commandmsgok == null)
                    _commandmsgok = new DelegateCommand(ExecuteCommandMsgOk, null);

                return _commandmsgok;
            }
        }

        private void ExecuteCommandMsgOk(object obj)
        {
            ViewMessageBox = Visibility.Collapsed;
            FocusedElements = 0;
        }
        #endregion

        #region Constructor

        public vmLogin()
        {
            StartProgress = false;
            BlackBox = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;

            if (Logged.Autenticado == true)
                Logged.Autenticado = false;

            GetName = "SIM.LOGIN";
            ViewID = Visibility.Visible;
            ViewPW = Visibility.Collapsed;
            ViewReturn = Visibility.Collapsed;
            FocusedElements = 0;
        }

        #endregion

        #region Methods

        private void ExecuteCommandGetID(object obj)
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;
            var tbox = (TextBox)obj;
            GetID = tbox.Text;
            AsyncUser(GetID);
        }

        private void ExecuteCommandVoltar(object obj)
        {
            GetName = "SIM.LOGIN";
            ViewID = Visibility.Visible;
            ViewPW = Visibility.Collapsed;
            ViewReturn = Visibility.Collapsed;
            LoginImage = new Uri("/Sim.Account;component/Imagens/UserAccountsIcon.png", UriKind.RelativeOrAbsolute);
            FocusedElements = 0;
        }

        private void ExecuteLoginCommand(object param)
        {
            AsyncLogin(param);
        }

        private void ExecuteCancelCommand(object obj)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Functions
        private async void AsyncUser(string _gid)
        {
            try
            {
                var login = new mData();

                if (GetID == null || GetID == string.Empty)
                {
                    BlackBox = Visibility.Collapsed;
                    StartProgress = false;
                    return;
                }

                var t = Task.Run(() =>
                {
                    GetName = login.Operador(_gid);
                });

                await t;

                if (t.IsCompleted)
                {
                    BlackBox = Visibility.Collapsed;
                    StartProgress = false;
                    LoginImage = new Uri("/Sim.Account;component/Imagens/keys.ico", UriKind.RelativeOrAbsolute);
                    ViewID = Visibility.Collapsed;
                    ViewPW = Visibility.Visible;
                    ViewReturn = Visibility.Visible;
                    FocusedElements = 1;
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
                    return;
                    throw new Exception("Digite sua senha!");
                }

                var t = Task.Run(() =>
                {
                    var user = login.AutenticarUsuario(GetID, pswd.Password);

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
