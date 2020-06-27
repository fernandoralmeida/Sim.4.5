 using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    using Mvvm.Observers;

    public enum Registro { Novo, Alteracao}
    
    public class mData : NotifyProperty
    {
        #region Consulta / Listas
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandsql"></param>
        /// <returns></returns>
        public ObservableCollection<mCNAE> CNAES(string commandsql)
        {
            try
            {
                ObservableCollection<mCNAE> list = new ObservableCollection<mCNAE>();

                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                foreach (DataRow dr in dataAccess.Read(commandsql).Rows)
                {
                    var line = new mCNAE();

                    line.Ocupacao = (string)dr[1];
                    line.CNAE = (string)dr[2]; 
                    line.Descricao = (string)dr[3];
                    line.ISS = (string)dr[4];
                    line.ICMS = (string)dr[5];
                    line.Ativo = (bool)dr[6]; 
                    list.Add(line);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public ObservableCollection<mTiposGenericos> Tipos(string commandsql)
        {
            try
            {
                ObservableCollection<mTiposGenericos> list = new ObservableCollection<mTiposGenericos>();

                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                foreach (DataRow dr in dataAccess.Read(commandsql).Rows)
                {
                    var line = new mTiposGenericos();

                    line.Valor = (int)dr[2]; //Valor
                    line.Nome = dr[1].ToString().ToUpper(); //Nome (Porte, Segmento, Situacao e UsoLocal)
                    line.Ativo = (bool)dr[3];  //Ativo
                    list.Add(line);
                }

                return list;
            }
            catch
            {
                return null;
            }            
        }
        
        public ObservableCollection<mViabilidade> Viabilidades(List<string> commands)
        {
            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                ObservableCollection<mViabilidade> _list = new ObservableCollection<mViabilidade>();

                var data_parecer_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_Viabilidade_Situacao ORDER BY Valor");

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@viabilidade", commands[2]);
                dataAccess.AddParameters("@logradouro", commands[3]);
                dataAccess.AddParameters("@valor1", commands[4]);
                dataAccess.AddParameters("@valor2", commands[5]);
                dataAccess.AddParameters("@requerente", commands[6]);

                string par = commands[0] + "\n" + commands[1] + "\n" + commands[2] + "\n" + commands[3] + "\n" + commands[4] + "\n" + commands[5];
                                
                string sql = @"SELECT * FROM SDT_SE_Viabilidade WHERE (Ativo = True) AND (Data BETWEEN @data1 AND @data2) AND (Viabilidade LIKE @viabilidade + '%') AND (Logradouro LIKE '%' + @logradouro + '%') AND (Parecer BETWEEN @valor1 AND @valor2) AND (Requerente LIKE '%' + @requerente + '%') ORDER BY Data";

                //System.Windows.MessageBox.Show(par);
                int cont = 1;
                foreach (DataRow dr in dataAccess.Read(sql).Rows)
                {
                    mViabilidade _vb = new mViabilidade();

                    _vb.Indice = (int)dr[0];
                    _vb.Protocolo = (string)dr[1];

                    string[] words = dr[2].ToString().Split('/');

                    _vb.Requerente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };
                    
                    _vb.CEP = (string)dr[3];
                    _vb.Logradouro = (string)dr[4];
                    _vb.Numero = (string)dr[5];
                    _vb.Complemento = (dr[6] != DBNull.Value) ? (string)dr[6] : "";
                    _vb.Bairro = (string)dr[7];
                    _vb.Municipio = (string)dr[8];
                    _vb.UF = (string)dr[9];
                    _vb.CTM = (string)dr[10];
                    _vb.Atividades = (string)dr[11];
                    _vb.TextoEmail = (string)dr[12];
                    _vb.SendMail = (bool)dr[13];
                    _vb.Data = (DateTime)dr[14];
                    _vb.DataParecer = (DateTime)dr[15];
                    _vb.Perecer = (int)dr[16];
                    _vb.PerecerString = (string)data_parecer_tipos.Rows[_vb.Perecer][1];
                    _vb.Motivo = (dr[17] != DBNull.Value) ? (string)dr[17] : "";
                    _vb.Operador = (string)dr[18];
                    _vb.RetornoCliente = (bool)dr[19];
                    _vb.DataRetorno = (dr[20] == DBNull.Value) ? new DateTime(2018, 1, 1): (DateTime)dr[20];
                    _vb.SemRetorno = (bool)dr[21];
                    _vb.Ativo = (bool)dr[22];

                    _vb.Contador = cont;
                    cont++;

                    _list.Add(_vb);
                }

                //System.Windows.MessageBox.Show(_list.Count.ToString());
                return _list;   
            }
            catch(Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mViabilidade> ViabilidadesNow(List<string> commands)
        {
            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                ObservableCollection<mViabilidade> _list = new ObservableCollection<mViabilidade>();

                var data_parecer_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_Viabilidade_Situacao ORDER BY Valor");

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@operador", commands[1]);
                dataAccess.AddParameters("@parecer", commands[0]);                

                //string par = commands[0] + "\n" + commands[1] + "\n" + commands[2] + "\n" + commands[3] + "\n" + commands[4] + "\n" + commands[5];

                string sql = @"SELECT * FROM SDT_SE_Viabilidade WHERE (Ativo = True) AND (Operador LIKE '%' +  @operador + '%') AND (SemRetorno = False) AND ((Parecer NOT LIKE @parecer) AND (RetornoCliente = False)) OR ((Parecer LIKE '1') AND (RetornoCliente = True)) ORDER BY Data";

                //System.Windows.MessageBox.Show(par);
                int cont = 1;
                foreach (DataRow dr in dataAccess.Read(sql).Rows)
                {
                    mViabilidade _vb = new mViabilidade();

                    _vb.Indice = (int)dr[0];
                    _vb.Protocolo = (string)dr[1];

                    string[] words = dr[2].ToString().Split('/');

                    _vb.Requerente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    _vb.CEP = (string)dr[3];
                    _vb.Logradouro = (string)dr[4];
                    _vb.Numero = (string)dr[5];
                    _vb.Complemento = (dr[6] != DBNull.Value) ? (string)dr[6] : "";
                    _vb.Bairro = (string)dr[7];
                    _vb.Municipio = (string)dr[8];
                    _vb.UF = (string)dr[9];
                    _vb.CTM = (string)dr[10];
                    _vb.Atividades = (string)dr[11];
                    _vb.TextoEmail = (string)dr[12];
                    _vb.SendMail = (bool)dr[13];
                    _vb.Data = (DateTime)dr[14];
                    _vb.DataParecer = (DateTime)dr[15];
                    _vb.Perecer = (int)dr[16];
                    _vb.PerecerString = (string)data_parecer_tipos.Rows[_vb.Perecer][1];
                    _vb.Motivo = (dr[17] != DBNull.Value) ? (string)dr[17] : "";
                    _vb.Operador = (string)dr[18];
                    _vb.RetornoCliente = (bool)dr[19];
                    _vb.DataRetorno = (dr[20] == DBNull.Value) ? new DateTime(2018, 1, 1) : (DateTime)dr[20];
                    _vb.SemRetorno = (bool)dr[21];
                    _vb.Ativo = (bool)dr[22];
                    _vb.Contador = cont;
                    cont++;
                    _list.Add(_vb);
                }

                //System.Windows.MessageBox.Show(_list.Count.ToString());
                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mViabilidade> ViabilidadesOperador(List<string> commands)
        {
            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                ObservableCollection<mViabilidade> _list = new ObservableCollection<mViabilidade>();

                var data_parecer_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_Viabilidade_Situacao ORDER BY Valor");

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@viabilidade", commands[2]);
                dataAccess.AddParameters("@logradouro", commands[3]);
                dataAccess.AddParameters("@valor1", commands[4]);
                dataAccess.AddParameters("@valor2", commands[5]);
                dataAccess.AddParameters("@operador", commands[7]);

                string par = commands[0] + "\n" + commands[1] + "\n" + commands[2] + "\n" + commands[3] + "\n" + commands[4] + "\n" + commands[5];

                string sql = @"SELECT * FROM SDT_SE_Viabilidade WHERE (Ativo = True) AND (Data BETWEEN @data1 AND @data2) AND (Viabilidade LIKE @viabilidade + '%') AND (Logradouro LIKE '%' + @logradouro + '%') AND (Parecer BETWEEN @valor1 AND @valor2) AND (Operador LIKE @operador) ORDER BY Data";

                //System.Windows.MessageBox.Show(par);
                int cont = 1;
                foreach (DataRow dr in dataAccess.Read(sql).Rows)
                {
                    mViabilidade _vb = new mViabilidade();

                    _vb.Indice = (int)dr[0];
                    _vb.Protocolo = (string)dr[1];

                    string[] words = dr[2].ToString().Split('/');

                    _vb.Requerente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    _vb.CEP = (string)dr[3];
                    _vb.Logradouro = (string)dr[4];
                    _vb.Numero = (string)dr[5];
                    _vb.Complemento = (dr[6] != DBNull.Value) ? (string)dr[6] : "";
                    _vb.Bairro = (string)dr[7];
                    _vb.Municipio = (string)dr[8];
                    _vb.UF = (string)dr[9];
                    _vb.CTM = (string)dr[10];
                    _vb.Atividades = (string)dr[11];
                    _vb.TextoEmail = (string)dr[12];
                    _vb.SendMail = (bool)dr[13];
                    _vb.Data = (DateTime)dr[14];
                    _vb.DataParecer = (DateTime)dr[15];
                    _vb.Perecer = (int)dr[16];
                    _vb.PerecerString = (string)data_parecer_tipos.Rows[_vb.Perecer][1];
                    _vb.Motivo = (dr[17] != DBNull.Value) ? (string)dr[17] : "";
                    _vb.Operador = (string)dr[18];
                    _vb.RetornoCliente = (bool)dr[19];
                    _vb.DataRetorno = (dr[20] == DBNull.Value) ? new DateTime(2018, 1, 1) : (DateTime)dr[20];
                    _vb.SemRetorno = (bool)dr[21];
                    _vb.Ativo = (bool)dr[22];
                    _vb.Contador = cont;
                    cont++;
                    _list.Add(_vb);
                }

                //System.Windows.MessageBox.Show(_list.Count.ToString());
                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mPJ_Ext> Empresas(List<object> commands)
        {
            try
            {
                ObservableCollection<mPJ_Ext> list = new ObservableCollection<mPJ_Ext>();

                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@porte1", commands[11]);
                dataAccess.AddParameters("@porte2", commands[12]);
                dataAccess.AddParameters("@uso1", commands[5]);
                dataAccess.AddParameters("@uso2", commands[6]);

                var data_fma = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Formalizada WHERE (Porte BETWEEN @porte1 AND @porte2) AND (UsoLocal BETWEEN @uso1 AND @uso2)");

                DataTable data_seg = new DataTable();

                DataTable data_seg_agro = new DataTable();
                DataTable data_seg_ind = new DataTable();
                DataTable data_seg_com = new DataTable();
                DataTable data_seg_serv = new DataTable();

                ObservableCollection<mSegmentos> list_of_seg = new ObservableCollection<mSegmentos>();

                if ((bool)commands[13])
                {

                    if ((bool)commands[7])
                    {
                        dataAccess.ClearParameters();
                        dataAccess.AddParameters("@agronegocio", commands[7]);
                        data_seg_agro = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento WHERE (Agronegocio = @agronegocio) AND (Industria = False) AND (Comercio = False) AND (Servicos = False)");
                        //System.Windows.MessageBox.Show(data_seg_agro.Rows.Count.ToString());
                    }

                    if ((bool)commands[8])
                    {
                        dataAccess.ClearParameters();
                        dataAccess.AddParameters("@industria", commands[8]);
                        data_seg_ind = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento WHERE (Industria = @industria) AND (Agronegocio = False) AND (Comercio = False) AND (Servicos = False)");
                        //System.Windows.MessageBox.Show(data_seg_ind.Rows.Count.ToString());
                    }

                    if ((bool)commands[9])
                    {
                        dataAccess.ClearParameters();
                        dataAccess.AddParameters("@comercio", commands[9]);
                        data_seg_com = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento WHERE (Comercio = @comercio) AND (Industria = False) AND (Agronegocio = False) AND (Servicos = False)");
                        //System.Windows.MessageBox.Show(data_seg_com.Rows.Count.ToString());
                    }

                    if ((bool)commands[10])
                    {
                        dataAccess.ClearParameters();
                        dataAccess.AddParameters("@servicos", commands[10]);
                        data_seg_serv = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento WHERE (Servicos = @servicos) AND (Industria = False) AND (Comercio = False) AND (Agronegocio = False)");
                        //System.Windows.MessageBox.Show(data_seg_serv.Rows.Count.ToString());
                    }

                    foreach (DataRow sg in data_seg_agro.Rows)
                    {
                        var nsg = new mSegmentos();
                        nsg.Indice = (int)sg[0];
                        nsg.CNPJ_CPF = (string)sg[1];
                        nsg.Agronegocio = (bool)sg[2];
                        nsg.Industria = (bool)sg[3];
                        nsg.Comercio = (bool)sg[4];
                        nsg.Servicos = (bool)sg[5];
                        nsg.Ativo = (bool)sg[6];
                        list_of_seg.Add(nsg);
                    }

                    foreach (DataRow sg in data_seg_ind.Rows)
                    {
                        var nsg = new mSegmentos();
                        nsg.Indice = (int)sg[0];
                        nsg.CNPJ_CPF = (string)sg[1];
                        nsg.Agronegocio = (bool)sg[2];
                        nsg.Industria = (bool)sg[3];
                        nsg.Comercio = (bool)sg[4];
                        nsg.Servicos = (bool)sg[5];
                        nsg.Ativo = (bool)sg[6];
                        list_of_seg.Add(nsg);
                    }

                    foreach (DataRow sg in data_seg_com.Rows)
                    {
                        var nsg = new mSegmentos();
                        nsg.Indice = (int)sg[0];
                        nsg.CNPJ_CPF = (string)sg[1];
                        nsg.Agronegocio = (bool)sg[2];
                        nsg.Industria = (bool)sg[3];
                        nsg.Comercio = (bool)sg[4];
                        nsg.Servicos = (bool)sg[5];
                        nsg.Ativo = (bool)sg[6];
                        list_of_seg.Add(nsg);
                    }

                    foreach (DataRow sg in data_seg_serv.Rows)
                    {
                        var nsg = new mSegmentos();
                        nsg.Indice = (int)sg[0];
                        nsg.CNPJ_CPF = (string)sg[1];
                        nsg.Agronegocio = (bool)sg[2];
                        nsg.Industria = (bool)sg[3];
                        nsg.Comercio = (bool)sg[4];
                        nsg.Servicos = (bool)sg[5];
                        nsg.Ativo = (bool)sg[6];
                        list_of_seg.Add(nsg);
                    }

                }

                if ((bool)commands[14])
                {
                    dataAccess.ClearParameters();

                    dataAccess.AddParameters("@agronegocio", commands[7]);
                    dataAccess.AddParameters("@industria", commands[8]);
                    dataAccess.AddParameters("@comercio", commands[9]);
                    dataAccess.AddParameters("@servicos", commands[10]);

                    data_seg = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento WHERE (Agronegocio = @agronegocio) AND (Industria = @industria) AND (Comercio = @comercio) AND (Servicos = @servicos)");
                }
                else
                    data_seg = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento");

                foreach (DataRow sg in data_seg.Rows)
                {
                    var nsg = new mSegmentos();
                    nsg.Indice = (int)sg[0];
                    nsg.CNPJ_CPF = (string)sg[1];
                    nsg.Agronegocio = (bool)sg[2];
                    nsg.Industria = (bool)sg[3];
                    nsg.Comercio = (bool)sg[4];
                    nsg.Servicos = (bool)sg[5];
                    nsg.Ativo = (bool)sg[6];
                    list_of_seg.Add(nsg);
                }

                dataAccess.ClearParameters();

                string _stc = (string)commands[15];

                if ((string)commands[15] == "...")
                    _stc = "%";
                
                dataAccess.AddParameters("@situacao", _stc);
                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@cnpj", commands[2]);
                dataAccess.AddParameters("@razaosocial", commands[3]);
                dataAccess.AddParameters("@atividadeprincipal", commands[4]);

                string sql = @"SELECT * FROM SDT_SE_PJ WHERE (Ativo = True) AND (SituacaoCadastral LIKE @situacao) AND (Cadastro BETWEEN @data1 AND @data2) AND (CNPJ LIKE @cnpj + '%') AND (RazaoSocial LIKE '%' + @razaosocial + '%') AND (AtividadePrincipal LIKE '%' + @atividadeprincipal + '%') ORDER BY RazaoSocial";

                int cont = 1;
                foreach (DataRow dr in dataAccess.Read(sql).Rows)
                {
                    var line = new mPJ_Ext();

                    line.Indice = (int)dr[0];
                    line.CNPJ = (string)dr[1];
                    line.MatrizFilial = (string)dr[2];
                    line.Abertura = (DateTime)dr[3];
                    line.RazaoSocial = (string)dr[4];
                    line.NomeFantasia = (string)dr[5];
                    line.NaturezaJuridica = (string)dr[6];
                    line.AtividadePrincipal = (string)dr[7];
                    line.AtividadeSecundaria = (string)dr[8];
                    line.SituacaoCadastral = (string)dr[9];
                    line.DataSituacaoCadastral = (DateTime)dr[10];

                    line.Logradouro = (string)dr[11];
                    line.Numero = (string)dr[12];
                    line.Complemento = (string)dr[13];
                    line.CEP = (string)dr[14];
                    line.Bairro = (string)dr[15];
                    line.Municipio = (string)dr[16];
                    line.UF = (string)dr[17];
                    line.Email = (string)dr[18];
                    line.Telefones = (string)dr[19];

                    line.Cadastro = (DateTime)dr[20];
                    line.Atualizado = (DateTime)dr[21];

                    line.Ativo = (bool)dr[22];

                    var _formalizada = from p in data_fma.AsEnumerable()
                               where p.Field<string>(1) == line.CNPJ
                               select p;

                    foreach (var fa in _formalizada)
                    {
                        var nfa = new mFormalizada();
                        nfa.Indice = (int)fa[0];
                        nfa.CNPJ = (string)fa[1];
                        nfa.InscricaoMunicipal = (string)fa[2];
                        nfa.Porte = (int)fa[3];
                        nfa.UsoLocal = (int)fa[4];
                        nfa.FormalizadoSE = (bool)fa[5];
                        nfa.Data = (DateTime)fa[6];
                        nfa.Ativo = (bool)fa[7];
                        line.Formalizada = nfa;
                    }

                    var _segmentos = from p in list_of_seg.AsEnumerable()
                                     where p.CNPJ_CPF == line.CNPJ
                                     select p;

                    foreach (var sg in _segmentos)
                    {
                        var nsg = new mSegmentos();
                        nsg.Indice = sg.Indice;
                        nsg.CNPJ_CPF = sg.CNPJ_CPF;
                        nsg.Agronegocio = sg.Agronegocio;
                        nsg.Industria = sg.Industria;
                        nsg.Comercio = sg.Comercio;
                        nsg.Servicos = sg.Servicos;
                        nsg.Ativo = sg.Ativo;
                        line.Segmentos = nsg;
                    }

                    list.Add(line);
                }

                ObservableCollection<mPJ_Ext> final_list = new ObservableCollection<mPJ_Ext>();

                var _data = from s in list_of_seg
                            from f in data_fma.AsEnumerable()
                            from d in list.AsEnumerable()
                            where s.CNPJ_CPF == d.CNPJ
                            where f.Field<string>(1) == d.CNPJ
                            select d;

                foreach (var dt in _data)
                {
                    var line = new mPJ_Ext();
                    line.Indice = dt.Indice;
                    line.CNPJ = dt.CNPJ;
                    line.MatrizFilial = dt.MatrizFilial;
                    line.Abertura = dt.Abertura;
                    line.RazaoSocial = dt.RazaoSocial;
                    line.NomeFantasia = dt.NomeFantasia;
                    line.NaturezaJuridica = dt.NaturezaJuridica;
                    line.AtividadePrincipal = dt.AtividadePrincipal;
                    line.AtividadeSecundaria = dt.AtividadeSecundaria;
                    line.SituacaoCadastral = dt.SituacaoCadastral;
                    line.DataSituacaoCadastral = dt.DataSituacaoCadastral;
                    line.Logradouro = dt.Logradouro;
                    line.Numero = dt.Numero;
                    line.Complemento = dt.Complemento;
                    line.CEP = dt.CEP;
                    line.Bairro = dt.Bairro;
                    line.Municipio = dt.Municipio;
                    line.UF = dt.UF;
                    line.Email = dt.Email;
                    line.Telefones = dt.Telefones;
                    line.Cadastro = dt.Cadastro;
                    line.Atualizado = dt.Atualizado;
                    line.Ativo = dt.Ativo;
                    line.Formalizada = dt.Formalizada;
                    line.Segmentos = dt.Segmentos;
                    line.Contador = cont;
                    final_list.Add(line);
                    cont++;
                }

                return final_list;
            }
            catch
            {
                return null;
            }
        }

        public ObservableCollection<mPJ_Ext> nEmpresas(List<object> commands)
        {
            string sqlcommand = string.Empty;

            string sqlstring1cnae =
@"SELECT SDT_SE_PJ.*, SDT_SE_PJ_Segmento.*, SDT_SE_PJ_Formalizada.*
FROM (((SDT_SE_PJ_Segmento INNER JOIN SDT_SE_PJ ON SDT_SE_PJ_Segmento.CNPJ = SDT_SE_PJ.CNPJ) INNER JOIN SDT_SE_PJ_Formalizada ON SDT_SE_PJ.CNPJ = SDT_SE_PJ_Formalizada.CNPJ) INNER JOIN SDT_SE_PJ_Porte ON SDT_SE_PJ_Formalizada.Porte = SDT_SE_PJ_Porte.Valor) INNER JOIN SDT_SE_PJ_UsoLocal ON SDT_SE_PJ_Formalizada.UsoLocal = SDT_SE_PJ_UsoLocal.Valor
WHERE (((SDT_SE_PJ_Formalizada.Ativo) = True) AND ((SDT_SE_PJ_Segmento.Ativo) = True) AND ((SDT_SE_PJ.Ativo) = True) AND ((SDT_SE_PJ.RazaoSocial) Like '%'+ @razaosocial +'%') AND ((SDT_SE_PJ.Logradouro) Like '%'+ @logradouro +'%') AND ((SDT_SE_PJ.Bairro) Like '%'+ @bairro +'%') AND ((SDT_SE_PJ_Segmento.CNPJ) Like '%'+ @cnpj +'%') AND ((SDT_SE_PJ.AtividadePrincipal) Like '%'+ @cnae1 +'%') AND ((SDT_SE_PJ.AtividadeSecundaria) Like '%'+ @cnae2 +'%') AND ((SDT_SE_PJ.Abertura) BETWEEN @data1 AND @data2) 
AND ((SDT_SE_PJ_Segmento.Agronegocio) Like @agro) AND ((SDT_SE_PJ_Segmento.Industria) Like @ind) AND ((SDT_SE_PJ_Segmento.Comercio) Like @comercio) AND ((SDT_SE_PJ_Segmento.Servicos) Like @servicos) AND ((SDT_SE_PJ_Formalizada.Porte) Like '%'+ @porte +'%') AND ((SDT_SE_PJ_Formalizada.UsoLocal) Like '%'+ @local +'%') AND ((SDT_SE_PJ.SituacaoCadastral) LIKE '%' + @situacao + '%')) ORDER BY RazaoSocial";

            string sqlstring2cnae =
@"SELECT SDT_SE_PJ.*, SDT_SE_PJ_Segmento.*, SDT_SE_PJ_Formalizada.*
FROM (((SDT_SE_PJ_Segmento INNER JOIN SDT_SE_PJ ON SDT_SE_PJ_Segmento.CNPJ = SDT_SE_PJ.CNPJ) INNER JOIN SDT_SE_PJ_Formalizada ON SDT_SE_PJ.CNPJ = SDT_SE_PJ_Formalizada.CNPJ) INNER JOIN SDT_SE_PJ_Porte ON SDT_SE_PJ_Formalizada.Porte = SDT_SE_PJ_Porte.Valor) INNER JOIN SDT_SE_PJ_UsoLocal ON SDT_SE_PJ_Formalizada.UsoLocal = SDT_SE_PJ_UsoLocal.Valor
WHERE (((SDT_SE_PJ_Formalizada.Ativo) = True) AND ((SDT_SE_PJ_Segmento.Ativo) = True) AND ((SDT_SE_PJ.Ativo) = True) AND ((SDT_SE_PJ.RazaoSocial) Like '%'+ @razaosocial +'%') AND ((SDT_SE_PJ.Logradouro) Like '%'+ @logradouro +'%') AND ((SDT_SE_PJ.Bairro) Like '%'+ @bairro +'%') AND ((SDT_SE_PJ_Segmento.CNPJ) Like '%'+ @cnpj +'%') AND ((SDT_SE_PJ.AtividadePrincipal) Like '%'+ @cnae1 +'%') OR ((SDT_SE_PJ.AtividadeSecundaria) Like '%'+ @cnae2 +'%') AND ((SDT_SE_PJ.Abertura) BETWEEN @data1 AND @data2) 
AND ((SDT_SE_PJ_Segmento.Agronegocio) Like @agro) AND ((SDT_SE_PJ_Segmento.Industria) Like @ind) AND ((SDT_SE_PJ_Segmento.Comercio) Like @comercio) AND ((SDT_SE_PJ_Segmento.Servicos) Like @servicos) AND ((SDT_SE_PJ_Formalizada.Porte) Like '%'+ @porte +'%') AND ((SDT_SE_PJ_Formalizada.UsoLocal) Like '%'+ @local +'%') AND ((SDT_SE_PJ.SituacaoCadastral) LIKE '%' + @situacao + '%')) ORDER BY RazaoSocial";


            if ((string)commands[3] == "%")
                sqlcommand = sqlstring1cnae;
            else
                sqlcommand = sqlstring2cnae;

            try
            {
                ObservableCollection<mPJ_Ext> list = new ObservableCollection<mPJ_Ext>();

                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@razaosocial", commands[0]);
                dataAccess.AddParameters("@logradouro", commands[13]);
                dataAccess.AddParameters("@bairro", commands[14]);
                dataAccess.AddParameters("@cnpj", commands[1]);
                dataAccess.AddParameters("@cnae1", commands[2]);
                dataAccess.AddParameters("@cnae2", commands[3]);
                dataAccess.AddParameters("@data1", commands[4]);
                dataAccess.AddParameters("@data2", commands[5]);
                dataAccess.AddParameters("@agro", commands[6]);
                dataAccess.AddParameters("@ind", commands[7]);
                dataAccess.AddParameters("@comercio", commands[8]);
                dataAccess.AddParameters("@servicos", commands[9]);
                dataAccess.AddParameters("@porte", commands[10].ToString());
                dataAccess.AddParameters("@local", commands[11]);
                dataAccess.AddParameters("@situacao", commands[12]);

                //string par = commands[0] + "\n" + commands[1] + "\n" + commands[2] + "\n" + commands[3] + "\n" + commands[4] + "\n" + commands[5] + "\n"
                //    + commands[6] + "\n" + commands[7] + "\n" + commands[8] + "\n" + commands[9] + "\n" + commands[10] + "\n" + commands[11] + "\n" + commands[12];

                //string sql = @"SELECT * FROM SDT_SE_PJ WHERE (Ativo = True) AND (Cadastro BETWEEN @data1 AND @data2) AND (CNPJ LIKE @cnpj + '%') AND (RazaoSocial LIKE '%' + @razaosocial + '%') AND (AtividadePrincipal LIKE '%' + @atividadeprincipal + '%') ORDER BY RazaoSocial";

                //System.Windows.MessageBox.Show(par);
                int cont = 1;
                foreach (DataRow dr in dataAccess.Read(sqlcommand).Rows)
                {
                    var line = new mPJ_Ext();
                    var nfa = new mFormalizada();
                    var nsg = new mSegmentos();

                    line.Indice = (int)dr[0];
                    line.CNPJ = (string)dr[1];
                    line.MatrizFilial = (string)dr[2];
                    line.Abertura = (DateTime)dr[3];
                    line.RazaoSocial = (string)dr[4];
                    line.NomeFantasia = (string)dr[5];
                    line.NaturezaJuridica = (string)dr[6];
                    line.AtividadePrincipal = (string)dr[7];
                    line.AtividadeSecundaria = (string)dr[8];
                    line.SituacaoCadastral = (string)dr[9];
                    line.DataSituacaoCadastral = (DateTime)dr[10];

                    line.Logradouro = (string)dr[11];
                    line.Numero = (string)dr[12];
                    line.Complemento = (string)dr[13];
                    line.CEP = (string)dr[14];
                    line.Bairro = (string)dr[15];
                    line.Municipio = (string)dr[16];
                    line.UF = (string)dr[17];
                    line.Email = (string)dr[18];
                    line.Telefones = (string)dr[19];

                    line.Cadastro = (DateTime)dr[20];
                    line.Atualizado = (DateTime)dr[21];

                    line.Ativo = (bool)dr[22];

                    nsg.Indice = (int)dr[23];
                    nsg.CNPJ_CPF = (string)dr[24];
                    nsg.Agronegocio = (bool)dr[25];
                    nsg.Industria = (bool)dr[26];
                    nsg.Comercio = (bool)dr[27];
                    nsg.Servicos = (bool)dr[28];
                    nsg.Ativo = (bool)dr[29];
                    line.Segmentos = nsg;

                    nfa.Indice = (int)dr[30];
                    nfa.CNPJ = (string)dr[31];
                    nfa.InscricaoMunicipal = (string)dr[32];
                    nfa.Porte = (int)dr[33];
                    nfa.UsoLocal = (int)dr[34];
                    nfa.FormalizadoSE = (bool)dr[35];
                    nfa.Data = (DateTime)dr[36];
                    nfa.Ativo = (bool)dr[37];
                    line.Formalizada = nfa;

                    line.Contador = cont;
                    list.Add(line);
                    cont++;

                }

                return list;
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }

        public ObservableCollection<mPJ_Ext> nEmpresasNow()
        {
            string sqlcommand = string.Empty;

            string sqlstring2cnae =
@"SELECT TOP 10 SDT_SE_PJ.*, SDT_SE_PJ_Segmento.*, SDT_SE_PJ_Formalizada.*
FROM (((SDT_SE_PJ_Segmento INNER JOIN SDT_SE_PJ ON SDT_SE_PJ_Segmento.CNPJ = SDT_SE_PJ.CNPJ) INNER JOIN SDT_SE_PJ_Formalizada ON SDT_SE_PJ.CNPJ = SDT_SE_PJ_Formalizada.CNPJ) INNER JOIN SDT_SE_PJ_Porte ON SDT_SE_PJ_Formalizada.Porte = SDT_SE_PJ_Porte.Valor) INNER JOIN SDT_SE_PJ_UsoLocal ON SDT_SE_PJ_Formalizada.UsoLocal = SDT_SE_PJ_UsoLocal.Valor
WHERE (((SDT_SE_PJ_Formalizada.Ativo) = True) AND ((SDT_SE_PJ_Segmento.Ativo) = True) AND ((SDT_SE_PJ.Ativo) = True)) ORDER BY Cadastro DESC";


            sqlcommand = sqlstring2cnae;

            try
            {
                ObservableCollection<mPJ_Ext> list = new ObservableCollection<mPJ_Ext>();

                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                dataAccess.ClearParameters();

                //string par = commands[0] + "\n" + commands[1] + "\n" + commands[2] + "\n" + commands[3] + "\n" + commands[4] + "\n" + commands[5] + "\n"
                //    + commands[6] + "\n" + commands[7] + "\n" + commands[8] + "\n" + commands[9] + "\n" + commands[10] + "\n" + commands[11] + "\n" + commands[12];

                //string sql = @"SELECT * FROM SDT_SE_PJ WHERE (Ativo = True) AND (Cadastro BETWEEN @data1 AND @data2) AND (CNPJ LIKE @cnpj + '%') AND (RazaoSocial LIKE '%' + @razaosocial + '%') AND (AtividadePrincipal LIKE '%' + @atividadeprincipal + '%') ORDER BY RazaoSocial";

                //System.Windows.MessageBox.Show(par);
                int cont = 1;
                foreach (DataRow dr in dataAccess.Read(sqlcommand).Rows)
                {
                    var line = new mPJ_Ext();
                    var nfa = new mFormalizada();
                    var nsg = new mSegmentos();

                    line.Indice = (int)dr[0];
                    line.CNPJ = (string)dr[1];
                    line.MatrizFilial = (string)dr[2];
                    line.Abertura = (DateTime)dr[3];
                    line.RazaoSocial = (string)dr[4];
                    line.NomeFantasia = (string)dr[5];
                    line.NaturezaJuridica = (string)dr[6];
                    line.AtividadePrincipal = (string)dr[7];
                    line.AtividadeSecundaria = (string)dr[8];
                    line.SituacaoCadastral = (string)dr[9];
                    line.DataSituacaoCadastral = (DateTime)dr[10];

                    line.Logradouro = (string)dr[11];
                    line.Numero = (string)dr[12];
                    line.Complemento = (string)dr[13];
                    line.CEP = (string)dr[14];
                    line.Bairro = (string)dr[15];
                    line.Municipio = (string)dr[16];
                    line.UF = (string)dr[17];
                    line.Email = (string)dr[18];
                    line.Telefones = (string)dr[19];

                    line.Cadastro = (DateTime)dr[20];
                    line.Atualizado = (DateTime)dr[21];

                    line.Ativo = (bool)dr[22];

                    nsg.Indice = (int)dr[23];
                    nsg.CNPJ_CPF = (string)dr[24];
                    nsg.Agronegocio = (bool)dr[25];
                    nsg.Industria = (bool)dr[26];
                    nsg.Comercio = (bool)dr[27];
                    nsg.Servicos = (bool)dr[28];
                    nsg.Ativo = (bool)dr[29];
                    line.Segmentos = nsg;

                    nfa.Indice = (int)dr[30];
                    nfa.CNPJ = (string)dr[31];
                    nfa.InscricaoMunicipal = (string)dr[32];
                    nfa.Porte = (int)dr[33];
                    nfa.UsoLocal = (int)dr[34];
                    nfa.FormalizadoSE = (bool)dr[35];
                    nfa.Data = (DateTime)dr[36];
                    nfa.Ativo = (bool)dr[37];
                    line.Formalizada = nfa;

                    line.Contador = cont;
                    list.Add(line);
                    cont++;

                }

                return list;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }

        public ObservableCollection<mPF_Ext> Pessoas(List<string> commands)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                var lista = new ObservableCollection<mPF_Ext>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@CPF", commands[2]);
                dataAccess.AddParameters("@Nome", commands[3]);
                                
                string sql = @"SELECT * FROM SDT_SE_PF WHERE (Cadastro BETWEEN @data1 AND @data2) AND (CPF LIKE @CPF + '%') AND (Nome LIKE '%' + @Nome + '%') AND (Ativo = true) ORDER BY Nome";

                var data_perfil = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil");
                var data_perfil_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil_Tipos ORDER BY Valor");
                var data_segmento = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento");
                var data_deficiente = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Deficiencia");
                var data_vinculo = dataAccess.Read(@"SELECT * FROM SDT_SE_PJPF_Vinculos");
                var data_vinculo_tipos= dataAccess.Read(@"SELECT * FROM SDT_SE_PJPF_Vinculos_Tipos ORDER BY Valor");
                int cont = 1;
                foreach (DataRow _p in dataAccess.Read(sql).Rows)
                {
                    var pessoa = new mPF_Ext();

                    pessoa.Indice = (int)_p[0];
                    pessoa.Nome = (string)_p[1];
                    pessoa.RG = Convert.ToString(_p[2]);
                    pessoa.CPF = (string)_p[3];
                    pessoa.DataNascimento = (DateTime)_p[4];
                    pessoa.Sexo = (int)_p[5];

                    pessoa.CEP = (string)_p[6];
                    pessoa.Logradouro = (string)_p[7];
                    pessoa.Numero = (string)_p[8];
                    pessoa.Complemento = (string)_p[9];
                    pessoa.Bairro = (string)_p[10];
                    pessoa.Municipio = (string)_p[11];
                    pessoa.UF = (string)_p[12];
                    pessoa.Email = (string)_p[13];
                    pessoa.Telefones = (string)_p[14];

                    pessoa.Cadastro = (DateTime)_p[15];
                    pessoa.Atualizado = (DateTime)_p[16];
                    pessoa.Ativo = (bool)_p[17];

                    //pega o perfil
                    foreach (DataRow _perfil in data_perfil.Rows)
                    {
                        if (pessoa.CPF == (string)_perfil[1])
                        {
                            var p = new mPerfil();
                            p.Indice = (int)_perfil[0];
                            p.CPF = (string)_perfil[1];
                            p.Perfil = (int)_perfil[2];
                            p.Negocio = (bool)_perfil[3];
                            p.Ativo = (bool)_perfil[4];
                            p.PerfilString = (string)data_perfil_tipos.Rows[p.Perfil][1];
                            pessoa.Perfil = p;
                        }
                    }

                    //pega vínculo
                    ObservableCollection<mVinculos> colecaovinculos = new ObservableCollection<mVinculos>();
                    foreach (DataRow _vinculo in data_vinculo.Rows)
                    {
                        if (pessoa.CPF == (string)_vinculo[2])
                        {
                            var v = new mVinculos();
                            v.Indice = (int)_vinculo[0];
                            v.CNPJ = (string)_vinculo[1];
                            v.CPF = (string)_vinculo[2];
                            v.Vinculo = (int)_vinculo[3];
                            v.Data = (DateTime)_vinculo[4];
                            v.Ativo = (bool)_vinculo[5];
                            v.VinculoString = (string)data_vinculo_tipos.Rows[v.Vinculo][1];

                            colecaovinculos.Add(v);
                        }
                    }

                    pessoa.ColecaoVinculos = colecaovinculos;

                    //Pega deficientes
                    foreach (DataRow _deficiencia in data_deficiente.Rows)
                    {
                        if (pessoa.CPF == (string)_deficiencia[1])
                        {
                            var d = new mDeficiencia();
                            d.Indice = (int)_deficiencia[0];
                            d.CPF = (string)_deficiencia[1];
                            d.Deficiencia = (bool)_deficiencia[2];
                            d.Fisica = (bool)_deficiencia[3];
                            d.Visual = (bool)_deficiencia[4];
                            d.Auditiva = (bool)_deficiencia[5];
                            d.Intelectual = (bool)_deficiencia[6];
                            d.Ativo = (bool)_deficiencia[7];

                            pessoa.Deficiente = d;
                        }
                    }

                    if (pessoa.Perfil.Perfil == 1)
                    {
                        if (commands[5] == "True")
                        {
                            pessoa.Contador = cont;
                            cont++;
                            lista.Add(pessoa);
                        }
                    }

                    if (pessoa.Perfil.Perfil == 2)
                    {
                        if (commands[4] == "True")
                        {
                            pessoa.Contador = cont;
                            cont++;
                            lista.Add(pessoa);
                        }
                    }

                    if (pessoa.Perfil.Perfil == 3)
                    {
                        if (commands[6] == "True")
                        {
                            pessoa.Contador = cont;
                            cont++;
                            lista.Add(pessoa);
                        }
                    }
                }
                
                return lista;
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mPF_Ext> PessoasNow()
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                var lista = new ObservableCollection<mPF_Ext>();

                dataAccess.ClearParameters();

                string sql = @"SELECT TOP 10 * FROM SDT_SE_PF WHERE (Ativo = true) ORDER BY Cadastro DESC";

                var data_perfil = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil");
                var data_perfil_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil_Tipos ORDER BY Valor");
                var data_segmento = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento");
                var data_deficiente = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Deficiencia");
                var data_vinculo = dataAccess.Read(@"SELECT * FROM SDT_SE_PJPF_Vinculos");
                var data_vinculo_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_PJPF_Vinculos_Tipos ORDER BY Valor");
                int cont = 1;
                foreach (DataRow _p in dataAccess.Read(sql).Rows)
                {
                    var pessoa = new mPF_Ext();

                    pessoa.Indice = (int)_p[0];
                    pessoa.Nome = (string)_p[1];
                    pessoa.RG = Convert.ToString(_p[2]);
                    pessoa.CPF = (string)_p[3];
                    pessoa.DataNascimento = (DateTime)_p[4];
                    pessoa.Sexo = (int)_p[5];

                    pessoa.CEP = (string)_p[6];
                    pessoa.Logradouro = (string)_p[7];
                    pessoa.Numero = (string)_p[8];
                    pessoa.Complemento = (string)_p[9];
                    pessoa.Bairro = (string)_p[10];
                    pessoa.Municipio = (string)_p[11];
                    pessoa.UF = (string)_p[12];
                    pessoa.Email = (string)_p[13];
                    pessoa.Telefones = (string)_p[14];

                    pessoa.Cadastro = (DateTime)_p[15];
                    pessoa.Atualizado = (DateTime)_p[16];
                    pessoa.Ativo = (bool)_p[17];

                    //pega o perfil
                    foreach (DataRow _perfil in data_perfil.Rows)
                    {
                        if (pessoa.CPF == (string)_perfil[1])
                        {
                            var p = new mPerfil();
                            p.Indice = (int)_perfil[0];
                            p.CPF = (string)_perfil[1];
                            p.Perfil = (int)_perfil[2];
                            p.Negocio = (bool)_perfil[3];
                            p.Ativo = (bool)_perfil[4];
                            p.PerfilString = (string)data_perfil_tipos.Rows[p.Perfil][1];
                            pessoa.Perfil = p;
                        }
                    }

                    //pega vínculo
                    ObservableCollection<mVinculos> colecaovinculos = new ObservableCollection<mVinculos>();
                    foreach (DataRow _vinculo in data_vinculo.Rows)
                    {
                        if (pessoa.CPF == (string)_vinculo[2])
                        {
                            var v = new mVinculos();
                            v.Indice = (int)_vinculo[0];
                            v.CNPJ = (string)_vinculo[1];
                            v.CPF = (string)_vinculo[2];
                            v.Vinculo = (int)_vinculo[3];
                            v.Data = (DateTime)_vinculo[4];
                            v.Ativo = (bool)_vinculo[5];
                            v.VinculoString = (string)data_vinculo_tipos.Rows[v.Vinculo][1];

                            colecaovinculos.Add(v);
                        }
                    }

                    pessoa.ColecaoVinculos = colecaovinculos;

                    //Pega deficientes
                    foreach (DataRow _deficiencia in data_deficiente.Rows)
                    {
                        if (pessoa.CPF == (string)_deficiencia[1])
                        {
                            var d = new mDeficiencia();
                            d.Indice = (int)_deficiencia[0];
                            d.CPF = (string)_deficiencia[1];
                            d.Deficiencia = (bool)_deficiencia[2];
                            d.Fisica = (bool)_deficiencia[3];
                            d.Visual = (bool)_deficiencia[4];
                            d.Auditiva = (bool)_deficiencia[5];
                            d.Intelectual = (bool)_deficiencia[6];
                            d.Ativo = (bool)_deficiencia[7];

                            pessoa.Deficiente = d;
                        }
                    }
                    
                    pessoa.Contador = cont;
                    cont++;
                    lista.Add(pessoa);
                }                                 
                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mPF_Ext> PessoasNotCNPJ()
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                var lista = new ObservableCollection<mPF_Ext>();

                dataAccess.ClearParameters();

                string sql = @"SELECT * FROM SDT_SE_PF WHERE (Ativo = true) ORDER BY Cadastro DESC";

                var data_perfil = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil");
                var data_perfil_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil_Tipos");
                var data_segmento = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento");
                var data_deficiente = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Deficiencia");
                var data_vinculo = dataAccess.Read(@"SELECT * FROM SDT_SE_PJPF_Vinculos");
                var data_vinculo_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_PJPF_Vinculos_Tipos ORDER BY Valor");
                int cont = 1;
                foreach (DataRow _p in dataAccess.Read(sql).Rows)
                {
                    var pessoa = new mPF_Ext();

                    pessoa.Indice = (int)_p[0];
                    pessoa.Nome = (string)_p[1];
                    pessoa.RG = Convert.ToString(_p[2]);
                    pessoa.CPF = (string)_p[3];
                    pessoa.DataNascimento = (DateTime)_p[4];
                    pessoa.Sexo = (int)_p[5];

                    pessoa.CEP = (string)_p[6];
                    pessoa.Logradouro = (string)_p[7];
                    pessoa.Numero = (string)_p[8];
                    pessoa.Complemento = (string)_p[9];
                    pessoa.Bairro = (string)_p[10];
                    pessoa.Municipio = (string)_p[11];
                    pessoa.UF = (string)_p[12];
                    pessoa.Email = (string)_p[13];
                    pessoa.Telefones = (string)_p[14];

                    pessoa.Cadastro = (DateTime)_p[15];
                    pessoa.Atualizado = (DateTime)_p[16];
                    pessoa.Ativo = (bool)_p[17];

                    //pega o perfil
                    foreach (DataRow _perfil in data_perfil.Rows)
                    {
                        if (pessoa.CPF == (string)_perfil[1])
                        {
                            var p = new mPerfil();
                            p.Indice = (int)_perfil[0];
                            p.CPF = (string)_perfil[1];
                            p.Perfil = (int)_perfil[2];
                            p.Negocio = (bool)_perfil[3];
                            p.Ativo = (bool)_perfil[4];
                            p.PerfilString = (string)data_perfil_tipos.Rows[p.Perfil][1];
                            pessoa.Perfil = p;
                        }
                    }

                    //pega vínculo
                    ObservableCollection<mVinculos> colecaovinculos = new ObservableCollection<mVinculos>();
                    foreach (DataRow _vinculo in data_vinculo.Rows)
                    {
                        if (pessoa.CPF == (string)_vinculo[2])
                        {
                            var v = new mVinculos();
                            v.Indice = (int)_vinculo[0];
                            v.CNPJ = (string)_vinculo[1];
                            v.CPF = (string)_vinculo[2];
                            v.Vinculo = (int)_vinculo[3];
                            v.Data = (DateTime)_vinculo[4];
                            v.Ativo = (bool)_vinculo[5];
                            v.VinculoString = (string)data_vinculo_tipos.Rows[v.Vinculo][1];

                            colecaovinculos.Add(v);
                        }
                    }

                    pessoa.ColecaoVinculos = colecaovinculos;

                    //Pega deficientes
                    foreach (DataRow _deficiencia in data_deficiente.Rows)
                    {
                        if (pessoa.CPF == (string)_deficiencia[1])
                        {
                            var d = new mDeficiencia();
                            d.Indice = (int)_deficiencia[0];
                            d.CPF = (string)_deficiencia[1];
                            d.Deficiencia = (bool)_deficiencia[2];
                            d.Fisica = (bool)_deficiencia[3];
                            d.Visual = (bool)_deficiencia[4];
                            d.Auditiva = (bool)_deficiencia[5];
                            d.Intelectual = (bool)_deficiencia[6];
                            d.Ativo = (bool)_deficiencia[7];

                            pessoa.Deficiente = d;
                        }
                    }                  
                    
                    if (pessoa.ColecaoVinculos.Count <= 0)
                    {
                        pessoa.Contador = cont;
                        cont++;
                        lista.Add(pessoa);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAtendimento> Atendimentos(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAtendimento>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@protocolo", commands[2]);
                dataAccess.AddParameters("@cliente", commands[3]);
                dataAccess.AddParameters("@origem1", commands[4]);
                dataAccess.AddParameters("@origem2", commands[5]);
                dataAccess.AddParameters("@tipo1", commands[6]);
                dataAccess.AddParameters("@tipo2", commands[7]);

                var _tipo = dataAccess.Read("SELECT * FROM SDT_Atendimento_Tipos ORDER BY Valor");
                var _origem = dataAccess.Read("SELECT * FROM SDT_Atendimento_Origem ORDER BY Valor");
                var _atendimentosebrae = dataAccess.Read("SELECT * FROM SDT_SAC_Atendimento");

                string sql = @"SELECT * FROM SDT_Atendimento WHERE (Data BETWEEN @data1 AND @data2) AND (Protocolo LIKE @protocolo + '%') AND (Cliente LIKE '%' +  @cliente + '%') AND (Origem BETWEEN @origem1 AND @origem2) AND (Tipo LIKE '%' +  @tipo1 + '%') AND (Ativo = true) ORDER BY Data, Hora";
                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var atendimento = new mAtendimento();

                    atendimento.Indice = (int)at[0];
                    atendimento.Protocolo = (string)at[1];
                    atendimento.Data = (DateTime)at[2];

                    atendimento.Hora = (DateTime)at[3];

                    string[] words = at[4].ToString().Split('/');

                    atendimento.Cliente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    atendimento.Origem = (int)at[5];
                    atendimento.TipoString = (string)at[6];
                    atendimento.Historico = (string)at[7];
                    atendimento.Operador = (string)at[8];
                    atendimento.Ativo = (bool)at[9];

                    //atendimento.TipoString = (string)_tipo.Rows[atendimento.Tipo][1];
                    atendimento.OrigemString = (string)_origem.Rows[atendimento.Origem][1];

                    foreach (DataRow dr in _atendimentosebrae.Rows)
                    {
                        if (atendimento.Protocolo == dr[2].ToString())
                            atendimento.AtendimentoSebrae = dr[1].ToString();
                    }

                    if (commands[8] == true.ToString() && commands[9] == true.ToString())
                    {
                        atendimento.Contador = cont;
                        cont++;
                        lista.Add(atendimento);
                    }
                    else if (commands[8] == true.ToString() && commands[9] != true.ToString())
                    {
                        if (atendimento.AtendimentoSebrae != string.Empty)
                        {
                            atendimento.Contador = cont;
                            cont++;
                            lista.Add(atendimento);
                        }
                    }
                    else if (commands[8] != true.ToString() && commands[9] == true.ToString())
                    {
                        if (atendimento.AtendimentoSebrae == string.Empty)
                        {
                            atendimento.Contador = cont;
                            cont++;
                            lista.Add(atendimento);
                        }
                    }
                    //atendimento.Contador = cont;
                    //cont++;
                    //lista.Add(atendimento);

                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;                
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAtendimento> AtendimentosOperador(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAtendimento>();

                dataAccess.ClearParameters();

                //System.Windows.MessageBox.Show(commands[5] + "\n" + commands[6]);

                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@tipo1", commands[2]);
                dataAccess.AddParameters("@tipo2", commands[3]);
                dataAccess.AddParameters("@operador", commands[4]);

                var _tipo = dataAccess.Read("SELECT * FROM SDT_Atendimento_Tipos ORDER BY Valor");
                var _origem = dataAccess.Read("SELECT * FROM SDT_Atendimento_Origem ORDER BY Valor");
                var _atendimentosebrae = dataAccess.Read("SELECT * FROM SDT_SAC_Atendimento");

                string sql = @"SELECT * FROM SDT_Atendimento WHERE (Data BETWEEN @data1 AND @data2) AND (Origem BETWEEN @tipo1 AND @tipo2) AND (Operador LIKE @operador) AND (Ativo = true) ORDER BY Data, Hora";
                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var atendimento = new mAtendimento();

                    atendimento.Indice = (int)at[0];
                    atendimento.Protocolo = (string)at[1];
                    atendimento.Data = (DateTime)at[2];
                    atendimento.Hora = (DateTime)at[3];

                    string[] words = at[4].ToString().Split('/');

                    atendimento.Cliente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    atendimento.Origem = (int)at[5];
                    atendimento.TipoString = (string)at[6];
                    atendimento.Historico = (string)at[7];
                    atendimento.Operador = (string)at[8];
                    atendimento.Ativo = (bool)at[9];

                    //atendimento.TipoString = (string)_tipo.Rows[atendimento.Tipo][1];
                    atendimento.OrigemString = (string)_origem.Rows[atendimento.Origem][1];

                    foreach (DataRow dr in _atendimentosebrae.Rows)
                    {
                        if (atendimento.Protocolo == dr[2].ToString())
                            atendimento.AtendimentoSebrae = dr[1].ToString();
                    }
                    
                    if (commands[5] == true.ToString() && commands[6] == true.ToString())
                    {
                        atendimento.Contador = cont;
                        cont++;
                        lista.Add(atendimento);
                    }
                    else if (commands[5] == true.ToString() && commands[6] != true.ToString())
                    {
                        if (atendimento.AtendimentoSebrae != string.Empty)
                        {
                            atendimento.Contador = cont;
                            cont++;
                            lista.Add(atendimento);
                        }
                    }
                    else if (commands[5] != true.ToString() && commands[6] == true.ToString())
                    {
                        if (atendimento.AtendimentoSebrae == string.Empty)
                        {
                            atendimento.Contador = cont;
                            cont++;
                            lista.Add(atendimento);
                        }
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAtendimento> AtendimentosNow(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAtendimento>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data", commands[0]);
                dataAccess.AddParameters("@operador", commands[1]);
                dataAccess.AddParameters("@origem", commands[2]);

                var _tipo = dataAccess.Read("SELECT * FROM SDT_Atendimento_Tipos ORDER BY Valor");
                var _origem = dataAccess.Read("SELECT * FROM SDT_Atendimento_Origem ORDER BY Valor");
                var _atendimentosebrae = dataAccess.Read("SELECT * FROM SDT_SAC_Atendimento");

                //System.Windows.MessageBox.Show(commands[2], commands[1]);

                string sql = @"SELECT * FROM SDT_Atendimento WHERE (Data LIKE @data) AND (Operador LIKE @operador) AND (Origem = @origem) AND (Ativo = true) ORDER BY Hora";

                int cont = 1;

                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var atendimento = new mAtendimento();

                    atendimento.Indice = (int)at[0];
                    atendimento.Protocolo = (string)at[1];
                    atendimento.Data = (DateTime)at[2];
                    atendimento.Hora = (DateTime)at[3];

                    string[] words = at[4].ToString().Split('/');

                    atendimento.Cliente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    atendimento.Origem = (int)at[5];
                    atendimento.TipoString = (string)at[6];
                    atendimento.Historico = (string)at[7];
                    atendimento.Operador = (string)at[8];
                    atendimento.Ativo = (bool)at[9];

                    //atendimento.TipoString = (string)_tipo.Rows[atendimento.Tipo][1];
                    atendimento.OrigemString = (string)_origem.Rows[atendimento.Origem][1];

                    foreach (DataRow dr in _atendimentosebrae.Rows)
                    {
                        if (atendimento.Protocolo == dr[2].ToString())
                            atendimento.AtendimentoSebrae = dr[1].ToString();

                        if (atendimento.AtendimentoSebrae == string.Empty || atendimento.AtendimentoSebrae == null)
                            atendimento.AtendimentoSebrae = "...";
                    }

                    atendimento.Contador = cont;
                    cont++;
                    lista.Add(atendimento);

                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAgenda> AgendaCliente(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAgenda>();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@datai", commands[0]);
                dataAccess.AddParameters("@dataf", commands[1]);
                dataAccess.AddParameters("@cpf", commands[2]);
                dataAccess.AddParameters("@cnpj", commands[3]);

                string sql_cliente = @"SELECT * FROM SDT_Inscricoes WHERE ((Data BETWEEN @datai AND @dataf) AND ((CPF LIKE @cpf) OR (CNPJ LIKE @cnpj))) ORDER BY Inscricao";

                int cont = 1;

                foreach (DataRow ev in dataAccess.Read(sql_cliente).Rows)
                {
                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@codigo", (string)ev[2]);

                    string sql = @"SELECT * FROM SDT_Agenda WHERE ((Codigo LIKE @codigo) AND (ATIVO = True)) ORDER BY Data, Hora";

                    var _tipos = dataAccess.Read("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Valor");
                    var _estados = dataAccess.Read("SELECT * FROM SDT_Agenda_Estado WHERE (Ativo = True) ORDER BY Valor");
                    var _setores = dataAccess.Read("SELECT * FROM SDT_Setores WHERE (Ativo = True) ORDER BY Valor");


                    foreach (DataRow ag in dataAccess.Read(sql).Rows)
                    {
                        dataAccess.ClearParameters();
                        dataAccess.AddParameters("@event", (string)ag[1]);
                        var _inscritos = dataAccess.Read("SELECT * FROM SDT_Inscricoes WHERE ((Evento = @evento) AND (Ativo = True))");

                        var agenda = new mAgenda();
                        agenda.Inscritos = _inscritos.Rows.Count;
                        agenda.Indice = (int)ag[0];
                        agenda.Codigo = (string)ag[1];
                        agenda.Tipo = (int)ag[2];
                        agenda.TipoString = (string)_tipos.Rows[agenda.Tipo][1];
                        agenda.Evento = (string)ag[3];
                        agenda.Vagas = (int)ag[4];
                        agenda.Descricao = (string)ag[5];
                        agenda.Data = (DateTime)ag[6];
                        agenda.Hora = (DateTime)ag[7];
                        agenda.Setor = (int)ag[8];
                        agenda.SetorString = (string)_setores.Rows[agenda.Setor][1];
                        agenda.Estado = (int)ag[9];
                        agenda.EstadoString = (string)_estados.Rows[agenda.Estado][1];
                        agenda.Criacao = (DateTime)ag[10];
                        agenda.Ativo = (bool)ag[11];
                        agenda.Contador = cont;
                        cont++;
                        lista.Add(agenda);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAgenda> AgendaInscritos(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAgenda>();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@datai", commands[0]);
                dataAccess.AddParameters("@dataf", commands[1]);
                dataAccess.AddParameters("@estado1", commands[2]);
                dataAccess.AddParameters("@estado2", commands[3]);
                dataAccess.AddParameters("@tipo1", commands[4]);
                dataAccess.AddParameters("@tipo2", commands[5]);
                dataAccess.AddParameters("@setor1", commands[6]);
                dataAccess.AddParameters("@setor2", commands[7]);

                string sql = @"SELECT * FROM SDT_Agenda WHERE ((Data BETWEEN @datai AND @dataf) AND (Estado BETWEEN @estado1 AND @estado2) AND (Tipo BETWEEN @tipo1 AND @tipo2) AND (Setor BETWEEN @setor1 AND @setor2) AND (ATIVO = True)) ORDER BY Data, Hora";

                var _tipos = dataAccess.Read("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Valor");
                var _estados = dataAccess.Read("SELECT * FROM SDT_Agenda_Estado WHERE (Ativo = True) ORDER BY Valor");
                var _setores = dataAccess.Read("SELECT * FROM SDT_Setores WHERE (Ativo = True) ORDER BY Valor");

                int cont = 1;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@event", (string)ag[1]);
                    var _inscritos = dataAccess.Read("SELECT * FROM SDT_Inscricoes WHERE ((Evento = @evento) AND (Ativo = True))");

                    var agenda = new mAgenda();
                    agenda.Inscritos = _inscritos.Rows.Count;
                    agenda.Indice = (int)ag[0];
                    agenda.Codigo = (string)ag[1];
                    agenda.Tipo = (int)ag[2];
                    agenda.TipoString = (string)_tipos.Rows[agenda.Tipo][1];
                    agenda.Evento = (string)ag[3];
                    agenda.Vagas = (int)ag[4];
                    agenda.Descricao = (string)ag[5];
                    agenda.Data = (DateTime)ag[6];
                    agenda.Hora = (DateTime)ag[7];
                    agenda.Setor = (int)ag[8];
                    agenda.SetorString = (string)_setores.Rows[agenda.Setor][1];
                    agenda.Estado = (int)ag[9];
                    agenda.EstadoString = (string)_estados.Rows[agenda.Estado][1];
                    agenda.Criacao = (DateTime)ag[10];
                    agenda.Ativo = (bool)ag[11];
                    agenda.Contador = cont;
                    cont++;
                    lista.Add(agenda);
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAgenda> Agenda(List<string>commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAgenda>();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@datai", commands[0]);
                dataAccess.AddParameters("@dataf", commands[1]);
                dataAccess.AddParameters("@estado1", commands[2]);
                dataAccess.AddParameters("@estado2", commands[3]);
                dataAccess.AddParameters("@tipo1", commands[4]);
                dataAccess.AddParameters("@tipo2", commands[5]);
                dataAccess.AddParameters("@setor1", commands[6]);
                dataAccess.AddParameters("@setor2", commands[7]);

                string sql = @"SELECT * FROM SDT_Agenda WHERE ((Data BETWEEN @datai AND @dataf) AND (Estado BETWEEN @estado1 AND @estado2) AND (Tipo BETWEEN @tipo1 AND @tipo2) AND (Setor BETWEEN @setor1 AND @setor2) AND (ATIVO = True)) ORDER BY Data, Hora";

                var _tipos = dataAccess.Read("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Valor");
                var _estados = dataAccess.Read("SELECT * FROM SDT_Agenda_Estado WHERE (Ativo = True) ORDER BY Valor");
                var _setores = dataAccess.Read("SELECT * FROM SDT_Setores WHERE (Ativo = True) ORDER BY Valor");

                int cont = 1;                

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    var agenda = new mAgenda();

                    agenda.Indice = (int)ag[0];
                    agenda.Codigo = (string)ag[1];
                    agenda.Tipo = (int)ag[2];
                    agenda.TipoString = (string)_tipos.Rows[agenda.Tipo][1];
                    agenda.Evento = (string)ag[3];
                    agenda.Vagas = (int)ag[4];
                    agenda.Descricao = (string)ag[5];
                    agenda.Data = (DateTime)ag[6];
                    agenda.Hora = (DateTime)ag[7];
                    agenda.Setor = (int)ag[8];
                    agenda.SetorString = (string)_setores.Rows[agenda.Setor][1];
                    agenda.Estado = (int)ag[9];
                    agenda.EstadoString = (string)_estados.Rows[agenda.Estado][1];
                    agenda.Criacao = (DateTime)ag[10];
                    agenda.Ativo = (bool)ag[11];
                    agenda.Contador = cont;
                    cont++;
                    lista.Add(agenda);
                }                

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public int VagasEvento(string evento)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                string sql = @"SELECT Vagas FROM SDT_Agenda WHERE (Codigo = @Evento) AND (Ativo = True)";

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Evento", evento);

                int cont = 0;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    cont = (int)ag[0];
                }

                return cont;
            }
            catch (Exception ex)
            {
                return -1;
                throw new Exception(ex.Message);
            }
        }

        public int InscritosEventos(string evento)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mInscricao>();

                string sql = @"SELECT * FROM SDT_Inscricoes WHERE (Evento = @Evento) AND (Ativo = True) ORDER BY Nome";

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Evento", evento);

                return dataAccess.Read(sql).Rows.Count;
            }
            catch (Exception ex)
            {
                return -1;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAgenda> AgendaAtivo(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAgenda>();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@datai", commands[0]);
                dataAccess.AddParameters("@estado1", commands[1]);

                string sql = @"SELECT * FROM SDT_Agenda WHERE ((Data >= @datai) AND (Estado = @estado1)) ORDER BY Data, Hora";

                var _tipos = dataAccess.Read("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Valor");

                int cont = 1;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    var agenda = new mAgenda();

                    agenda.Indice = (int)ag[0];
                    agenda.Codigo = (string)ag[1];
                    agenda.Tipo = (int)ag[2];

                    agenda.TipoString = (string)_tipos.Rows[agenda.Tipo][1];

                    agenda.Evento = (string)ag[3];
                    agenda.Vagas = (int)ag[4];
                    agenda.Descricao = (string)ag[5];
                    agenda.Data = (DateTime)ag[6];
                    agenda.Hora = (DateTime)ag[7];
                    agenda.Setor = (int)ag[8];
                    agenda.Estado = (int)ag[9];
                    agenda.Criacao = (DateTime)ag[10];
                    agenda.Ativo = (bool)ag[11];
                    //agenda.Contador = cont;
                    cont++;
                    lista.Add(agenda);
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAgenda> AgendaVencida(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAgenda>();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@datai", commands[0]);
                dataAccess.AddParameters("@estado1", commands[1]);

                string sql = @"SELECT * FROM SDT_Agenda WHERE ((Data < @datai) AND (Estado = @estado1)) ORDER BY Data, Hora";

                var _tipos = dataAccess.Read("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Valor");

                int cont = 1;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    var agenda = new mAgenda();

                    agenda.Indice = (int)ag[0];
                    agenda.Codigo = (string)ag[1];
                    agenda.Tipo = (int)ag[2];

                    agenda.TipoString = (string)_tipos.Rows[agenda.Tipo][1];

                    agenda.Evento = (string)ag[3];
                    agenda.Vagas = (int)ag[4];
                    agenda.Descricao = (string)ag[5];
                    agenda.Data = (DateTime)ag[6];
                    agenda.Hora = (DateTime)ag[7];
                    agenda.Setor = (int)ag[8];
                    agenda.Estado = (int)ag[9];
                    agenda.Criacao = (DateTime)ag[10];
                    agenda.Ativo = (bool)ag[11];
                    //agenda.Contador = cont;
                    cont++;
                    lista.Add(agenda);
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAgenda> AgendaCancelada(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAgenda>();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@estado1", commands[0]);

                string sql = @"SELECT * FROM SDT_Agenda WHERE (Estado = @estado1) ORDER BY Data, Hora";

                var _tipos = dataAccess.Read("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Valor");

                int cont = 1;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    var agenda = new mAgenda();

                    agenda.Indice = (int)ag[0];
                    agenda.Codigo = (string)ag[1];
                    agenda.Tipo = (int)ag[2];

                    agenda.TipoString = (string)_tipos.Rows[agenda.Tipo][1];

                    agenda.Evento = (string)ag[3];
                    agenda.Vagas = (int)ag[4];
                    agenda.Descricao = (string)ag[5];
                    agenda.Data = (DateTime)ag[6];
                    agenda.Hora = (DateTime)ag[7];
                    agenda.Setor = (int)ag[8];
                    agenda.Estado = (int)ag[9];
                    agenda.Criacao = (DateTime)ag[10];
                    agenda.Ativo = (bool)ag[11];
                    //agenda.Contador = cont;
                    cont++;
                    lista.Add(agenda);
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAgenda> AgendaFinalizado(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAgenda>();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@estado1", commands[0]);

                string sql = @"SELECT * FROM SDT_Agenda WHERE (Estado = @estado1) ORDER BY Data DESC, Hora";

                var _tipos = dataAccess.Read("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Valor");

                int cont = 1;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    var agenda = new mAgenda();

                    agenda.Indice = (int)ag[0];
                    agenda.Codigo = (string)ag[1];
                    agenda.Tipo = (int)ag[2];

                    agenda.TipoString = (string)_tipos.Rows[agenda.Tipo][1];

                    agenda.Evento = (string)ag[3];
                    agenda.Vagas = (int)ag[4];
                    agenda.Descricao = (string)ag[5];
                    agenda.Data = (DateTime)ag[6];
                    agenda.Hora = (DateTime)ag[7];
                    agenda.Setor = (int)ag[8];
                    agenda.Estado = (int)ag[9];
                    agenda.Criacao = (DateTime)ag[10];
                    agenda.Ativo = (bool)ag[11];
                    //agenda.Contador = cont;
                    cont++;
                    lista.Add(agenda);
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mInscricao> Inscritos(string evento)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mInscricao>();

                string sql = @"SELECT * FROM SDT_Inscricoes WHERE (Evento = @Evento) AND (Ativo = True) ORDER BY Nome";

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Evento", evento);

                int cont = 1;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    var inscricao = new mInscricao();

                    inscricao.Indice = (int)ag[0];
                    inscricao.Inscricao = (string)ag[1];
                    inscricao.Evento = (string)ag[2];
                    inscricao.Atendimento = (string)ag[3];
                    inscricao.CPF = (string)ag[4];
                    inscricao.Nome = (string)ag[5];
                    inscricao.CNPJ = (string)ag[6];
                    inscricao.Telefone = (string)ag[7];
                    inscricao.Email = (string)ag[8];
                    inscricao.Data = (DateTime)ag[9];
                    inscricao.Presente = (bool)ag[10];
                    inscricao.Ativo = (bool)ag[11];
                    inscricao.Contador = cont;
                    cont++;
                    lista.Add(inscricao);
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mInscricao> InscritosI(string evento)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mInscricao>();

                string sql = @"SELECT * FROM SDT_Inscricoes WHERE (Evento = @Evento) AND (Ativo = True) ORDER BY Inscricao";

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Evento", evento);

                int cont = 1;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    var inscricao = new mInscricao();

                    inscricao.Indice = (int)ag[0];
                    inscricao.Inscricao = (string)ag[1];
                    inscricao.Evento = (string)ag[2];
                    inscricao.Atendimento = (string)ag[3];
                    inscricao.CPF = (string)ag[4];
                    inscricao.Nome = (string)ag[5];
                    inscricao.CNPJ = (string)ag[6];
                    inscricao.Telefone = (string)ag[7];
                    inscricao.Email = (string)ag[8];
                    inscricao.Data = (DateTime)ag[9];
                    inscricao.Presente = (bool)ag[10];
                    inscricao.Ativo = (bool)ag[11];
                    inscricao.Contador = cont;
                    cont++;
                    lista.Add(inscricao);
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mInscricao> InscritosCancelados(string evento)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mInscricao>();

                string sql = @"SELECT * FROM SDT_Inscricoes WHERE (Evento = @Evento) AND (Ativo = False) ORDER BY Nome";

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Evento", evento);

                int cont = 1;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    var inscricao = new mInscricao();

                    inscricao.Indice = (int)ag[0];
                    inscricao.Inscricao = (string)ag[1];
                    inscricao.Evento = (string)ag[2];
                    inscricao.Atendimento = (string)ag[3];
                    inscricao.CPF = (string)ag[4];
                    inscricao.Nome = (string)ag[5];
                    inscricao.CNPJ = (string)ag[6];
                    inscricao.Telefone = (string)ag[7];
                    inscricao.Email = (string)ag[8];
                    inscricao.Data = (DateTime)ag[9];
                    inscricao.Presente = (bool)ag[10];
                    inscricao.Ativo = (bool)ag[11];
                    inscricao.Contador = cont;
                    cont++;
                    lista.Add(inscricao);
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Inserção / Edição / Remoção

        public bool GravarPF(mPF obj, Registro tipo)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Nome", obj.Nome);
                dataAccess.AddParameters("@RG", new mMascaras().Remove(obj.RG));
                dataAccess.AddParameters("@CPF", new mMascaras().Remove(obj.CPF));
                dataAccess.AddParameters("@Nascimento", obj.DataNascimento.ToShortDateString());
                dataAccess.AddParameters("@Sexo", obj.Sexo);
                dataAccess.AddParameters("@CEP", new mMascaras().Remove(obj.CEP));
                dataAccess.AddParameters("@Logradouro", obj.Logradouro);
                dataAccess.AddParameters("@Numero", obj.Numero);
                dataAccess.AddParameters("@Complemento", obj.Complemento);
                dataAccess.AddParameters("@Bairro", obj.Bairro);
                dataAccess.AddParameters("@Municipio", obj.Municipio);
                dataAccess.AddParameters("@UF", obj.UF);
                dataAccess.AddParameters("@Email", obj.Email);
                dataAccess.AddParameters("@Telefones", new mMascaras().Remove(obj.Telefones));

                dataAccess.AddParameters("@Cadastro", obj.Cadastro.ToShortDateString());
                dataAccess.AddParameters("@Atualizado", obj.Atualizado.ToShortDateString());
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                if (tipo == Registro.Alteracao)
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string Novo = @"INSERT INTO SDT_SE_PF (Nome, RG, CPF, Nascimento, Sexo, CEP, Logradouro, Numero, Complemento, Bairro, Municipio, UF, Email, Telefones, Cadastro, Atualizado, Ativo) VALUES (@Nome, @RG, @CPF, @Nascimento, @Sexo, @CEP, @Logradouro, @Numero, @Complemento, @Bairro, @Municipio, @UF, @Email, @Telefones, @Cadastro, @Atualizado, @Ativo)";

                string Alteracao = @"UPDATE SDT_SE_PF SET Nome = @Nome, RG = @RG, CPF = @CPF, Nascimento = @Nascimento, Sexo = @Sexo, CEP = @CEP, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro, Municipio = @Municipio, UF = @UF, Email = @Email, Telefones = @Telefones, Cadastro = @Cadastro, Atualizado = @Atualizado, Ativo = @Ativo WHERE (Indice = @Original_Indice)";

                if (tipo == Registro.Alteracao)
                    return dataAccess.Write(Alteracao);
                else
                    return dataAccess.Write(Novo);

            }
            catch(Exception ex) { dataAccess = null; return false; throw new Exception(ex.Message); }
        }

        public bool GravarPJ(mPJ obj, Registro tipo)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                
                dataAccess.ClearParameters();

                dataAccess.AddParameters("@CNPJ", new mMascaras().Remove(obj.CNPJ));
                dataAccess.AddParameters("@MatrizFilial", obj.MatrizFilial);
                dataAccess.AddParameters("@Abertura", obj.Abertura.ToShortDateString());
                dataAccess.AddParameters("@RazaoSocial", obj.RazaoSocial);
                dataAccess.AddParameters("@NomeFantasia", obj.NomeFantasia);
                dataAccess.AddParameters("@NaturezaJuridica", obj.NaturezaJuridica);
                dataAccess.AddParameters("@AtividadePrincipal", obj.AtividadePrincipal);
                dataAccess.AddParameters("@AtividadeSecundaria", obj.AtividadeSecundaria);
                dataAccess.AddParameters("@SituacaoCadastral", obj.SituacaoCadastral);
                dataAccess.AddParameters("@DataSituacaoCadastral", obj.DataSituacaoCadastral.ToShortDateString());

                dataAccess.AddParameters("@Logradouro", obj.Logradouro);
                dataAccess.AddParameters("@Numero", obj.Numero);
                dataAccess.AddParameters("@Complemento", obj.Complemento);
                dataAccess.AddParameters("@CEP", new mMascaras().Remove(obj.CEP));
                dataAccess.AddParameters("@Bairro", obj.Bairro);
                dataAccess.AddParameters("@Municipio", obj.Municipio);
                dataAccess.AddParameters("@UF", obj.UF);
                dataAccess.AddParameters("@Email", obj.Email);
                dataAccess.AddParameters("@Telefones", new mMascaras().Remove(obj.Telefones));

                dataAccess.AddParameters("@Cadastro", obj.Cadastro.ToShortDateString());
                dataAccess.AddParameters("@Atualizado", obj.Atualizado.ToShortDateString());
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                if (tipo == Registro.Alteracao)
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string Novo = @"INSERT INTO
SDT_SE_PJ
(CNPJ, MatrizFilial, Abertura, RazaoSocial, NomeFantasia, NaturezaJuridica, AtividadePrincipal, AtividadeSecundaria, SituacaoCadastral, DataSituacaoCadastral, Logradouro, Numero, Complemento, CEP, Bairro, Municipio, UF, Email, Telefones, Cadastro, Atualizado, Ativo)
VALUES
(@CNPJ, @MatrizFilial, @Abertura, @RazaoSocial, @NomeFantasia, @NaturezaJuridica, @AtividadePrincipal, @AtividadeSecundaria, @SituacaoCadastral, @DataSituacaoCadastral, @Logradouro, @Numero, @Complemento, @CEP, @Bairro, @Municipio, @UF, @Email, @Telefones, @Cadastro, @Atualizado, @Ativo)";


                string Alteracao = @"UPDATE SDT_SE_PJ
SET            
CNPJ = @CNPJ, MatrizFilial = @MatrizFilial, Abertura = @Abertura, RazaoSocial = @RazaoSocial, NomeFantasia = @NomeFantasia, NaturezaJuridica = @NaturezaJuridica, AtividadePrincipal = @AtividadePrincipal, AtividadeSecundaria = @AtividadeSecundaria, SituacaoCadastral = @SituacaoCadastral, DataSituacaoCadastral = @DataSituacaoCadastral , Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, CEP = @CEP, Bairro = @Bairro, Municipio = @Municipio, UF = @UF, Email = @Email, Telefones = @Telefones, Cadastro = @Cadastro, Atualizado = @Atualizado, Ativo = @Ativo
WHERE
(Indice = @Original_Indice)";

                if (tipo == Registro.Alteracao)
                    return dataAccess.Write(Alteracao);
                else
                    return dataAccess.Write(Novo);
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool GravarFormalizacao(mFormalizada obj, Registro tipo)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@CNPJ", new mMascaras().Remove(obj.CNPJ));
                dataAccess.AddParameters("@InscricaoMunicipal", obj.InscricaoMunicipal);
                dataAccess.AddParameters("@Porte", obj.Porte);
                dataAccess.AddParameters("@UsoLocal", obj.UsoLocal);
                dataAccess.AddParameters("@FormalizadoSE", obj.FormalizadoSE);
                dataAccess.AddParameters("@Data", obj.Data.ToShortDateString());
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                if (tipo == Registro.Alteracao)
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string Novo = @"INSERT INTO
            SDT_SE_PJ_Formalizada
            (CNPJ, InscricaoMunicipal, Porte, UsoLocal, FormalizadoSE, Data, Ativo)
            VALUES
            (@CNPJ, @InscricaoMunicipal, @Porte, @UsoLocal, @FormalizadoSE, @Data, @Ativo)";

                string Alterar = @"UPDATE SDT_SE_PJ_Formalizada
            SET
            CNPJ = @CNPJ, InscricaoMunicipal = @InscricaoMunicipal, Porte = @Porte, UsoLocal = @UsoLocal, FormalizadoSE = @FormalizadoSE, Data = @Data, Ativo = @Ativo
            WHERE
            (Indice = @Original_Indice)";


                if (tipo == Registro.Alteracao)
                    return dataAccess.Write(Alterar);
                else
                    return dataAccess.Write(Novo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GravarSegmentos(mSegmentos obj, Registro tipo)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@CNPJ", new mMascaras().Remove(obj.CNPJ_CPF));
                dataAccess.AddParameters("@Agronegocio", obj.Agronegocio);
                dataAccess.AddParameters("@Industria", obj.Industria);
                dataAccess.AddParameters("@Comercio", obj.Comercio);
                dataAccess.AddParameters("@Servicos", obj.Servicos);
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                if (tipo == Registro.Alteracao)
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string Novo = @"INSERT INTO
            SDT_SE_PJ_Segmento
            (CNPJ, Agronegocio, Industria, Comercio, Servicos, Ativo)
            VALUES
            (@CNPJ, @Agronegocio, @Industria, @Comercio, @Servicos, @Ativo)";

                string Alteracao = @"UPDATE SDT_SE_PJ_Segmento
            SET
            CNPJ = @CNPJ, Agronegocio = @Agronegocio, Industria = @Industria, Comercio = @Comercio, Servicos = @Servicos, Ativo = @Ativo
            WHERE
            (Indice = @Original_Indice)";

                if (tipo == Registro.Alteracao)
                    return dataAccess.Write(Alteracao);
                else
                    return dataAccess.Write(Novo);
                
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool GravarDeficiencia(mDeficiencia obj, Registro tipo)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@CPF", new mMascaras().Remove(obj.CPF));
                dataAccess.AddParameters("@Deficiencia", obj.Deficiencia);
                dataAccess.AddParameters("@Fisica", obj.Fisica);
                dataAccess.AddParameters("@Visual", obj.Visual);
                dataAccess.AddParameters("@Auditiva", obj.Auditiva);
                dataAccess.AddParameters("@Intelectual", obj.Intelectual);
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                if (tipo == Registro.Alteracao)
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string Novo = @"INSERT INTO SDT_SE_PF_Deficiencia (CPF, Deficiencia, Fisica, Visual, Auditiva, Intelectual, Ativo) VALUES (@CPF, @Deficiencia, @Fisica, @Visual, @Servicos, @Profissional_Liberal, @Ativo)";

                string Alteracao = @"UPDATE SDT_SE_PF_Deficiencia SET CPF = @CPF, Deficiencia = @Deficiencia, Fisica = @Fisica, Visual = @Visual, Auditiva = @Auditiva, Intelectual = @Intelectual, Ativo = @Ativo WHERE (Indice = @Original_Indice)";

                if (tipo == Registro.Alteracao)
                    return dataAccess.Write(Alteracao);
                else
                    return dataAccess.Write(Novo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GravarVinculos(mVinculos obj, Registro tipo)
        {
            bool res = false;
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                dataAccess.AddParameters("@CNPJ", new mMascaras().Remove(obj.CNPJ));
                dataAccess.AddParameters("@CPF", new mMascaras().Remove(obj.CPF));
                dataAccess.AddParameters("@Vinculo", obj.Vinculo);
                dataAccess.AddParameters("@Data", obj.Data.ToShortDateString());
                dataAccess.AddParameters("@Ativo", obj.Ativo);               

                if (obj.Acao == 1)
                    res = dataAccess.Write(@"INSERT INTO SDT_SE_PJPF_Vinculos (CNPJ, CPF, Vinculo, Data, Ativo) VALUES (@CNPJ, @CPF, @Vinculo, @Data, @Ativo)");

                if (obj.Acao == -1)
                {
                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);
                    res = dataAccess.Write(@"DELETE FROM SDT_SE_PJPF_Vinculos WHERE (Indice = @Original_Indice)");
                }

                return res;
            }
            catch (Exception ex)
            { dataAccess = null; throw new Exception(ex.Message); }
        }

        public bool GravarPerfil(mPerfil obj, Registro tipo)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                dataAccess.AddParameters("@CPF", new mMascaras().Remove(obj.CPF));
                dataAccess.AddParameters("@Perfil", obj.Perfil);
                dataAccess.AddParameters("@Negocio", obj.Negocio);
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                if (tipo == Registro.Alteracao)
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string Novo = @"INSERT INTO SDT_SE_PF_Perfil (CPF, Perfil, Negocio, Ativo) VALUES (@CPF, @Perfil, @Negocio, @Ativo)";

                string Alteracao = @"UPDATE SDT_SE_PF_Perfil SET CPF = @CPF, Perfil = @Perfil, Negocio = @Negocio, Ativo = @Ativo WHERE (Indice = @Original_Indice)";

                if (tipo == Registro.Alteracao)
                    return dataAccess.Write(Alteracao);
                else
                    return dataAccess.Write(Novo);

            }
            catch (Exception ex)
            { dataAccess = null; throw new Exception(ex.Message); }
        }

        public bool GravarViabilidade(mViabilidade obj, Registro tipo)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                string _cliente = string.Format(@"{0}/{1}/{2}/{3}",
                    new mMascaras().Remove(obj.Requerente.Inscricao),
                    obj.Requerente.NomeRazao,
                    obj.Requerente.Telefones,
                    obj.Requerente.Email);

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Viabilidade", obj.Protocolo);
                dataAccess.AddParameters("@Requerente", _cliente);
                dataAccess.AddParameters("@CEP", new mMascaras().Remove(obj.CEP));
                dataAccess.AddParameters("@Logradouro", obj.Logradouro);
                dataAccess.AddParameters("@Numero", obj.Numero);
                dataAccess.AddParameters("@Complemento", obj.Complemento);
                dataAccess.AddParameters("@Bairro", obj.Bairro);
                dataAccess.AddParameters("@Municipio", obj.Municipio);
                dataAccess.AddParameters("@UF", obj.UF);
                dataAccess.AddParameters("@CTM", obj.CTM);
                dataAccess.AddParameters("@Atividades", obj.Atividades);
                dataAccess.AddParameters("@TextoEmail", obj.TextoEmail);
                dataAccess.AddParameters("@EmailEnviado", obj.SendMail);
                dataAccess.AddParameters("@Data", obj.Data.ToShortDateString());
                dataAccess.AddParameters("@DataParecer", obj.DataParecer);
                dataAccess.AddParameters("@Parecer", obj.Perecer);
                dataAccess.AddParameters("@Motivo", obj.Motivo);
                dataAccess.AddParameters("@Operador", obj.Operador);
                dataAccess.AddParameters("@RetornoCliente", obj.RetornoCliente);
                dataAccess.AddParameters("@DataRetorno", obj.DataRetorno.ToShortDateString());
                dataAccess.AddParameters("@SemRetorno", obj.SemRetorno);
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                if (tipo == Registro.Alteracao)
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string Novo = @"INSERT INTO
            SDT_SE_Viabilidade
            (Viabilidade, Requerente, CEP, Logradouro, Numero, Complemento, Bairro, Municipio, UF, CTM, Atividades, TextoEmail, EmailEnviado, Data, DataParecer, Parecer, Motivo, Operador, RetornoCliente, DataRetorno, SemRetorno, Ativo)
            VALUES
            (@Viabilidade, @Requerente, @CEP, @Logradouro, @Numero, @Complemento, @Bairro, @Municipio, @UF, @CTM, @Atividades, @TextoEmail, @EmailEnviado, @Data, @DataParecer, @Parecer, @Motivo, @Operador, @RetornoCliente, @DataRetorno, @SemRetorno, @Ativo)";

                string Alterar = @"UPDATE SDT_SE_Viabilidade
            SET
            DataParecer = @DataParecer, Parecer = @Parecer, Motivo = @Motivo, RetornoCliente = @RetornoCliente, DataRetorno = @DataRetorno,  Ativo = @Ativo
            WHERE
            (Indice = @Original_Indice)";

                if (tipo == Registro.Alteracao)
                    return dataAccess.Write(Alterar);
                else
                    return dataAccess.Write(Novo);

            }
            catch(Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool AtualizarViabilidade(object obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                var objetos = (object[])obj;
                var viabilidade = (string)objetos[0];
                var parecer = (System.Windows.Controls.ComboBox)objetos[1];
                var data = DateTime.Now.ToShortDateString();
                var motivo = (System.Windows.Controls.TextBox)objetos[2];
                
                dataAccess.ClearParameters();

                dataAccess.AddParameters("@DataParecer", data);
                dataAccess.AddParameters("@Parecer", parecer.SelectedValue);
                dataAccess.AddParameters("@Motivo", motivo.Text);
                dataAccess.AddParameters("@Viabilidade", viabilidade);

                string Alterar = @"UPDATE SDT_SE_Viabilidade
            SET
            DataParecer = @DataParecer, Parecer = @Parecer, Motivo = @Motivo
            WHERE
            (Viabilidade = @Viabilidade)";

                return dataAccess.Write(Alterar);              

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;                
            }
        }

        public bool AtualizarViabilidadeEmailOk(string protocolo, bool obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@emailenviado", obj);
                dataAccess.AddParameters("@protocolo", protocolo);

                string Alterar = @"UPDATE SDT_SE_Viabilidade
            SET
            EmailEnviado = @emailenviado
            WHERE
            (Viabilidade = @protocolo)";

                return dataAccess.Write(Alterar);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool AtualizarViabilidadeClienteRetorno(string protocolo, bool retorno, bool semretorno)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@retorno", retorno);
                dataAccess.AddParameters("@data", DateTime.Now.ToShortDateString());
                dataAccess.AddParameters("@semretorno", semretorno);
                dataAccess.AddParameters("@protocolo", protocolo);

                string Alterar = @"UPDATE SDT_SE_Viabilidade
            SET
            RetornoCliente = @retorno, DataRetorno = @data, SemRetorno = @semretorno
            WHERE
            (Viabilidade = @protocolo)";

                return dataAccess.Write(Alterar);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool GravarAtendimento(mAtendimento obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();


                string _cliente = string.Format(@"{0}/{1}/{2}/{3}",
                    new mMascaras().Remove(obj.Cliente.Inscricao), 
                    obj.Cliente.NomeRazao, 
                    obj.Cliente.Telefones, 
                    obj.Cliente.Email);

                //System.Windows.MessageBox.Show(_cliente);

                dataAccess.AddParameters("@Protocolo", obj.Protocolo);
                dataAccess.AddParameters("@Data", obj.Data.ToShortDateString());
                dataAccess.AddParameters("@Hora", obj.Data.ToShortTimeString());
                dataAccess.AddParameters("@Cliente", _cliente);
                dataAccess.AddParameters("@Origem", obj.Origem);
                dataAccess.AddParameters("@Tipo", obj.TipoString);
                dataAccess.AddParameters("@Historico", obj.Historico);
                dataAccess.AddParameters("@Operador", obj.Operador);
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                string Novo = @"INSERT INTO
            SDT_Atendimento
            (Protocolo, Data, Hora, Cliente, Origem, Tipo, Historico, Operador, Ativo)
            VALUES
            (@Protocolo, @Data, @Hora, @Cliente, @Origem, @Tipo, @Historico, @Operador, @Ativo)";

                return dataAccess.Write(Novo);

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool AtualizarAtendimento(mAtendimento obj, string id)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Protocolo", obj.Protocolo);

                if (dataAccess.Read("SELECT Operador FROM SDT_Atendimento WHERE (Protocolo = @Protocolo)").Rows[0][0].ToString() != id)
                    return false;

                string _cliente = string.Format(@"{0}/{1}/{2}/{3}",
                    new mMascaras().Remove(obj.Cliente.Inscricao),
                    obj.Cliente.NomeRazao,
                    obj.Cliente.Telefones,
                    obj.Cliente.Email);

                //System.Windows.MessageBox.Show(_cliente);
                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Cliente", _cliente);
                dataAccess.AddParameters("@Origem", obj.Origem);
                dataAccess.AddParameters("@Tipo", obj.TipoString);
                dataAccess.AddParameters("@Historico", obj.Historico);
                dataAccess.AddParameters("@Ativo", obj.Ativo);
                dataAccess.AddParameters("@Protocolo", obj.Protocolo);

                string Alterar = @"UPDATE SDT_Atendimento SET
            Cliente = @Cliente, Origem = @Origem, Tipo = @Tipo, Historico = @Historico, Ativo = @Ativo
            WHERE
            (Protocolo = @Protocolo)";

                return dataAccess.Write(Alterar);

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool GravarAmbulanteAtendimento(string _cadastro, string _atendimento)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Atendimento", _atendimento);
                dataAccess.AddParameters("@Cadastro", _cadastro);

                string _update = @"UPDATE SDT_CAmbulante SET [Atendimento] = @Atendimento WHERE ([Cadastro] = @Cadastro)";

                //dataAccess.WriteR(_update);

                return dataAccess.Write(_update);
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message);
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool GravarEvento(mAgenda obj)
        {
            string Novo = @"INSERT INTO
            SDT_Agenda
            (Codigo, Tipo, Evento, Vagas, Descricao, Data, Hora, Setor, Estado, Criacao, Ativo)
            VALUES
            (@Codigo, @Tipo, @Evento, @Vagas, @Descricao, @Data, @Hora, @Setor, @Estado, @Criacao, @Ativo)";

            string Update = @"UPDATE
            SDT_Agenda
            SET Tipo = @Tipo, Evento = @Evento, Vagas = @Vagas, Descricao = @Descricao, Data = @Data, Hora = @Hora, Setor = @Setor, Estado = @Estado, Criacao = @Criacao, Ativo = @Ativo
            WHERE (Codigo = @Codigo)";

            string check = @"SELECT * FROM SDT_Agenda WHERE (Codigo = @Codigo)";

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Codigo", obj.Codigo);

                //novo
                if (dataAccess.Read(check).Rows.Count == 0)
                {
                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Codigo", obj.Codigo);
                    dataAccess.AddParameters("@Tipo", obj.Tipo);
                    dataAccess.AddParameters("@Evento", obj.Evento);
                    dataAccess.AddParameters("@Vagas", obj.Vagas);
                    dataAccess.AddParameters("@Descricao", obj.Descricao);
                    dataAccess.AddParameters("@Data", obj.Data.ToShortDateString());
                    dataAccess.AddParameters("@Hora", obj.Hora.ToShortTimeString());
                    dataAccess.AddParameters("@Setor", obj.Setor);
                    dataAccess.AddParameters("@Estado", obj.Estado);
                    dataAccess.AddParameters("@Criacao", obj.Criacao.ToShortDateString());
                    dataAccess.AddParameters("@Ativo", obj.Ativo);
                    return dataAccess.Write(Novo);
                }

                //update
                else
                {
                    dataAccess.ClearParameters();                    
                    dataAccess.AddParameters("@Tipo", obj.Tipo);
                    dataAccess.AddParameters("@Evento", obj.Evento);
                    dataAccess.AddParameters("@Vagas", obj.Vagas);
                    dataAccess.AddParameters("@Descricao", obj.Descricao);
                    dataAccess.AddParameters("@Data", obj.Data.ToShortDateString());
                    dataAccess.AddParameters("@Hora", obj.Hora.ToShortTimeString());
                    dataAccess.AddParameters("@Setor", obj.Setor);
                    dataAccess.AddParameters("@Estado", obj.Estado);
                    dataAccess.AddParameters("@Ativo", obj.Ativo);
                    dataAccess.AddParameters("@Codigo", obj.Codigo);
                    return dataAccess.Write(Update);
                }

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool EditarEvento(string obj, mAgenda ag)
        {

            string Update = @"UPDATE
            SDT_Agenda
            SET Tipo = @Tipo, Evento = @Evento, Vagas = @Vagas, Descricao = @Descricao, Data = @Data, Hora = @Hora, Setor = @Setor, Estado = @Estado
            WHERE (Codigo = @Codigo)";

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Tipo", ag.Tipo);
                dataAccess.AddParameters("@Evento", ag.Evento);
                dataAccess.AddParameters("@Vagas", ag.Vagas);
                dataAccess.AddParameters("@Descricao", ag.Descricao);
                dataAccess.AddParameters("@Data", ag.Data);
                dataAccess.AddParameters("@Hora", ag.Hora);
                dataAccess.AddParameters("@Setor", ag.Setor);
                dataAccess.AddParameters("@Estado", ag.Estado);
                dataAccess.AddParameters("@Codigo", obj);
                return dataAccess.Write(Update);

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool AtualizarEvento(string obj, int valor)
        {

            string Update = @"UPDATE
            SDT_Agenda
            SET Estado = @Estado
            WHERE (Codigo = @Codigo)";

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Estado", valor);
                dataAccess.AddParameters("@Codigo", obj);
                return dataAccess.Write(Update);               

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool ExiteClienteEvento(string evento, string cpf)
        {

            string check = @"SELECT * FROM SDT_Inscricoes WHERE (Evento = @Evento) AND (CPF = @CPF)";

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Evento", evento);
                dataAccess.AddParameters("@CPF", cpf);

                if (dataAccess.Read(check).Rows.Count == 0)
                    return false;

                else
                    return true;

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool GravarInscricao(mInscricao obj)
        {
            string Novo = @"INSERT INTO
            SDT_Inscricoes
            (Inscricao, Evento, Atendimento, CPF, Nome, CNPJ, Telefones, Email, Data, Presente, Ativo)
            VALUES
            (@Inscricao, @Evento, @Atendimento, @CPF, @Nome, @CNPJ, @Telefones, @Email, @Data, @Presente, @Ativo)";

            string check = @"SELECT * FROM SDT_Inscricoes WHERE (Evento = @Evento) AND (CPF = @CPF)";

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Evento", obj.Evento);
                dataAccess.AddParameters("@CPF", obj.CPF);

                //novo
                if (dataAccess.Read(check).Rows.Count == 0)
                {
                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Inscricao", obj.Inscricao);
                    dataAccess.AddParameters("@Evento", obj.Evento);
                    dataAccess.AddParameters("@Atendimento", obj.Evento);
                    dataAccess.AddParameters("@CPF", obj.CPF);
                    dataAccess.AddParameters("@Nome", obj.Nome);
                    dataAccess.AddParameters("@CNPJ", obj.CNPJ);
                    dataAccess.AddParameters("@Telefones", obj.Telefone);
                    dataAccess.AddParameters("@Email", obj.Email);
                    dataAccess.AddParameters("@Data", obj.Data.ToShortDateString());
                    dataAccess.AddParameters("@Presente", obj.Presente);
                    dataAccess.AddParameters("@Ativo", obj.Ativo);
                    return dataAccess.Write(Novo);
                }

                else
                {
                    return false;
                    throw new Exception(string.Format("Cliente {0} já encontra-se inscrito no evento,\n verifique a lista normal e cancelados", obj.CPF));
                }

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool AtualizarAtendimentoInscricao(mInscricao obj)
        {

            string Update = @"UPDATE
            SDT_Inscricoes
            SET Atendimento = @Atendimento
            WHERE (Inscricao = @Inscricao)";
            
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Atendimento", obj.Atendimento);
                    dataAccess.AddParameters("@Inscricao", obj.Inscricao);
                    return dataAccess.Write(Update);

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool InscricaoPresente(string inscricao, bool valor)
        {

            string Update = @"UPDATE
            SDT_Inscricoes
            SET Presente = @Presente
            WHERE (Inscricao = @Inscricao)";

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Presente", valor);
                dataAccess.AddParameters("@Inscricao", inscricao);
                return dataAccess.Write(Update);
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool CancelarInscricao(string inscricao, bool valor)
        {

            string Update = @"UPDATE
            SDT_Inscricoes
            SET Ativo = @Ativo
            WHERE (Inscricao = @Inscricao)";

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Ativo", valor);
                dataAccess.AddParameters("@Inscricao", inscricao);
                return dataAccess.Write(Update);
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Consulta / Objeto

        public mPJ ConsultaCNPJ_N(string cnpj)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@CNPJ", cnpj);

                DataRow dr = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ WHERE (CNPJ = @CNPJ) AND (Ativo = true)").Rows[0];

                if ((string)dr[1] == cnpj)
                {

                    var line = new mPJ();

                    line.Indice = (int)dr[0];
                    line.CNPJ = (string)dr[1];
                    line.MatrizFilial = (string)dr[2];
                    line.Abertura = (DateTime)dr[3];
                    line.RazaoSocial = (string)dr[4];
                    line.NomeFantasia = (string)dr[5];
                    line.NaturezaJuridica = (string)dr[6];
                    line.AtividadePrincipal = (string)dr[7];
                    line.AtividadeSecundaria = (string)dr[8];
                    line.SituacaoCadastral = (string)dr[9];
                    line.DataSituacaoCadastral = (DateTime)dr[10];

                    line.Logradouro = (string)dr[11];
                    line.Numero = (string)dr[12];
                    line.Complemento = (string)dr[13];
                    line.CEP = (string)dr[14];
                    line.Bairro = (string)dr[15];
                    line.Municipio = (string)dr[16];
                    line.UF = (string)dr[17];
                    line.Email = (string)dr[18];
                    line.Telefones = (string)dr[19];

                    line.Cadastro = (DateTime)dr[20];
                    line.Atualizado = (DateTime)dr[21];

                    line.Ativo = (bool)dr[22];

                    return line;
                }
                else
                    return null;

            }
            catch
            { return null; }

        }
        
        public mPF_Ext ExistPessoaFisica(string cpf)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@CPF", new mMascaras().Remove(cpf));

                string commandsql = @"SELECT * FROM SDT_SE_PF WHERE (CPF = @CPF) AND (Ativo = true)";

                var data_perfil_t = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil_Tipos ORDER BY Valor");
                var data_perfil = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil");
                var data_segmento = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento");
                var data_deficiente = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Deficiencia");
                var data_vinculo = dataAccess.Read(@"SELECT * FROM SDT_SE_PJPF_Vinculos");
                var data_vinculo_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_PJPF_Vinculos_Tipos ORDER BY Valor");

                var pessoa = new mPF_Ext();

                foreach (DataRow _p in dataAccess.Read(commandsql).Rows)
                {

                    if ((string)_p[3] == string.Empty)
                        return null;

                    pessoa.Indice = (int)_p[0];
                    pessoa.Nome = (string)_p[1];
                    pessoa.RG = Convert.ToString(_p[2]);
                    pessoa.CPF = (string)_p[3];
                    pessoa.DataNascimento = (DateTime)_p[4];
                    pessoa.Sexo = (int)_p[5];

                    pessoa.CEP = (string)_p[6];
                    pessoa.Logradouro = (string)_p[7];
                    pessoa.Numero = (string)_p[8];
                    pessoa.Complemento = (string)_p[9];
                    pessoa.Bairro = (string)_p[10];
                    pessoa.Municipio = (string)_p[11];
                    pessoa.UF = (string)_p[12];
                    pessoa.Email = (string)_p[13];
                    pessoa.Telefones = (string)_p[14];

                    pessoa.Cadastro = (DateTime)_p[15];
                    pessoa.Atualizado = (DateTime)_p[16];
                    pessoa.Ativo = (bool)_p[17];

                    //pega o perfil
                    foreach (DataRow _perfil in data_perfil.Rows)
                    {
                        if (pessoa.CPF == (string)_perfil[1])
                        {
                            var p = new mPerfil();
                            p.Indice = (int)_perfil[0];
                            p.CPF = (string)_perfil[1];
                            p.Perfil = (int)_perfil[2];
                            p.Negocio = (bool)_perfil[3];
                            p.Ativo = (bool)_perfil[4];

                            p.PerfilString= (string)data_perfil_t.Rows[p.Perfil][1];

                            pessoa.Perfil = p;
                        }
                    }

                    //pega segmento (se for profissional liberal)
                    foreach (DataRow _segmento in data_segmento.Rows)
                    {
                        if (pessoa.CPF == (string)_segmento[1])
                        {
                            var s = new mSegmentos();
                            s.Indice = (int)_segmento[0];
                            s.CNPJ_CPF = (string)_segmento[1];
                            s.Agronegocio = (bool)_segmento[2];
                            s.Industria = (bool)_segmento[3];
                            s.Servicos = (bool)_segmento[4];
                            s.Comercio = (bool)_segmento[5];
                            s.Ativo = (bool)_segmento[6];
                            pessoa.Segmentos = s;
                        }
                    }

                    //pega vinculo
                    ObservableCollection<mVinculos> colecaovinculos = new ObservableCollection<mVinculos>();
                    foreach (DataRow _vinculo in data_vinculo.Rows)
                    {
                        if (pessoa.CPF == (string)_vinculo[2])
                        {
                            var v = new mVinculos();
                            v.Indice = (int)_vinculo[0];
                            v.CNPJ = (string)_vinculo[1];
                            v.CPF = (string)_vinculo[2];
                            v.Vinculo = (int)_vinculo[3];
                            v.Data = (DateTime)_vinculo[4];
                            v.Ativo = (bool)_vinculo[5];
                            v.VinculoString = (string)data_vinculo_tipos.Rows[v.Vinculo][1];
                            colecaovinculos.Add(v);
                        }
                    }

                    pessoa.ColecaoVinculos = colecaovinculos;

                    //Pega deficientes
                    foreach (DataRow _deficiencia in data_deficiente.Rows)
                    {
                        if (pessoa.CPF == (string)_deficiencia[1])
                        {
                            var d = new mDeficiencia();
                            d.Indice = (int)_deficiencia[0];
                            d.CPF = (string)_deficiencia[1];
                            d.Deficiencia = (bool)_deficiencia[2];
                            d.Fisica = (bool)_deficiencia[3];
                            d.Visual = (bool)_deficiencia[4];
                            d.Auditiva = (bool)_deficiencia[5];
                            d.Intelectual = (bool)_deficiencia[6];
                            d.Ativo = (bool)_deficiencia[7];

                            pessoa.Deficiente = d;
                        }
                    }

                }

                if (pessoa.CPF == null || pessoa.CPF == string.Empty)
                    pessoa = null;

                return pessoa;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }
        
        public mPJ_Ext ExistPessoaJuridica(string cnpj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                var pjuridica = new mPJ_Ext();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@CNPJ", new mMascaras().Remove(cnpj));

                var data_formalizada = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Formalizada WHERE (CNPJ = @CNPJ) AND (Ativo = true)");
                var data_segmento = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento WHERE (CNPJ = @CNPJ) AND (Ativo = true)");

                foreach (DataRow _pj in dataAccess.Read(@"SELECT * FROM SDT_SE_PJ WHERE (CNPJ = @CNPJ) AND (Ativo = true)").Rows)
                {

                    pjuridica.Indice = (int)_pj[0];
                    pjuridica.CNPJ = (string)_pj[1];
                    pjuridica.MatrizFilial = (string)_pj[2];
                    pjuridica.Abertura = (DateTime)_pj[3];
                    pjuridica.RazaoSocial = (string)_pj[4];
                    pjuridica.NomeFantasia = (string)_pj[5];
                    pjuridica.NaturezaJuridica = (string)_pj[6];
                    pjuridica.AtividadePrincipal = (string)_pj[7];
                    pjuridica.AtividadeSecundaria = (string)_pj[8];
                    pjuridica.SituacaoCadastral = (string)_pj[9];
                    pjuridica.DataSituacaoCadastral = (DateTime)_pj[10];

                    pjuridica.Logradouro = (string)_pj[11];
                    pjuridica.Numero = (string)_pj[12];
                    pjuridica.Complemento = (string)_pj[13];
                    pjuridica.CEP = (string)_pj[14];
                    pjuridica.Bairro = (string)_pj[15];
                    pjuridica.Municipio = (string)_pj[16];
                    pjuridica.UF = (string)_pj[17];
                    pjuridica.Email = (string)_pj[18];
                    pjuridica.Telefones = (string)_pj[19];

                    pjuridica.Cadastro = (DateTime)_pj[20];
                    pjuridica.Atualizado = (DateTime)_pj[21];

                    pjuridica.Ativo = (bool)_pj[22];

                    foreach (DataRow _formalizada in data_formalizada.Rows)
                    {
                        if (pjuridica.CNPJ == (string)_formalizada[1])
                        {
                            var f = new mFormalizada();
                            f.Indice = (int)_formalizada[0];
                            f.CNPJ = (string)_formalizada[1];
                            f.InscricaoMunicipal = (string)_formalizada[2];
                            f.Porte = (int)_formalizada[3];
                            f.UsoLocal = (int)_formalizada[4];
                            f.FormalizadoSE = (bool)_formalizada[5];
                            f.Data = (DateTime)_formalizada[6];
                            f.Ativo = (bool)_formalizada[7];
                            pjuridica.Formalizada = f;
                        }
                    }

                    foreach (DataRow _segmento in data_segmento.Rows)
                    {
                        if (pjuridica.CNPJ == (string)_segmento[1])
                        {
                            var s = new mSegmentos();
                            s.Indice = (int)_segmento[0];
                            s.CNPJ_CPF = (string)_segmento[1];
                            s.Agronegocio = (bool)_segmento[2];
                            s.Industria = (bool)_segmento[3];
                            s.Comercio = (bool)_segmento[4];
                            s.Servicos = (bool)_segmento[5];
                            s.Ativo = (bool)_segmento[6];
                            pjuridica.Segmentos = s;
                        }
                    }

                    
                }

                if (pjuridica.CNPJ == null)
                    pjuridica = null;
                
                return pjuridica;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public mPJ_Ext ExistPessoaJuridicaAtiva(string cnpj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                var pjuridica = new mPJ_Ext();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@CNPJ", new mMascaras().Remove(cnpj));

                var data_formalizada = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Formalizada WHERE (CNPJ = @CNPJ) AND (Ativo = true)");
                var data_segmento = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento WHERE (CNPJ = @CNPJ) AND (Ativo = true)");

                foreach (DataRow _pj in dataAccess.Read(@"SELECT * FROM SDT_SE_PJ WHERE ((CNPJ = @CNPJ) AND (SituacaoCadastral = 'ATIVA')  AND (Ativo = true))").Rows)
                {

                    pjuridica.Indice = (int)_pj[0];
                    pjuridica.CNPJ = (string)_pj[1];
                    pjuridica.MatrizFilial = (string)_pj[2];
                    pjuridica.Abertura = (DateTime)_pj[3];
                    pjuridica.RazaoSocial = (string)_pj[4];
                    pjuridica.NomeFantasia = (string)_pj[5];
                    pjuridica.NaturezaJuridica = (string)_pj[6];
                    pjuridica.AtividadePrincipal = (string)_pj[7];
                    pjuridica.AtividadeSecundaria = (string)_pj[8];
                    pjuridica.SituacaoCadastral = (string)_pj[9];
                    pjuridica.DataSituacaoCadastral = (DateTime)_pj[10];

                    pjuridica.Logradouro = (string)_pj[11];
                    pjuridica.Numero = (string)_pj[12];
                    pjuridica.Complemento = (string)_pj[13];
                    pjuridica.CEP = (string)_pj[14];
                    pjuridica.Bairro = (string)_pj[15];
                    pjuridica.Municipio = (string)_pj[16];
                    pjuridica.UF = (string)_pj[17];
                    pjuridica.Email = (string)_pj[18];
                    pjuridica.Telefones = (string)_pj[19];

                    pjuridica.Cadastro = (DateTime)_pj[20];
                    pjuridica.Atualizado = (DateTime)_pj[21];

                    pjuridica.Ativo = (bool)_pj[22];

                    foreach (DataRow _formalizada in data_formalizada.Rows)
                    {
                        if (pjuridica.CNPJ == (string)_formalizada[1])
                        {
                            var f = new mFormalizada();
                            f.Indice = (int)_formalizada[0];
                            f.CNPJ = (string)_formalizada[1];
                            f.InscricaoMunicipal = (string)_formalizada[2];
                            f.Porte = (int)_formalizada[3];
                            f.UsoLocal = (int)_formalizada[4];
                            f.FormalizadoSE = (bool)_formalizada[5];
                            f.Data = (DateTime)_formalizada[6];
                            f.Ativo = (bool)_formalizada[7];
                            pjuridica.Formalizada = f;
                        }
                    }

                    foreach (DataRow _segmento in data_segmento.Rows)
                    {
                        if (pjuridica.CNPJ == (string)_segmento[1])
                        {
                            var s = new mSegmentos();
                            s.Indice = (int)_segmento[0];
                            s.CNPJ_CPF = (string)_segmento[1];
                            s.Agronegocio = (bool)_segmento[2];
                            s.Industria = (bool)_segmento[3];
                            s.Comercio = (bool)_segmento[4];
                            s.Servicos = (bool)_segmento[5];
                            s.Ativo = (bool)_segmento[6];
                            pjuridica.Segmentos = s;
                        }
                    }


                }

                if (pjuridica.CNPJ == null)
                    pjuridica = null;

                return pjuridica;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public mCNAE ConsultaCNAE(string cnae)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                var _cnae = new mCNAE();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@CNAE", cnae);

                foreach (DataRow _atv in dataAccess.Read(@"SELECT * FROM SDT_SE_CNAE_MEI WHERE (CNAE = @CNAE) AND (Ativo = true)").Rows)
                {

                    _cnae.Indice = (int)_atv[0];
                    _cnae.Ocupacao = (string)_atv[1];
                    _cnae.CNAE = (string)_atv[2];
                    _cnae.Descricao = (string)_atv[3];
                    _cnae.ISS = (string)_atv[4];
                    _cnae.ICMS = (string)_atv[5];
                    _cnae.Ativo = (bool)_atv[6];

                }

                if (_cnae.CNAE == null)
                    _cnae = null;

                return _cnae;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public mAtendimento Atendimento(string protocolo)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var atendimento = new mAtendimento();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@protocolo", protocolo);

                var _tipo = dataAccess.Read("SELECT * FROM SDT_Atendimento_Tipos ORDER BY Valor");
                var _origem = dataAccess.Read("SELECT * FROM SDT_Atendimento_Origem ORDER BY Valor");

                string sql = @"SELECT * FROM SDT_Atendimento WHERE (Protocolo LIKE @protocolo)";
                int cont = 1;

                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    atendimento.Indice = (int)at[0];
                    atendimento.Protocolo = (string)at[1];
                    atendimento.Data = (DateTime)at[2];

                    atendimento.Hora = (DateTime)at[3];

                    string[] words = at[4].ToString().Split('/');

                    atendimento.Cliente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    atendimento.Origem = (int)at[5];
                    atendimento.TipoString = (string)at[6];
                    atendimento.Historico = (string)at[7];
                    atendimento.Operador = (string)at[8];
                    atendimento.Ativo = (bool)at[9];

                    //atendimento.TipoString = (string)_tipo.Rows[atendimento.Tipo][1];
                    atendimento.OrigemString = (string)_origem.Rows[atendimento.Origem][1];
                    atendimento.Contador = cont;
                    cont++;
                }

                return atendimento;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public mViabilidade Viabilidade(string protocolo)
        {
            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                mViabilidade _vb = new mViabilidade();

                var data_parecer_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_Viabilidade_Situacao ORDER BY Valor");

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@protocolo", protocolo);
               
                string sql = @"SELECT * FROM SDT_SE_Viabilidade WHERE (Viabilidade LIKE @protocolo)";

                //System.Windows.MessageBox.Show(par);
                int cont = 1;
                foreach (DataRow dr in dataAccess.Read(sql).Rows)
                {                  

                    _vb.Indice = (int)dr[0];
                    _vb.Protocolo = (string)dr[1];

                    string[] words = dr[2].ToString().Split('/');

                    _vb.Requerente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    _vb.CEP = (string)dr[3];
                    _vb.Logradouro = (string)dr[4];
                    _vb.Numero = (string)dr[5];
                    _vb.Complemento = (dr[6] != DBNull.Value) ? (string)dr[6] : "";
                    _vb.Bairro = (string)dr[7];
                    _vb.Municipio = (string)dr[8];
                    _vb.UF = (string)dr[9];
                    _vb.CTM = (string)dr[10];
                    _vb.Atividades = (string)dr[11];
                    _vb.TextoEmail = (string)dr[12];
                    _vb.SendMail = (bool)dr[13];
                    _vb.Data = (DateTime)dr[14];
                    _vb.DataParecer = (DateTime)dr[15];
                    _vb.Perecer = (int)dr[16];

                    _vb.PerecerString = (string)data_parecer_tipos.Rows[_vb.Perecer][1];

                    _vb.Motivo = (dr[17] != DBNull.Value) ? (string)dr[17] : "";
                    _vb.Operador = (string)dr[18];
                    _vb.Ativo = (bool)dr[19];

                    _vb.Contador = cont;
                    cont++;                    
                }

                //System.Windows.MessageBox.Show(_list.Count.ToString());
                return _vb;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public mAgenda Evento(string codigo)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var agenda = new mAgenda();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@codigo", codigo);

                string sql = @"SELECT * FROM SDT_Agenda WHERE (Codigo LIKE @codigo)";
                var _tipos = dataAccess.Read("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Valor");
                int cont = 1;

                foreach (DataRow ag in dataAccess.Read(sql).Rows)
                {
                    agenda.Indice = (int)ag[0];
                    agenda.Codigo = (string)ag[1];
                    agenda.Tipo = (int)ag[2];

                    agenda.TipoString = (string)_tipos.Rows[agenda.Tipo][1];

                    agenda.Evento = (string)ag[3];
                    agenda.Vagas = (int)ag[4];
                    agenda.Descricao = (string)ag[5];
                    agenda.Data = (DateTime)ag[6];
                    agenda.Hora = (DateTime)ag[7];
                    agenda.Setor = (int)ag[8];
                    agenda.Estado = (int)ag[9];
                    agenda.Criacao = (DateTime)ag[10];
                    agenda.Ativo = (bool)ag[11];
                    cont++;
                }

                return agenda;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Relatorios
        /// <summary>
        /// Relatório de Atendimentos
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public ObservableCollection<mAtendimento> RAtendimentos(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAtendimento>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@origem1", commands[2]);
                dataAccess.AddParameters("@origem2", commands[3]);
                dataAccess.AddParameters("@tipo1", commands[4]);
                dataAccess.AddParameters("@tipo2", commands[5]);

                var _tipo = dataAccess.Read("SELECT * FROM SDT_Atendimento_Tipos ORDER BY Valor");
                var _origem = dataAccess.Read("SELECT * FROM SDT_Atendimento_Origem ORDER BY Valor");

                string sql = @"SELECT * FROM SDT_Atendimento WHERE (Data BETWEEN @data1 AND @data2) AND (Origem BETWEEN @origem1 AND @origem2) AND (Tipo LIKE '%' +  @tipo1 + '%') AND (Ativo = true) ORDER BY Data";

                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var atendimento = new mAtendimento();

                    atendimento.Indice = (int)at[0];
                    atendimento.Protocolo = (string)at[1];
                    atendimento.Data = (DateTime)at[2];

                    atendimento.Hora = (DateTime)at[3];

                    string[] words = at[4].ToString().Split('/');

                    atendimento.Cliente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    atendimento.Origem = (int)at[5];
                    atendimento.TipoString = (string)at[6];
                    atendimento.Historico = (string)at[7];
                    atendimento.Operador = (string)at[8];
                    atendimento.Ativo = (bool)at[9];

                    //atendimento.TipoString = (string)_tipo.Rows[atendimento.Tipo][1];
                    atendimento.OrigemString = (string)_origem.Rows[atendimento.Origem][1];

                    lista.Add(atendimento);

                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAtendimento> RAtendimentosOperador(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAtendimento>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@operador", commands[6]);

                var _tipo = dataAccess.Read("SELECT * FROM SDT_Atendimento_Tipos ORDER BY Valor");
                var _origem = dataAccess.Read("SELECT * FROM SDT_Atendimento_Origem ORDER BY Valor");

                string sql = @"SELECT * FROM SDT_Atendimento WHERE (Data BETWEEN @data1 AND @data2) AND (Operador LIKE @operador) AND (Ativo = true) ORDER BY Data";

                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var atendimento = new mAtendimento();

                    atendimento.Indice = (int)at[0];
                    atendimento.Protocolo = (string)at[1];
                    atendimento.Data = (DateTime)at[2];
                    atendimento.Hora = (DateTime)at[3];

                    string[] words = at[4].ToString().Split('/');

                    atendimento.Cliente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    atendimento.Origem = (int)at[5];
                    atendimento.TipoString = (string)at[6];
                    atendimento.Historico = (string)at[7];
                    atendimento.Operador = (string)at[8];
                    atendimento.Ativo = (bool)at[9];

                    //atendimento.TipoString = (string)_tipo.Rows[atendimento.Tipo][1];
                    atendimento.OrigemString = (string)_origem.Rows[atendimento.Origem][1];

                    lista.Add(atendimento);

                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mPJ_Ext> R_Empresas(List<object> commands)
        {

            string sqlcommand = string.Empty;

            string sql1 =
@"SELECT SDT_SE_PJ.*, SDT_SE_PJ_Segmento.*, SDT_SE_PJ_Formalizada.*
FROM(SDT_SE_PJ_Segmento INNER JOIN((SDT_SE_PJ INNER JOIN SDT_SE_PJ_Formalizada ON SDT_SE_PJ.CNPJ = SDT_SE_PJ_Formalizada.CNPJ) INNER JOIN SDT_SE_PJ_Porte ON SDT_SE_PJ_Formalizada.Porte = SDT_SE_PJ_Porte.Valor) ON SDT_SE_PJ_Segmento.CNPJ = SDT_SE_PJ.CNPJ) INNER JOIN SDT_SE_PJ_UsoLocal ON SDT_SE_PJ_Formalizada.UsoLocal = SDT_SE_PJ_UsoLocal.Valor
WHERE(((SDT_SE_PJ.Cadastro)Between @data1 And @data2) AND((SDT_SE_PJ.AtividadePrincipal)Like '%' + @atividadeprincipal + '%') AND((SDT_SE_PJ.AtividadeSecundaria)Like '%' + @atividadesecundaria + '%') AND((SDT_SE_PJ.SituacaoCadastral)Like  '%' + @situacao + '%') AND((SDT_SE_PJ_Formalizada.Porte)Like '%' + @porte + '%') AND((SDT_SE_PJ_Formalizada.UsoLocal)Like '%' + @usolocal + '%') AND((SDT_SE_PJ_Segmento.Agronegocio)Like @agro) AND((SDT_SE_PJ_Segmento.Industria)Like @industria) AND((SDT_SE_PJ_Segmento.Comercio)Like @comercio) AND((SDT_SE_PJ_Segmento.Servicos)Like @servicos))
ORDER BY SDT_SE_PJ.RazaoSocial";

            string sql2 =
@"SELECT SDT_SE_PJ.*, SDT_SE_PJ_Segmento.*, SDT_SE_PJ_Formalizada.*
FROM(SDT_SE_PJ_Segmento INNER JOIN((SDT_SE_PJ INNER JOIN SDT_SE_PJ_Formalizada ON SDT_SE_PJ.CNPJ = SDT_SE_PJ_Formalizada.CNPJ) INNER JOIN SDT_SE_PJ_Porte ON SDT_SE_PJ_Formalizada.Porte = SDT_SE_PJ_Porte.Valor) ON SDT_SE_PJ_Segmento.CNPJ = SDT_SE_PJ.CNPJ) INNER JOIN SDT_SE_PJ_UsoLocal ON SDT_SE_PJ_Formalizada.UsoLocal = SDT_SE_PJ_UsoLocal.Valor
WHERE(((SDT_SE_PJ.Cadastro)Between @data1 And @data2) AND((SDT_SE_PJ.AtividadePrincipal)Like '%' + @atividadeprincipal + '%') OR((SDT_SE_PJ.AtividadeSecundaria)Like '%' + @atividadesecundaria + '%') AND((SDT_SE_PJ.SituacaoCadastral)Like '%' + @situacao + '%') AND((SDT_SE_PJ_Formalizada.Porte)Like '%' + @porte + '%') AND((SDT_SE_PJ_Formalizada.UsoLocal)Like '%' + @usolocal + '%') AND((SDT_SE_PJ_Segmento.Agronegocio)Like @agro) AND((SDT_SE_PJ_Segmento.Industria)Like @industria) AND((SDT_SE_PJ_Segmento.Comercio)Like @comercio) AND((SDT_SE_PJ_Segmento.Servicos)Like @servicos))
ORDER BY SDT_SE_PJ.RazaoSocial";

            if ((string)commands[3] == "%")
                sqlcommand = sql1;
            else
                sqlcommand = sql2;

            try
            {
                ObservableCollection<mPJ_Ext> list = new ObservableCollection<mPJ_Ext>();

                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@atividadeprincipal", commands[2]);
                dataAccess.AddParameters("@atividadesecundaria", commands[3]);
                dataAccess.AddParameters("@situacao", commands[4]);
                dataAccess.AddParameters("@porte", commands[5]);
                dataAccess.AddParameters("@usolocal", commands[6]);
                dataAccess.AddParameters("@agro", commands[7]);
                dataAccess.AddParameters("@industria", commands[8]);
                dataAccess.AddParameters("@comercio", commands[9]);
                dataAccess.AddParameters("@servicos", commands[10]);

                //string par = commands[0] + "\n" + commands[1] + "\n" + commands[2] + "\n" + commands[3] + "\n" + commands[4] + "\n" + commands[5] + "\n"
                //    + commands[6] + "\n" + commands[7] + "\n" + commands[8] + "\n" + commands[9] + "\n" + commands[10];

                //string sql = @"SELECT * FROM SDT_SE_PJ WHERE (Ativo = True) AND (SituacaoCadastral LIKE @situacao) AND (Cadastro BETWEEN @data1 AND @data2) AND (AtividadePrincipal LIKE '%' + @atividadeprincipal + '%') ORDER BY RazaoSocial";

                //System.Windows.MessageBox.Show(par);

                foreach (DataRow dr in dataAccess.Read(sqlcommand).Rows)
                {
                    var line = new mPJ_Ext();
                    var nfa = new mFormalizada();
                    var nsg = new mSegmentos();

                    line.Indice = (int)dr[0];
                    line.CNPJ = (string)dr[1];
                    line.MatrizFilial = (string)dr[2];
                    line.Abertura = (DateTime)dr[3];
                    line.RazaoSocial = (string)dr[4];
                    line.NomeFantasia = (string)dr[5];
                    line.NaturezaJuridica = (string)dr[6];
                    line.AtividadePrincipal = (string)dr[7];
                    line.AtividadeSecundaria = (string)dr[8];
                    line.SituacaoCadastral = (string)dr[9];
                    line.DataSituacaoCadastral = (DateTime)dr[10];

                    line.Logradouro = (string)dr[11];
                    line.Numero = (string)dr[12];
                    line.Complemento = (string)dr[13];
                    line.CEP = (string)dr[14];
                    line.Bairro = (string)dr[15];
                    line.Municipio = (string)dr[16];
                    line.UF = (string)dr[17];
                    line.Email = (string)dr[18];
                    line.Telefones = (string)dr[19];

                    line.Cadastro = (DateTime)dr[20];
                    line.Atualizado = (DateTime)dr[21];

                    line.Ativo = (bool)dr[22];

                    nsg.Indice = (int)dr[23];
                    nsg.CNPJ_CPF = (string)dr[24];
                    nsg.Agronegocio = (bool)dr[25];
                    nsg.Industria = (bool)dr[26];
                    nsg.Comercio = (bool)dr[27];
                    nsg.Servicos = (bool)dr[28];
                    nsg.Ativo = (bool)dr[29];
                    line.Segmentos = nsg;

                    nfa.Indice = (int)dr[30];
                    nfa.CNPJ = (string)dr[31];
                    nfa.InscricaoMunicipal = (string)dr[32];
                    nfa.Porte = (int)dr[33];
                    nfa.UsoLocal = (int)dr[34];
                    nfa.FormalizadoSE = (bool)dr[35];
                    nfa.Data = (DateTime)dr[36];
                    nfa.Ativo = (bool)dr[37];
                    line.Formalizada = nfa;

                    list.Add(line);
                }


                return list;
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Sim.Alerta!");
                return null;
            }
        }

        public ObservableCollection<mPF_Ext> R_Pessoas(List<string> commands)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {

                var lista = new ObservableCollection<mPF_Ext>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@fem", commands[2]);
                dataAccess.AddParameters("@mas", commands[3]);

                string sql = @"SELECT * FROM SDT_SE_PF WHERE (Cadastro BETWEEN @data1 AND @data2) AND (Sexo BETWEEN @fem AND @masc) AND (Ativo = true) ORDER BY Nome";

                var data_perfil = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil");
                var data_perfil_tipos = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Perfil_Tipos ORDER BY Valor");
                //var data_segmento = dataAccess.Read(@"SELECT * FROM SDT_SE_PJ_Segmento");
                var data_deficiente = dataAccess.Read(@"SELECT * FROM SDT_SE_PF_Deficiencia");
                var data_vinculo = dataAccess.Read(@"SELECT * FROM SDT_SE_PJPF_Vinculos");

                foreach (DataRow _p in dataAccess.Read(sql).Rows)
                {
                    var pessoa = new mPF_Ext();

                    pessoa.Indice = (int)_p[0];
                    pessoa.Nome = (string)_p[1];
                    pessoa.RG = (string)_p[2];
                    pessoa.CPF = (string)_p[3];
                    pessoa.DataNascimento = (DateTime)_p[4];
                    pessoa.Sexo = (int)_p[5];

                    pessoa.CEP = (string)_p[6];
                    pessoa.Logradouro = (string)_p[7];
                    pessoa.Numero = (string)_p[8];
                    pessoa.Complemento = (string)_p[9];
                    pessoa.Bairro = (string)_p[10];
                    pessoa.Municipio = (string)_p[11];
                    pessoa.UF = (string)_p[12];
                    pessoa.Email = (string)_p[13];
                    pessoa.Telefones = (string)_p[14];

                    pessoa.Cadastro = (DateTime)_p[15];
                    pessoa.Atualizado = (DateTime)_p[16];
                    pessoa.Ativo = (bool)_p[17];

                    //pega o perfil
                    foreach (DataRow _perfil in data_perfil.Rows)
                    {
                        if (pessoa.CPF == (string)_perfil[1])
                        {
                            var p = new mPerfil();
                            p.Indice = (int)_perfil[0];
                            p.CPF = (string)_perfil[1];
                            p.Perfil = (int)_perfil[2];
                            p.Negocio = (bool)_perfil[3];
                            p.Ativo = (bool)_perfil[4];
                            p.PerfilString = (string)data_perfil_tipos.Rows[p.Perfil][1];
                            pessoa.Perfil = p;
                        }
                    }

                    //pega vinculo
                    ObservableCollection<mVinculos> colecaovinculos = new ObservableCollection<mVinculos>();
                    foreach (DataRow _vinculo in data_vinculo.Rows)
                    {
                        if (pessoa.CPF == (string)_vinculo[2])
                        {
                            var v = new mVinculos();
                            v.Indice = (int)_vinculo[0];
                            v.CNPJ = (string)_vinculo[1];
                            v.CPF = (string)_vinculo[2];
                            v.Vinculo = (int)_vinculo[3];
                            v.Data = (DateTime)_vinculo[4];
                            v.Ativo = (bool)_vinculo[5];
                            colecaovinculos.Add(v);
                        }
                    }

                    pessoa.ColecaoVinculos = colecaovinculos;

                    //Pega deficientes
                    foreach (DataRow _deficiencia in data_deficiente.Rows)
                    {
                        if (pessoa.CPF == (string)_deficiencia[1])
                        {
                            var d = new mDeficiencia();
                            d.Indice = (int)_deficiencia[0];
                            d.CPF = (string)_deficiencia[1];
                            d.Deficiencia = (bool)_deficiencia[2];
                            d.Fisica = (bool)_deficiencia[3];
                            d.Visual = (bool)_deficiencia[4];
                            d.Auditiva = (bool)_deficiencia[5];
                            d.Intelectual = (bool)_deficiencia[6];
                            d.Ativo = (bool)_deficiencia[7];

                            pessoa.Deficiente = d;
                        }
                    }
                    
                    if (commands[4] == "0")
                    {
                        lista.Add(pessoa);
                    }
                    else
                    {
                        if (pessoa.Perfil.Perfil.ToString() == commands[4])
                        {
                            if (commands[5] == "True" && pessoa.Perfil.Negocio == true)
                                lista.Add(pessoa);

                            else if (commands[5] == "False" && pessoa.Perfil.Negocio == false || pessoa.Perfil.Negocio == true)
                                lista.Add(pessoa);

                        }
                        else if (pessoa.Perfil.Perfil.ToString() == commands[4])
                            lista.Add(pessoa);

                        else if (pessoa.Perfil.Perfil.ToString() == commands[4])
                            lista.Add(pessoa);

                        else if (pessoa.Perfil.Perfil.ToString() == commands[4])
                            lista.Add(pessoa);

                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mViabilidade> R_Viabilidades(List<string> commands)
        {
            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                ObservableCollection<mViabilidade> _list = new ObservableCollection<mViabilidade>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data1", commands[0]);
                dataAccess.AddParameters("@data2", commands[1]);
                dataAccess.AddParameters("@valor1", commands[2]);
                dataAccess.AddParameters("@valor2", commands[3]);

                //string par = commands[0] + "\n" + commands[1] + "\n" + commands[2] + "\n" + commands[3] + "\n" + commands[4] + "\n" + commands[5];

                string sql = @"SELECT * FROM SDT_SE_Viabilidade WHERE (Ativo = True) AND (Data BETWEEN @data1 AND @data2) AND (Parecer BETWEEN @valor1 AND @valor2) ORDER BY Data";

                //System.Windows.MessageBox.Show(par);

                foreach (DataRow dr in dataAccess.Read(sql).Rows)
                {
                    mViabilidade _vb = new mViabilidade();

                    _vb.Indice = (int)dr[0];
                    _vb.Protocolo = (string)dr[1];

                    string[] words = dr[2].ToString().Split('/');

                    _vb.Requerente = new mCliente() { Inscricao = words[0], NomeRazao = words[1], Telefones = words[2], Email = words[3] };

                    _vb.CEP = (string)dr[3];
                    _vb.Logradouro = (string)dr[4];
                    _vb.Numero = (string)dr[5];
                    _vb.Complemento = (dr[6] != DBNull.Value) ? (string)dr[6] : "";
                    _vb.Bairro = (string)dr[7];
                    _vb.Municipio = (string)dr[8];
                    _vb.UF = (string)dr[9];
                    _vb.CTM = (string)dr[10];

                    //var result = mystring.Split(new string[] { "\\n" }, StringSplitOptions.None);
                    //(new [] { '\r', '\n' }).FirstOrDefault()
                    string _atv = dr[11].ToString().Split(new[] { '\r', '\n' }).FirstOrDefault();

                    _vb.Atividades = _atv;
                    _vb.TextoEmail = (string)dr[12];
                    _vb.SendMail=(bool)dr[13];
                    _vb.Data = (DateTime)dr[14];
                    _vb.DataParecer = (DateTime)dr[15];
                    _vb.Perecer = (int)dr[16];
                    _vb.Motivo = (dr[17] != DBNull.Value) ? (string)dr[17] : "";
                    _vb.Operador = (string)dr[18];
                    _vb.RetornoCliente = (bool)dr[19];
                    _vb.DataRetorno = (dr[20] == DBNull.Value) ? new DateTime(2018, 1, 1) : (DateTime)dr[20];
                    _vb.SemRetorno = (bool)dr[21];
                    _vb.Ativo = (bool)dr[22];

                    _list.Add(_vb);
                }

                //System.Windows.MessageBox.Show(_list.Count.ToString());
                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
