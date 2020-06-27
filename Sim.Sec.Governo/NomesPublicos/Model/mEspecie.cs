using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.NomesPublicos.Model
{
    using Mvvm.Observers;

    class mEspecie:NotifyProperty
    {
        #region Declaration
        private int _indice;
        private int _especie;
        private string _descricao = string.Empty;
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

        public int Codigo
        {
            get { return _especie; }
            set
            {
                _especie = value;
                RaisePropertyChanged("Codigo");
            }
        }

        public string Especie
        {
            get { return _descricao; }
            set
            {
                _descricao = value;
                RaisePropertyChanged("Especie");
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
