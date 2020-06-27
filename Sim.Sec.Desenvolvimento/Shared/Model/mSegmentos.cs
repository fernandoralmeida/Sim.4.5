using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{

    using Mvvm.Observers;

    public class mSegmentos: NotifyProperty
    {
        #region Declarations

        private int _indice;
        private string _cnpj_cpf;
        private bool _agronegocio;
        private bool _industria;
        private bool _comercio;
        private bool _servico;
        private bool _ativo;

        #endregion

        #region Properties

        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string CNPJ_CPF { get { return _cnpj_cpf; } set { _cnpj_cpf = value; RaisePropertyChanged("CNPJ_CPF"); } }
        public bool Agronegocio { get { return _agronegocio; } set { _agronegocio = value; RaisePropertyChanged("Agronegocio"); } }
        public bool Industria { get { return _industria; } set { _industria = value; RaisePropertyChanged("Industria"); } }
        public bool Comercio { get { return _comercio; } set { _comercio = value; RaisePropertyChanged("Comercio"); } }
        public bool Servicos { get { return _servico; } set { _servico = value; RaisePropertyChanged("Servicos"); } }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }

        #endregion

        #region Functions

        public void Clear()
        {
            Indice = 0;
            CNPJ_CPF = string.Empty;
            Agronegocio = false;
            Industria = false;
            Comercio = false;
            Servicos = false;
            Ativo = true;
        }

        #endregion
    }
}
