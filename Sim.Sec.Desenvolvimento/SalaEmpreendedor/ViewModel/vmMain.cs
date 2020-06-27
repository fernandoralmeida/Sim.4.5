using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Media;

namespace Sim.Sec.Desenvolvimento.SalaEmpreendedor.ViewModel
{

    using Shared.Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Controls.ViewModels;
    using Account;
    using Shared.Control;

    class vmMain : VMBase, IAgenda
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private ObservableCollection<mAtendimento> _listaat = new ObservableCollection<mAtendimento>();
        private ObservableCollection<mAgenda> _listag = new ObservableCollection<mAgenda>();

        private FlowDocument _flowdoc = new FlowDocument();
        private FlowDocument _flowtemp = new FlowDocument();

        private DateTime _datai = DateTime.Now;

        private Visibility _previewbox;
        private Visibility _retvisible = Visibility.Collapsed;

        private string _preview = string.Empty;

        private bool _viewcnpj = false;
        private bool _isadmin;
        private bool _isenable;

        private int _selectedrow;

        private ICommand _commandclosepreview;
        private ICommand _commandpreviewbox;
        private ICommand _commandpreviewbox2;
        private ICommand _commandrefreshdate;
        private ICommand _commandgocnpj;
        private ICommand _commandretcpf;
        #endregion

        #region Properties
        public ObservableCollection<mTiposGenericos> Tipos
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Tipo"); }
        }

        public FlowDocument FlowDoc
        {
            get { return _flowdoc; }
            set
            {
                _flowdoc = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public ObservableCollection<mAgenda> ListarAgenda
        {
            get { return _listag; }
            set
            {
                _listag = value;
                RaisePropertyChanged("ListarAgenda");
            }
        }

        public ObservableCollection<mAtendimento> ListarAtendimentos
        {
            get { return _listaat; }
            set
            {
                _listaat = value;
                RaisePropertyChanged("ListarAtendimentos");
            }
        }

        public DateTime DataI
        {
            get { return _datai; }
            set
            {
                _datai = value;
                AsyncListarAtendimentoHoje(Parametros("1"));
                RaisePropertyChanged("DataI");
            }
        }

        public Visibility PreviewBox
        {
            get { return _previewbox; }
            set
            {
                _previewbox = value;
                RaisePropertyChanged("PreviewBox");
            }
        }

        public Visibility RetVisible
        {
            get { return _retvisible; }
            set
            {
                _retvisible = value;
                RaisePropertyChanged("RetVisible");
            }
        }

        public bool IsAdmin
        {
            get { return _isadmin; }
            set
            {
                _isadmin = value;
                RaisePropertyChanged("IsAdmin");
            }
        }

        public bool IsEnable
        {
            get { return _isenable; }
            set
            {
                _isenable = value;
                RaisePropertyChanged("IsEnable");
            }
        }

        public string Preview
        {
            get { return _preview; }
            set
            {
                _preview = value;
                RaisePropertyChanged("Preview");
            }
        }

        public string PageName
        {
            get { return GlobalNavigation.Pagina; }
        }

        public int SelectedRow
        {
            get { return _selectedrow; }
            set
            {
                _selectedrow = value;
                RaisePropertyChanged("SelectedRow");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandDataPrev => new RelayCommand(p =>
        {
            DataI = DataI.AddDays(-1);
        });

        public ICommand CommandDataNext => new RelayCommand(p =>
        {
            DataI = DataI.AddDays(1);
        });

        public ICommand CommandGoCNPJ
        {
            get
            {
                if (_commandgocnpj == null)
                    _commandgocnpj = new DelegateCommand(ExecCommandGoCNPJ, null);
                return _commandgocnpj;
            }
        }

        private void ExecCommandGoCNPJ(object obj)
        {
            _viewcnpj = true;
            _flowtemp = FlowDoc;
            FlowDoc = null;
            FlowDoc = FlowPJ((string)obj);
        }

        public ICommand CommandRetCPF
        {
            get
            {
                if (_commandretcpf == null)
                    _commandretcpf = new DelegateCommand(ExecCommandRetCPF, null);
                return _commandretcpf;
            }
        }

        private void ExecCommandRetCPF(object obj)
        {
            _viewcnpj = false;
            FlowDoc = null;
            FlowDoc = _flowtemp;
            RetVisible = Visibility.Collapsed;
        }

        public ICommand CommandClosePreview
        {
            get
            {
                if (_commandclosepreview == null)
                    _commandclosepreview = new DelegateCommand(ExecCommandClosePreview, null);
                return _commandclosepreview;
            }
        }

        private void ExecCommandClosePreview(object obj)
        {
            PreviewBox = Visibility.Collapsed;
            FlowDoc = null;
            FlowDoc = new FlowDocument();
            _viewcnpj = false;
        }

        public ICommand CommandPreviewBox
        {
            get
            {
                if (_commandpreviewbox == null)
                    _commandpreviewbox = new DelegateCommand(ExecCommandPreviewBox, null);
                return _commandpreviewbox;
            }
        }

        private void ExecCommandPreviewBox(object obj)
        {
            try
            {
                AsyncApresentarDados(obj);
                PreviewBox = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        public ICommand CommandPreviewBox2
        {
            get
            {
                if (_commandpreviewbox2 == null)
                    _commandpreviewbox2 = new DelegateCommand(ExecCommandPreviewBox2, null);
                return _commandpreviewbox2;
            }
        }

        private void ExecCommandPreviewBox2(object obj)
        {
            try
            {
                AsyncFlowAtendimento((string)obj);
                PreviewBox = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        public ICommand CommandRefreshDate
        {
            get
            {
                if (_commandrefreshdate == null)
                    _commandrefreshdate = new DelegateCommand(ExecCommandRefreshDate, null);

                return _commandrefreshdate;
            }
        }

        private void ExecCommandRefreshDate(object obj)
        {
            //if (GlobalNavigation.Parametro != string.Empty)
                //AsyncListarAtendimentoHoje(Parametros("1"));
        }

        public ICommand CommandAgendaNavigate => new RelayCommand(p => { ns.Navigate(new Uri(p.ToString(), UriKind.RelativeOrAbsolute)); });

        public ICommand CommandEventoAtivo => new RelayCommand(p => { AsyncListarAgendaAtivo(); });

        public ICommand CommandEventoVencido => new RelayCommand(p => { AsyncListarAgendaVencida(); });

        public ICommand CommandEventoCancelado => new RelayCommand(p => { AsyncListarAgendaCancelada(); });

        public ICommand CommandEventoFinalizado => new RelayCommand(p => { AsyncListarAgendaFinalizado(); });

        public ICommand CommandDetalharEvento => new RelayCommand(p =>
        {
            AreaTransferencia.Evento = p.ToString();
            ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pDetalhe.xaml", UriKind.RelativeOrAbsolute));
        });

        public ICommand CommandEventoAlterar => new RelayCommand(p=> {

            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pEditar.xaml", UriKind.Relative));
                    AreaTransferencia.Parametro = (string)p;
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pEditar.xaml", UriKind.Relative));
                                AreaTransferencia.Parametro = (string)p;
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Sim.Alerta!"); }

        });
        #endregion

        #region Constructor
        public vmMain()
        {
            GlobalNavigation.Pagina = string.Empty;
            ns = GlobalNavigation.NavService;
            GlobalNavigation.SubModulo = "SALA DO EMPREENDEDOR";
            GlobalNavigation.Parametro = "1";

            BlackBox = Visibility.Collapsed;
            PreviewBox = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;
            StartProgress = false;
            AreaTransferencia.Limpar();
            //AsyncListarAtendimentoHoje(Parametros("1"));
            IsEnable = false;
            IsAdmin = false;
            CommandEventoAtivo.Execute(null);

            if (Logged.Acesso != 0)
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    IsEnable = true;
                    IsAdmin = true;
                }

                if (GlobalNavigation.SubModulo.ToLower() == "Sala do Empreendedor".ToLower())
                {

                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                                IsEnable = true;

                            if (m.Acesso > (int)SubModuloAccess.Operador)
                                IsAdmin = true;
                        }
                    }
                }
            }
        }
        #endregion

        #region Functions

        private List<string> Parametros(string param)
        {

            List<string> _param = new List<string>() { };

            _param.Add(DataI.ToShortDateString());

            _param.Add(Logged.Identificador);

            _param.Add(param);

            return _param;
        }

        private void AsyncListarAtendimentoHoje(List<string> sqlcommand)
        {
            //BlackBox = Visibility.Visible;
            //StartProgress = true;

            Task.Factory.StartNew(() => mdata.AtendimentosNow(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarAtendimentos = task.Result;
                        //BlackBox = Visibility.Collapsed;
                        //StartProgress = false;
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                });
        }

        public void AsyncListarAgenda()
        {

            List<string> _list = new List<string>();

            _list.Add(DateTime.Now.ToShortDateString());
            _list.Add("1");
            _list.Add("1");

            Task.Factory.StartNew(() => new mData().Agenda(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                });
        }

        public void AsyncListarAgendaAtivo()
        {

            List<string> _list = new List<string>();

            _list.Add(DateTime.Now.ToShortDateString());
            _list.Add("31/12/" + DateTime.Now.Year);

            _list.Add("1");
            _list.Add("1");

            _list.Add("0");
            _list.Add("99");

            _list.Add("1");
            _list.Add("1");


            Task.Factory.StartNew(() => mdata.Agenda(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void AsyncListarAgendaVencida()
        {

            List<string> _list = new List<string>();

            _list.Add("01/01/2010");
            _list.Add(DateTime.Now.ToShortDateString());

            _list.Add("1");
            _list.Add("1");

            _list.Add("0");
            _list.Add("99");

            _list.Add("1");
            _list.Add("1");

            //_list.Add(DateTime.Now.ToShortDateString());
            //_list.Add("1");

            Task.Factory.StartNew(() => mdata.Agenda(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void AsyncListarAgendaCancelada()
        {

            List<string> _list = new List<string>();

            _list.Add("01/01/2010");
            _list.Add("31/12/" + DateTime.Now.Year);

            _list.Add("3");
            _list.Add("3");

            _list.Add("0");
            _list.Add("99");

            _list.Add("1");
            _list.Add("1");

            //_list.Add("3");

            Task.Factory.StartNew(() => mdata.Agenda(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void AsyncListarAgendaFinalizado()
        {

            List<string> _list = new List<string>();

            _list.Add("01/01/2010");
            _list.Add(DateTime.Now.ToShortDateString());

            _list.Add("2");
            _list.Add("2");

            _list.Add("0");
            _list.Add("99");

            _list.Add("1");
            _list.Add("1");

            //_list.Add("2");

            Task.Factory.StartNew(() => mdata.Agenda(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncApresentarDados(object cliente)
        {
            string _cliente = new mMascaras().Remove((string)cliente);

            FlowDocument flow = new FlowDocument();

            new System.Threading.Thread(() =>
            {
                System.Windows.Threading.DispatcherOperation op =
                FlowDoc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(delegate () {

                        switch (_cliente.Length)
                        {
                            case 11:
                                FlowDoc = FlowPF(_cliente);
                                break;

                            case 14:
                                FlowDoc = FlowPJ(_cliente);
                                break;
                        }

                    }));

                op.Completed += (o, args) =>
                {
                    //BlackBox = Visibility.Collapsed;
                    //MainBox = Visibility.Collapsed;
                    //PrintBox = Visibility.Visible;
                    //StartProgress = false;
                };
            }).Start();
        }

        private void AsyncFlowAtendimento(string protocolo)
        {

            FlowDocument flow = new FlowDocument();

            new System.Threading.Thread(() =>
            {
                System.Windows.Threading.DispatcherOperation op =
                FlowDoc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(delegate () {

                        FlowDoc = FlowAtendimento(protocolo);

                    }));

                op.Completed += (o, args) =>
                {
                    //BlackBox = Visibility.Collapsed;
                    //MainBox = Visibility.Collapsed;
                    //PrintBox = Visibility.Visible;
                    //StartProgress = false;
                };
            }).Start();
        }

        private FlowDocument FlowPF(string cliente)
        {

            mPF_Ext pessoafisica = new mPF_Ext();
            pessoafisica = new mData().ExistPessoaFisica(cliente);

            FlowDocument flow = new FlowDocument();
            flow.Foreground = (Brush)Application.Current.Resources["WindowText"];
            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(10);

            Paragraph pr = new Paragraph();
            pr.Inlines.Add(new Run("CLIENTE:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}, {2} ", pessoafisica.Nome, new mMascaras().CPF(pessoafisica.CPF), pessoafisica.DataNascimento.ToShortDateString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CEP:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoafisica.CEP);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ENDEREÇO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}", pessoafisica.Logradouro, pessoafisica.Numero));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("BAIRRO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}-{2}", pessoafisica.Bairro, pessoafisica.Municipio, pessoafisica.UF));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoafisica.Telefones + " " + pessoafisica.Email);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("PERFIL:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoafisica.Perfil.PerfilString);
            pr.Inlines.Add(new LineBreak());

            if (pessoafisica.ColecaoVinculos.Count > 0)
                pr.Inlines.Add(new Run("EMPRESAS:") { FontSize = 10, Foreground = Brushes.Gray });


            foreach (mVinculos v in pessoafisica.ColecaoVinculos)
            {
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("CNPJ "));
                pr.Inlines.Add(new Hyperlink(new Run(new mMascaras().CNPJ(v.CNPJ))) { Command = CommandGoCNPJ, CommandParameter = v.CNPJ });
                pr.Inlines.Add(new Run(" - " + v.VinculoString));
            }

            flow.Blocks.Add(pr);


            return flow;
        }

        private FlowDocument FlowPJ(string cliente)
        {

            mPJ_Ext pessoajuridica = new mPJ_Ext();

            pessoajuridica = new mData().ExistPessoaJuridica(cliente);

            FlowDocument flow = new FlowDocument();
            flow.Foreground = (Brush)Application.Current.Resources["WindowText"];
            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(10);

            Paragraph pr = new Paragraph();
            pr.Inlines.Add(new Run("EMPRESA:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}, {2} ", new mMascaras().CNPJ(pessoajuridica.CNPJ), pessoajuridica.MatrizFilial, pessoajuridica.Abertura.ToShortDateString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ROZÃO SOCIAL") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.RazaoSocial);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("NOME FANTASIA") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.NomeFantasia);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ATIVIDADE PRINCIPAL") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.AtividadePrincipal);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ATIVIDADES SECUNDÁRIAS") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.AtividadeSecundaria);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("NATUREZA JURÍDICA") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.NaturezaJuridica);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("SITUAÇÃO CADASTRAL") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.SituacaoCadastral + " " + pessoajuridica.DataSituacaoCadastral.ToShortDateString());
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CEP:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.CEP);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ENDEREÇO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}", pessoajuridica.Logradouro, pessoajuridica.Numero));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("BAIRRO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}-{2} ", pessoajuridica.Bairro, pessoajuridica.Municipio, pessoajuridica.UF));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.Telefones + " " + pessoajuridica.Email);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("SETOR") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());

            if (pessoajuridica.Segmentos.Agronegocio)
                pr.Inlines.Add("AGRONEGÓCIO ");

            if (pessoajuridica.Segmentos.Industria)
                pr.Inlines.Add("INDÚSTRIA ");

            if (pessoajuridica.Segmentos.Comercio)
                pr.Inlines.Add("COMÉRCIO ");

            if (pessoajuridica.Segmentos.Servicos)
                pr.Inlines.Add("SERVIÇOS ");

            if (_viewcnpj == true)
            {
                RetVisible = Visibility.Visible;
                /*
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Hyperlink(new Run("VOLTAR")) { Command = CommandRetCPF });
                */
            }

            flow.Blocks.Add(pr);

            return flow;

        }

        private FlowDocument FlowAtendimento(string protocolo)
        {
            var atdo = new mAtendimento();

            atdo = new mData().Atendimento(protocolo);

            FlowDocument flow = new FlowDocument();

            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.PageHeight = 768;
            //flow.PageWidth = 1104;
            //flow.ColumnWidth = 1104;
            //flow.PageHeight = 11.5 * 96.0;
            flow.Foreground = (Brush)Application.Current.Resources["WindowText"];
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(10);

            Paragraph pr = new Paragraph();
            pr.Inlines.Add(new Run("ATENDIMENTO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1} {2} ", atdo.Protocolo, atdo.Data.ToShortDateString(), atdo.Hora.ToShortTimeString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CLIENTE") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}", atdo.Cliente.NomeRazao));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.Cliente.Telefones, atdo.Cliente.Email));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("TIPO E ORIGEM DO ATENDIMENTO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.TipoString, atdo.OrigemString));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("DESCRIÇÃO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}", atdo.Historico));
            pr.Inlines.Add(new LineBreak());

            flow.Blocks.Add(pr);

            return flow;
        }
        #endregion
    }
}
