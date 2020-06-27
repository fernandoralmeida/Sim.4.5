using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{

    using Mvvm.Observers;

    public class mCNAE : NotifyProperty
    {
        #region Declarations

        private int _indice;
        private string _ocupacao = string.Empty;
        private string _cnae = string.Empty;
        private string _descricao = string.Empty;
        private string _iss = string.Empty;
        private string _icms = string.Empty;
        private bool _ativo = true;

        #endregion

        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string Ocupacao { get { return _ocupacao; } set { _ocupacao = value; RaisePropertyChanged("Ocupacao"); } }
        public string CNAE { get { return _cnae; } set { _cnae = value; RaisePropertyChanged("CNAE"); } }
        public string Descricao { get { return _descricao; } set { _descricao = value; RaisePropertyChanged("Descricao"); } }
        public string ISS { get { return _iss; } set { _iss = value; RaisePropertyChanged("ISS"); } }
        public string ICMS { get { return _icms; } set { _icms = value; RaisePropertyChanged("ICMS"); } }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }
    }
}
