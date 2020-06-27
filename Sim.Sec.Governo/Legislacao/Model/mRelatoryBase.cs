using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    public abstract class mRelatoryBase
    {
        public List<KeyValuePair<string, int>> Tipo { get; set; }
        public List<KeyValuePair<string, int>> Situacao { get; set; }
        public List<KeyValuePair<string, int>> Origem { get; set; }
        public List<KeyValuePair<string, int>> Autor { get; set; }
        public List<KeyValuePair<string, int>> Classificacao { get; set; }

        public void ResetValues()
        {
            this.Tipo.Clear();
            this.Situacao.Clear();
            this.Origem.Clear();
            this.Autor.Clear();
            this.Classificacao.Clear();
        }

    }
}
