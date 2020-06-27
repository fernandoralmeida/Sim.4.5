using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    enum TipoOrigem { Executivo = 1, Legislativo }
    class mTipoOrigem
    {
        public string Autal(int valor)
        {
            string retornar = string.Empty;

            switch (valor)
            {
                case (int)TipoOrigem.Executivo:
                    retornar = "EXECUTIVO";
                    break;
                case (int)TipoOrigem.Legislativo:
                    retornar = "LEGISLATIVO";
                    break;
            }
            return retornar;
        }
    }
}
