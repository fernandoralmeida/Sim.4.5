using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Portarias.Model
{
    public class mEstatistics
    {
        public List<KeyValuePair<string, int>> Tipo { get; set; }
        public List<KeyValuePair<string, int>> Servidor { get; set; }
        public List<KeyValuePair<string, int>> Ano { get; set; }

        public void Clear()
        {
            this.Tipo.Clear();
            this.Ano.Clear();
            this.Servidor.Clear();
        }

        public mEstatistics()
        {
            this.Ano = new List<KeyValuePair<string, int>>();
            this.Tipo = new List<KeyValuePair<string, int>>();
            this.Servidor = new List<KeyValuePair<string, int>>();
        }
    }
}
