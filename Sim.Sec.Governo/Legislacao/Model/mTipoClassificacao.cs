using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{

    using SqlCommands;

    class mTipoClassificacao
    {

        List<mTiposGenericos> LE = new List<mTiposGenericos>();
        List<mTiposGenericos> DE = new List<mTiposGenericos>();

        public mTipoClassificacao()
        {
            LE = new mListaTiposGenericos().GoList(SqlCollections.Class_L_All);
            DE = new mListaTiposGenericos().GoList(SqlCollections.Class_D_All);
        }

        public string ToName(string tipo, int value)
        {
            string retorno = string.Empty;

            switch (tipo)
            {
                case "LEI":
                    retorno = LE[value].Nome;
                    break;

                case "DECRETO":
                    retorno = DE[value].Nome;
                    break;

                case "LEI COMPLEMENTAR":
                    retorno = LE[value].Nome;
                    break;
            }

            return retorno;
        }
    }
}
