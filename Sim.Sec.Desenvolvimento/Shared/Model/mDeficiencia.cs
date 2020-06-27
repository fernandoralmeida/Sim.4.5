using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    using Mvvm.Observers;

    public class mDeficiencia : NotifyProperty
    {
        #region Declarations

        private int _indice;
        private string _cpf;
        private bool _deficiencia;
        private bool _fisica;
        private bool _visual;
        private bool _auditiva;
        private bool _intelectual;
        private bool _ativo;

        #endregion

        #region Properties

        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string CPF { get { return _cpf; } set { _cpf = value; RaisePropertyChanged("CPF"); } }
        public bool Deficiencia { get { return _deficiencia; } set { _deficiencia = value; RaisePropertyChanged("Deficiencia"); } }
        public bool Fisica { get { return _fisica; } set { _fisica = value; RaisePropertyChanged("Fisica"); } }        
        public bool Visual { get { return _visual; } set { _visual = value; RaisePropertyChanged("Visual"); } }
        public bool Auditiva { get { return _auditiva; } set { _auditiva = value; RaisePropertyChanged("Auditiva"); } }
        public bool Intelectual { get { return _intelectual; } set { _intelectual = value; RaisePropertyChanged("Intelectual"); } }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }

        #endregion

        #region Functions

        public void Clear()
        {
            Indice = 0;
            CPF = string.Empty;
            Deficiencia = false;
            Fisica = false;
            Visual = false;
            Auditiva = false;
            Intelectual = false;
            Ativo = true;
        }

        #endregion
    }
}
