using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Atendimento
{
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Model;
    using Controls.ViewModels;

    class vmEdite : VMBase
    {
        #region Declaracoes
        NavigationService ns;
        private mData mdata = new mData();
        private mAtendimento _atendimento = new mAtendimento();
        private ObservableCollection<string> _servicosrealizados = new ObservableCollection<string>();

        private string _servicoselecionado = string.Empty;
        private string _servicoremovido = string.Empty;

        private ICommand _commandsave;
        private ICommand _commandpesquisarinscricao;
        #endregion

        #region Propriedades
        public mAtendimento Atendimento
        {
            get { return _atendimento; }
            set
            {
                _atendimento = value;
                RaisePropertyChanged("Atendimento");
            }
        }

        public ObservableCollection<mTiposGenericos> Origens
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_Atendimento_Origem WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mTiposGenericos> Tipos
        {
            get
            {
                return new mData().Tipos(@"SELECT * FROM SDT_Atendimento_Tipos WHERE (Ativo = True) ORDER BY Tipo");
            }
        }

        public ObservableCollection<mTiposGenericos> Servicos
        {
            get
            {
                var at = new ObservableCollection<mTiposGenericos>();
                at = new mData().Tipos(@"SELECT * FROM SDT_Atendimento_Tipos WHERE (Ativo = True) ORDER BY Tipo");
                //at.RemoveAt(0);
                return at;
            }
        }

        public ObservableCollection<string> ServicosRealizados
        {
            get { return _servicosrealizados; }
            set
            {
                _servicosrealizados = value;
                RaisePropertyChanged("ServicosRealizados");
            }
        }

        public string ServicoSelecionado
        {
            get { return _servicoselecionado; }
            set
            {
                _servicoselecionado = value;

                if (_servicoselecionado != string.Empty && _servicoselecionado != "...")
                {
                    if (!ServicosRealizados.Any(l => l == _servicoselecionado))
                        ServicosRealizados.Add(_servicoselecionado);
                }

                RaisePropertyChanged("ServicoSelecionado");
            }
        }

        public string ServicoRemovido
        {
            get { return _servicoremovido; }
            set
            {
                _servicoremovido = value;

                if (_servicoremovido != string.Empty)
                {
                    if (ServicosRealizados.Any(l => l == _servicoremovido))
                    {
                        ServicosRealizados.Remove(_servicoremovido);
                        ServicoSelecionado = "...";
                    }
                }

                RaisePropertyChanged("ServicoRemovido");
            }
        }

        #endregion

        #region Comandos
        public ICommand CommandSave
        {
            get
            {
                if (_commandsave == null)
                    _commandsave = new DelegateCommand(ExecCommandSave, null);
                return _commandsave;
            }
        }

        private void ExecCommandSave(object obj)
        {
            GravarAtendimento();
        }

        public ICommand CommandPesquisarInscricao
        {
            get
            {
                if (_commandpesquisarinscricao == null)
                    _commandpesquisarinscricao = new DelegateCommand(ExecuteCommandPesquisarInscricao, null);
                return _commandpesquisarinscricao;
            }
        }

        private void ExecuteCommandPesquisarInscricao(object obj)
        {
            try
            {
                if (Atendimento.Cliente.Inscricao == string.Empty)
                    return;

                string identificador = new mMascaras().Remove(Atendimento.Cliente.Inscricao);

                switch (identificador.Length)
                {
                    case 11:
                        ClientePF(new mData().ExistPessoaFisica(identificador));
                        break;

                    case 14:
                        ClientePJ(new mData().ExistPessoaJuridica(identificador));
                        break;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }
        #endregion

        #region Construtor
        public vmEdite()
        {
            ns = GlobalNavigation.NavService;
            AsyncMostrarAtendimento(AreaTransferencia.Parametro);
        }
        #endregion

        #region Metodos

        #endregion

        #region Funcoes
        private void AsyncMostrarAtendimento(string protocolo)
        {

            Task.Factory.StartNew(() => mdata.Atendimento(protocolo))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Atendimento = task.Result;

                        string[] words = Atendimento.TipoString.ToString().Split(';');

                        foreach (string sv in words)
                        {
                            if (sv != null && sv != string.Empty)
                                ServicosRealizados.Add(sv);
                        }
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void GravarAtendimento()
        {
            try
            {

                StringBuilder sb = new StringBuilder();

                foreach (string sv in ServicosRealizados)
                {
                    if (sv != null || sv != string.Empty)
                        sb.Append(sv + ";");
                }

                Atendimento.TipoString = sb.ToString();

                if (!mdata.AtualizarAtendimento(Atendimento, Account.Logged.Identificador))
                    MessageBox.Show("Erro inesperado :( \nAtendimento não cadastrado!", "Sim.Alerta!");

                else
                {
                    MessageBox.Show(string.Format("Atendimento {0} alterado com sucesso!", Atendimento.Protocolo), "Sim.Alerta!");
                    AreaTransferencia.Limpar();
                    NavigationCommands.BrowseBack.Execute(null, null);
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Sim.Alerta!"); }
        }

        private void ClientePF(mPF_Ext obj)
        {
            try
            {

                string identificador = new mMascaras().Remove(Atendimento.Cliente.Inscricao);


                if (obj == null)
                {
                    if (MessageBox.Show("CLIENTE " + Atendimento.Cliente.Inscricao + " não encontrado! Cadastrá-lo agora?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPF.xaml", UriKind.Relative));
                        //NavigationCommands.GoToPage.Execute(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPFe.xaml", UriKind.Relative), null);
                        AreaTransferencia.CPF = Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CPF_On = true;
                    }

                    return;
                }

                Atendimento.Cliente.Inscricao = new mMascaras().CPF(obj.CPF);
                Atendimento.Cliente.NomeRazao = obj.Nome;
                Atendimento.Cliente.Telefones = obj.Telefones;
                Atendimento.Cliente.Email = obj.Email;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClientePJ(mPJ_Ext obj)
        {
            try
            {

                string identificador = new mMascaras().Remove(Atendimento.Cliente.Inscricao);


                if (obj == null)
                {
                    if (MessageBox.Show("CLIENTE " + Atendimento.Cliente.Inscricao + " não encontrado! Cadastrá-lo agora?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        //NavigationCommands.GoToPage.Execute(new Uri("pAddPJe.xaml", UriKind.RelativeOrAbsolute), null);
                        ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPJ.xaml", UriKind.Relative));
                        AreaTransferencia.CNPJ = Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CNPJ_On = true;
                    }

                    return;
                }

                Atendimento.Cliente.Inscricao = new mMascaras().CNPJ(obj.CNPJ);
                Atendimento.Cliente.NomeRazao = obj.RazaoSocial;
                Atendimento.Cliente.Telefones = obj.Telefones;
                Atendimento.Cliente.Email = obj.Email;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
