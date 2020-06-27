using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace Sim.Sec.Desenvolvimento.SebraeAqui.Model
{
    using Shared.Model;

    public class mDataSA
    {
        public bool GravarAtendimentoSebrae(mAtendimentoSebrae obj)
        {

            string Novo = @"INSERT INTO
            SDT_SAC_Atendimento
            (Atendimento, Atendimento_Sac, Cliente, Ativo)
            VALUES
            (@Atendimento, @AtendimentoSac, @Cliente, @Ativo)";

            string Update = @"UPDATE
            SDT_SAC_Atendimento
            SET Atendimento_Sac = @AtendimentoSAC, Cliente = @Cliente, Ativo = @Ativo
            WHERE (Atendimento = @Atendimento)";

            string check = @"SELECT * FROM SDT_SAC_Atendimento WHERE (Atendimento = @Atendimento)";            

            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Atendimento", obj.Atendimento);

                if (dataAccess.Read(check).Rows.Count == 0)
                {
                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@Atendimento", obj.Atendimento);
                    dataAccess.AddParameters("@AtendimentoSAC", obj.AtendimentoSAC);
                    dataAccess.AddParameters("@Cliente", obj.Cliente);
                    dataAccess.AddParameters("@Ativo", obj.Ativo);

                    return dataAccess.Write(Novo);
                }

                else
                {
                    dataAccess.ClearParameters();
                    dataAccess.AddParameters("@AtendimentoSAC", obj.AtendimentoSAC);
                    dataAccess.AddParameters("@Cliente", obj.Cliente);
                    dataAccess.AddParameters("@Ativo", obj.Ativo);
                    dataAccess.AddParameters("@Atendimento", obj.Atendimento);

                    return dataAccess.Write(Update);
                }

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAtendimento> Atendimentos(List<string> commands)
        {            
            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
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
            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
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
            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

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
    }
}
