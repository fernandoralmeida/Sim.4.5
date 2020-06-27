using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    /// <summary>
    /// Classe generica pra listar
    /// tipos, classificacao, origem, situação
    /// </summary>
    class mTiposGenericos
    {
        public int Indice { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public DateTime Cadastro { get; set; }
        public DateTime Alterado { get; set; }
        public bool Bloqueado { get; set; }
    }
}
