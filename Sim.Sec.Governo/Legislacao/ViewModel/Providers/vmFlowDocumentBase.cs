using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.ViewModel.Providers
{

    using Model;
    using Sim.Mvvm.Observers;

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
            NameOrg = Services.Properties.Settings.Default.Prefeitura;// mXml.Listar("sim.apps", "App", "Organizacao", "Name")[0];
            SloganOrg = Services.Properties.Settings.Default.Fundada;// mXml.Listar("sim.apps", "App", "Organizacao", "Slogan")[0];
            DepOrg = Services.Properties.Settings.Default.Secretaria;// mXml.Listar("sim.apps", "App", "Organizacao", "Departamento")[0];
            SetorOrg = Services.Properties.Settings.Default.Setor;// mXml.Listar("sim.apps", "App", "Organizacao", "Setor")[0];
        }
    }
}
