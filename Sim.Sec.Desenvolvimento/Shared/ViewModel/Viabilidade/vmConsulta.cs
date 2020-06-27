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

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Viabilidade
{
    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Account;

    class vmConsulta : NotifyProperty
    {
        #region Declarations
        private mData mdata = new mData();

        private ObservableCollection<mViabilidade> _lista = new ObservableCollection<mViabilidade>();
        private List<string> _filtros = new List<string>();

        private FlowDocument _flowdoc = new FlowDocument();
        private FlowDocument _flowdocp = new FlowDocument();
        private FlowDocument _flowmail = new FlowDocument();

        private DateTime _datai;
        private DateTime _dataf;

        private string _getprotocolo;
        private string _getlogradouro;
        private string _descricaomotivo;
        private string _viabilidade;
        private string _getrequerente;

        private int _getsituacao;
        private int _indiceselecionado;
        private int _seletedsituacao;

        private Visibility _blackbox;
        private Visibility _previewbox;
        private Visibility _emailbox;
        private Visibility _printbox;
        private Visibility _mainbox;

        private ICommand _commandfiltrar;
        private ICommand _commandlimpar;
        private ICommand _commandprint;
        private ICommand _commandatualizarparecer;
        private ICommand _commandminhasviabilidades;
        private ICommand _commandclosepreview;
        private ICommand _commandpreviewbox;
        private ICommand _commandviewemail;
        private ICommand _commandcloseprintbox;
        private ICommand _commandlistar;

        private bool _starprogress;

        #endregion

        #region Properties
        public FlowDocument FlowDoc
        {
            get { return _flowdoc; }
            set
            {
                _flowdoc = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public FlowDocument FlowDocP
        {
            get { return _flowdocp; }
            set
            {
                _flowdocp = value;
                RaisePropertyChanged("FlowDocP");
            }
        }

        public FlowDocument FlowMail
        {
            get { return _flowmail; }
            set
            {
                _flowmail = value;
                RaisePropertyChanged("FlowMail");
            }
        }

        public ObservableCollection<mViabilidade> ListarViabilidades
        {
            get { return _lista; }
            set
            {
                _lista = value;
                RaisePropertyChanged("ListarViabilidades");
            }
        }

        public ObservableCollection<mTiposGenericos> Situacoes
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_Viabilidade_Situacao WHERE (Ativo = True) ORDER BY Valor"); }
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

        public Visibility BlackBox
        {
            get { return _blackbox; }
            set
            {
                _blackbox = value;
                RaisePropertyChanged("BlackBox");
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

        public Visibility EmailBox
        {
            get { return _emailbox; }
            set
            {
                _emailbox = value;
                RaisePropertyChanged("EmailBox");
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

        public int IndiceSelecionado
        {
            get { return _indiceselecionado; }
            set
            {
                _indiceselecionado = value;
                RaisePropertyChanged("IndiceSelecionado");
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

        public int GetSituacao
        {
            get { return _getsituacao; }
            set { _getsituacao = value; RaisePropertyChanged("GetSituacao"); }
        }

        public int SelectedSituacao
        {
            get { return _seletedsituacao; }
            set { _seletedsituacao = value; RaisePropertyChanged("SelectedSituacao"); }
        }

        public string Viabilidade
        {
            get { return _viabilidade; }
            set
            {
                _viabilidade = value;
                RaisePropertyChanged("Viabilidade");
            }
        }

        public string GetProtocolo
        {
            get { return _getprotocolo; }
            set { _getprotocolo = value; RaisePropertyChanged("GetProtocolo"); }
        }

        public string GetLogradouro
        {
            get { return _getlogradouro; }
            set { _getlogradouro = value; RaisePropertyChanged("GetLogradouro"); }
        }

        public string GetRequerente
        {
            get { return _getrequerente; }
            set
            {
                _getrequerente = value;
                RaisePropertyChanged("GetRequerente");
            }
        }

        public string DescricaoMotivo
        {
            get { return _descricaomotivo; }
            set
            {
                _descricaomotivo = value;
                RaisePropertyChanged("DescricaoMotivo");
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
                AsyncListarViabilidade(Parametros());
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
            if (ListarViabilidades != null)
                ListarViabilidades.Clear();

            GetLogradouro = string.Empty;
            GetProtocolo = string.Empty;
            GetSituacao = 0;
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
                            var pd = new PrintDialog();
                            pd.UserPageRangeEnabled = true;
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

        public ICommand CommandMinhasViabilidades
        {
            get
            {
                if (_commandminhasviabilidades == null)
                    _commandminhasviabilidades = new DelegateCommand(ExecCommandMinhasViabilidades, null);
                return _commandminhasviabilidades;
            }
        }

        private void ExecCommandMinhasViabilidades(object obj)
        {
            try
            {
                AsyncListarViabilidadeOperador(Parametros());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public ICommand CommandAtualizarParecer
        {
            get
            {
                if (_commandatualizarparecer == null)
                    _commandatualizarparecer = new DelegateCommand(ExecCommandAtualizarParecer, null);
                return _commandatualizarparecer;
            }
        }

        private void ExecCommandAtualizarParecer(object obj)
        {
            try
            {

                if (mdata.AtualizarViabilidade(obj))
                {
                    MessageBox.Show("Viabilidade Atualizada!", "Sim.Alerta!");

                    PreviewBox = Visibility.Collapsed;
                    BlackBox = Visibility.Collapsed;
                    EmailBox = Visibility.Collapsed;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

            if (Logged.Acesso == (int)AccountAccess.Master)
            {
                FlowDocP = CreateFlow((string)obj);
                PreviewBox = Visibility.Visible;
                BlackBox = Visibility.Collapsed;
            }
            else
            {
                foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                {
                    if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                    {
                        if (m.Acesso > (int)SubModuloAccess.Consulta)
                        {
                            FlowDocP = CreateFlow((string)obj);
                            PreviewBox = Visibility.Visible;
                            BlackBox = Visibility.Collapsed;
                        }
                    }
                }
            }

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
            BlackBox = Visibility.Collapsed;
            EmailBox = Visibility.Collapsed;
        }

        public ICommand CommandViewEmail
        {
            get
            {
                if (_commandviewemail == null)
                    _commandviewemail = new DelegateCommand(ExecCommandViewEmail, null);
                return _commandviewemail;
            }
        }

        private void ExecCommandViewEmail(object obj)
        {
            Viabilidade = ListarViabilidades[IndiceSelecionado].Protocolo;
            FlowMail = FlowEmail((string)obj);
            EmailBox = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
        }
        #endregion

        #region Constructor

        public vmConsulta()
        {
            GlobalNavigation.Pagina = "CONSULTA VIABILIDADES";
            DataI = new DateTime(DateTime.Now.Year, 1, 1);
            DataF = DateTime.Now;
            MainBox = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            PrintBox = Visibility.Collapsed;
            PreviewBox = Visibility.Collapsed;
            EmailBox = Visibility.Collapsed;
            StartProgress = false;
        }

        #endregion

        #region Functions

        private List<string> Parametros()
        {

            List<string> _param = new List<string>() { };

            _param.Add(DataI.ToShortDateString());
            _param.Add(DataF.ToShortDateString());

            if (GetProtocolo == string.Empty || GetProtocolo == null)
                _param.Add("%");
            else
                _param.Add(GetProtocolo);

            if (GetLogradouro == string.Empty || GetLogradouro == null)
                _param.Add("%");
            else
                _param.Add(GetLogradouro);

            if (GetSituacao < 1)
            {
                _param.Add("0");
                _param.Add("99");
            }
            else
            {
                _param.Add(GetSituacao.ToString());
                _param.Add(GetSituacao.ToString());
            }

            if (GetRequerente == string.Empty || GetRequerente == null)
                _param.Add("%");
            else
                _param.Add(GetRequerente);

            _param.Add(Account.Logged.Identificador);

            return _param;
        }

        private void AsyncListarViabilidade(List<string> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            Task.Factory.StartNew(() => mdata.Viabilidades(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarViabilidades = task.Result;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                });
        }

        private void AsyncListarViabilidadeOperador(List<string> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            Task.Factory.StartNew(() => mdata.ViabilidadesOperador(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarViabilidades = task.Result;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                });
        }

        private FlowDocument CreateFlow(string obj)
        {

            var atdo = new mViabilidade();

            atdo = new mData().Viabilidade(obj);

            Viabilidade = atdo.Protocolo;
            SelectedSituacao = atdo.Perecer;
            DescricaoMotivo = atdo.Motivo;

            FlowDocument flow = new FlowDocument();

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
            pr.Inlines.Add(new Run("VIABILIDADE:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.Protocolo, atdo.Data.ToShortDateString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CLIENTE") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.Requerente.NomeRazao, atdo.Requerente.Inscricao));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.Requerente.Telefones, atdo.Requerente.Email));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("DADOS DA VIABILIDADE") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}", atdo.TextoEmail));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("PARECER") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.PerecerString, atdo.Motivo));
            pr.Inlines.Add(new LineBreak());

            flow.Blocks.Add(pr);

            return flow;
        }

        private FlowDocument FlowEmail(string obj)
        {
            FlowDocument flow = new FlowDocument();
            flow.Background = Brushes.White;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(5);

            Paragraph r = new Paragraph();
            r.Inlines.Add(obj);

            flow.Blocks.Add(r);

            return flow;
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
                        FlowDoc = PreviewInTable(ListarViabilidades);
                    }));

                op.Completed += (o, args) =>
                {
                    BlackBox = Visibility.Collapsed;
                    MainBox = Visibility.Collapsed;
                    PrintBox = Visibility.Visible;
                    StartProgress = false;
                };
            }).Start();*/

            Task.Factory.StartNew(() => mdata.Viabilidades(Parametros())).ContinueWith(task =>
            {

                Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    FlowDoc = PreviewInTable(ListarViabilidades);
                }));

                if (task.IsCompleted)
                {
                    ListarViabilidades = task.Result;
                    BlackBox = Visibility.Collapsed;
                    MainBox = Visibility.Collapsed;
                    PrintBox = Visibility.Visible;
                    StartProgress = false;
                }

            });
        }

        private FlowDocument PreviewPrint(ObservableCollection<mViabilidade> obj)
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

            foreach (mViabilidade a in obj)
            {

                Paragraph pr = new Paragraph();
                pr.Inlines.Add(new Run("VIABILIDADE: " + a.Contador) { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(string.Format("{0}, {1}", a.Protocolo, a.Data.ToShortDateString()));
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("CLIENTE:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(string.Format("{0}, {1}, {2} ", a.Requerente.NomeRazao, a.Requerente.Inscricao, a.Requerente.Telefones));
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("END:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(string.Format("{0}, {1}, {2}, {3} ", a.CEP, a.Logradouro, a.Numero, a.Bairro));
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("CTM:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.CTM);
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("ATIVIDADES:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.Atividades);
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("SITUAÇÃO:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.PerecerString);
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("HISTÓRICO:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run(a.Motivo));

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

        private FlowDocument PreviewInTable(ObservableCollection<mViabilidade> obj)
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
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
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
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("VIABILIDADE"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("CLIENTE"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("TELEFONE"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("..."))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            //header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ASSINATURA"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(1, 0, 0, 0), BorderBrush = Brushes.Black });

            int alt = 0;

            if (obj != null)
            {

                foreach (mViabilidade a in obj)
                {

                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Contador.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Protocolo)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Requerente.NomeRazao + "\n\n" + a.CTM + "\n" + a.Logradouro + " " + a.Numero + "\n" + a.Atividades)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Requerente.Telefones)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.PerecerString)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
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
