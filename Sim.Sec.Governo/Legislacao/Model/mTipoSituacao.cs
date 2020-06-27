using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    enum TipoSituacao { EmVigor = 1, Alterada = 2, Revogada = 3 }
    class mTipoSituacao
    {
        public string Autal(int valor)
        {
            string retornar = string.Empty;

            switch (valor)
            {
                case (int)TipoSituacao.EmVigor:
                    retornar = "INALTERADO(A)";
                    break;
                case (int)TipoSituacao.Alterada:
                    retornar = "ALTERADO(A)";
                    break;
                case (int)TipoSituacao.Revogada:
                    retornar = "REVOGADO(A)";
                    break;
            }
            return retornar;
        }
    }
}
