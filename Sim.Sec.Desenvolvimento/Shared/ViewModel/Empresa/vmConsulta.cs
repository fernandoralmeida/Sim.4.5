using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Empresa
{
    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Account;

    class vmConsulta : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private ObservableCollection<mPJ_Ext> _listapj = new ObservableCollection<mPJ_Ext>();
        private ObservableCollection<mCNAE> _listacnae = new ObservableCollection<mCNAE>();

        private List<string> _filtros = new List<string>();

        private FlowDocument flwdoc = new FlowDocument();

        private DateTime _datai;
        private DateTime _dataf;

        private string _getcnpj = string.Empty;
        private string _getrazaosocial = string.Empty;
        private string _getcnae = string.Empty;
        private string _getsituacao = string.Empty;
        private string _getendereco = string.Empty;
        private string _getbairro = string.Empty;
        private int _getporte;
        private int _getlocal;

        private Visibility _blackbox;
        private Visibility _printbox;
        private Visibility _mainbox;

        private bool _starprogress;

        private bool _estabelecimentofixo;
        private bool _correspondencia;

        private bool? _segmentoagro;
        private bool? _segmentoindustria;
        private bool? _segmentocomercio;
        private bool? _segmentoservicos;

        private bool _cnae2;

        private ICommand _commandfiltrar;
        private ICommand _commandlimpar;
        private ICommand _commandprint;
        private ICommand _commandalterar;
        //private ICommand _commandstartprint;
        private ICommand _commandcloseprintbox;
        private ICommand _commandlistar;
        #endregion

        #region Properties

        public FlowDocument FlowDoc
        {
            get { return flwdoc; }
            set
            {
                flwdoc = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public ObservableCollection<mPJ_Ext> ListarPJ
        {
            get { return _listapj; }
            set
            {
                _listapj = value;
                RaisePropertyChanged("ListarPJ");
            }
        }

        public ObservableCollection<mCNAE> ListaCNAE
        {
            get { return _listacnae; }
            set { _listacnae = value; RaisePropertyChanged("ListaCNAE"); }
        }

        public ObservableCollection<mTiposGenericos> ListaPorte
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_PJ_Porte WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mTiposGenericos> ListaUsoLocal
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_PJ_UsoLocal WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public DateTime DataI
        {
            get { return _datai; }
            set { _datai = value; RaisePropertyChanged("DataI"); }
        }

        public DateTime DataF
        {
            get { return _dataf; }
            set { _dataf = value; RaisePropertyChanged("DataF"); }
        }

        public List<string> Filtros
        {
            get { return _filtros; }
            set
            {
                _filtros = value;
                RaisePropertyChanged("Filtros");
            }
        }

        public string GetCNPJ
        {
            get { return _getcnpj; }
            set
            {
                _getcnpj = value;
                RaisePropertyChanged("GetCNPJ");
            }
        }

        public string GetRazaoSocial
        {
            get { return _getrazaosocial; }
            set { _getrazaosocial = value; RaisePropertyChanged("GetRazaoSocial"); }
        }

        public int GetPorte
        {
            get { return _getporte; }
            set { _getporte = value; RaisePropertyChanged("GetPorte"); }
        }

        public int GetLocal
        {
            get { return _getlocal; }
            set { _getlocal = value; RaisePropertyChanged("GetLocal"); }
        }

        public string GetCNAE
        {
            get { return _getcnae; }
            set { _getcnae = value; RaisePropertyChanged("GetCNAE"); }
        }

        public string GetSituacao
        {
            get { return _getsituacao; }
            set
            {
                _getsituacao = value;
                RaisePropertyChanged("GetSituacao");
            }
        }

        public string GetEndereco
        {
            get { return _getendereco; }
            set
            {
                _getendereco = value;
                RaisePropertyChanged("GetEndereco");
            }
        }

        public string GetBairro
        {
            get { return _getbairro; }
            set
            {
                _getbairro = value;
                RaisePropertyChanged("GetBairro");
            }
        }

        public Visibility BlackBox
        {
            get { return _blackbox; }
            set
            {
                _blackbox = value;
                RaisePropertyChanged("BlackBox");
            }
        }

        public Visibility PrintBox
        {
            get { return _printbox; }
            set
            {
                _printbox = value;
                RaisePropertyChanged("PrintBox");
            }
        }

        public Visibility MainBox
        {
            get { return _mainbox; }
            set
            {
                _mainbox = value;
                RaisePropertyChanged("MainBox");
            }
        }

        public bool StartProgress
        {
            get { return _starprogress; }
            set
            {
                _starprogress = value;
                RaisePropertyChanged("StartProgress");
            }
        }

        public bool EstabelecimentoFixo
        {
            get { return _estabelecimentofixo; }
            set
            {
                _estabelecimentofixo = value;
                RaisePropertyChanged("EstabelecimentoFixo");
            }
        }

        public bool Correspondencia
        {
            get { return _correspondencia; }
            set
            {
                _correspondencia = value;
                RaisePropertyChanged("Correspondencia");
            }
        }

        public bool? SegmentoAgro
        {
            get { return _segmentoagro; }
            set
            {
                _segmentoagro = value;
                RaisePropertyChanged("SegmentoAgro");
            }
        }

        public bool? SegmentoIndustria
        {
            get { return _segmentoindustria; }
            set
            {
                _segmentoindustria = value;
                RaisePropertyChanged("SegmentoIndustria");
            }
        }

        public bool? SegmentoComercio
        {
            get { return _segmentocomercio; }
            set
            {
                _segmentocomercio = value;
                RaisePropertyChanged("SegmentoComercio");
            }
        }

        public bool? SegmentoServicos
        {
            get { return _segmentoservicos; }
            set
            {
                _segmentoservicos = value;
                RaisePropertyChanged("SegmentoServicos");
            }
        }

        public bool CNAE2
        {
            get { return _cnae2; }
            set
            {
                _cnae2 = value;
                RaisePropertyChanged("CNAE2");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandFiltrar
        {
            get
            {
                if (_commandfiltrar == null)
                    _commandfiltrar = new DelegateCommand(ExecCommandFiltrar, null);
                return _commandfiltrar;
            }
        }

        private void ExecCommandFiltrar(object obj)
        {
            try
            {
                AsyncListarEmpresas(Parametros());
                //ListarViabilidades = mdata.Viabilidades(Parametros());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public ICommand CommandLimpar
        {
            get
            {
                if (_commandlimpar == null)
                    _commandlimpar = new DelegateCommand(ExecCommandLimpar, null);
                return _commandlimpar;
            }
        }

        private void ExecCommandLimpar(object obj)
        {
            if (ListarPJ != null)
                ListarPJ.Clear();

            GetCNPJ = string.Empty;
            GetRazaoSocial = string.Empty;
            GetCNAE = string.Empty;
            GetBairro = string.Empty;
            GetEndereco = string.Empty;
            GetPorte = 0;
            GetLocal = 0;
            DataI = new DateTime(DateTime.Now.Year, 1, 1);
            DataF = DateTime.Now;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
        }

        public ICommand CommandPrint
        {
            get
            {
                if (_commandprint == null)
                    _commandprint = new RelayCommand(p => {

                        try
                        {
                            StartProgress = true;
                            BlackBox = Visibility.Visible;

                            // paginador
                            FlowDoc.PageHeight = 768;
                            FlowDoc.PageWidth = 1104;
                            FlowDoc.ColumnWidth = 1104;
                            IDocumentPaginatorSource idocument = FlowDoc as IDocumentPaginatorSource;
                            idocument.DocumentPaginator.ComputePageCountAsync();

                            //Now print using PrintDialog
                            var pd = new PrintDialog
                            {
                                UserPageRangeEnabled = true
                            };
                            pd.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

                            if (pd.ShowDialog().Value)
                                pd.PrintDocument(idocument.DocumentPaginator, "Printing....");

                            StartProgress = false;
                            BlackBox = Visibility.Collapsed;
                        }
                        catch (Exception ex)
                        {
                            StartProgress = false;
                            BlackBox = Visibility.Collapsed;
                            MessageBox.Show(ex.Message);
                        }

                    });
                return _commandprint;
            }
        }

        public ICommand CommandListar
        {
            get
            {
                if (_commandlistar == null)
                    _commandlistar = new RelayCommand(p => {
                        AsyncFlowDoc();
                    });
                return _commandlistar;
            }
        }

        public ICommand CommandClosePrintBox
        {
            get
            {
                if (_commandcloseprintbox == null)
                    _commandcloseprintbox = new DelegateCommand(ExecCommandClosePrintBox, null);
                return _commandcloseprintbox;
            }
        }

        private void ExecCommandClosePrintBox(object obj)
        {
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
        }

        public ICommand CommandAlterar
        {
            get
            {
                if (_commandalterar == null)
                    _commandalterar = new DelegateCommand(ExecCommandAlterar, null);
                return _commandalterar;
            }
        }

        private void ExecCommandAlterar(object obj)
        {
            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    string identificador = new mMascaras().Remove(obj.ToString());
                    ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                    AreaTransferencia.CNPJ = identificador;
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                string identificador = new mMascaras().Remove(obj.ToString());
                                ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                                AreaTransferencia.CNPJ = identificador;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Constructor
        public vmConsulta()
        {
            //AsyncCNAE();
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "CONSULTA EMPRESAS";
            DataI = new DateTime(2013, 1, 1);
            DataF = DateTime.Now;
            //ListarPJ = new mData().ListaPJ(@"SELECT * FROM SDT_SE_PJ WHERE (Ativo = true)");
            MainBox = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            PrintBox = Visibility.Collapsed;
            StartProgress = false;
            Correspondencia = true;
            EstabelecimentoFixo = true;
        }
        #endregion

        #region Functions
        private List<object> Parametros()
        {

            List<object> _param = new List<object>() { };

            if (GetRazaoSocial == string.Empty || GetRazaoSocial == null)
                _param.Add("%");//0
            else
                _param.Add(GetRazaoSocial);//0

            if (GetCNPJ == string.Empty || GetCNPJ == null)
                _param.Add("%");//1
            else
                _param.Add(new mMascaras().Remove(GetCNPJ));//1

            if (GetCNAE == string.Empty)
                _param.Add("%");//2
            else
                _param.Add(GetCNAE);//2

            if (CNAE2 == true)
                _param.Add(GetCNAE);//3
            else
                _param.Add("%");//3

            _param.Add(DataI.ToShortDateString());//4
            _param.Add(DataF.ToShortDateString());//5

            if (SegmentoAgro == true || SegmentoAgro == false)
                _param.Add(SegmentoAgro);//6
            else
                _param.Add("%");//6

            if (SegmentoIndustria == true || SegmentoIndustria == false)
                _param.Add(SegmentoIndustria);//7
            else
                _param.Add("%");//7

            if (SegmentoComercio == true || SegmentoComercio == false)
                _param.Add(SegmentoComercio);//8
            else
                _param.Add("%");//8

            if (SegmentoServicos == true || SegmentoServicos == false)
                _param.Add(SegmentoServicos);//9
            else
                _param.Add("%");//9

            if (GetPorte == 0)
                _param.Add("%");//10
            else
                _param.Add(GetPorte);//10

            if (EstabelecimentoFixo && Correspondencia)
            {
                _param.Add("%");//11
            }
            else
            if (!EstabelecimentoFixo && !Correspondencia)
            {
                _param.Add("0");//11
            }
            else
            if (EstabelecimentoFixo && !Correspondencia)
            {
                _param.Add("1");//11
            }
            else
            if (!EstabelecimentoFixo && Correspondencia)
            {
                _param.Add("2");//11
            }

            if (GetSituacao == "...")
                _param.Add("%");//12
            else
                _param.Add(GetSituacao);//12

            if (GetEndereco == string.Empty || GetEndereco == null)
                _param.Add("%");//13
            else
                _param.Add(GetEndereco);//13

            if (GetBairro == string.Empty || GetBairro == null)
                _param.Add("%");//14
            else
                _param.Add(GetBairro);//14

            return _param;
        }

        private void AsyncCNAE()
        {

            string sql = @"SELECT * FROM SDT_SE_PJ_CNAE_MEI WHERE (Ativo = True) ORDER BY Ocupacao";

            Task.Factory.StartNew(() => new mData().CNAES(sql))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListaCNAE = task.Result;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncListarEmpresas(List<object> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            Task.Factory.StartNew(() => mdata.nEmpresas(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarPJ = task.Result;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncFlowDoc()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            /*
            new System.Threading.Thread(() =>
            {
                System.Windows.Threading.DispatcherOperation op =
                FlowDoc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(delegate () {
                        FlowDoc = PreviewInTable(ListarPJ);
                    }));

                op.Completed += (o, args) =>
                {
                    BlackBox = Visibility.Collapsed;
                    MainBox = Visibility.Collapsed;
                    PrintBox = Visibility.Visible;
                    StartProgress = false;
                };
            }).Start();*/

            Task.Factory.StartNew(() => mdata.nEmpresas(Parametros())).ContinueWith(task =>
            {

                Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    FlowDoc = PreviewInTable(ListarPJ);
                }));

                if (task.IsCompleted)
                {
                    ListarPJ = task.Result;
                    BlackBox = Visibility.Collapsed;
                    MainBox = Visibility.Collapsed;
                    PrintBox = Visibility.Visible;
                    StartProgress = false;
                }

            });
        }

        private FlowDocument PreviewPrint(ObservableCollection<mPJ_Ext> obj)
        {
            FlowDocument flow = new FlowDocument();

            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            flow.ColumnWidth = 8.5 * 96.0;
            flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(40, 40, 40, 60);

            Paragraph pH = new Paragraph(new Run(new mFlowHeader().NameOrg));
            pH.Typography.Capitals = FontCapitals.SmallCaps;
            pH.FontSize = 20;
            pH.FontWeight = FontWeights.Bold;
            pH.Margin = new Thickness(0);

            Paragraph pH1 = new Paragraph(new Run(new mFlowHeader().SloganOrg));
            pH1.FontSize = 9;
            pH1.Margin = new Thickness(1, 0, 0, 0);

            Paragraph pH2 = new Paragraph(new Run(new mFlowHeader().DepOrg));
            pH2.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.FontWeight = FontWeights.Bold;
            pH2.FontSize = 14;
            pH2.Margin = new Thickness(0, 10, 0, 0);

            Paragraph pH3 = new Paragraph(new Run(new mFlowHeader().SetorOrg));
            pH3.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.FontWeight = FontWeights.Bold;
            pH3.FontSize = 12;
            pH3.Margin = new Thickness(0, 0, 0, 40);

            flow.Blocks.Add(pH);
            flow.Blocks.Add(pH1);
            flow.Blocks.Add(pH2);
            flow.Blocks.Add(pH3);

            string f = string.Empty;
            foreach (string filtro in Filtros)
                f += filtro;

            Figure lfiltro = new Figure();
            lfiltro.Width = new FigureLength(flow.PageHeight);
            lfiltro.Height = new FigureLength(1);
            lfiltro.Background = Brushes.Gray;
            lfiltro.Margin = new Thickness(0, 0, 0, 0);

            Paragraph ft = new Paragraph();
            ft.Margin = new Thickness(0, 0, 0, 0);
            ft.FontSize = 10;
            ft.Inlines.Add(new Run("FILTROS: "));
            ft.Inlines.Add(new Run(f));

            Paragraph ft1 = new Paragraph();
            ft1.Margin = new Thickness(0, 0, 0, 0);
            ft1.FontSize = 10;
            ft1.Inlines.Add(new Run("[REG. ENCONTRADO(S): "));
            ft1.Inlines.Add(new Bold(new Run(obj.Count.ToString())));
            ft1.Inlines.Add(new Run("]"));

            flow.Blocks.Add(ft);
            flow.Blocks.Add(ft1);
            //flow.Blocks.Add(new Paragraph(lfiltro) { Margin = new Thickness(0, 0, 0, 10) });

            foreach (mPJ_Ext a in obj)
            {

                Paragraph pr = new Paragraph();
                pr.Inlines.Add(new Run("REGISTRO: " + a.Contador) { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(string.Format("CNPJ {0}, {1}, {2} ", new mMascaras().CNPJ(a.CNPJ), a.MatrizFilial, a.Abertura.ToShortDateString()));
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("ROZÃO SOCIAL") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.RazaoSocial);
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("NOME FANTASIA") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.NomeFantasia);
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("ATIVIDADE PRINCIPAL") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.AtividadePrincipal);
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("ATIVIDADES SECUNDÁRIAS") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.AtividadeSecundaria);
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("NATUREZA JURÍDICA") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.NaturezaJuridica);
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("SITUAÇÃO CADASTRAL") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.SituacaoCadastral + " " + a.DataSituacaoCadastral.ToShortDateString());
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("END:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(string.Format("{0}, {1}, {2}, {3}, {4}-{5} ", a.CEP, a.Logradouro, a.Numero, a.Bairro, a.Municipio, a.UF));
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("CONTATO:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.Telefones + " " + a.Email);
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("SETOR") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());

                if (a.Segmentos.Agronegocio)
                    pr.Inlines.Add("AGRONEGÓCIO ");

                if (a.Segmentos.Industria)
                    pr.Inlines.Add("INDÚSTRIA ");

                if (a.Segmentos.Comercio)
                    pr.Inlines.Add("COMÉRCIO ");

                if (a.Segmentos.Servicos)
                    pr.Inlines.Add("SERVIÇOS ");

                pr.Inlines.Add(new LineBreak());

                flow.Blocks.Add(pr);
            }


            Figure lrodape = new Figure();
            lrodape.Width = new FigureLength(1000);
            lrodape.Height = new FigureLength(1);
            lrodape.Background = Brushes.Gray;
            lrodape.Margin = new Thickness(0, 10, 0, 0);

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 0, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            //r.Inlines.Add(lrodape);
            r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
            r.Inlines.Add(new Run(" / V" + version + " / "));
            r.Inlines.Add(new Bold(new Run(Account.Logged.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));


            flow.Blocks.Add(r);

            return flow;
        }

        private FlowDocument PreviewInTable(ObservableCollection<mPJ_Ext> obj)
        {

            FlowDocument flow = new FlowDocument();

            flow.ColumnGap = 0;
            flow.Background = Brushes.White;
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.ColumnWidth =  96.0 * 8.5;
            //flow.PageHeight = 11.5 * 96.0;
            //flow.PageWidth = 8.5 * 96.0;
            flow.PageHeight = 768;
            flow.PageWidth = 1104;
            flow.ColumnWidth = 1104;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 11;
            flow.PagePadding = new Thickness(40, 20, 40, 20);
            flow.TextAlignment = TextAlignment.Left;

            Paragraph pH = new Paragraph(new Run(new mFlowHeader().NameOrg));
            pH.Typography.Capitals = FontCapitals.SmallCaps;
            pH.Foreground = Brushes.Black;
            pH.FontSize = 16;
            pH.FontWeight = FontWeights.Bold;
            pH.Margin = new Thickness(0);

            Paragraph pH1 = new Paragraph(new Run(new mFlowHeader().SloganOrg));
            pH1.Foreground = Brushes.Black;
            pH1.FontSize = 9;
            pH1.Margin = new Thickness(1, 0, 0, 0);

            Paragraph pH2 = new Paragraph(new Run(new mFlowHeader().DepOrg));
            pH2.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.Foreground = Brushes.Black;
            pH2.FontWeight = FontWeights.Bold;
            pH2.FontSize = 12;
            pH2.Margin = new Thickness(0, 10, 0, 0);

            Paragraph pH3 = new Paragraph(new Run(new mFlowHeader().SetorOrg));
            pH3.Typography.Capitals = FontCapitals.SmallCaps;
            pH3.FontWeight = FontWeights.Bold;
            pH3.Foreground = Brushes.Black;
            pH3.Margin = new Thickness(0, 0, 0, 20);

            flow.Blocks.Add(pH);
            flow.Blocks.Add(pH1);
            flow.Blocks.Add(pH2);
            flow.Blocks.Add(pH3);

            /*
            var lfiltro = new Rectangle();
            lfiltro.Stroke = new SolidColorBrush(Colors.Silver);
            lfiltro.StrokeThickness = 1;
            lfiltro.Height = 1;
            lfiltro.Width = double.NaN;
            lfiltro.Margin = new Thickness(0, 0, 0, 0);

            var linefiltro = new BlockUIContainer(lfiltro);
            flow.Blocks.Add(linefiltro);            
            */

            //SolidColorBrush bgc1 = Application.Current.Resources["ButtonBackgroundHover"] as SolidColorBrush;

            Table tb = new Table();
            tb.CellSpacing = 0;
            tb.BorderThickness = new Thickness(0.5);
            tb.BorderBrush = Brushes.Black;

            tb.Columns.Add(new TableColumn() { Width = new GridLength(50) });
            tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            //tb.Columns.Add(new TableColumn() { Width = new GridLength(200) });

            //tb.Columns.Add(new TableColumn() { Width = new GridLength(10, GridUnitType.Star) }); 

            TableRowGroup rg = new TableRowGroup();

            string f = string.Empty;
            foreach (string filtro in Filtros)
                f += filtro;

            TableRow pheader = new TableRow();
            pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("FILTROS: {0}", f))) { Padding = new Thickness(5) }) { ColumnSpan = 4, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("REGISTROS: {0}", obj.Count))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

            tb.RowGroups.Add(rg);

            Paragraph p = new Paragraph();
            p.Padding = new Thickness(5);

            TableRow header = new TableRow();
            rg.Rows.Add(pheader);
            rg.Rows.Add(header);
            rg.Foreground = Brushes.Black;
            //rw2.Background = bgc1;
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(""))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("RAZÃO SOCIAL"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("CNPJ"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("TELEFONE"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("EMAIL"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            //header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ASSINATURA"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(1, 0, 0, 0), BorderBrush = Brushes.Black });

            int alt = 0;

            if (obj != null)
            {

                foreach (mPJ_Ext a in obj)
                {

                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Contador.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.RazaoSocial)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(new mMascaras().CNPJ(a.CNPJ))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Telefones)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Email)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    //row.Cells.Add(new TableCell(new Paragraph(new Run("")) { Padding = new Thickness(5, 20, 5, 20) }) { BorderThickness = new Thickness(1, 1, 0, 0), BorderBrush = Brushes.Black });
                    /*
                    if (alt % 2 != 0)
                        row.Background = bgc1;
                        */
                    alt++;
                }
            }

            flow.Blocks.Add(tb);
            /*
            var lrodape = new Rectangle();
            lrodape.Stroke = new SolidColorBrush(Colors.Silver);
            lrodape.StrokeThickness = 1;
            lrodape.Height = 1;
            lrodape.Width = double.NaN;
            lrodape.Margin = new Thickness(0, 10, 0, 0);

            var lineBlock = new BlockUIContainer(lrodape);
            flow.Blocks.Add(lineBlock);
            */

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 0, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            //r.Inlines.Add(lrodape);
            r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
            r.Inlines.Add(new Run(" / V" + version + " / "));
            r.Inlines.Add(new Bold(new Run(Account.Logged.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));


            flow.Blocks.Add(r);

            return flow;
        }
        #endregion
    }
}
