using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;

namespace Sim.Account.ViewModel
{
    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Controls.ViewModels;

    class vmChange : vmAdd
    {
        #region Declarations
        private mAccount _mccount = new mAccount();

        private string _message;
        #endregion

        #region Properties
        public mAccount Account
        {
            get { return _mccount; }

            set
            {
                _mccount = value;
                RaisePropertyChanged("Account");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandGravarChange=> new RelayCommand(p => 
        {
            AsyncUpdate();
        });
        #endregion

        #region Construtor
        public vmChange()
        {
            ns = GlobalNavigation.NavService;
            ViewMessageBox = Visibility.Collapsed;
            BlackBox = Visibility.Collapsed;
            StartProgress = false;

            if (mIDInternal.IDTransport != null || mIDInternal.IDTransport != string.Empty)
                AsyncAccounts();
        }
        #endregion

        #region Methods

        #endregion

        #region  Functions
        private bool Update()
        {
            try
            {

                if (Logged.Acesso >= Account.Conta.Conta)
                {

                    var updateUser = new mUser();

                    updateUser.Indice = Account.Indice;
                    updateUser.Identificador = Account.Identificador;
                    updateUser.Nome = Account.Nome;
                    updateUser.Sexo = Account.Sexo;
                    updateUser.Email = Account.Email;
                    updateUser.Cadastro = Account.Cadastro;
                    updateUser.Atualizado = Account.Atualizado;
                    updateUser.Ativo = Account.Ativo;

                    if (new mData().UpdateUsuario(updateUser))
                    {

                        Opcoes = new mOpcoes()
                        {
                            Identificador = Account.Identificador,
                            Thema = Account.Thema,
                            Color = Account.Color
                        };

                        Conta = new mContas()
                        {
                            Identificador = Account.Identificador,
                            Conta = Account.Conta.Conta,
                            Ativo = true,
                        };

                        new mData().RemoveAcessoModulos(Account.Identificador);
                        foreach (mModuloGenerico md in ListaModulos)
                        {
                            Modulo.Identificador = Account.Identificador;
                            Modulo.Modulo = md.ValorModulo;
                            Modulo.Acesso = md.AcessoModulo;
                            new mData().GravarModulos(Modulo);
                        }

                        new mData().RemoveAcessoSubModulos(Account.Identificador);
                        foreach (mModuloGenerico smd in ListaSubModulos)
                        {
                            SubModulo.Identificador = Account.Identificador;
                            SubModulo.SubModulo = smd.ValorModulo;
                            SubModulo.Acesso = smd.ValorAcesso;
                            new mData().GravarSubModulos(SubModulo);
                        }

                        if (new mData().GravarOpcoes(Opcoes))

                            if (new mData().GravarConta(Conta))
                                _message = (string.Format("Dados do usuário {1} ID {0} alterados com sucesso!", User.Identificador, User.Nome));

                        return true;
                    }
                    else
                        return false;

                }
                else
                {
                    _message = string.Format("Conta com nível de acesso inválido!\nSelecione uma opção inferior à [{0}].", Logged.Conta);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return false;
            }
        }

        private async void AsyncAccounts()
        {
            var task = Task.Run(() => 
            {
                Account = new mData().User(mIDInternal.IDTransport);

                Application.Current.Dispatcher.BeginInvoke(
                    new Action(delegate 
                    {
                        foreach (mModulos m in Account.Modulos)
                        {
                            ListaModulos.Add(new mModuloGenerico()
                            {
                                Indice = m.Indice,
                                Identificador = m.Identificador,
                                NomeModulo = Modulos[m.Modulo - 1].Nome,
                                ValorModulo = m.Modulo,
                                AcessoModulo = m.Acesso,
                                AcessoNome = m.Acesso.ToString(),
                                Ativo = true
                            });
                        }

                        foreach (mSubModulos m in Account.SubModulos)
                        {
                            ListaSubModulos.Add(new mModuloGenerico()
                            {

                                Indice = m.Indice,
                                Identificador = m.Identificador,
                                NomeModulo = SubModulos[m.SubModulo - 1].Nome,
                                ValorModulo = m.SubModulo,
                                ValorAcesso = m.Acesso,
                                AcessoNome = AcessoSubModulos[m.Acesso].Nome,
                                Ativo = true
                            });
                        }
                    }));
            });

            await task;

        }

        private async void AsyncUpdate()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            var _cor = DialogBoxColor.Blue;

            var task = Task<bool>.Factory.StartNew(() => Update());

            await task;

            if(task.Result)
                _cor = DialogBoxColor.Green;

            else
                _cor = DialogBoxColor.Red;
                        
            BlackBox = Visibility.Collapsed;
            StartProgress = false;
            AsyncMessageBox(_message, _cor, true);
        }
        #endregion
    }
}
