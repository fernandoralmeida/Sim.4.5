using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    enum AcoesRecebidas { Alterada = 1, Revogado = 2, Regulamentado = 3 }
    class mAcoesRecebidas
    {
        public string Recebidas(int valor)
        {
            string retornar = string.Empty;

            switch (valor)
            {
                case (int)AcoesRecebidas.Alterada:
                    retornar = "Alterado por";
                    break;
                case (int)AcoesRecebidas.Revogado:
                    retornar = "Revogado por";
                    break;
                case (int)AcoesRecebidas.Regulamentado:
                    retornar = "Regulamentado por";
                    break;
            }
            return string.Format("{0}", retornar.ToUpper());
        }
    }
}
