using System.Windows.Input;
using System.Windows;

namespace Sim.Sec.Governo.Portarias.ViewModel.Pages
{
    using Mvvm.Commands;
    using Mvvm.Observers;

    class vmLinkPdf : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;
        private bool _textboxenabledjo;
        private bool _isfocusedpdf;
        private bool _isfocusedpdfjo;
        
        private string _rootpdf;
        private string _rootpdfrotulo;
        private string _pathjornaloficial;
        private string _buttonjornaloficial;

        private ICommand _commandupdaterootpdf;
        private ICommand _commandjornaloficial;
        #endregion

        #region Properties


        public bool TextBoxEnabled
        {
            get { return _textboxenabled; }
            set
            {
                _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
            }
        }

        public bool TextBoxEnabledJO
        {
            get { return _textboxenabledjo; }
            set
            {
                _textboxenabledjo = value;
                RaisePropertyChanged("TextBoxEnabledJO");
            }
        }

        public bool IsFocusedPdf
        {
            get { return _isfocusedpdf; }
            set
            {
                _isfocusedpdf = value;
                RaisePropertyChanged("IsFocusedPdf");
            }
        }

        public bool IsFocusedPdfJo
        {
            get { return _isfocusedpdfjo; }
            set
            {
                _isfocusedpdfjo = value;
                RaisePropertyChanged("IsFocusedPdfJo");
            }
        }

        public string RootPDF
        {
            get { return _rootpdf; }
            set
            {
                _rootpdf = value;
                RaisePropertyChanged("RootPDF");
            }
        }

        public string PathJornalOficial
        {
            get { return _pathjornaloficial; }
            set
            {
                _pathjornaloficial = value;
                RaisePropertyChanged("PathJornalOficial");
            }
        }

        public string RootPDFRotulo
        {
            get { return _rootpdfrotulo; }
            set
            {
                _rootpdfrotulo = value;
                RaisePropertyChanged("RootPDFRotulo");
            }
        }

        public string ButtonJornalOficial
        {
            get { return _buttonjornaloficial; }
            set
            {
                _buttonjornaloficial = value;
                RaisePropertyChanged("ButtonJornalOficial");
            }
        }
        
        #endregion

        #region Commands

        public ICommand CommandUpdateRootPDF
        {
            get
            {
                if (_commandupdaterootpdf == null)
                    _commandupdaterootpdf = new DelegateCommand(ExecuteCommandUpdateRootPDF, null);

                return _commandupdaterootpdf;
            }
        }

        public ICommand CommandJornalOficial
        {
            get
            {
                if (_commandjornaloficial == null)
                    _commandjornaloficial = new DelegateCommand(ExecuteCommandJornalOficial, null);

                return _commandjornaloficial;
            }
        }

        #endregion

        #region Start Instance

        public vmLinkPdf()
        {
            RootPDF = Properties.Settings.Default.PDFPortarias;
            PathJornalOficial = Properties.Settings.Default.JOPortarias;
            RootPDFRotulo = "Alterar Caminho";
            ButtonJornalOficial = "Alterar Caminho";
            TextBoxEnabled = false;
            TextBoxEnabledJO = false;
            IsFocusedPdf = false;
            IsFocusedPdfJo = false;
        }

        #endregion

        #region Methods

        private void ExecuteCommandUpdateRootPDF(object obj)
        {
            if (RootPDFRotulo == "Alterar Caminho")
            {
                TextBoxEnabled = true;
                RootPDFRotulo = "Salvar";
                IsFocusedPdf = true;
            }
            else
            {
                RootPDFRotulo = "Alterar Caminho";
                TextBoxEnabled = false;
                Properties.Settings.Default.PDFPortarias = RootPDF;
                Properties.Settings.Default.Save();
                IsFocusedPdf = false;
            }
        }

        private void ExecuteCommandJornalOficial(object obj)
        {
            if (ButtonJornalOficial == "Alterar Caminho")
            {
                TextBoxEnabledJO = true;
                ButtonJornalOficial = "Salvar";
                IsFocusedPdfJo = true;
            }
            else
            {
                ButtonJornalOficial = "Alterar Caminho";
                TextBoxEnabledJO = false;
                Properties.Settings.Default.JOPortarias = PathJornalOficial;
                Properties.Settings.Default.Save();

                IsFocusedPdfJo = false;
            }
        }

        #endregion
    }
}
