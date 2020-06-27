using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sim.Sec.Governo.Legislacao.Model
{
    class mListaTiposGenericos
    {
        public List<mTiposGenericos> GoList(string comandoSql)
        {
            var lst = new List<mTiposGenericos>();

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {

                foreach (DataRow dr in dataAccess.Read(comandoSql).Rows)
                {
                    var classe = new mTiposGenericos();
                    classe.Codigo = (int)dr[1];//Codigo
                    classe.Nome = (string)dr[2];//Nome
                    //classe.Cadastro = (DateTime)dr[3];
                    //classe.Alterado = (DateTime)dr[4];
                    classe.Bloqueado = (bool)dr[5];//Bloqueado
                    lst.Add(classe);
                }

            }
            catch
            {
                lst.Clear();
                var classe = new mTiposGenericos();
                classe.Codigo = 0;
                classe.Nome = "...";
                lst.Add(classe);
            }

            return lst;
        }
    }
}
