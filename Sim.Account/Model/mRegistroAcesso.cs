using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Account.Model
{

    using Mvvm.Observers;

    public class mRegistroAcesso : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _cessao;
        private string _identificador;
        private DateTime _data;
        private DateTime _horain;
        private DateTime _horafi;
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

        public string CodigoAcesso
        {
            get { return _cessao; }
            set
            {
                _cessao = value;
                RaisePropertyChanged("CodigoAcesso");
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

        public DateTime Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged("Data");
            }
        }

        public DateTime HoraInicio
        {
            get { return _horain; }
            set
            {
                _horain = value;
                RaisePropertyChanged("HoraInicio");
            }
        }

        public DateTime HoraFim
        {
            get { return _horafi; }
            set
            {
                _horafi = value;
                RaisePropertyChanged("HoraFim");
            }
        }
        #endregion
    }
}
