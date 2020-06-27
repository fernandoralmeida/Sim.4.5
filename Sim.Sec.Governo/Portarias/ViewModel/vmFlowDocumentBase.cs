using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Portarias.ViewModel
{
    using Model;
    using Mvvm.Observers;

    class vmFlowDocumentBase : NotifyProperty
    {
        private string _nameorg;
        private string _slogan;
        private string _deporg;
        private string _setororg;

        public string NameOrg
        {
            get { return _nameorg; }
            set
            {
                _nameorg = value;
                RaisePropertyChanged("NameOrg");
            }
        }

        public string SloganOrg
        {
            get { return _slogan; }
            set
            {
                _slogan = value;
                RaisePropertyChanged("SloganOrg");
            }
        }

        public string DepOrg
        {
            get { return _deporg; }
            set
            {
                _deporg = value;
                RaisePropertyChanged("DepOrg");
            }
        }

        public string SetorOrg
        {
            get { return _setororg; }
            set
            {
                _setororg = value;
                RaisePropertyChanged("SetorOrg");
            }
        }

        public vmFlowDocumentBase()
        {
            NameOrg = Services.Properties.Settings.Default.Prefeitura;
            SloganOrg = Services.Properties.Settings.Default.Fundada;
            DepOrg = Services.Properties.Settings.Default.Secretaria;
            SetorOrg = Services.Properties.Settings.Default.Setor;
        }
    }
}
