using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{

    using Mvvm.Observers;

    abstract class mAcoesBase : NotifyProperty
    {
        public int Indice { get; set; }
        public string Acao { get; set; }
        public string TipoAlvo { get; set; }
        public int NumeroAlvo { get; set; }
        public string ComplementoAlvo { get; set; }
        public DateTime DataAlvo { get; set; }
    }
}
