using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.ViewModel.Providers
{
    using SqlCommands;
    using Model;

    class vmDataReports
    {
        
        mRelatoryLeis LO = new mRelatoryLeis();
        mRelatoryDecretos DE = new mRelatoryDecretos();
        mRelatoryLeisComplementares LC = new mRelatoryLeisComplementares();

        public mRelatoryDecretos InfoDecretos()
        {
            return DE;
        }

        public mRelatoryLeis InfoLeisOrdinarias()
        {
            return LO;
        }

        public mRelatoryLeisComplementares InfoLeisComp()
        {
            return LC;
        }

        public void Reset()
        {
            DE.ResetValues();
            LO.ResetValues();
            LC.ResetValues();
        }

        public int GoRelatory(List<object> obj)
        {
            
            string sqlcommand_l = @"SELECT  * FROM Legislacao
WHERE(Tipo LIKE 'LEI')
AND(Data BETWEEN @Data1 AND @Data2)
AND(Situacao LIKE @Situacao)
AND(Origem LIKE @Origem) AND(Excluido = 0)
ORDER BY Data DESC, Numero DESC";

            string sqlcommand_lc = @"SELECT  * FROM Legislacao
WHERE(Tipo LIKE 'LEI COMPLEMENTAR')
AND(Data BETWEEN @Data1 AND @Data2)
AND(Situacao LIKE @Situacao)
AND(Origem LIKE @Origem) AND(Excluido = 0)
ORDER BY Data DESC, Numero DESC";

            string sqlcommand_d = @"SELECT  * FROM Legislacao
WHERE(Tipo LIKE 'DECRETO')
AND(Data BETWEEN @Data1 AND @Data2)
AND(Situacao LIKE @Situacao)
AND(Origem LIKE @Origem) AND(Excluido = 0)
ORDER BY Data DESC, Numero DESC";

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);
            int total = 0;

            DE.ResetValues();
            LO.ResetValues();
            LC.ResetValues();

            try
            {

                dataAccess.AddParameters("@Data1", (string)obj[3]);
                dataAccess.AddParameters("@Data2", (string)obj[4]);
                dataAccess.AddParameters("@Situacao", (string)obj[5]);
                dataAccess.AddParameters("@Origem", (string)obj[6]);

                var full_data = new System.Data.DataTable();
                var d_leis = new System.Data.DataTable();
                var d_leis_c = new System.Data.DataTable();
                var d_decs = new System.Data.DataTable();

                full_data = dataAccess.Read("SELECT  * FROM Legislacao WHERE (Tipo LIKE 'ZERO')");
                full_data.Clear();

                if ((bool)obj[0])
                    d_leis = dataAccess.Read(sqlcommand_l);

                if ((bool)obj[0])
                    d_leis_c = dataAccess.Read(sqlcommand_lc);

                if ((bool)obj[0])
                    d_decs = dataAccess.Read(sqlcommand_d);                               

                foreach (System.Data.DataRow rl in d_leis.Rows)
                    full_data.ImportRow(rl);

                foreach (System.Data.DataRow rlc in d_leis_c.Rows)
                    full_data.ImportRow(rlc);

                foreach (System.Data.DataRow rd in d_decs.Rows)
                    full_data.ImportRow(rd);

                d_leis.Clear(); 
                d_leis_c.Clear();
                d_decs.Clear();
                d_leis = null;
                d_leis_c = null;
                d_decs = null;

                List<mTiposGenericos> leisClassificacao = new List<mTiposGenericos>();
                List<mTiposGenericos> decretosClassificacao = new List<mTiposGenericos>();

                List<string> llotipo = new List<string>();
                List<string> llosituacao = new List<string>();
                List<string> lloorigem = new List<string>();
                List<string> lloautor = new List<string>();
                List<string> lloclass = new List<string>();

                List<string> llctipo = new List<string>();
                List<string> llcsituacao = new List<string>();
                List<string> llcorigem = new List<string>();
                List<string> llcautor = new List<string>();
                List<string> llcclass = new List<string>();

                List<string> ldctipo = new List<string>();
                List<string> ldcsituacao = new List<string>();
                List<string> ldcorigem = new List<string>();
                List<string> ldcautor = new List<string>();
                List<string> ldcclass = new List<string>();

                decretosClassificacao = new mListaTiposGenericos().GoList(SqlCollections.Class_D_All);
                leisClassificacao = new mListaTiposGenericos().GoList(SqlCollections.Class_L_All);

                int cont_reg = 0;
                int vreg = full_data.Rows.Count;

                foreach (System.Data.DataRow leg in full_data.Rows)
                {

                    cont_reg ++;

                    total = leg.Table.Rows.Count;

                    var legislacao = new mLegislacao();

                    legislacao.Tipo = leg["Tipo"].ToString();

                    legislacao.Classificacao = leg["Classificado"].ToString();

                    legislacao.Situacao = leg["Situacao"].ToString();

                    legislacao.Origem = leg["Origem"].ToString();

                    legislacao.Autor = leg["Autor"].ToString();

                    switch (legislacao.Tipo)
                    {
                        case "LEI":
                            llotipo.Add(legislacao.Tipo);

                            if (legislacao.Situacao == 1.ToString()) { llosituacao.Add("INALTERADA"); }

                            if (legislacao.Situacao == 2.ToString()) { llosituacao.Add("ALTERADA"); }

                            if (legislacao.Situacao == 3.ToString()) { llosituacao.Add("REVOGADA"); }

                            if (legislacao.Origem == 1.ToString()) { lloorigem.Add("EXECUTIVO"); }

                            if (legislacao.Origem == 2.ToString())
                            {
                                lloorigem.Add("LEGISLATIVO");
                            }


                            if(legislacao.Autor == string.Empty)
                                lloautor.Add("SEM AUTOR");
                            else
                                lloautor.Add(legislacao.Autor);

                            for (int i = 0; i < leisClassificacao.Count; i++)
                            {
                                if (legislacao.Classificacao == i.ToString())
                                {
                                    if (leisClassificacao[i].Nome == string.Empty ||
                                        leisClassificacao[i].Nome == "...")
                                        lloclass.Add("SEM CLASSIFICAÇÃO");
                                    else
                                        lloclass.Add(leisClassificacao[i].Nome.ToUpper());
                                }
                            }

                            break;

                        case "LEI COMPLEMENTAR":
                            llctipo.Add(legislacao.Tipo);
                            if (legislacao.Situacao == 1.ToString()) { llcsituacao.Add("INALTERADA"); }

                            if (legislacao.Situacao == 2.ToString()) { llcsituacao.Add("ALTERADA"); }

                            if (legislacao.Situacao == 3.ToString()) { llcsituacao.Add("REVOGADA"); }

                            if (legislacao.Origem == 1.ToString()) { llcorigem.Add("EXECUTIVO"); }

                            if (legislacao.Origem == 2.ToString())
                            {
                                llcorigem.Add("LEGISLATIVO");                                
                            }

                            if (legislacao.Autor == string.Empty)
                                llcautor.Add("SEM AUTOR");
                            else
                                llcautor.Add(legislacao.Autor);

                            for (int i = 0; i < leisClassificacao.Count; i++)
                            {
                                if (legislacao.Classificacao == i.ToString())
                                {
                                    if (leisClassificacao[i].Nome == string.Empty ||
                                        leisClassificacao[i].Nome == "...")
                                        llcclass.Add("SEM CLASSIFICAÇÃO");
                                    else
                                        llcclass.Add(leisClassificacao[i].Nome.ToUpper());
                                }
                            }

                            break;

                        case "DECRETO":
                            ldctipo.Add(legislacao.Tipo);

                            if (legislacao.Situacao == 1.ToString()) { ldcsituacao.Add("INALTERADO"); }

                            if (legislacao.Situacao == 2.ToString()) { ldcsituacao.Add("ALTERADO"); }

                            if (legislacao.Situacao == 3.ToString()) { ldcsituacao.Add("REVOGADO"); }

                            if (legislacao.Origem == 1.ToString()) { ldcorigem.Add("EXECUTIVO"); }

                            if (legislacao.Origem == 2.ToString())
                            {
                                ldcorigem.Add("LEGISLATIVO");                                
                            }

                            if (legislacao.Origem != 2.ToString() && legislacao.Origem != 1.ToString())
                            {
                                ldcorigem.Add("SEM ORIGEM");
                            }

                            if (legislacao.Autor == string.Empty)
                                ldcautor.Add("SEM AUTOR");
                            else
                                ldcautor.Add(legislacao.Autor);

                            for (int i = 0; i < decretosClassificacao.Count; i++)
                            {
                                if (legislacao.Classificacao == i.ToString())
                                {
                                    if (decretosClassificacao[i].Nome == string.Empty)
                                        ldcclass.Add("SEM CLASSIFICAÇÃO");
                                    else
                                        ldcclass.Add(decretosClassificacao[i].Nome.ToUpper());
                                }
                            }

                            break;
                    }
                    
                }

                // Lei
                var lotipo = from x in llotipo
                             group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in lotipo)
                {
                    LO.Tipo.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var losituacao = from x in llosituacao
                                 group x by x into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count };

                foreach (var x in losituacao)
                {
                    LO.Situacao.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var loorigem = from x in lloorigem
                               group x by x into g
                               let count = g.Count()
                               orderby count descending
                               select new { Value = g.Key, Count = count };

                foreach (var x in loorigem)
                {
                    LO.Origem.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var loautor = from x in lloautor
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in loautor)
                {
                    LO.Autor.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }


                var loclass = from x in lloclass
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in loclass)
                {
                    LO.Classificacao.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                // Leis Complementares
                var lctipo = from x in llctipo
                             group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in lctipo)
                {
                    LC.Tipo.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var lcsituacao = from x in llcsituacao
                                 group x by x into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count };

                foreach (var x in lcsituacao)
                {
                    LC.Situacao.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var lcorigem = from x in llcorigem
                               group x by x into g
                               let count = g.Count()
                               orderby count descending
                               select new { Value = g.Key, Count = count };

                foreach (var x in lcorigem)
                {
                    LC.Origem.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var lcautor = from x in llcautor
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in lcautor)
                {
                    LC.Autor.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var lcclass = from x in llcclass
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in lcclass)
                {
                    LC.Classificacao.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                //Decretos
                var detipo = from x in ldctipo
                             group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in detipo)
                {
                    DE.Tipo.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var desituacao = from x in ldcsituacao
                                 group x by x into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count };

                foreach (var x in desituacao)
                {
                    DE.Situacao.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var deorigem = from x in ldcorigem
                               group x by x into g
                               let count = g.Count()
                               orderby count descending
                               select new { Value = g.Key, Count = count };

                foreach (var x in deorigem)
                {
                    DE.Origem.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var deautor = from x in ldcautor
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in deautor)
                {
                    DE.Autor.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var declas = from x in ldcclass
                             group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in declas)
                {
                    DE.Classificacao.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }
                
                return total;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Sim.Alerta!");
                return 0;                
            }
        }

    }
}
