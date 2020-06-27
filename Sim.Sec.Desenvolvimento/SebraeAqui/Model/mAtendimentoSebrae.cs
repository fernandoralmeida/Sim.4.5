using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.SebraeAqui.Model
{

    using Mvvm.Observers;

    public class mAtendimentoSebrae : NotifyProperty
    {
        #region Declarations
        private int _indice = 0;
        private string _atendimento_sac =string.Empty;
        private string _atendimento = string.Empty;
        private string _cliente = string.Empty;
        private bool _ativo = true;
        #endregion

        #region Properties
        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string AtendimentoSAC { get { return _atendimento_sac; } set { _atendimento_sac = value.Trim(); RaisePropertyChanged("AtendimentoSAC"); } }
        public string Atendimento { get { return _atendimento; } set { _atendimento = value; RaisePropertyChanged("Atendimento"); } }
        public string Cliente { get { return _cliente; } set { _cliente = value; RaisePropertyChanged("Cliente"); } }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }
        #endregion

        #region Functions
        public void Clear()
        {
            Indice = 0;
            AtendimentoSAC = string.Empty;
            Atendimento = string.Empty;
            Cliente = string.Empty;
            Ativo = true;
        }
        #endregion
    }
}
