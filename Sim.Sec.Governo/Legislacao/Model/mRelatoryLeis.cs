using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    public class mRelatoryLeis : mRelatoryBase
    {
        public mRelatoryLeis()
        {
            this.Tipo = new List<KeyValuePair<string, int>>();
            this.Situacao = new List<KeyValuePair<string, int>>();
            this.Origem = new List<KeyValuePair<string, int>>();
            this.Autor = new List<KeyValuePair<string, int>>();
            this.Classificacao = new List<KeyValuePair<string, int>>();
        }
    }
}
