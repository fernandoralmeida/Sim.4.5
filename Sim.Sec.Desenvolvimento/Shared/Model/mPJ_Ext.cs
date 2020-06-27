using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    public class mPJ_Ext : mPJ
    {

        #region Declarations
        private mFormalizada _formalizada = new mFormalizada();
        private mSegmentos _segmentos = new mSegmentos();
        #endregion

        #region Properties

        public mFormalizada Formalizada
        {
            get { return _formalizada; }
            set
            {
                _formalizada = value;
                RaisePropertyChanged("Formalizada");
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
