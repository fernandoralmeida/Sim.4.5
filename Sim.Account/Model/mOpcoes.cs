using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Account.Model
{

    using Mvvm.Observers;

    public class mOpcoes : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _identificador;
        private string _thema;
        private string _color;
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

        public string Thema
        {
            get { return _thema; }
            set
            {
                _thema = value;
                RaisePropertyChanged("Thema");
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChanged("Color");
            }
        }
        #endregion
    }
}
