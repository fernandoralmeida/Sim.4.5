using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Model
{
    using Mvvm.Observers;

    public class mPeriodos : NotifyProperty
    {
        #region Declaration
        private int _indice;
        private string _dias;
        private string _periodos;
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

        public string Dias
        {
            get { return _dias; }
            set
            {
                _dias = value;
                RaisePropertyChanged("Dias");
            }
        }

        public string Periodos
        {
            get { return _periodos; }
            set
            {
                _periodos = value;
                RaisePropertyChanged("Periodos");
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

        #region Functions
        public void Clear()
        {
            Indice = 0;
            Dias = string.Empty;
            Periodos = string.Empty;     
            Ativo = true;
        }
        #endregion
    }
}
