using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    class mAcoes : mAcoesBase
    {
        public string TipoOrigem { get; set; }
        public int NumeroOrigem { get; set; }
        public string ComplementoOrigem { get; set; }
        public DateTime DataOrigem { get; set; }
        public DateTime Incluido { get; set; }
    }
}
