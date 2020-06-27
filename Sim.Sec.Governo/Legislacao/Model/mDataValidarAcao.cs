using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sim.Sec.Governo.Legislacao.Model
{
    class mDataValidarAcao
    {
        /// <summary>
        /// Verifica se documento alvo de uma acao existe no banco de dados Legislacao
        /// </summary>
        /// <param name="tipo">Lei, Decreto ou Lei Complementar</param>
        /// <param name="numero">999</param>
        /// <param name="complemento">A, B, C, etc...</param>
        /// <returns>Lista</returns>
        public List<string> Validate(string tipo, int numero, string complemento)
        {
            string sqlcommand = @"SELECT Tipo, Numero, Complemento, Data FROM Legislacao WHERE (Tipo LIKE @Tipo) AND (Numero = @Numero) AND (Complemento LIKE @Comp) AND (Excluido = 0)";
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            string comp = complemento;

            if (comp == null || comp == string.Empty)
                comp = "%";

            try
            {
                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Tipo", tipo);
                dataAccess.AddParameters("@Numero", numero);
                dataAccess.AddParameters("@Comp", comp);

                List<string> lista = new List<string> { };

                foreach (DataRow dr in dataAccess.Read(sqlcommand).Rows)
                {
                    lista.Add(dr["Tipo"].ToString());
                    lista.Add(dr["Numero"].ToString());
                    lista.Add(dr["Complemento"].ToString());
                    lista.Add(dr["Data"].ToString());
                }

                return lista;

            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
            finally
            {
                dataAccess = null;
            }
        }
    }
}
