using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    enum AcoesExercidas { Altera = 1, Revoga = 2, Regulamenta = 3 }
    class mAcoesExercidas
    {
        public string Exercidas(int valor)
        {
            string retornar = string.Empty;

            switch (valor)
            {
                case (int)AcoesExercidas.Altera:
                    retornar = "Altera";
                    break;
                case (int)AcoesExercidas.Revoga:
                    retornar = "Revoga";
                    break;
                case (int)AcoesExercidas.Regulamenta:
                    retornar = "Regulamenta";
                    break;
            }
            return string.Format("{0}", retornar.ToUpper());
        }
    }
}
