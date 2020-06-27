using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Account.Model
{

    using Mvvm.Observers;

    public class mGenerico : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _nome;
        private int _valor;
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

        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                RaisePropertyChanged("Nome");
            }
        }

        public int Valor
        {
            get { return _valor; }
            set
            {
                _valor = value;
                RaisePropertyChanged("Valor");
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
