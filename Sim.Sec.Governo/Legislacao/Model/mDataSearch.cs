using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace Sim.Sec.Governo.Legislacao.Model
{

    using Sim.Data;
    using SqlCommands;

    enum TypeSearch { Simple, Detailed }

    class mDataSearch
    {
        public ObservableCollection<mLegislacaoConsulta> Lista { get; set; }

        public void GoSearch(TypeSearch searchtype, List<object> obj, BackgroundWorker bgk)
        {

            ObservableCollection<mLegislacaoConsulta> listdoc = new ObservableCollection<mLegislacaoConsulta>();
            
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            //System.Windows.MessageBox.Show(searchtype.ToString());
                        
            string tipo = (string)obj[0];
            int numero = (int)obj[1]; 
            string classificacao = (string)obj[2];
            string origem = (string)obj[3];
            string situacao = (string)obj[4];            
            string autor = (string)obj[5];
            string resumo = (string)obj[6];
            string dataI = obj[7].ToString();
            string dataF = obj[8].ToString();

            if (tipo == "...")
                tipo = "%";

            if (resumo == null || resumo == string.Empty)
                resumo = "%";

            if (classificacao == "0" || classificacao == null)
                classificacao = "%";

            if (situacao == "0" || situacao == null)
                situacao = "%";

            if (origem == "0" || origem == null)
                origem = "%";

            if (autor == null || autor == string.Empty)
                autor = "%";
               
            try
            {

                string SqlCommand = string.Empty;

                switch (searchtype)
                {
                    case TypeSearch.Simple:
                        dataAccess.ClearParameters();
                        dataAccess.AddParameters("@Tipo", tipo);
                        dataAccess.AddParameters("@Numero", numero);
                        dataAccess.AddParameters("@Complemento", "%");
                        SqlCommand = SqlCollections.SelectSimples;
                        break;
                    case TypeSearch.Detailed:
                        dataAccess.ClearParameters();
                        dataAccess.AddParameters("@Tipo", tipo);
                        dataAccess.AddParameters("@Data1", dataI);
                        dataAccess.AddParameters("@Data2", dataF);
                        dataAccess.AddParameters("@Resumo", resumo);
                        dataAccess.AddParameters("@Classificado", classificacao);
                        dataAccess.AddParameters("@Situacao", situacao);
                        dataAccess.AddParameters("@Origem", origem);
                        dataAccess.AddParameters("@Autor", autor);
                        SqlCommand = SqlCollections.SelectDetalhado;
                        break;
                }
                
                var Acoes = dataAccess.Read(SqlCollections.AcoesSemFiltro);

                int vreg = dataAccess.Read(SqlCommand).Rows.Count;

                mTipoClassificacao TipoClass = new mTipoClassificacao();

                int progresso = 0;
                int cont_reg = 0;

                foreach (DataRow leg in dataAccess.Read(SqlCommand).Rows)
                {

                    cont_reg += 1;

                    var legislacao = new mLegislacaoConsulta();

                    legislacao.Indice = (int)leg["Indice"];
                    legislacao.Tipo = leg["Tipo"].ToString();

                    legislacao.Numero = (int)leg["Numero"];
                    legislacao.Complemento = leg["Complemento"].ToString();
                    legislacao.Data = (DateTime)leg["Data"];
                    legislacao.Publicado = leg["Publicado"].ToString();
                    legislacao.Resumo = leg["Resumo"].ToString();

                    legislacao.Classificacao = TipoClass.ToName(legislacao.Tipo, (int)leg["Classificado"]).ToUpper();

                    legislacao.Link = mLink.Create(legislacao.Tipo, legislacao.Data.Year.ToString(), legislacao.Numero);

                    legislacao.Situacao = new mTipoSituacao().Autal((int)leg["Situacao"]);

                    legislacao.Origem = new mTipoOrigem().Autal((int)leg["Origem"]);
                    legislacao.Autor = leg["Autor"].ToString();
                    legislacao.Cadastro = (DateTime)leg["Cadastro"];
                    legislacao.Atualizado = (DateTime)leg["Atualizado"];
                    legislacao.Excluido = (bool)leg["Excluido"];
                    
                    //var listaAcoes = new Model.ViewAcoesCollections();
                    var listaAcoesExercidas = new List<mAcoesConsulta>();
                    var listaAcoesRecebidas = new List<mAcoesConsulta>();

                    foreach (DataRow aed in Acoes.Rows)
                    {

                        if ((int)aed["NumeroAlvo"] == legislacao.Numero &&
                            aed["TipoAlvo"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                            aed["ComplementoAlvo"].ToString().ToLower() == legislacao.Complemento.ToLower())
                        {
                            var acoes = new mAcoesConsulta();
                            acoes.TipoAlvo = aed["TipoOrigem"].ToString().ToUpper();
                            acoes.NumeroAlvo = (int)aed["NumeroOrigem"];
                            acoes.ComplementoAlvo = aed["ComplementoOrigem"].ToString();
                            acoes.DataAlvo = (DateTime)aed["DataOrigem"];
                            acoes.Acao = new mAcoesRecebidas().Recebidas((int)(aed["AcaoExecutada"]));
                            acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                            listaAcoesRecebidas.Add(acoes);
                        }

                        if ((int)aed["NumeroOrigem"] == legislacao.Numero &&
                            aed["TipoOrigem"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                            aed["ComplementoOrigem"].ToString().ToLower() == legislacao.Complemento.ToLower())
                        {
                            var acoes = new mAcoesConsulta();
                            acoes.TipoAlvo = aed["TipoAlvo"].ToString().ToUpper();
                            acoes.NumeroAlvo = (int)aed["NumeroAlvo"];
                            acoes.ComplementoAlvo = aed["ComplementoAlvo"].ToString();
                            acoes.DataAlvo = (DateTime)aed["DataAlvo"];
                            acoes.Acao = new mAcoesExercidas().Exercidas((int)(aed["AcaoExecutada"]));
                            acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                            listaAcoesExercidas.Add(acoes);
                        }
                    }

                    
                    progresso = ((cont_reg + 1) * 100) / vreg;
                    bgk.ReportProgress(progresso);
                    

                    if (bgk.CancellationPending == true)
                        break;

                    legislacao.ListaAcoesExercidas = listaAcoesExercidas;
                    legislacao.ListaAcoesRecebidas = listaAcoesRecebidas;

                    listdoc.Add(legislacao);                    
                }

                Lista = listdoc;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataAccess = null;
            }
        }

        public ObservableCollection<mLegislacaoConsulta> Legislacao_S(List<object> obj)
        {
            try
            {
                var _list = new ObservableCollection<mLegislacaoConsulta>();
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);
                var TipoClass = new mTipoClassificacao();

                string sqlAcoes = @"SELECT * FROM  Leg_Acoes WHERE (NumeroOrigem = @Numero) OR (NumeroAlvo = @Numero)";
                string sqlLeg = @"SELECT * FROM Legislacao WHERE (Tipo LIKE @Tipo) AND (Numero = @Numero) AND (Complemento LIKE @Complemento) AND (Excluido = 0) ORDER BY Data DESC, Numero DESC";

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Numero", (int)obj[3]);
                
                var Acoes = dataAccess.Read(sqlAcoes);                
                
                #region Leis
                
                if ((bool)obj[0])
                {

                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Tipo", "LEI");
                    dataAccess.AddParameters("@Numero", (int)obj[3]);
                    dataAccess.AddParameters("@Complemento", "%");                              

                    foreach (DataRow leis in dataAccess.Read(sqlLeg).Rows)
                    {

                        var legislacao = new mLegislacaoConsulta();

                        legislacao.Indice = (int)leis["Indice"];
                        legislacao.Tipo = leis["Tipo"].ToString();

                        legislacao.Numero = (int)leis["Numero"];
                        legislacao.Complemento = leis["Complemento"].ToString();
                        legislacao.Data = (DateTime)leis["Data"];
                        legislacao.Publicado = leis["Publicado"].ToString();
                        legislacao.Resumo = leis["Resumo"].ToString();

                        legislacao.Classificacao = TipoClass.ToName(legislacao.Tipo, (int)leis["Classificado"]).ToUpper();

                        legislacao.Link = mLink.Create(legislacao.Tipo, legislacao.Data.Year.ToString(), legislacao.Numero);

                        legislacao.Situacao = new mTipoSituacao().Autal((int)leis["Situacao"]);

                        legislacao.Origem = new mTipoOrigem().Autal((int)leis["Origem"]);
                        legislacao.Autor = leis["Autor"].ToString();
                        legislacao.Cadastro = (DateTime)leis["Cadastro"];
                        legislacao.Atualizado = (DateTime)leis["Atualizado"];
                        legislacao.Excluido = (bool)leis["Excluido"];

                        //var listaAcoes = new Model.ViewAcoesCollections();
                        var listaAcoesExercidas = new List<mAcoesConsulta>();
                        var listaAcoesRecebidas = new List<mAcoesConsulta>();

                        foreach (DataRow aed in Acoes.Rows)
                        {

                            if ((int)aed["NumeroAlvo"] == legislacao.Numero &&
                                aed["TipoAlvo"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoAlvo"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoOrigem"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroOrigem"];
                                acoes.ComplementoAlvo = aed["ComplementoOrigem"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataOrigem"];
                                acoes.Acao = new mAcoesRecebidas().Recebidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesRecebidas.Add(acoes);
                            }

                            if ((int)aed["NumeroOrigem"] == legislacao.Numero &&
                                aed["TipoOrigem"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoOrigem"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoAlvo"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroAlvo"];
                                acoes.ComplementoAlvo = aed["ComplementoAlvo"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataAlvo"];
                                acoes.Acao = new mAcoesExercidas().Exercidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesExercidas.Add(acoes);
                            }
                        }

                        legislacao.ListaAcoesExercidas = listaAcoesExercidas;
                        legislacao.ListaAcoesRecebidas = listaAcoesRecebidas;

                        _list.Add(legislacao);
                    }

                }

                #endregion

                #region Leis Complementares

                if ((bool)obj[1])
                {

                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Tipo", "LEI COMPLEMENTAR");
                    dataAccess.AddParameters("@Numero", (int)obj[3]);
                    dataAccess.AddParameters("@Complemento", "%");

                    foreach (DataRow leis in dataAccess.Read(sqlLeg).Rows)
                    {

                        var legislacao = new mLegislacaoConsulta();

                        legislacao.Indice = (int)leis["Indice"];
                        legislacao.Tipo = leis["Tipo"].ToString();

                        legislacao.Numero = (int)leis["Numero"];
                        legislacao.Complemento = leis["Complemento"].ToString();
                        legislacao.Data = (DateTime)leis["Data"];
                        legislacao.Publicado = leis["Publicado"].ToString();
                        legislacao.Resumo = leis["Resumo"].ToString();

                        legislacao.Classificacao = TipoClass.ToName(legislacao.Tipo, (int)leis["Classificado"]).ToUpper();

                        legislacao.Link = mLink.Create(legislacao.Tipo, legislacao.Data.Year.ToString(), legislacao.Numero);

                        legislacao.Situacao = new mTipoSituacao().Autal((int)leis["Situacao"]);

                        legislacao.Origem = new mTipoOrigem().Autal((int)leis["Origem"]);
                        legislacao.Autor = leis["Autor"].ToString();
                        legislacao.Cadastro = (DateTime)leis["Cadastro"];
                        legislacao.Atualizado = (DateTime)leis["Atualizado"];
                        legislacao.Excluido = (bool)leis["Excluido"];

                        //var listaAcoes = new Model.ViewAcoesCollections();
                        var listaAcoesExercidas = new List<mAcoesConsulta>();
                        var listaAcoesRecebidas = new List<mAcoesConsulta>();

                        foreach (DataRow aed in Acoes.Rows)
                        {

                            if ((int)aed["NumeroAlvo"] == legislacao.Numero &&
                                aed["TipoAlvo"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoAlvo"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoOrigem"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroOrigem"];
                                acoes.ComplementoAlvo = aed["ComplementoOrigem"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataOrigem"];
                                acoes.Acao = new mAcoesRecebidas().Recebidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesRecebidas.Add(acoes);
                            }

                            if ((int)aed["NumeroOrigem"] == legislacao.Numero &&
                                aed["TipoOrigem"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoOrigem"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoAlvo"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroAlvo"];
                                acoes.ComplementoAlvo = aed["ComplementoAlvo"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataAlvo"];
                                acoes.Acao = new mAcoesExercidas().Exercidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesExercidas.Add(acoes);
                            }
                        }

                        legislacao.ListaAcoesExercidas = listaAcoesExercidas;
                        legislacao.ListaAcoesRecebidas = listaAcoesRecebidas;

                        _list.Add(legislacao);
                    }

                }

                #endregion

                #region Decretos

                if ((bool)obj[2])
                {

                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Tipo", "DECRETO");
                    dataAccess.AddParameters("@Numero", (int)obj[3]);
                    dataAccess.AddParameters("@Complemento", "%");

                    foreach (DataRow leis in dataAccess.Read(sqlLeg).Rows)
                    {

                        var legislacao = new mLegislacaoConsulta();

                        legislacao.Indice = (int)leis["Indice"];
                        legislacao.Tipo = leis["Tipo"].ToString();

                        legislacao.Numero = (int)leis["Numero"];
                        legislacao.Complemento = leis["Complemento"].ToString();
                        legislacao.Data = (DateTime)leis["Data"];
                        legislacao.Publicado = leis["Publicado"].ToString();
                        legislacao.Resumo = leis["Resumo"].ToString();

                        legislacao.Classificacao = TipoClass.ToName(legislacao.Tipo, (int)leis["Classificado"]).ToUpper();

                        legislacao.Link = mLink.Create(legislacao.Tipo, legislacao.Data.Year.ToString(), legislacao.Numero);

                        legislacao.Situacao = new mTipoSituacao().Autal((int)leis["Situacao"]);

                        legislacao.Origem = new mTipoOrigem().Autal((int)leis["Origem"]);
                        legislacao.Autor = leis["Autor"].ToString();
                        legislacao.Cadastro = (DateTime)leis["Cadastro"];
                        legislacao.Atualizado = (DateTime)leis["Atualizado"];
                        legislacao.Excluido = (bool)leis["Excluido"];

                        //var listaAcoes = new Model.ViewAcoesCollections();
                        var listaAcoesExercidas = new List<mAcoesConsulta>();
                        var listaAcoesRecebidas = new List<mAcoesConsulta>();

                        foreach (DataRow aed in Acoes.Rows)
                        {

                            if ((int)aed["NumeroAlvo"] == legislacao.Numero &&
                                aed["TipoAlvo"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoAlvo"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoOrigem"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroOrigem"];
                                acoes.ComplementoAlvo = aed["ComplementoOrigem"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataOrigem"];
                                acoes.Acao = new mAcoesRecebidas().Recebidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesRecebidas.Add(acoes);
                            }

                            if ((int)aed["NumeroOrigem"] == legislacao.Numero &&
                                aed["TipoOrigem"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoOrigem"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoAlvo"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroAlvo"];
                                acoes.ComplementoAlvo = aed["ComplementoAlvo"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataAlvo"];
                                acoes.Acao = new mAcoesExercidas().Exercidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesExercidas.Add(acoes);
                            }
                        }

                        legislacao.ListaAcoesExercidas = listaAcoesExercidas;
                        legislacao.ListaAcoesRecebidas = listaAcoesRecebidas;

                        _list.Add(legislacao);
                    }

                }

                #endregion

                return _list;

            }
            catch
            {
                return null;
            }
            
        }

        public ObservableCollection<mLegislacaoConsulta> Legislacao_C(List<object> obj)
        {
            try
            {
                var _list = new ObservableCollection<mLegislacaoConsulta>();
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);
                var TipoClass = new mTipoClassificacao();

                string sqlAcoes = @"SELECT * FROM  Leg_Acoes";
                string sqlLeg = @"SELECT * FROM Legislacao WHERE 
                                (Tipo LIKE @Tipo) AND (Data BETWEEN @Data1 AND @Data2) AND
                                (Classificado LIKE @Classificado) AND
                                (Resumo LIKE '%' +  @Resumo + '%') AND 
                                (Situacao LIKE @Situacao) AND (Origem LIKE @Origem) AND 
                                (Autor LIKE '%' + @Autor + '%') AND (Excluido = 0)
                                ORDER BY Numero DESC";

                var Acoes = dataAccess.Read(sqlAcoes);

                #region Leis

                if ((bool)obj[0])
                {

                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Tipo", "LEI");
                    dataAccess.AddParameters("@Data1", (string)obj[3]);
                    dataAccess.AddParameters("@Data2", (string)obj[4]);
                    dataAccess.AddParameters("@Classificado", (string)obj[9]);
                    dataAccess.AddParameters("@Resumo", (string)obj[5]);
                    dataAccess.AddParameters("@Situacao", (string)obj[6]);
                    dataAccess.AddParameters("@Origem", (string)obj[7]);
                    dataAccess.AddParameters("@Autor", (string)obj[8]);

                    foreach (DataRow leis in dataAccess.Read(sqlLeg).Rows)
                    {

                        var legislacao = new mLegislacaoConsulta();

                        legislacao.Indice = (int)leis["Indice"];
                        legislacao.Tipo = leis["Tipo"].ToString();

                        legislacao.Numero = (int)leis["Numero"];
                        legislacao.Complemento = leis["Complemento"].ToString();
                        legislacao.Data = (DateTime)leis["Data"];
                        legislacao.Publicado = leis["Publicado"].ToString();
                        legislacao.Resumo = leis["Resumo"].ToString();

                        legislacao.Classificacao = TipoClass.ToName(legislacao.Tipo, (int)leis["Classificado"]).ToUpper();

                        legislacao.Link = mLink.Create(legislacao.Tipo, legislacao.Data.Year.ToString(), legislacao.Numero);

                        legislacao.Situacao = new mTipoSituacao().Autal((int)leis["Situacao"]);

                        legislacao.Origem = new mTipoOrigem().Autal((int)leis["Origem"]);
                        legislacao.Autor = leis["Autor"].ToString();
                        legislacao.Cadastro = (DateTime)leis["Cadastro"];
                        legislacao.Atualizado = (DateTime)leis["Atualizado"];
                        legislacao.Excluido = (bool)leis["Excluido"];

                        //var listaAcoes = new Model.ViewAcoesCollections();
                        var listaAcoesExercidas = new List<mAcoesConsulta>();
                        var listaAcoesRecebidas = new List<mAcoesConsulta>();

                        foreach (DataRow aed in Acoes.Rows)
                        {

                            if ((int)aed["NumeroAlvo"] == legislacao.Numero &&
                                aed["TipoAlvo"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoAlvo"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoOrigem"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroOrigem"];
                                acoes.ComplementoAlvo = aed["ComplementoOrigem"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataOrigem"];
                                acoes.Acao = new mAcoesRecebidas().Recebidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesRecebidas.Add(acoes);
                            }

                            if ((int)aed["NumeroOrigem"] == legislacao.Numero &&
                                aed["TipoOrigem"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoOrigem"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoAlvo"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroAlvo"];
                                acoes.ComplementoAlvo = aed["ComplementoAlvo"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataAlvo"];
                                acoes.Acao = new mAcoesExercidas().Exercidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesExercidas.Add(acoes);
                            }
                        }

                        legislacao.ListaAcoesExercidas = listaAcoesExercidas;
                        legislacao.ListaAcoesRecebidas = listaAcoesRecebidas;

                        _list.Add(legislacao);
                    }

                }

                #endregion

                #region Leis Complementares

                if ((bool)obj[1])
                {

                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Tipo", "LEI COMPLEMENTAR");
                    dataAccess.AddParameters("@Data1", (string)obj[3]);
                    dataAccess.AddParameters("@Data2", (string)obj[4]);
                    dataAccess.AddParameters("@Classificado", (string)obj[9]);
                    dataAccess.AddParameters("@Resumo", (string)obj[5]);
                    dataAccess.AddParameters("@Situacao", (string)obj[6]);
                    dataAccess.AddParameters("@Origem", (string)obj[7]);
                    dataAccess.AddParameters("@Autor", (string)obj[8]);

                    foreach (DataRow leis in dataAccess.Read(sqlLeg).Rows)
                    {

                        var legislacao = new mLegislacaoConsulta();

                        legislacao.Indice = (int)leis["Indice"];
                        legislacao.Tipo = leis["Tipo"].ToString();

                        legislacao.Numero = (int)leis["Numero"];
                        legislacao.Complemento = leis["Complemento"].ToString();
                        legislacao.Data = (DateTime)leis["Data"];
                        legislacao.Publicado = leis["Publicado"].ToString();
                        legislacao.Resumo = leis["Resumo"].ToString();

                        legislacao.Classificacao = TipoClass.ToName(legislacao.Tipo, (int)leis["Classificado"]).ToUpper();

                        legislacao.Link = mLink.Create(legislacao.Tipo, legislacao.Data.Year.ToString(), legislacao.Numero);

                        legislacao.Situacao = new mTipoSituacao().Autal((int)leis["Situacao"]);

                        legislacao.Origem = new mTipoOrigem().Autal((int)leis["Origem"]);
                        legislacao.Autor = leis["Autor"].ToString();
                        legislacao.Cadastro = (DateTime)leis["Cadastro"];
                        legislacao.Atualizado = (DateTime)leis["Atualizado"];
                        legislacao.Excluido = (bool)leis["Excluido"];

                        //var listaAcoes = new Model.ViewAcoesCollections();
                        var listaAcoesExercidas = new List<mAcoesConsulta>();
                        var listaAcoesRecebidas = new List<mAcoesConsulta>();

                        foreach (DataRow aed in Acoes.Rows)
                        {

                            if ((int)aed["NumeroAlvo"] == legislacao.Numero &&
                                aed["TipoAlvo"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoAlvo"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoOrigem"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroOrigem"];
                                acoes.ComplementoAlvo = aed["ComplementoOrigem"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataOrigem"];
                                acoes.Acao = new mAcoesRecebidas().Recebidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesRecebidas.Add(acoes);
                            }

                            if ((int)aed["NumeroOrigem"] == legislacao.Numero &&
                                aed["TipoOrigem"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoOrigem"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoAlvo"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroAlvo"];
                                acoes.ComplementoAlvo = aed["ComplementoAlvo"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataAlvo"];
                                acoes.Acao = new mAcoesExercidas().Exercidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesExercidas.Add(acoes);
                            }
                        }

                        legislacao.ListaAcoesExercidas = listaAcoesExercidas;
                        legislacao.ListaAcoesRecebidas = listaAcoesRecebidas;

                        _list.Add(legislacao);
                    }

                }

                #endregion

                #region Decretos

                if ((bool)obj[2])
                {

                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Tipo", "DECRETO");
                    dataAccess.AddParameters("@Data1", (string)obj[3]);
                    dataAccess.AddParameters("@Data2", (string)obj[4]);
                    dataAccess.AddParameters("@Classificado", (string)obj[10]);
                    dataAccess.AddParameters("@Resumo", (string)obj[5]);
                    dataAccess.AddParameters("@Situacao", (string)obj[6]);
                    dataAccess.AddParameters("@Origem", (string)obj[7]);
                    dataAccess.AddParameters("@Autor", (string)obj[8]);

                    foreach (DataRow leis in dataAccess.Read(sqlLeg).Rows)
                    {

                        var legislacao = new mLegislacaoConsulta();

                        legislacao.Indice = (int)leis["Indice"];
                        legislacao.Tipo = leis["Tipo"].ToString();

                        legislacao.Numero = (int)leis["Numero"];
                        legislacao.Complemento = leis["Complemento"].ToString();
                        legislacao.Data = (DateTime)leis["Data"];
                        legislacao.Publicado = leis["Publicado"].ToString();
                        legislacao.Resumo = leis["Resumo"].ToString();

                        legislacao.Classificacao = TipoClass.ToName(legislacao.Tipo, (int)leis["Classificado"]).ToUpper();

                        legislacao.Link = mLink.Create(legislacao.Tipo, legislacao.Data.Year.ToString(), legislacao.Numero);

                        legislacao.Situacao = new mTipoSituacao().Autal((int)leis["Situacao"]);

                        legislacao.Origem = new mTipoOrigem().Autal((int)leis["Origem"]);
                        legislacao.Autor = leis["Autor"].ToString();
                        legislacao.Cadastro = (DateTime)leis["Cadastro"];
                        legislacao.Atualizado = (DateTime)leis["Atualizado"];
                        legislacao.Excluido = (bool)leis["Excluido"];

                        //var listaAcoes = new Model.ViewAcoesCollections();
                        var listaAcoesExercidas = new List<mAcoesConsulta>();
                        var listaAcoesRecebidas = new List<mAcoesConsulta>();

                        foreach (DataRow aed in Acoes.Rows)
                        {

                            if ((int)aed["NumeroAlvo"] == legislacao.Numero &&
                                aed["TipoAlvo"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoAlvo"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoOrigem"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroOrigem"];
                                acoes.ComplementoAlvo = aed["ComplementoOrigem"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataOrigem"];
                                acoes.Acao = new mAcoesRecebidas().Recebidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesRecebidas.Add(acoes);
                            }

                            if ((int)aed["NumeroOrigem"] == legislacao.Numero &&
                                aed["TipoOrigem"].ToString().ToLower() == legislacao.Tipo.ToLower() &&
                                aed["ComplementoOrigem"].ToString().ToLower() == legislacao.Complemento.ToLower())
                            {
                                var acoes = new mAcoesConsulta();
                                acoes.TipoAlvo = aed["TipoAlvo"].ToString().ToUpper();
                                acoes.NumeroAlvo = (int)aed["NumeroAlvo"];
                                acoes.ComplementoAlvo = aed["ComplementoAlvo"].ToString();
                                acoes.DataAlvo = (DateTime)aed["DataAlvo"];
                                acoes.Acao = new mAcoesExercidas().Exercidas((int)(aed["AcaoExecutada"]));
                                acoes.Link = mLink.Create(acoes.TipoAlvo, acoes.DataAlvo.Year.ToString(), acoes.NumeroAlvo);
                                listaAcoesExercidas.Add(acoes);
                            }
                        }

                        legislacao.ListaAcoesExercidas = listaAcoesExercidas;
                        legislacao.ListaAcoesRecebidas = listaAcoesRecebidas;

                        _list.Add(legislacao);
                    }

                }

                #endregion

                return _list;

            }
            catch
            {
                return null;
            }

        }

        public ObservableCollection<mLegislacaoConsulta> LastRows()
        {
            try
            {
                var _list = new ObservableCollection<mLegislacaoConsulta>();

                var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

                var TipoClass = new mTipoClassificacao();

                string sqlLeg = @"SELECT TOP 20 * FROM Legislacao ORDER BY Cadastro DESC, Numero DESC";

                foreach (DataRow leis in dataAccess.Read(sqlLeg).Rows)
                {

                    var legislacao = new mLegislacaoConsulta();

                    legislacao.Indice = (int)leis["Indice"];
                    legislacao.Tipo = leis["Tipo"].ToString();

                    legislacao.Numero = (int)leis["Numero"];
                    legislacao.Complemento = leis["Complemento"].ToString();
                    legislacao.Data = (DateTime)leis["Data"];
                    legislacao.Publicado = leis["Publicado"].ToString();
                    legislacao.Resumo = leis["Resumo"].ToString();

                    legislacao.Classificacao = TipoClass.ToName(legislacao.Tipo, (int)leis["Classificado"]).ToUpper();

                    legislacao.Link = mLink.Create(legislacao.Tipo, legislacao.Data.Year.ToString(), legislacao.Numero);

                    legislacao.Situacao = new mTipoSituacao().Autal((int)leis["Situacao"]);

                    legislacao.Origem = new mTipoOrigem().Autal((int)leis["Origem"]);
                    legislacao.Autor = leis["Autor"].ToString();
                    legislacao.Cadastro = (DateTime)leis["Cadastro"];
                    legislacao.Atualizado = (DateTime)leis["Atualizado"];
                    legislacao.Excluido = (bool)leis["Excluido"];

                    _list.Add(legislacao);
                }
                
                return _list;

            }
            catch
            {
                return null;
            }
        }
    }
}
