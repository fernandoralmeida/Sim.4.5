using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Model
{
    public class Ambulante : DIA
    {
        private bool _temempresa;
        public bool TemCNPJ { get { return _temempresa; } set { _temempresa = value; RaisePropertyChanged("TemCNPJ"); } }
  

    }
}
