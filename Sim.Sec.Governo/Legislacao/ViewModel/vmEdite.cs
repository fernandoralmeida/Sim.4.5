using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;


namespace Sim.Sec.Governo.Legislacao.ViewModel
{

    using Sim.Mvvm.Commands;
    using Sim.Mvvm.Observers;
    using Model;

    class vmEdite:NotifyProperty
    {
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

        string sitio = Properties.Settings.Default.PDFLegislacao; // mXml.Listar("sim.apps", "App", "PDF", "Legislacao")[0];

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

        #region Constructor
        public vmEdite()
        {
            Mvvm.Observers.GlobalNavigation.Pagina = "EDIÇÃO";
            try
            {
                var obj = GlobalNavigation.Parametro;

                ShowDataEdit(Convert.ToInt32(obj));
            }
            catch
            { Doc.ResetValues(); }
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
                NavigationCommands.BrowseBack.Execute(null, null);
            }
            else
                MessageBox.Show(string.Format("Não foi possivel alterar {0} {1}!", Doc.Tipo, Doc.Numero), "Sim.Apps.Info!");

        }

        private void ExecuteCommandCancelEdit(object param)
        {
            NavigationCommands.BrowseBack.Execute(null, null);
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
    }
}
