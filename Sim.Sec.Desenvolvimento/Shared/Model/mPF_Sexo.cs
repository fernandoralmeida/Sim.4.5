using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{

    using Mvvm.Observers;

    public class mPF_Sexo : NotifyProperty
    {
        #region Declarations
        private int _indice = 0;
        private int _valor = 0;
        private string _nome = string.Empty;
        private bool _ativo = true;
        #endregion

        #region Properties
        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public int Valor { get { return _valor; } set { _valor = value; RaisePropertyChanged("Valor"); } }
        public string Nome { get { return _nome; } set { _nome = value; RaisePropertyChanged("Nome"); } }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }
        #endregion
    }
}
