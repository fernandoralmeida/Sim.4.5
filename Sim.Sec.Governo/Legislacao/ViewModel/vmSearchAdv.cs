using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Sim.Sec.Governo.Legislacao.ViewModel
{

    using Sim.Mvvm.Commands;
    using Sim.Mvvm.Observers;
    using Model;
    using Providers;
    using Account;

    class vmSearchAdv : NotifyProperty
    {
        #region Declarations
        mDataSearch DataSearch = new mDataSearch();

        BackgroundWorker bgWorker = new BackgroundWorker();

        private ObservableCollection<mLegislacaoConsulta> _listalegislativa = new ObservableCollection<mLegislacaoConsulta>();

        FlowDocument flowDocument = new FlowDocument();
        FixedDocument fixedDocument = new FixedDocument();

        private IDocumentPaginatorSource _documentpreview;

        private List<mTiposGenericos> _tipos = new List<mTiposGenericos>();
        private List<mTiposGenericos> _classificacao;
        private List<mTiposGenericos> _situacao = new List<mTiposGenericos>();
        private List<mTiposGenericos> _origem = new List<mTiposGenericos>();
        private List<string> _filters = new List<string>();

        private Visibility _blackbox;
        private Visibility _editbox;
        private Visibility _mainbox;
        private Visibility _viewprint;

        private DateTime _datainicial;
        private DateTime _datafinal;

        private bool _startprogress;
        private bool _printmode;

        private string _selectedvalue;
        private string _fileXps = @"RELL";

        private List<object> _listconsult = new List<object>();

        private TypeSearch _consulta;

        private int _progressvalue;

        private int _cnumero;
        private string _ctipo;
        private string _cclassificacao;
        private string _corigem;
        private string _csituacao;
        private string _cautor;
        private string _cresumo;

        private ICommand _commandsearch;
        private ICommand _commandprint;
        private ICommand _commandcancel;
        private ICommand _commandclear;
        private ICommand _commandshowpdf;
        private ICommand _commandEdit;
        private ICommand _commandcloseprint;
        #endregion

        #region Properties

        public int cNumero
        {
            get { return _cnumero; }
            set
            {
                _cnumero = value;
                RaisePropertyChanged("cNumero");
            }
        }

        public string cTipo
        {
            get { return _ctipo; }
            set
            {
                _ctipo = value;
                RaisePropertyChanged("cTipo");
            }
        }

        public string cClassificacao
        {
            get { return _cclassificacao; }
            set
            {
                _cclassificacao = value;
                RaisePropertyChanged("cClassificacao");
            }
        }

        public string cOrigem
        {
            get { return _corigem; }
            set
            {
                _corigem = value;
                RaisePropertyChanged("cOrigem");
            }
        }

        public string cAutor
        {
            get { return _cautor; }
            set
            {
                _cautor = value;
                RaisePropertyChanged("cAutor");
            }
        }

        public string cResumo
        {
            get { return _cresumo; }
            set
            {
                _cresumo = value;
                RaisePropertyChanged("cResumo");
            }
        }

        public string cSituacao
        {
            get { return _csituacao; }
            set
            {
                _csituacao = value;
                RaisePropertyChanged("cSituacao");
            }
        }

        public IDocumentPaginatorSource DocumentPreview
        {
            get { return _documentpreview; }
            set
            {
                _documentpreview = value;
                RaisePropertyChanged("DocumentPreview");
            }
        }

        private TypeSearch TipoConsulta
        {
            get { return _consulta; }
            set
            {
                _consulta = value;
                RaisePropertyChanged("TipoConsulta");
            }
        }

        public List<mTiposGenericos> Tipos
        {
            get { return _tipos; }
            set
            {
                _tipos = value;
                RaisePropertyChanged("Tipos");
            }
        }

        public List<mTiposGenericos> Classificacao
        {
            get { return _classificacao; }
            set
            {
                _classificacao = new List<mTiposGenericos>();
                _classificacao = value;
                RaisePropertyChanged("Classificacao");
            }
        }

        public List<mTiposGenericos> Origem
        {
            get { return _origem; }
            set
            {
                _origem = value;
                RaisePropertyChanged("Origem");
            }
        }

        public List<mTiposGenericos> Situacao
        {
            get { return _situacao; }
            set
            {
                _situacao = value;
                RaisePropertyChanged("Origem");
            }
        }

        public List<string> Filters
        {
            get { return _filters; }
            set
            {
                _filters = value;
                RaisePropertyChanged("Filters");
            }
        }

        public ObservableCollection<mLegislacaoConsulta> ListaLegislativa
        {
            get { return _listalegislativa; }
            set
            {
                _listalegislativa = value;
                RaisePropertyChanged("ListaLegislativa");
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

        public Visibility ViewPrint
        {
            get { return _viewprint; }
            set
            {
                _viewprint = value;
                RaisePropertyChanged("ViewPrint");
            }
        }

        public DateTime DataInicial
        {
            get { return _datainicial; }
            set
            {
                _datainicial = value;
                RaisePropertyChanged("DataInicial");
            }
        }

        public DateTime DataFinal
        {
            get { return _datafinal; }
            set
            {
                _datafinal = value;
                RaisePropertyChanged("DataFinal");
            }
        }

        public bool PrintMode
        {
            get { return _printmode; }
            set
            {
                _printmode = value;
                RaisePropertyChanged("PrintMode");
            }
        }

        public bool StartProgress
        {
            get { return _startprogress; }
            set
            {
                _startprogress = value;
                RaisePropertyChanged("StartProgress");
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

        public Visibility EditBox
        {
            get { return _editbox; }
            set
            {
                _editbox = value;
                RaisePropertyChanged("EditBox");
            }
        }

        public string SelectedValue
        {
            get { return _selectedvalue; }
            set
            {
                _selectedvalue = value;

                if (value == "0")
                    this.Classificacao = new mListaTiposGenericos().GoList(null);

                if (value == "1" || value == "2")
                    this.Classificacao = new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Class_L_Only_Non_Blocked);

                if (value == "3")
                    this.Classificacao = new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Class_D_Only_Non_Blocked);

                cClassificacao = "0";

                RaisePropertyChanged("SelectedType");
            }
        }

        public int ProgressValue
        {
            get { return _progressvalue; }
            set
            {
                _progressvalue = value;
                RaisePropertyChanged("ProgressValue");
            }
        }

        #endregion

        #region Commands

        public ICommand CommandSearch
        {
            get
            {
                if (_commandsearch == null)
                    _commandsearch = new DelegateCommand(ExecuteCommandSearch, null);

                return _commandsearch;
            }
        }

        public ICommand CommandEdit
        {
            get
            {
                if (_commandEdit == null)
                    _commandEdit = new DelegateCommand(ExecuteCommandEdit, null);

                return _commandEdit;
            }
        }

        public ICommand CommandShowPDF
        {
            get
            {
                if (_commandshowpdf == null)
                    _commandshowpdf = new DelegateCommand(ExecuteCommandShowPDF, null);
                return _commandshowpdf;
            }
        }

        public ICommand CommandPrint
        {
            get
            {
                if (_commandprint == null)
                    _commandprint = new DelegateCommand(ExecuteCommandPrint, null);

                return _commandprint;
            }
        }

        public ICommand CommandClosePrint
        {
            get
            {
                if (_commandcloseprint == null)
                    _commandcloseprint = new DelegateCommand(ExecuteCommandClosePrint, null);

                return _commandcloseprint;
            }
        }

        public ICommand CommandClear
        {
            get
            {
                if (_commandclear == null)
                    _commandclear = new DelegateCommand(ExecuteCommandClear, null);

                return _commandclear;
            }
        }

        public ICommand CommandCancel
        {
            get
            {
                if (_commandcancel == null)
                    _commandcancel = new DelegateCommand(ExecuteCommandCancel, null);

                return _commandcancel;
            }
        }

        #endregion

        #region Construtor
        /// <summary>
        /// Start Instance of vmSearch
        /// </summary>
        public vmSearchAdv()
        {
            TipoConsulta = TypeSearch.Detailed;

            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

            this.Tipos = new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Tipo_Only_Non_Blocked);
            this.Origem = new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Origem_Only_Non_Blocked);
            this.Situacao = new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Situacao_Only_Non_Blocked);

            StartProgress = false;
            BlackBox = Visibility.Collapsed;
            EditBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            ViewPrint = Visibility.Collapsed;

            cTipo = "...";
            cOrigem = "0";
            cSituacao = "0";

            DataInicial = new DateTime(1853, 8, 15);
            DataFinal = DateTime.Now;
        }
        #endregion

        #region Metodos

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (PrintMode == true)
            {
                PrintMode = false;
                MainBox = Visibility.Collapsed;
                ViewPrint = Visibility.Visible;
                DocPreview(_fileXps);
            }
            else
                ListaLegislativa = DataSearch.Lista;

            BlackBox = Visibility.Collapsed;
            StartProgress = false;
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (PrintMode == true)
            {
                flowDocument = new vmFlowDocumentRel().CreateFlowDocument(ListaLegislativa, Filters);

                /*
                _fileXps = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}.xps", Sim.Common.Helpers.Folders.Xps, @"\CONL",
                    DateTime.Now.Day,
                    DateTime.Now.Month,
                    DateTime.Now.Year,
                    DateTime.Now.Hour,
                    DateTime.Now.Minute,
                    DateTime.Now.Second);
                    */
                //XpsFile.Save(flowDocument, _fileXps);
            }
            else
                DataSearch.GoSearch(TipoConsulta, _listconsult, bgWorker);

        }

        private void ExecuteCommandSearch(object param)
        {
            ProgressValue = 0;
            StartProgress = true;
            BlackBox = Visibility.Visible;
            PrintMode = false;

            Filters.Clear();

            if (cTipo == "...")
                Filters.Add("[TIPO:TODOS]");
            else
                Filters.Add("[TIPO:" + cTipo + "]");

            if (TipoConsulta == TypeSearch.Detailed)
            {

                if (cClassificacao == "0")
                    Filters.Add("[CLASSIFICAÇÃO:TODOS]");
                else
                    Filters.Add("[ CLASSIFICAÇÃO:" + Classificacao[Convert.ToInt32(cClassificacao)].Nome + "]");

                if (cOrigem == "0")
                    Filters.Add("[ORIGEM:TODOS]");
                else
                    Filters.Add("[ORIGEM:" + Origem[Convert.ToInt32(cOrigem)].Nome + "]");

                if (cSituacao == "0")
                    Filters.Add("[SITUAÇÃO:TODOS]");
                else
                    Filters.Add("[SITUAÇÃO:" + Situacao[Convert.ToInt32(cSituacao)].Nome + "]");

                if (cAutor != "")
                    Filters.Add("[AUTOR:" + cAutor + "]");

                if (cResumo != "")
                    Filters.Add("[RESUMO:" + cResumo + "]");

                Filters.Add("[PERÍODO:" + DataInicial.ToShortDateString() + " à " + DataFinal.ToShortDateString() + "]");

            }
            else
                Filters.Add("[Nº:" + cNumero.ToString() + "]");

            _listconsult.Clear();
            _listconsult.Add(cTipo);
            _listconsult.Add(cNumero);
            _listconsult.Add(cClassificacao);
            _listconsult.Add(cOrigem);
            _listconsult.Add(cSituacao);
            _listconsult.Add(cAutor);
            _listconsult.Add(cResumo);
            _listconsult.Add(DataInicial);
            _listconsult.Add(DataFinal);

            bgWorker.RunWorkerAsync();
        }

        private void ExecuteCommandShowPDF(object param)
        {
            /*
            Common.Views.WindowCommon win = new Common.Views.WindowCommon();

            string link = (string)param;

            string sitio = mXml.Listar("sim.apps", "App", "PDF", "Legislacao")[0];

            string urlLINK = string.Format(@"{0}{1}", sitio, link);

            Pdf.View.vPDFImage ctrl = new Pdf.View.vPDFImage(sitio, link);
            win.Height = ctrl.AlternateHeight();
            win.Width = ctrl.AlternateWidth();
            win.Content = ctrl;
            win.Title = "Sim.App.PDFViewer";
            win.Owner = Mvvm.Helpers.Observers.GlobalMasterWindow.MasterWindow;
            win.Show();
            */
            string link = (string)param;

            string sitio = Properties.Settings.Default.PDFLegislacao; // mXml.Listar("sim.apps", "App", "PDF", "Legislacao")[0];

            string urlLINK = string.Format(@"{0}{1}", sitio, link);

            Process p = new Process();
            ProcessStartInfo s = new ProcessStartInfo(urlLINK);
            p.StartInfo = s;
            p.Start();
        }

        private void ExecuteCommandEdit(object param)
        {
            if (Logged.Acesso >= (int)AccountAccess.Administrador)
            {
                MainBox = Visibility.Collapsed;
                EditBox = Visibility.Visible;
                ShowDataEdit(param);
            }
            else
                MessageBox.Show("Acesso Negado!", "Sim.App.Alerta!");
        }

        private void ExecuteCommandPrint(object param)
        {
            PrintMode = true;
            ProgressValue = 0;
            StartProgress = true;
            EditBox = Visibility.Collapsed;
            BlackBox = Visibility.Visible;
            bgWorker.RunWorkerAsync();
        }

        private void ExecuteCommandClear(object param)
        {
            ListaLegislativa.Clear();
            Doc.ResetValues();
            cTipo = "...";
            cOrigem = "0";
            cSituacao = "0";
            cResumo = string.Empty;
            cAutor = string.Empty;
            DataInicial = new DateTime(1853, 8, 15);// Convert.ToDateTime("15/08/1853");
            DataFinal = DateTime.Now;
        }

        private void ExecuteCommandCancel(object param)
        {
            bgWorker.CancelAsync();
        }

        private void ExecuteCommandClosePrint(object obj)
        {
            MainBox = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            EditBox = Visibility.Collapsed;
            ViewPrint = Visibility.Collapsed;
            DocumentPreview = null;
            //new Common.Helpers.MemoryManagement().FlushMemory();            
        }

        private void DocPreview(string filename)
        {
            try
            {
                /*
                using (XpsDocument document = new XpsDocument(filename, System.IO.FileAccess.Read))
                    DocumentPreview = document.GetFixedDocumentSequence();*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}", ex.Message), "Sim.Alerta!");
            }
        }

        #endregion

        #region Edite

        #region Declarações

        private mDataEdit DataEdit = new mDataEdit();

        private mLegislacao _doc = new mLegislacao();
        private mLegislacao _docrestore = new mLegislacao();

        private ObservableCollection<mAcoes> _listacoes = new ObservableCollection<mAcoes>();

        private ContentControl _contentpdfview = new ContentControl();

        private DateTime _dataalvo;

        private string _selectedorigem;
        private string _selectedautor;
        private string _selectedtipoalvo;
        private string _selectedacao;
        private string _compalvo;

        private int _numeroalvo;
        private int _selectedindex;

        private bool _regdesativado;
        private bool _regativado;

        private ICommand _commandsave;
        private ICommand _commandcanceledit;
        private ICommand _commandupdatepdf;
        private ICommand _commandaddaction;
        private ICommand _commandremoveaction;
        private ICommand _commandrollback;

        string sitio = Properties.Settings.Default.PDFLegislacao;// mXml.Listar("sim.apps", "App", "PDF", "Legislacao")[0];

        #endregion

        #region Properties

        public mLegislacao Doc
        {
            get { return _doc; }
            set
            {
                _doc = value;
                RaisePropertyChanged("Doc");
            }
        }

        public mLegislacao DocRestore
        {
            get { return _docrestore; }
            set
            {
                _docrestore = value;
                RaisePropertyChanged("DocRestore");
            }
        }

        public ObservableCollection<mAcoes> ListAcoes
        {
            get { return _listacoes; }
            set
            {
                _listacoes = value;
                RaisePropertyChanged("ListAcoes");
            }
        }


        public ContentControl ContentPdfView
        {
            get { return _contentpdfview; }
            set
            {
                _contentpdfview = value;
                RaisePropertyChanged("ContentPdfView");
            }
        }

        public Window OwnerWindow { get; set; }

        public DateTime DataAlvo
        {
            get { return _dataalvo; }
            set
            {
                _dataalvo = value;
                RaisePropertyChanged("DataAlvo");
            }
        }

        public string SelectedOrigem
        {
            get { return _selectedorigem; }
            set
            {
                _selectedorigem = value;

                if (value == "2" || value == "0")
                    Doc.Autor = string.Empty;

                Doc.Origem = value;
                RaisePropertyChanged("SelectedOrigem");
            }
        }

        public string SelectedAutor
        {
            get { return _selectedautor; }
            set
            {
                _selectedautor = value;

                if (value == "1")
                    Doc.Autor = "MESA DIRETORA";

                if (value == "2" || value == "0")
                {
                    Doc.Autor = string.Empty;
                    if (SelectedOrigem == "1") Doc.Autor = "EXECUTIVO";
                }

                RaisePropertyChanged("SelectedAutor");
            }
        }

        public string SelectedAcao
        {
            get { return _selectedacao; }
            set
            {
                _selectedacao = value;
                RaisePropertyChanged("SelectedAcao");
            }
        }

        public string SelectedTipoAlvo
        {
            get { return _selectedtipoalvo; }
            set
            {
                _selectedtipoalvo = value;
                RaisePropertyChanged("SelectedTipoAlvo");
            }
        }


        public string ComplementoAlvo
        {
            get { return _compalvo; }
            set
            {
                _compalvo = value;
                RaisePropertyChanged("ComplementoAlvo");
            }
        }

        public int NumeroAlvo
        {
            get { return _numeroalvo; }
            set
            {
                _numeroalvo = value;
                RaisePropertyChanged("NumeroAlvo");
            }
        }

        public int SelectedIndex
        {
            get { return _selectedindex; }
            set
            {
                _selectedindex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        public bool RegDesativado
        {
            get { return _regdesativado; }
            set
            {
                _regdesativado = value;

                Doc.Excluido = _regdesativado;

                if (_regdesativado == false)
                    RegAtivado = true;

                RaisePropertyChanged("RegDesativado");
            }
        }

        public bool RegAtivado
        {
            get { return _regativado; }
            set
            {
                _regativado = value;

                if (_regativado == true)
                    Doc.Excluido = false;

                RaisePropertyChanged("RegAtivado");
            }
        }

        #endregion

        #region Commands

        public ICommand CommandSave
        {
            get
            {
                if (_commandsave == null)
                    _commandsave = new DelegateCommand(ExecuteCommandSave, null);
                return _commandsave;
            }

        }

        public ICommand CommandCancelEdit
        {
            get
            {
                if (_commandcanceledit == null)
                    _commandcanceledit = new DelegateCommand(ExecuteCommandCancelEdit, null);
                return _commandcanceledit;
            }
        }

        public ICommand CommandUpdatePdf
        {
            get
            {
                if (_commandupdatepdf == null)
                    _commandupdatepdf = new DelegateCommand(ExecuteCommandUpdatePdf, null);
                return _commandupdatepdf;
            }
        }

        public ICommand CommandAddAction
        {
            get
            {
                if (_commandaddaction == null)
                    _commandaddaction = new DelegateCommand(ExecuteCommandAddAction, null);
                return _commandaddaction;
            }
        }

        public ICommand CommandRemoveAction
        {
            get
            {
                if (_commandremoveaction == null)
                    _commandremoveaction = new DelegateCommand(ExecuteCommandRemoveAction, null);
                return _commandremoveaction;
            }
        }

        public ICommand CommandRollBack
        {
            get
            {
                if (_commandrollback == null)
                    _commandrollback = new DelegateCommand(ExecuteCommandRollBack, null);

                return _commandrollback;
            }
        }
        #endregion

        #region Metodos

        public void ShowDataEdit(object param)
        {
            int indice = (int)param;

            Doc = new mDataEdit().EditDoc(indice);
            DocRestore = new mDataEdit().EditDoc(indice);

            SelectedOrigem = Doc.Origem;

            if (Doc.Autor.ToLower() == "mesa diretora") SelectedAutor = "1";

            if (Doc.Autor.ToLower() != "mesa diretora" || Doc.Autor.ToLower() != "executivo") SelectedAutor = "2";

            //list<t> to observablecollection<t>
            ListToObservableCollection(Doc.ListaAcoes);

            SelectedAcao = "0";
            SelectedTipoAlvo = "...";

            RegAtivado = Doc.Excluido;
            RegDesativado = Doc.Excluido;

            //ContentPdfView.Content = new Pdf.View.vPDFImage(sitio, Doc.Link);

            Doc.Autor = DocRestore.Autor;
        }

        private void ExecuteCommandSave(object param)
        {
            Doc.Atualizado = DateTime.Now;
            if (new mDataEdit().Update(Doc))
            {
                MessageBox.Show("Documento alterado com sucesso!", "Sim.Apps.Info!");
                EditBox = Visibility.Collapsed;
                MainBox = Visibility.Visible;
            }
            else
                MessageBox.Show(string.Format("Não foi possivel alterar {0} {1}!", Doc.Tipo, Doc.Numero), "Sim.Apps.Info!");

        }

        private void ExecuteCommandCancelEdit(object param)
        {
            EditBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
        }

        private void ExecuteCommandRollBack(object param)
        {
            Doc = DocRestore;
            ListToObservableCollection(DocRestore.ListaAcoes);
        }

        private void ExecuteCommandUpdatePdf(object param)
        {
            //ContentPdfView.Content = new Pdf.View.vPDFImage(sitio, Doc.Link);
        }

        private void ExecuteCommandAddAction(object param)
        {

            var acoes = new mAcoes();

            try
            {

                List<string> lista = new mDataValidarAcao().Validate(SelectedTipoAlvo, NumeroAlvo, ComplementoAlvo);

                acoes.TipoOrigem = Doc.Tipo;
                acoes.NumeroOrigem = Doc.Numero;
                acoes.ComplementoOrigem = Doc.Complemento;
                acoes.DataOrigem = Doc.Data;
                acoes.Acao = SelectedAcao;
                acoes.TipoAlvo = lista[0];
                acoes.NumeroAlvo = Convert.ToInt32(lista[1]);
                acoes.ComplementoAlvo = lista[2];
                acoes.DataAlvo = Convert.ToDateTime(lista[3]);
                acoes.Incluido = DateTime.Now;

                ListAcoes.Add(new mDataAddAcao().Add(acoes));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                SelectedAcao = "0";
                SelectedTipoAlvo = "...";
                NumeroAlvo = 0;
                ComplementoAlvo = string.Empty;
            }
        }

        private void ExecuteCommandRemoveAction(object param)
        {
            if (MessageBox.Show("Excluir Ação?", "Sim.Apps.Alerta!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                try
                {
                    var objacao = new mAcoes();

                    objacao.Indice = ListAcoes[SelectedIndex].Indice;

                    objacao.TipoAlvo = ListAcoes[SelectedIndex].TipoAlvo;
                    objacao.NumeroAlvo = ListAcoes[SelectedIndex].NumeroAlvo;
                    objacao.ComplementoAlvo = ListAcoes[SelectedIndex].ComplementoAlvo;
                    objacao.DataAlvo = ListAcoes[SelectedIndex].DataAlvo;

                    objacao.TipoOrigem = Doc.Tipo;
                    objacao.NumeroOrigem = Doc.Numero;
                    objacao.ComplementoOrigem = Doc.Complemento;
                    objacao.DataOrigem = Doc.Data;

                    ListToObservableCollection(new mDataRemoveAcao().DelAcao(objacao));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        //list<t> to observablecollection<t>  
        private void ListToObservableCollection(List<mAcoes> listaAcoes)
        {
            ListAcoes.Clear();
            foreach (mAcoes Ac in listaAcoes)
            {
                var nAc = new mAcoes();

                nAc.Indice = Ac.Indice;
                nAc.TipoOrigem = Ac.TipoOrigem;
                nAc.NumeroOrigem = Ac.NumeroOrigem;
                nAc.ComplementoOrigem = Ac.ComplementoOrigem;
                nAc.DataOrigem = Ac.DataOrigem;
                nAc.Acao = Ac.Acao;
                nAc.TipoAlvo = Ac.TipoAlvo;
                nAc.NumeroAlvo = Ac.NumeroAlvo;
                nAc.ComplementoAlvo = Ac.ComplementoAlvo;
                nAc.DataAlvo = Ac.DataAlvo;
                nAc.Incluido = Ac.Incluido;

                ListAcoes.Add(nAc);
            }
        }

        #endregion

        #endregion
    }
}
