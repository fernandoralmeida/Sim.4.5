using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Account.Model
{

    using Mvvm.Observers;

    public class mContas : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _identificador;
        private int _conta;
        private string _contaacesso;
        private bool _ativo;
        #endregion

        #region Properties
        public int Indice
        {
            get { return _indice; }
            set
            {
                _indice = value;
                RaisePropertyChanged("Indice");
            }
        }

        public string Identificador
        {
            get { return _identificador; }
            set
            {
                _identificador = value;
                RaisePropertyChanged("Identificador");
            }
        }

        public int Conta
        {
            get { return _conta; }
            set
            {
                _conta = value;
                RaisePropertyChanged("Conta");
            }
        }

        public string ContaAcesso
        {
            get { return _contaacesso; }
            set
            {
                _contaacesso = value;
                RaisePropertyChanged("ContaAcesso");
            }
        }

        public bool Ativo
        {
            get { return _ativo; }
            set
            {
                _ativo = value;
                RaisePropertyChanged("Ativo");
            }
        }
        #endregion
    }
}
