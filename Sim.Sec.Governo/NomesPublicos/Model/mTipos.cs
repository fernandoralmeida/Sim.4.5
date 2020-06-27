using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.NomesPublicos.Model
{
    using Sim.Mvvm.Observers;

    class mTipos : NotifyProperty
    {
        #region Declaration
        private int _indice;
        private string _descricao = string.Empty;
        private bool _ativo;
        #endregion

        #region Properties
        public int Indice
        {
            get { return _indice; }
            set { _indice = value;
                RaisePropertyChanged("Indice");
            }
        }

        public string Tipo
        {
            get { return _descricao; }
            set
            {
                _descricao = value;
                RaisePropertyChanged("Tipo");
            }
        }

        public bool Ativo
        {
            get { return _ativo; }
            set { _ativo = value;
                RaisePropertyChanged("Ativo");
            }
        }
        #endregion
    }
}
