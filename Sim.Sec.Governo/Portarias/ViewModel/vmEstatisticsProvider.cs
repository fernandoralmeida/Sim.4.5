using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace Sim.Sec.Governo.Portarias.ViewModel
{
    using Sql;
    using Model;    

    public class vmEstatisticsProvider
    {

        mEstatistics RE = new mEstatistics();

        public mEstatistics Relatories { get { return RE; } }

        public void Reset()
        {
            RE.Clear();
        }

        public void GoStatistics(List<object> obj)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {

                RE.Clear();

                List<mClassificacao> tipos = new mData().ListaGenerica(SqlCollections.Classi_With_Blocked);

                List<string> l_nomes = new List<string>();
                List<string> l_ano = new List<string>();
                List<string> l_tipos = new List<string>();

                string nome = "%";

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Data1", (string)obj[0]);
                dataAccess.AddParameters("@Data2", (string)obj[1]);
                dataAccess.AddParameters("@Tipo", (string)obj[2]);
                dataAccess.AddParameters("@Nome", "%" + (string)obj[3] + "%");

                foreach(DataRow dr in dataAccess.Read(SqlCollections.RelatoriesByParameters).Rows)
                {
                    l_ano.Add(Convert.ToDateTime(dr["Data"]).Year.ToString());

                    string[] linha = dr["Servidor"].ToString().Split(';');
                    List<string> func = new List<string>();
                    foreach (string l in linha)
                    {
                        if (l == string.Empty)
                            l_nomes.Add("SEM NOME");
                        else
                        {
                            if (nome != "%")
                            {
                                if (l == nome)
                                    l_nomes.Add(l);
                            }
                            else
                                l_nomes.Add(l);
                        }
                    }                    

                    for (int i = 0; i < tipos.Count;i++)
                    {
                        if(dr["Tipo"].ToString() == i.ToString())
                        {

                            if (tipos[i].Nome == string.Empty ||
                                tipos[i].Nome == "...")
                                l_tipos.Add("SEM CLASSIFICAÇÃO");
                            else
                                l_tipos.Add(tipos[i].Nome.ToUpper());
                        }
                    }
                }

                var c_tipo = from x in l_tipos
                            group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in c_tipo)
                {
                    RE.Tipo.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_nome = from x in l_nomes
                            group x by x into g
                            let count = g.Count()
                            orderby count descending
                            select new { Value = g.Key, Count = count };

                foreach (var x in c_nome)
                {
                    RE.Servidor.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_ano = from x in l_ano
                             group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in c_ano)
                {
                    RE.Ano.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataAccess = null;
            }
        }
    }
}
