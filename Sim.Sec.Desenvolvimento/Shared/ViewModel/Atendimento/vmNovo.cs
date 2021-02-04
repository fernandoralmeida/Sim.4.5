using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Atendimento
{
    using Controls.ViewModels;
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Model;
    using Account;

    class vmNovo : VMBase
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private mAtendimento _atendimento = new mAtendimento();
        private ObservableCollection<mTiposGenericos> _origemat = new ObservableCollection<mTiposGenericos>();
        private ObservableCollection<string> _servicos = new ObservableCollection<string>();
        private ObservableCollection<mCliente> _pf = new ObservableCollection<mCliente>();
        private ObservableCollection<mCliente> _lpj = new ObservableCollection<mCliente>();
        private ObservableCollection<mCliente> _pj = new ObservableCollection<mCliente>();
        private ObservableCollection<string> _servicosrealizados = new ObservableCollection<string>();

        private string _protocolo = string.Empty;
        private Visibility _cabecalho;
        private Visibility _corpo;
        private Visibility _viewlistapj;
        
        #endregion

        #region Properties
        public ObservableCollection<mTiposGenericos> OrigemAtendimento
        {
            get { return _origemat; }
            set
            {
                _origemat = value;                

                RaisePropertyChanged("OrigemAtendimento");
            }
        }

        public ObservableCollection<string> Servicos
        {
            get
            {
                return _servicos;
            }
            set
            {
                _servicos = value;
                RaisePropertyChanged("Servicos");
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

        public mAtendimento Atendimento
        {
            get { return _atendimento; }
            set
            {
                _atendimento = value;

                RaisePropertyChanged("Atendimento");

                if (_atendimento.Origem > -1)
                {
                    AsyncServicos(OrigemAtendimento[Atendimento.Origem].Nome);
                }
            }
        }

        public ObservableCollection<mCliente> PF
        {
            get { return _pf; }
            set
            {
                _pf = value;
                RaisePropertyChanged("PF");
            }
        }

        public ObservableCollection<mCliente> ListaPJ
        {
            get { return _lpj; }
            set
            {
                _lpj = value;
                RaisePropertyChanged("ListaPJ");
            }
        }

        public ObservableCollection<mCliente> PJ
        {
            get { return _pj; }
            set
            {
                _pj = value;
                RaisePropertyChanged("PJ");
            }
        }

        public string Protocolo
        {
            get { return _protocolo; }
            set
            {
                _protocolo = value;
                RaisePropertyChanged("Protocolo");
            }
        }

        public string PageName
        {
            get { return GlobalNavigation.Pagina; }
        }

        public Visibility Cabecalho
        {
            get { return _cabecalho; }
            set
            {
                _cabecalho = value;
                RaisePropertyChanged("Cabecalho");
            }
        }

        public Visibility Corpo
        {
            get { return _corpo; }
            set
            {
                _corpo = value;
                RaisePropertyChanged("Corpo");
            }
        }

        public Visibility ViewListaPJ
        {
            get { return _viewlistapj; }
            set
            {
                _viewlistapj = value;
                RaisePropertyChanged("ViewListaPJ");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandAddService => new RelayCommand(p =>
        {

        ServicosRealizados.Add(p.ToString());
        Servicos.Remove(p.ToString());

            try
            {
                switch (p.ToString())
                {
                    case "INSCRIÇÃO":
                        Atendimento.Tipo = 3;
                        AreaTransferencia.InscricaoOK = false;
                        AreaTransferencia.CPF = Atendimento.Cliente.Inscricao;
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pInscricao.xaml", UriKind.Relative));
                        break;

                    case "MEI - FORMALIZAÇÃO":
                        Atendimento.Tipo = 7;
                        AreaTransferencia.CNPJ = string.Empty;
                        AreaTransferencia.MEI_F = true;
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                        break;

                    case "MEI - ALTERAÇÃO":
                        Atendimento.Tipo = 8;
                        AreaTransferencia.MEI_A = true;
                        AreaTransferencia.CNPJ = PJ[0].Inscricao;
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                        break;

                    case "MEI - BAIXA":
                        Atendimento.Tipo = 9;
                        AreaTransferencia.MEI_B = true;
                        AreaTransferencia.CNPJ = PJ[0].Inscricao;
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                        break;

                    case "MEI - VIABILIDADE":
                        Atendimento.Tipo = 10;
                        AreaTransferencia.ViabilidadeOK = false;
                        AreaTransferencia.CPF = Atendimento.Cliente.Inscricao;
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Viabilidade/pView.xaml", UriKind.Relative));
                        break;

                    case "CADASTRO DE COMÉRCIO AMBULANTE":
                        Atendimento.Tipo = 12;
                        AreaTransferencia.CadAmbulanteOK = false;
                        AreaTransferencia.CPF = new mMascaras().Remove(Atendimento.Cliente.Inscricao).TrimEnd();

                        if (PJ.Count > 0)
                            AreaTransferencia.CNPJ = PJ[0].Inscricao;
                        else
                            AreaTransferencia.CNPJ = string.Empty;

                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/CadAmbulante.xaml", UriKind.Relative));
                        break;

                    case "D.I.A - NOVA INSCRIÇÃO":
                        Atendimento.Tipo = 17;
                        AreaTransferencia.DIA_OK = false;
                        AreaTransferencia.CPF = new mMascaras().Remove(Atendimento.Cliente.Inscricao).TrimEnd();

                        var t_cad_exist = new ComercioAmbulante.Repositorio.RAmbulante().Exist_Ambulante_Cadastrado(AreaTransferencia.CPF);
                        var t_dia_exist = new ComercioAmbulante.Repositorio.RDIA().Exist_DIA(AreaTransferencia.CPF);

                        if (t_cad_exist.Titular.CPF == AreaTransferencia.CPF)
                        {

                            if (t_dia_exist.Titular.CPF != AreaTransferencia.CPF)
                                ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/D-I-A.xaml", UriKind.Relative));

                            else
                            {
                                AsyncMessageBox("Ambulante já tem D.I.A ativo ou vencido no momento!", DialogBoxColor.Red, false);
                                ServicosRealizados.Remove(p.ToString());
                                Servicos.Add(p.ToString());
                            }

                        }
                        else
                        {
                            AsyncMessageBox("Não existe Cadastro de Ambulante válido!", DialogBoxColor.Red, false);
                            ServicosRealizados.Remove(p.ToString());
                            Servicos.Add(p.ToString());
                        }

                        break;

                    case "D.I.A - RENOVAÇÃO":
                        Atendimento.Tipo = 18;
                        AreaTransferencia.DIA_OK = false;
                        AreaTransferencia.CPF = new mMascaras().Remove(Atendimento.Cliente.Inscricao).TrimEnd();

                        var t_rn = new ComercioAmbulante.Repositorio.RDIA().Exist_DIA(AreaTransferencia.CPF);

                        if (t_rn.Titular.CPF == AreaTransferencia.CPF)
                        {
                            AreaTransferencia.Indice = new ComercioAmbulante.Repositorio.RDIA().GetIndice(AreaTransferencia.CPF);
                            ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/DIA_Renov.xaml", UriKind.Relative));
                        }
                        else
                        {
                            AsyncMessageBox("Não existe D.I.A válido!", DialogBoxColor.Red, false);
                            ServicosRealizados.Remove(p.ToString());
                            Servicos.Add(p.ToString());
                        }

                        break;

                    case "D.I.A - BAIXA":
                        Atendimento.Tipo = 19;
                        AreaTransferencia.DIA_OK = false;
                        AreaTransferencia.CPF = new mMascaras().Remove(Atendimento.Cliente.Inscricao).TrimEnd();

                        var t_bx = new ComercioAmbulante.Repositorio.RDIA().Exist_DIA(AreaTransferencia.CPF);

                        if (t_bx.Titular.CPF == AreaTransferencia.CPF)
                        {
                            AreaTransferencia.Indice = new ComercioAmbulante.Repositorio.RDIA().GetIndice(AreaTransferencia.CPF);
                            ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/DIA_Baixa.xaml", UriKind.Relative));
                        }
                        else
                        {
                            AsyncMessageBox("Não existe D.I.A válido!", DialogBoxColor.Red, false);
                            ServicosRealizados.Remove(p.ToString());
                            Servicos.Add(p.ToString());
                        }

                        break;

                    case "D.I.A - 2ª VIA":
                        Atendimento.Tipo = 20;
                        AreaTransferencia.CPF = new mMascaras().Remove(Atendimento.Cliente.Inscricao).TrimEnd();

                        var t_2v = new ComercioAmbulante.Repositorio.RDIA().Exist_DIA(AreaTransferencia.CPF);

                        if (t_2v.Titular.CPF == AreaTransferencia.CPF)
                        {
                            AreaTransferencia.Objeto = t_2v;
                            ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/PreviewDIA.xaml", UriKind.Relative));
                        }
                        else
                        {
                            AsyncMessageBox("Não existe D.I.A válido!", DialogBoxColor.Red, false);
                            ServicosRealizados.Remove(p.ToString());
                            Servicos.Add(p.ToString());
                        }
                        
                        break;

                    case "D.I.A - ALTERAÇÃO":
                        Atendimento.Tipo = 17;
                        AreaTransferencia.DIA_OK = false;
                        AreaTransferencia.CPF = new mMascaras().Remove(Atendimento.Cliente.Inscricao).TrimEnd();

                        var t_alt = new ComercioAmbulante.Repositorio.RDIA().Exist_DIA(AreaTransferencia.CPF);

                        if (t_alt.Titular.CPF == AreaTransferencia.CPF)
                        {
                            AreaTransferencia.Indice = new ComercioAmbulante.Repositorio.RDIA().GetIndice(AreaTransferencia.CPF);
                            ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/DIA_Edit.xaml", UriKind.Relative));
                        }
                        else
                        {
                            AsyncMessageBox("Não existe D.I.A válido!", DialogBoxColor.Red, false);
                            ServicosRealizados.Remove(p.ToString());
                            Servicos.Add(p.ToString());
                        }

                        break;
                }
            }
            catch
            {
                AsyncMessageBox("Serviço inválido!", DialogBoxColor.Red, false);
            }

        });

        public ICommand CommandRemoveService => new RelayCommand(p => {

            Servicos.Add(p.ToString());
            ServicosRealizados.Remove(p.ToString());        
        });

        public ICommand CommandPesquisarInscricao => new RelayCommand(p =>
        {
            try
            {
                if (Atendimento.Cliente.Inscricao == string.Empty)
                    return;

                string identificador = new mMascaras().Remove(Atendimento.Cliente.Inscricao);

                PF.Clear();
                PJ.Clear();

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
            { AsyncMessageBox(ex.Message, DialogBoxColor.Red, false); }
        });


        public ICommand CommandAlterar => new RelayCommand(p=> {
            try
            {
                if (Atendimento.Cliente.Inscricao == string.Empty)
                    return;

                string identificador = new mMascaras().Remove((string)p);

                //Cliente.Clear();
                //PJ.Clear();
                //PF.Clear();

                switch (identificador.Length)
                {
                    case 11:
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Pessoa/pNovo.xaml", UriKind.Relative));
                        AreaTransferencia.CPF = (string)p;// Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CadPF = true;
                        break;

                    case 14:
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                        AreaTransferencia.CNPJ = (string)p; // Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CadPJ = true;
                        break;
                }
            }
            catch (Exception ex)
            { AsyncMessageBox(ex.Message, DialogBoxColor.Red, false); }
        });

        public ICommand CommandSave => new RelayCommand(p => {

            StartProgress = true;
            BlackBox = Visibility.Visible;
            Gravar();

        });

        public ICommand CommandCancel => new RelayCommand(p => {

            Atendimento.Clear();
            PJ.Clear();
            PF.Clear();
            Atendimento.Operador = Logged.Identificador;
            AreaTransferencia.Limpar();
            Corpo = Visibility.Collapsed;
            Cabecalho = Visibility.Visible;
            if (ns.CanGoBack)
                ns.GoBack();
        });


        public ICommand CommandCancelCliente => new RelayCommand(p => {
            if (MessageBox.Show(string.Format("Cancelar Atendimento do Cliente  {0}?", Atendimento.Cliente.NomeRazao), "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Atendimento.Clear();
                PJ.Clear();
                PF.Clear();
                Atendimento.Operador = Logged.Identificador;
                AreaTransferencia.Limpar();
                Corpo = Visibility.Collapsed;
                Cabecalho = Visibility.Visible;
            }
        });

        public ICommand CommandSelectedCNPJ => new RelayCommand(p =>
                    {

                        var t = Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica((string)p));

                        t.Wait();

                        if (t.IsCompleted)
                        {
                            PJ.Add(new mCliente()
                            {
                                Inscricao = t.Result.CNPJ,
                                NomeRazao = t.Result.RazaoSocial,
                                Telefones = t.Result.Telefones,
                                Email = t.Result.Email
                            });
                            ViewListaPJ = Visibility.Collapsed;
                        }

                    });

        public ICommand CommandReloadServicos => new RelayCommand(p => {

            Servicos.Clear();
            AsyncServicos(OrigemAtendimento[Atendimento.Origem].Nome);

        });
        #endregion

        #region Constructor
        public vmNovo()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "NOVO ATENDIMENTO";
            GlobalNotifyProperty.GlobalPropertyChanged += GlobalNotifyProperty_GlobalPropertyChanged;
            GlobalNavigation.NavService.Navigated += NavService_Navigated;
            //GlobalNavigation.BrowseBack = Visibility.Collapsed;

            Protocolo = NProtocolo();
            Atendimento.Data = DateTime.Now;
            Atendimento.Operador = Logged.Identificador;
            Atendimento.Ativo = true;
            StartProgress = false;
            Cabecalho = Visibility.Visible;
            Corpo = Visibility.Collapsed;
            BlackBox = Visibility.Collapsed;
            ViewListaPJ = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;

            AsyncOrigems();
        }

        private void NavService_Navigated(object sender, NavigationEventArgs e)
        {
            if (AreaTransferencia.Preview_DIA == true)
            {
                AreaTransferencia.Preview_DIA = false;
                ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/PreviewDIA.xaml", UriKind.Relative));
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalNotifyProperty_GlobalPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DIA_OK")
                if (AreaTransferencia.DIA_OK == true)
                {
                    if (Atendimento.Historico.Length > 0)
                        Atendimento.Historico = string.Concat(Atendimento.Historico, string.Format("\nD.I.A GERADO COM SUCESSO, Nº {0}", AreaTransferencia.Numero_DIA));

                    else
                        Atendimento.Historico = string.Format(@"D.I.A GERADO COM SUCESSO, Nº {0}", AreaTransferencia.Numero_DIA);
                }

            if (e.PropertyName == "CadAmbulanteOK")
                if (AreaTransferencia.CadAmbulanteOK == true)
                {
                    if (Atendimento.Historico.Length > 0)
                        Atendimento.Historico = string.Concat(Atendimento.Historico, string.Format("\nCOMERCIANTE DE RUA CADASTRADO, PROTOCOLO Nº {0}", AreaTransferencia.CadastroAmbulante));

                    else
                        Atendimento.Historico = string.Format(@"COMERCIANTE DE RUA CADASTRADO, PROTOCOLO Nº {0}", AreaTransferencia.CadastroAmbulante);
                }

            if (e.PropertyName == "CadPF")
                if (AreaTransferencia.CadPF == false)
                    CommandPesquisarInscricao.Execute(null);

            if (e.PropertyName == "CadPJ")
                if (AreaTransferencia.CadPJ == false)
                    CommandPesquisarInscricao.Execute(null);

            if (e.PropertyName == "ViabilidadeOK")
                if (AreaTransferencia.ViabilidadeOK == true)
                {
                    if (Atendimento.Historico.Length > 0)
                        Atendimento.Historico = string.Concat(Atendimento.Historico, string.Format("\nVIABILIDADE {0}", AreaTransferencia.Viabilidade));

                    else
                        Atendimento.Historico = string.Format(@"VIABILIDADE {0}", AreaTransferencia.Viabilidade);
                }

            if (e.PropertyName == "MEI_F")
                if (AreaTransferencia.MEI_F == false)
                {
                    if (Atendimento.Historico.Length > 0)
                        Atendimento.Historico = string.Concat(Atendimento.Historico, string.Format("\nFORMALIZAÇÃO DO MEI CNPJ {0}", AreaTransferencia.CNPJ));

                    else
                        Atendimento.Historico = string.Format(@"FORMALIZAÇÃO DO MEI CNPJ {0}", AreaTransferencia.CNPJ);
                }

            if (e.PropertyName == "MEI_A")
                if (AreaTransferencia.MEI_A == false)
                {
                    if (Atendimento.Historico.Length > 0)
                        Atendimento.Historico = string.Concat(Atendimento.Historico, string.Format("\nALTERAÇÃO DE DADOS DO MEI CNPJ {0}", AreaTransferencia.CNPJ));

                    else
                        Atendimento.Historico = string.Format(@"ALTERAÇÃO DE DADOS DO MEI CNPJ {0}", AreaTransferencia.CNPJ);
                }

            if (e.PropertyName == "MEI_B")
                if (AreaTransferencia.MEI_B == false)
                {
                    if (Atendimento.Historico.Length > 0)
                        Atendimento.Historico = string.Concat(Atendimento.Historico, string.Format("\nBAIXA DO MEI CNPJ {0}", AreaTransferencia.CNPJ));

                    else
                        Atendimento.Historico = string.Format(@"BAIXA DO MEI CNPJ {0}", AreaTransferencia.CNPJ);
                }

            if (e.PropertyName == "InscricaoOK")
                if (AreaTransferencia.InscricaoOK == true)
                {

                    mAgenda ag = new mAgenda();

                    Task.Factory.StartNew(() => new mData().Evento(AreaTransferencia.Evento))
                        .ContinueWith(task =>
                        {
                            if (task.IsCompleted)
                            {
                                ag.Clear();
                                ag.Indice = task.Result.Indice;
                                ag.Codigo = task.Result.Codigo;
                                ag.Evento = task.Result.Evento;
                                ag.Tipo = task.Result.Tipo;
                                ag.TipoString = task.Result.TipoString;
                                ag.Descricao = task.Result.Descricao;
                                ag.Vagas = task.Result.Vagas;
                                ag.Data = task.Result.Data;
                                ag.Hora = task.Result.Hora;
                                ag.Setor = task.Result.Setor;
                                ag.Estado = task.Result.Estado;
                                ag.Criacao = task.Result.Criacao;
                                ag.Ativo = task.Result.Ativo;

                                if (Atendimento.Historico.Length > 0)
                                    Atendimento.Historico = string.Concat(Atendimento.Historico, string.Format("\nINSCRIÇÃO {0} NA {1} {2} {3} {4}", AreaTransferencia.Inscricao[AreaTransferencia.Inscricao.Count - 1], ag.TipoString, ag.Evento, ag.Data.ToShortDateString(), ag.Hora.ToShortTimeString()));

                                else
                                    Atendimento.Historico = string.Format(@"INSCRIÇÃO {0} NA {1} {2} {3} {4}", AreaTransferencia.Inscricao[AreaTransferencia.Inscricao.Count - 1], ag.TipoString, ag.Evento, ag.Data.ToShortDateString(), ag.Hora.ToShortTimeString());

                            }
                        });
                }

        }
        #endregion

        #region Functions
        private string NProtocolo()
        {
            string r = string.Empty;

            string a = DateTime.Now.Year.ToString("0000");
            string m = DateTime.Now.Month.ToString("00");
            string d = DateTime.Now.Day.ToString("00");

            string hs = DateTime.Now.Hour.ToString("00");
            string mn = DateTime.Now.Minute.ToString("00");
            string ss = DateTime.Now.Second.ToString("00");

            r = string.Format(@"AT{0}{1}{2}{3}{4}{5}",
                a, m, d, hs, mn, ss);

            return r;
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
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Pessoa/pNovo.xaml", UriKind.Relative));
                        AreaTransferencia.CPF = Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CadPF = true;
                    }

                    return;
                }

                //AsyncMostrarCliente(obj.CPF);
                MostrarCliente(obj.CPF);

                Atendimento.Cliente.Inscricao = new mMascaras().CPF(obj.CPF);
                Atendimento.Cliente.NomeRazao = obj.Nome;
                Atendimento.Cliente.Telefones = obj.Telefones;
                Atendimento.Cliente.Email = obj.Email;

                Corpo = Visibility.Visible;
                Cabecalho = Visibility.Collapsed;
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
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                        AreaTransferencia.CNPJ = Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CadPJ = true;
                    }

                    return;
                }

                AsyncPJ(obj.CNPJ);

                Atendimento.Cliente.Inscricao = new mMascaras().CNPJ(obj.CNPJ);
                Atendimento.Cliente.NomeRazao = obj.RazaoSocial;
                Atendimento.Cliente.Telefones = obj.Telefones;
                Atendimento.Cliente.Email = obj.Email;

                Corpo = Visibility.Visible;
                Cabecalho = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void AsyncOrigems()
        {
           var t = Task.Factory.StartNew(() => new mData().Tipos(@"SELECT * FROM SDT_Atendimento_Origem WHERE (Ativo = True) ORDER BY Valor"));
            await t;

            if (t.IsCompleted)
            {
                OrigemAtendimento = t.Result;

                Atendimento.Origem = Convert.ToInt32(GlobalNavigation.Parametro);

                AsyncServicos(OrigemAtendimento[Atendimento.Origem].Nome);
            }
        }

        private async void AsyncServicos(string _origem)
        {
            var t = Task.Factory.StartNew(() => new mData().Servicos(_origem));

            await t;

            if (t.IsCompleted)
            {
                foreach (mTiposGenericos s in t.Result)
                    Servicos.Add(s.Nome);
            }
                
        }

        private async void MostrarCliente(string cpf)
        {
            PF.Clear();
            ListaPJ.Clear();

            var pf = Task.Run(() => mdata.ExistPessoaFisica(cpf));
            
            await pf;

            if (pf.IsCompleted)
                PF.Add(new mCliente()
                {
                    Inscricao = pf.Result.CPF,
                    NomeRazao = pf.Result.Nome,
                    Telefones = pf.Result.Telefones,
                    Email = pf.Result.Email
                });

            List<Task<mPJ_Ext>> l_pj = new List<Task<mPJ_Ext>>();

            foreach (mVinculos v in pf.Result.ColecaoVinculos)
                l_pj.Add(Task.Run(() => mdata.ExistPessoaJuridica(v.CNPJ)));
            
            foreach(Task<mPJ_Ext> pj in l_pj)
            {
                await pj;

                //if (pj.Result.SituacaoCadastral != "BAIXADO")
                    if (pj.IsCompleted)
                    {
                        if (pf.Result.ColecaoVinculos.Count > 1)
                        {
                            ListaPJ.Add(new mCliente()
                            {
                                Inscricao = pj.Result.CNPJ,
                                NomeRazao = pj.Result.RazaoSocial,
                                Telefones = pj.Result.Telefones,
                                Email = pj.Result.Email
                            });
                            ViewListaPJ = Visibility.Visible;
                        }
                        else
                        {
                            PJ.Add(new mCliente()
                            {
                                Inscricao = pj.Result.CNPJ,
                                NomeRazao = pj.Result.RazaoSocial,
                                Telefones = pj.Result.Telefones,
                                Email = pj.Result.Email
                            });
                        }
                    }
            }
        }

        private void AsyncMostrarCliente(string cpf)
        {
            Task<mPF_Ext>.Factory.StartNew(() => mdata.ExistPessoaFisica(cpf))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        PF.Clear();
                        PF.Add(new mCliente()
                        {
                            Inscricao = task.Result.CPF,
                            NomeRazao = task.Result.Nome,
                            Telefones = task.Result.Telefones,
                            Email = task.Result.Email
                        });
                        ListaPJ.Clear();
                        foreach (mVinculos v in task.Result.ColecaoVinculos)
                        {
                            Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica(v.CNPJ)).ContinueWith(task_two =>
                            {
                                if (task_two.IsCompleted)
                                {

                                    if (task.Result.ColecaoVinculos.Count > 1)
                                    {
                                        ListaPJ.Add(new mCliente()
                                        {
                                            Inscricao = task_two.Result.CNPJ,
                                            NomeRazao = task_two.Result.RazaoSocial,
                                            Telefones = task_two.Result.Telefones,
                                            Email = task_two.Result.Email
                                        });
                                        ViewListaPJ = Visibility.Visible;
                                    }
                                    else
                                    {
                                        PJ.Add(new mCliente()
                                        {
                                            Inscricao = task_two.Result.CNPJ,
                                            NomeRazao = task_two.Result.RazaoSocial,
                                            Telefones = task_two.Result.Telefones,
                                            Email = task_two.Result.Email
                                        });
                                    }
                                }
                            },
                            System.Threading.CancellationToken.None,
                            TaskContinuationOptions.None,
                            TaskScheduler.FromCurrentSynchronizationContext());
                        }

                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncPJ(string cnpj)
        {
            Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica(cnpj)).ContinueWith(task_two =>
            {
                if (task_two.IsCompleted)
                {
                    PJ.Clear();
                    PJ.Add(new mCliente()
                    {
                        Inscricao = task_two.Result.CNPJ,
                        NomeRazao = task_two.Result.RazaoSocial,
                        Telefones = task_two.Result.Telefones,
                        Email = task_two.Result.Email
                    });
                }
            }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncGravarVinculo()
        {
            var v = new mVinculos();
            v.CNPJ = AreaTransferencia.CNPJ;
            v.CPF = Atendimento.Cliente.Inscricao;
            v.Vinculo = 2;//Proprietario
            v.Acao = 1;
            v.Data = DateTime.Now;
            v.Ativo = true;

            Task.Factory.StartNew(() => mdata.GravarVinculos(v, Registro.Novo)).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (!task.Result)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                        {
                            AsyncMessageBox("Erro inesperado :( \nVinculo não cadastrado!", DialogBoxColor.Orange, false);
                        }));
                    }
                }
            }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncGravarPerfil()
        {
            var pf = new mData().ExistPessoaFisica(Atendimento.Cliente.Inscricao);
            var p = new mPerfil();
            p.Indice = pf.Perfil.Indice;
            p.CPF = Atendimento.Cliente.Inscricao;
            p.Perfil = 2;
            p.Negocio = true;
            p.Ativo = true;

            Task.Factory.StartNew(() => mdata.GravarPerfil(p, Registro.Alteracao)).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (!task.Result)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                        {
                            AsyncMessageBox("Erro inesperado :( \nPerfil não gravado!", DialogBoxColor.Orange ,false);
                        }));
                    }
                }
            }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncGravarAmbulante()
        {
            Task.Factory.StartNew(() => mdata.GravarAmbulanteAtendimento(AreaTransferencia.CadastroAmbulante, Atendimento.Protocolo)).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (!task.Result)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                        {
                            AsyncMessageBox("Erro inesperado :( \nCadastro não gravado!", DialogBoxColor.Orange, false);
                        }));
                    }
                }
            }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncGravarInscricao()
        {
            foreach (string str in AreaTransferencia.Inscricao)
            {
                mInscricao insc = new mInscricao();
                insc.Inscricao = str;
                insc.Atendimento = Atendimento.Protocolo;
                insc.Ativo = true;
                mdata.AtualizarAtendimentoInscricao(insc);
                Task.Factory.StartNew(() => mdata.AtualizarAtendimentoInscricao(insc)).ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        if (!task.Result)
                        {
                            Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                            {
                                AsyncMessageBox("Erro inesperado :( \nInscricao não gravada!", DialogBoxColor.Orange, false);
                            }));
                        }
                    }
                }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void AsyncGravarAtendimento()
        {
            Task.Factory.StartNew(() => mdata.GravarAtendimento(Atendimento)).ContinueWith(task =>
            {
                if (!task.Result)
                {
                    Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                    {
                        AsyncMessageBox("Erro inesperado :( \nAtendimento não gravado!", DialogBoxColor.Orange, false);
                    }));
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                    {
                        AsyncMessageBox(string.Format("Atendimento {0} gravado com sucesso!", Atendimento.Protocolo), DialogBoxColor.Green, true);
                    }));
                }

            }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Gravar()
        {

            try
            {
                Atendimento.Protocolo = Protocolo;
                Atendimento.Data = DateTime.Now;

                // Formalização
                if (ServicosRealizados.Any(l => l == "MEI - FORMALIZAÇÃO"))
                {
                    AsyncGravarVinculo();
                    AsyncGravarPerfil();
                }

                //Ambulante
                if (ServicosRealizados.Any(l => l == "CADASTRO DE COMÉRCIO AMBULANTE"))//(Atendimento.Tipo == 12)
                    AsyncGravarAmbulante();

                //Inscrição
                if (ServicosRealizados.Any(l => l == "INSCRIÇÃO"))//(Atendimento.Tipo == 3)
                    AsyncGravarInscricao();

                StringBuilder sb = new StringBuilder();

                foreach (string sv in ServicosRealizados)
                {
                    if (sv != null && sv != string.Empty)
                        sb.Append(sv + ";");
                }

                Atendimento.TipoString = sb.ToString();

                AsyncGravarAtendimento();
            }
            catch (Exception ex)
            {
                AsyncMessageBox(ex.Message, DialogBoxColor.Green, false);
            }
        }
        #endregion
    }
}
