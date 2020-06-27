using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Account.Model
{
    public class mGenero
    {
        public string Sexo { get; set; }
        public string Valor { get; set; }

        public List<mGenero> Generos()
        {
            var l = new List<mGenero>();

            l.Add(new mGenero() { Sexo = "MASCULINO", Valor = "M" });
            l.Add(new mGenero() { Sexo = "FEMININO", Valor = "F" });

            return l;
        }
    }
}
