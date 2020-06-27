using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Legislacao.ViewModel.vmPages
{

    using Sim.Mvvm.Commands;
    using Sim.Mvvm.Observers;
    using Model;
    using SqlCommands;

    class vmLinkPdf : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;
        private bool _isfocusedpdf;

        private string _rootpdf;
        private string _rootpdfrotulo;

        private ICommand _commandupdaterootpdf;
        #endregion

        #region Properties

        public bool  TextBoxEnabled
        {
            get { return _textboxenabled; }
            set { _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
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

        public string RootPDFRotulo
        {
            get { return _rootpdfrotulo; }
            set { _rootpdfrotulo = value;
                RaisePropertyChanged("RootPDFRotulo");
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

        #endregion

        #region Start Instance

        public vmLinkPdf()
        {
            RootPDF = Properties.Settings.Default.PDFLegislacao;//  mXml.Listar("sim.apps", "App", "PDF", "Legislacao")[0];
            RootPDFRotulo = "Alterar Caminho";
            TextBoxEnabled = false;
            IsFocusedPdf = false;
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
                Properties.Settings.Default.PDFLegislacao = RootPDF;
                Properties.Settings.Default.Save();
                //mXml.SaveXml(RootPDF, "Legislacao");
                IsFocusedPdf = false;
            }
        }

        #endregion
    }
}
