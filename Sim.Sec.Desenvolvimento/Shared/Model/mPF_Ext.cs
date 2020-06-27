using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    public class mPF_Ext : mPF
    {
        #region Declarations
        private mPerfil _perfil = new mPerfil();
        private mDeficiencia _deficiente = new mDeficiencia();
        private ObservableCollection<mVinculos> _vinculos = new ObservableCollection<mVinculos>();
        private mSegmentos _segmentos = new mSegmentos();
        #endregion

        #region Properties
        public mPerfil Perfil
        {
            get { return _perfil; }
            set { _perfil = value;
                RaisePropertyChanged("Perfil");
            }
        }

        public mDeficiencia Deficiente
        {
            get { return _deficiente; }
            set
            {
                _deficiente = value;
                RaisePropertyChanged("Deficiente");
            }
        }

        public ObservableCollection<mVinculos> ColecaoVinculos
        {
            get { return _vinculos; }
            set
            {
                _vinculos = value;
                RaisePropertyChanged("Vinculos");
            }
        }

        public mSegmentos Segmentos
        {
            get { return _segmentos; }
            set
            {
                _segmentos = value;
                RaisePropertyChanged("Segmentos");
            }
        }

        public int Contador { get; set; }
        #endregion
    }
}
