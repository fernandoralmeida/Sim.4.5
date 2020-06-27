using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Portarias.Model
{
    public class mClassificacao
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public DateTime Cadastro { get; set; }
        public DateTime Alterado { get; set; }
        public bool Bloqueado { get; set; }
        public string CodigoLink { get; set; }
    }
}
