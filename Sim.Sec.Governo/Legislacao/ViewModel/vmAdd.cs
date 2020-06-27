using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Legislacao.ViewModel
{
    using Account;
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Model;

    class vmAdd : NotifyProperty
    {
        #region Declarações

        private mDataEdit DataEdit = new mDataEdit();

        private BackgroundWorker bgWorker = new BackgroundWorker();

        private mLegislacao _doc = new mLegislacao();

        private List<string> _pdfs = new List<string>();

        private ObservableCollection<mAcoes> _listacoes = new ObservableCollection<mAcoes>();

        private ContentControl _contentpdfview = new ContentControl();

        private DateTime _dataalvo;

        private string _selectedorigem;
        private string _selectedautor;
        private string _selectedtipoalvo;
        private string _selectedacao;
        private string _compalvo;
        private string _pdfpath;

        private int _numeroalvo;
        private int _selectedindex;

        private bool _regdesativado;
        private bool _regativado;

        private Visibility _boxgroups;
        private Visibility _viewgrouppdf;
        private Visibility _viewstartpdf;

        private ICommand _commandsave;
        private ICommand _commandcancel;
        private ICommand _commandstart;
        private ICommand _commandaddaction;
        private ICommand _commandremoveaction;
        private ICommand _commandrefresh;

        string sitio = Properties.Settings.Default.PDFLegislacao;//  mXml.Listar("sim.apps", "App", "PDF", "Legislacao")[0];

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
                
        public DateTime DataAlvo
        {
            get { return _dataalvo; }
            set
            {
                _dataalvo = value;
                RaisePropertyChanged("DataAlvo");
            }
        }
        
        public List<string> PDFs
        {
            get { return _pdfs; }
            set
            {
                _pdfs = value;
                RaisePropertyChanged("PDFs");
            }
        }

        public string PdfPath
        {
            get { return _pdfpath; }
            set { _pdfpath = value;
                RaisePropertyChanged("PdfPath");
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

        public Visibility BoxGroups
        {
            get { return _boxgroups; }
            set
            {
                _boxgroups = value;
                RaisePropertyChanged("BoxGroups");
            }
        }

        public Visibility ViewGroupPDF
        {
            get { return _viewgrouppdf; }
            set
            {
                _viewgrouppdf = value;
                RaisePropertyChanged("ViewGroupPDF");
            }
        }

        public Visibility ViewStartPDF
        {
            get { return _viewstartpdf; }
            set
            {
                _viewstartpdf = value;
                RaisePropertyChanged("ViewStartPDF");
            }
        }
        #endregion

        #region Commands

        public ICommand CommandRefresh
        {
            get
            {
                if (_commandrefresh == null)
                    _commandrefresh = new DelegateCommand(ExecuteCommandRefresh, null);
                return _commandrefresh;
            }
        }

        public ICommand CommandSave
        {
            get
            {
                if (_commandsave == null)
                    _commandsave = new DelegateCommand(ExecuteCommandSave, null);
                return _commandsave;
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

        public ICommand CommandStart
        {
            get
            {
                if (_commandstart == null)
                    _commandstart = new DelegateCommand(ExecuteCommandStart, null);
                return _commandstart;
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

        #endregion

        #region Metodos

        public vmAdd()
        {

            GlobalNavigation.Pagina = "INCLUIR";

            /*
            if (Account.Logged.Acesso >= (int)AccountAccess.Administrador)
            {
                Common.Helpers.Folders.Create(Common.Helpers.Folders.SystemDesktop, Common.Helpers.Folders.SimPDF);

                PDFs = Common.Helpers.Files.ListFiles(string.Format(@"{0}\{1}", Common.Helpers.Folders.SystemDesktop, Common.Helpers.Folders.SimPDF), "*.pdf");
            }
            */

            BoxGroups = Visibility.Visible;
            ViewStartPDF = Visibility.Collapsed;
            ViewGroupPDF = Visibility.Visible;

            RegAtivado = Doc.Excluido;
            RegDesativado = Doc.Excluido;

            Doc.Cadastro = DateTime.Now;
            Doc.Atualizado = DateTime.Now;
            Doc.ResetValues();
            Doc.Situacao = "1";
            SelectedAutor = "0";
            SelectedOrigem = "0";

            SelectedAcao = "0";
            SelectedTipoAlvo = "...";
            NumeroAlvo = 0;
            ComplementoAlvo = "";
        }


        private void ExecuteCommandRefresh(object obj)
        {
            /*
            try
            {
                PDFs = Common.Helpers.Files.ListFiles(string.Format(@"{0}\{1}", Common.Helpers.Folders.SystemDesktop, Common.Helpers.Folders.SimPDF), "*.pdf");
            }
            catch
            {
                PDFs = null;
            }*/
        }

        private void ExecuteCommandSave(object param)
        {

            Doc.Link = mLink.Create(Doc.Tipo, Doc.Data.Year.ToString(), Doc.Numero);
            ObservableCollectionToList(ListAcoes);
            //MessageBox.Show(Doc.ToString());
            if (new mDataInsert().Insert(Doc))
            {
                MessageBox.Show("Documento Incluido com sucesso!", "Sim.Apps.Info!");

                /*

                string filedestino = Common.Helpers.XmlFile.Listar("sim.apps", "App", "PDF", "Legislacao")[0];
                
                string fileorigem = Common.Helpers.Folders.SystemDesktop 
                    + @"\" + Common.Helpers.Folders.SimPDF 
                    + @"\" + PdfPath;

                if (Doc.Tipo == "LEI")
                    filedestino += @"\leis\" + Doc.Data.Year + @"\" + Doc.Numero + ".pdf";

                if (Doc.Tipo == "LEI COMPLEMENTAR")
                    filedestino += @"\leis_complementares\" + Doc.Data.Year + @"\" + Doc.Numero + ".pdf";

                if (Doc.Tipo == "DECRETO")
                    filedestino += @"\decretos\" + Doc.Data.Year + @"\" + Doc.Numero + ".pdf";

                try
                {
                    System.IO.File.Copy(fileorigem, filedestino, true);
                    System.IO.File.Delete(fileorigem);
                }
                catch { }  
                */
                ContentPdfView.Content = null;
                BoxGroups = Visibility.Collapsed;
                ViewStartPDF = Visibility.Visible;
                ViewGroupPDF = Visibility.Collapsed;
                ListAcoes.Clear();
                Doc.ResetValues();
                Doc.Situacao = "1";
                SelectedAutor = "0";
                SelectedOrigem = "0";

                PdfPath = string.Empty;
                //PDFs = Common.Helpers.Files.ListFiles(string.Format(@"{0}\{1}", Common.Helpers.Folders.SystemDesktop, Common.Helpers.Folders.SimPDF), "*.pdf");
            }
            else
                MessageBox.Show(string.Format("Documento já cadastrado [{0} {1}]!", Doc.Tipo, Doc.Numero), "Sim.Apps.Info!");                
        }

        private void ExecuteCommandCancel(object param)
        {
            BoxGroups = Visibility.Collapsed;
            ViewStartPDF = Visibility.Visible;
            ViewGroupPDF = Visibility.Collapsed;
            ContentPdfView.Content = null;
            ListAcoes.Clear();
            Doc.ResetValues();
            Doc.Situacao = "1";
            SelectedAutor = "0";
            SelectedOrigem = "0";
            SelectedAcao = "0";
            SelectedTipoAlvo = "...";
            NumeroAlvo = 0;
            ComplementoAlvo = "";
        }

        private void ExecuteCommandStart(object param)
        {
            if (PdfPath != null)
            {
                BoxGroups = Visibility.Visible;
                //ContentPdfView.Content = new Pdf.View.ucPdfViewer(PdfPath);
                //string path = string.Format(@"{0}\{1}\", Common.Helpers.Folders.SystemDesktop, Common.Helpers.Folders.SimPDF);
                //MessageBox.Show(path + @"\" +  PdfPath);
                //ContentPdfView.Content = new Pdf.View.vPDFImage(path, PdfPath);
                ViewStartPDF = Visibility.Collapsed;
                ViewGroupPDF = Visibility.Visible;
            }
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

                ListAcoes.Add(acoes);

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
                    ListAcoes.RemoveAt(SelectedIndex);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        //list<t> to observablecollection<t>  
        private void ObservableCollectionToList(ObservableCollection<mAcoes> lista)
        {
            List<mAcoes> ac = new List<mAcoes>();
            foreach (mAcoes Ac in lista)
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

                ac.Add(nAc);
            }
            Doc.ListaAcoes = ac;
        }

        #endregion
    }
}
