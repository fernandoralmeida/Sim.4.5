using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Account.Model
{

    using Mvvm.Observers;

    public class mSubModulos : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _identificador;        
        private int _submodulo;
        private int _acesso;
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
        public int SubModulo
        {
            get { return _submodulo; }
            set
            {
                _submodulo = value;
                RaisePropertyChanged("SubModulo");
            }
        }
        public int Acesso
        {
            get { return _acesso; }
            set
            {
                _acesso = value;
                RaisePropertyChanged("Acesso");
            }
        }
        #endregion

        #region Functions
        public void Clear()
        {
            Indice = 0;
            Identificador = string.Empty;
            SubModulo = 0;
            Acesso = 0;
        }
        #endregion
    }
}
