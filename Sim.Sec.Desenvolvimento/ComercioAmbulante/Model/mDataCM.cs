using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Model
{

    using Mvvm.Observers;
    using Shared.Model;

    public class mDataCM : NotifyProperty
    {
        #region Inserção / Edição / Remoção
        public bool GravarAmbulante(mAmbulante obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {

                bool _exist = false;

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Cadastro", obj.Cadastro);

                if (dataAccess.Read(@"SELECT * FROM SDT_CAmbulante WHERE (Cadastro = @Cadastro) ORDER BY DataCadastro DESC").Rows.Count > 0)
                    _exist = true;

                string _pessoa = string.Format(@"{0}/{1}/{2}/{3}",
                    new mMascaras().Remove(obj.Pessoa.Inscricao),
                    obj.Pessoa.NomeRazao,
                    obj.Pessoa.Telefones,
                    obj.Pessoa.Email);

                string _empresa = string.Empty;
                if (obj.Empresa.Inscricao != null)
                {
                    _empresa = string.Format(@"{0}/{1}/{2}/{3}",
                        new mMascaras().Remove(obj.Empresa.Inscricao),
                        obj.Empresa.NomeRazao,
                        obj.Empresa.Telefones,
                        obj.Empresa.Email);
                }

                /*
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Cadastro: {0}", obj.Cadastro));
                sb.AppendLine(string.Format("Atendimento: {0}", obj.Atendimento));
                sb.AppendLine(string.Format("Pessoa: {0}", _pessoa));
                sb.AppendLine(string.Format("Empresa: {0}", _empresa));
                sb.AppendLine(string.Format("Atividades: {0}", obj.Atividades));
                sb.AppendLine(string.Format("TipoInstalacoes: {0}", obj.TipoInstalacoes));
                sb.AppendLine(string.Format("PeriodoTrabalho: {0}", obj.PeridoTrabalho));
                sb.AppendLine(string.Format("PessoasEnvolvidas: {0}", obj.PessoasEnvolvidas));
                sb.AppendLine(string.Format("Local: {0}", obj.Local));
                sb.AppendLine(string.Format("EntidadeRepresentativa: {0}", obj.EntRepresentativa));
                sb.AppendLine(string.Format("QuerFormalizar: {0}", obj.QuerFormalizar));
                sb.AppendLine(string.Format("DescricaoNegocio: {0}", obj.DescricaoNegocio));
                sb.AppendLine(string.Format("TempoAtividade: {0}", obj.TempoAtividade));
                sb.AppendLine(string.Format("DataCadastro: {0}", obj.DataCadastro.ToShortDateString()));
                sb.AppendLine(string.Format("DataAlteracao: {0}", obj.DataAlteracao.ToShortDateString()));
                sb.AppendLine(string.Format("Situacao: {0}", obj.Situacao));
                sb.AppendLine(string.Format("Justificativa: {0}", obj.Justificativa));
                sb.AppendLine(string.Format("TemCadastro: {0}", obj.TemCadastro));
                sb.AppendLine(string.Format("TemLicenca: {0}", obj.TemLicenca));
                sb.AppendLine(string.Format("DataLicenca: {0}", obj.DataLicenca.ToShortDateString()));
                sb.AppendLine(string.Format("Ativo: {0}", obj.Ativo));
                */
                //System.Windows.MessageBox.Show(sb.ToString());

                dataAccess.ClearParameters();

                if(_exist == false)
                {
                    dataAccess.AddParameters("@Cadastro", obj.Cadastro);
                    dataAccess.AddParameters("@Atendimento", obj.Atendimento);
                }
                
                dataAccess.AddParameters("@Pessoa", _pessoa);
                dataAccess.AddParameters("@Empresa", _empresa);
                dataAccess.AddParameters("@Atividades", obj.Atividades);
                dataAccess.AddParameters("@TipoInstalacoes", obj.TipoInstalacoes);
                dataAccess.AddParameters("@PeriodoTrabalho", obj.PeridoTrabalho);
                dataAccess.AddParameters("@PessoasEnvolvidas", obj.PessoasEnvolvidas);
                dataAccess.AddParameters("@Local", obj.Local);
                dataAccess.AddParameters("@EntidadeRepresentativa", obj.EntRepresentativa);                
                dataAccess.AddParameters("@QuerFormalizar", obj.QuerFormalizar);
                dataAccess.AddParameters("@DescricaoNegocio", obj.DescricaoNegocio);
                dataAccess.AddParameters("@TempoAtividade", obj.TempoAtividade);
                dataAccess.AddParameters("@DataCadastro", obj.DataCadastro.ToShortDateString());
                dataAccess.AddParameters("@DataAlteracao", obj.DataAlteracao.ToShortDateString());
                dataAccess.AddParameters("@Situacao", obj.Situacao);
                dataAccess.AddParameters("@Justificativa", obj.Justificativa);
                dataAccess.AddParameters("@TemCadastro", obj.TemCadastro);
                dataAccess.AddParameters("@TemLicenca", obj.TemLicenca);
                dataAccess.AddParameters("@DataLicenca", obj.DataLicenca.ToShortDateString());
                dataAccess.AddParameters("@Ativo", obj.Ativo);


                if(_exist == true)
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string _novo = @"INSERT INTO SDT_CAmbulante 
([Cadastro], [Atendimento], [Pessoa], [Empresa], [Atividades], [TipoInstalacoes], [PeriodoTrabalho], [PessoasEnvolvidas], [Local], [EntidadeRepresentativa], [QuerFormalizar], [DescricaoNegocio], [TempoAtividade], [DataCadastro], [DataAlteracao], [Situacao], [Justificativa], [TemCadastro], [TemLicenca], [DataLicenca], [Ativo]) 
VALUES 
(@Cadastro, @Atendimento, @Pessoa, @Empresa, @Atividades, @TipoInstalacoes, @PeriodoTrabalho, @PessoasEnvolvidas, @Local, @EntidadeRepresentativa, @QuerFormalizar, @DescricaoNegocio, @TempoAtividade, @DataCadastro, @DataAlteracao, @Situacao, @Justificativa, @TemCadastro, @TemLicenca, @DataLicenca, @Ativo)";

                string _update = @"UPDATE SDT_CAmbulante SET
[Pessoa] = @Pessoa, [Empresa] = @Empresa, [Atividades] = @Atividades, [TipoInstalacoes] = @TipoInstalacoes, [PeriodoTrabalho] = @PeriodoTrabalho, [PessoasEnvolvidas] = @PessoasEnvolvidas, [Local] = @Local, [EntidadeRepresentativa] = @EntidadeRepresentativa, [QuerFormalizar] = @QuerFormalizar, [DescricaoNegocio] = @DescricaoNegocio, [TempoAtividade] = @TempoAtividade, [DataCadastro] = @DataCadastro, [DataAlteracao] = @DataAlteracao, [Situacao] = @Situacao, [Justificativa] = @Justificativa, [TemCadastro] = @TemCadastro, [TemLicenca] = @TemLicenca, [DataLicenca] = @DataLicenca, [Ativo] = @Ativo WHERE (Indice = @Original_Indice)";

                //dataAccess.WriteR(_nsql);

                if (_exist == true)
                {
                    if (System.Windows.MessageBox.Show("Gravar Alterações?", "Sim.Alerta!", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Exclamation) == System.Windows.MessageBoxResult.Yes)
                        return dataAccess.Write(_update);
                    else
                        return false;
                }
                else
                    return dataAccess.Write(_novo);

            }
            catch (Exception ex)
            {                
                //System.Windows.MessageBox.Show(ex.Message);
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
        #endregion

        #region Consultas / Listas
        public ObservableCollection<mAmbulante> Top10CAmbulantes()
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var lista = new ObservableCollection<mAmbulante>();

                dataAccess.ClearParameters();

                string sql = @"SELECT TOP 10 * FROM SDT_CAmbulante WHERE (Ativo = true) ORDER BY DataCadastro DESC";

                //string sql = @"SELECT * FROM SDT_CAmbulante WHERE (DataCadastro BETWEEN @data1 AND @data2) AND (Cadastro LIKE @Cadastro + '%') AND (Pessoa LIKE '%' +  @Pessoa + '%') AND (Empresa LIKE '%' +  @Empresa + '%') AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var ambulante = new mAmbulante();

                    ambulante.Indice = (int)at[0];
                    ambulante.Cadastro = (string)at[1];
                    ambulante.Atendimento = (string)at[2];

                    string[] _pessoa = at[3].ToString().Split('/');
                    ambulante.Pessoa = new mCliente() { Inscricao = _pessoa[0], NomeRazao = _pessoa[1], Telefones = _pessoa[2], Email = _pessoa[3] };

                    string[] _empresa = at[4].ToString().Split('/');
                    ambulante.Empresa = new mCliente() { Inscricao = _empresa[0], NomeRazao = _empresa[1], Telefones = _empresa[2], Email = _empresa[3] };
                    
                    ambulante.Atividades = (string)at[5];
                    ambulante.TipoInstalacoes = (string)at[6];
                    ambulante.PeridoTrabalho = (string)at[7];
                    ambulante.PessoasEnvolvidas = (int)at[8];
                    ambulante.Local = (string)at[9];
                    ambulante.EntRepresentativa = (bool)at[10];
                    ambulante.QuerFormalizar = (bool)at[11];
                    ambulante.DescricaoNegocio = (string)at[12];
                    ambulante.TempoAtividade = (int)at[13];
                    ambulante.DataCadastro = (DateTime)at[14];
                    ambulante.DataAlteracao = (DateTime)at[15];
                    ambulante.Situacao = (int)at[16];
                    ambulante.Justificativa = (string)at[17];
                    ambulante.TemCadastro = (bool)at[18];
                    ambulante.TemLicenca = (bool)at[19];
                    ambulante.DataLicenca = (DateTime)at[20];
                    ambulante.Ativo = (bool)at[21];

                    ambulante.Contador = cont;
                    cont++;
                    lista.Add(ambulante);
                }

                return lista;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAmbulante> LCAmbulantes(List<object> _cmd)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var lista = new ObservableCollection<mAmbulante>();

                dataAccess.ClearParameters();

                //dataAccess.AddParameters("@data1", _cmd[0]);
                //dataAccess.AddParameters("@data2", _cmd[1]);
                dataAccess.AddParameters("@Cadastro", _cmd[0]);
                dataAccess.AddParameters("@Pessoa", _cmd[1]);
                dataAccess.AddParameters("@Empresa", _cmd[1]);
                dataAccess.AddParameters("@Atividades", _cmd[2]);
                dataAccess.AddParameters("@DescricaoNegocio", _cmd[2]);
                dataAccess.AddParameters("@Local", _cmd[3]);
                dataAccess.AddParameters("@Situacao1", _cmd[4]);
                dataAccess.AddParameters("@Situacao2", _cmd[5]);
                dataAccess.AddParameters("@QuerFormalizar", _cmd[6]);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Cadastro: {0}", _cmd[0]));
                sb.AppendLine(string.Format("Pessoa: {0}", _cmd[1]));
                sb.AppendLine(string.Format("Empresa: {0}", _cmd[1]));
                sb.AppendLine(string.Format("Atividades: {0}", _cmd[2]));
                sb.AppendLine(string.Format("DescricaoNegocio: {0}", _cmd[2]));
                sb.AppendLine(string.Format("Local: {0}", _cmd[3]));
                sb.AppendLine(string.Format("Situacao: {0} - {1}", _cmd[4], _cmd[5]));
                //System.Windows.MessageBox.Show(sb.ToString());

                string sql = @"SELECT * FROM SDT_CAmbulante WHERE (Cadastro LIKE '%' + @Cadastro + '%') AND ((Pessoa LIKE '%' +  @Pessoa + '%') OR (Empresa LIKE '%' +  @Empresa + '%')) AND ((Atividades LIKE '%' + @Atividades + '%') OR (DescricaoNegocio LIKE '%' + @DescricaoNegocio + '%')) AND ([Local] LIKE '%' + @Local + '%') AND (Situacao BETWEEN @Situacao1 AND @Situacao2) AND (QuerFormalizar LIKE @QuerFormalizar) AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                //System.Windows.MessageBox.Show(dataAccess.Read(sql).Rows.Count.ToString());

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var ambulante = new mAmbulante();

                    ambulante.Indice = (int)at[0];
                    ambulante.Cadastro = (string)at[1];
                    ambulante.Atendimento = (string)at[2];

                    string[] _pessoa = at[3].ToString().Split('/');
                    ambulante.Pessoa = new mCliente() { Inscricao = _pessoa[0], NomeRazao = _pessoa[1], Telefones = _pessoa[2], Email = _pessoa[3] };

                    string[] _empresa = at[4].ToString().Split('/');
                    ambulante.Empresa = new mCliente() { Inscricao = _empresa[0], NomeRazao = _empresa[1], Telefones = _empresa[2], Email = _empresa[3] };

                    ambulante.Atividades = (string)at[5];
                    ambulante.TipoInstalacoes = (string)at[6];
                    ambulante.PeridoTrabalho = (string)at[7];
                    ambulante.PessoasEnvolvidas = (int)at[8];
                    ambulante.Local = (string)at[9];
                    ambulante.EntRepresentativa = (bool)at[10];
                    ambulante.QuerFormalizar = (bool)at[11];
                    ambulante.DescricaoNegocio = (string)at[12];
                    ambulante.TempoAtividade = (int)at[13];
                    ambulante.DataCadastro = (DateTime)at[14];
                    ambulante.DataAlteracao = (DateTime)at[15];
                    ambulante.Situacao = (int)at[16];
                    ambulante.Justificativa = (string)at[17];
                    ambulante.TemCadastro = (bool)at[18];
                    ambulante.TemLicenca = (bool)at[19];
                    ambulante.DataLicenca = (DateTime)at[20];
                    ambulante.Ativo = (bool)at[21];

                    ambulante.Contador = cont;
                    cont++;
                    lista.Add(ambulante);
                }

                return lista;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAtendimento> AtendimentosComercioAmbulanteNow(List<string> commands)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                var lista = new ObservableCollection<mAtendimento>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data", commands[0]);                
                dataAccess.AddParameters("@tipo", commands[2]);
                dataAccess.AddParameters("@operador", commands[1]);

                var _tipo = dataAccess.Read("SELECT * FROM SDT_Atendimento_Tipos ORDER BY Valor");
                var _origem = dataAccess.Read("SELECT * FROM SDT_Atendimento_Origem ORDER BY Valor");
                var _atendimentosebrae = dataAccess.Read("SELECT * FROM SDT_SAC_Atendimento");

                //System.Windows.MessageBox.Show(commands[2], commands[1]);

                string sql = @"SELECT * FROM SDT_Atendimento WHERE (Data LIKE @data) AND (Tipo LIKE '%' + @tipo + '%') AND (Operador LIKE @operador) AND (Ativo = true) ORDER BY Hora";

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
        #endregion

        #region Consulta / Objeto
        public mAmbulante GetCAmbulante(string _cpf_or_ca)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var _amb = new mAmbulante();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Cadastro", _cpf_or_ca);
                dataAccess.AddParameters("@Pessoa", _cpf_or_ca);

                string sql = @"SELECT * FROM SDT_CAmbulante WHERE ((Cadastro LIKE @Cadastro) OR (Pessoa LIKE '%' +  @Pessoa + '%')) AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                var ambulante = new mAmbulante();

                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {

                    ambulante.Indice = (int)at[0];
                    ambulante.Cadastro = (string)at[1];
                    ambulante.Atendimento = (string)at[2];

                    string[] _pessoa = at[3].ToString().Split('/');
                    ambulante.Pessoa = new mCliente() { Inscricao = _pessoa[0], NomeRazao = _pessoa[1], Telefones = _pessoa[2], Email = _pessoa[3] };

                   string[] _empresa = at[4].ToString().Split('/');
                    ambulante.Empresa = new mCliente() { Inscricao = _empresa[0], NomeRazao = _empresa[1], Telefones = _empresa[2], Email = _empresa[3] };
                    
                    ambulante.Atividades = (string)at[5];
                    ambulante.TipoInstalacoes = (string)at[6];
                    ambulante.PeridoTrabalho = (string)at[7];
                    ambulante.PessoasEnvolvidas = (int)at[8];
                    ambulante.Local = (string)at[9];
                    ambulante.EntRepresentativa = (bool)at[10];
                    ambulante.QuerFormalizar = (bool)at[11];
                    ambulante.DescricaoNegocio = (string)at[12];
                    ambulante.TempoAtividade = (int)at[13];
                    ambulante.DataCadastro = (DateTime)at[14];
                    ambulante.DataAlteracao = (DateTime)at[15];
                    ambulante.Situacao = (int)at[16];
                    ambulante.Justificativa = (string)at[17];
                    ambulante.TemCadastro = (bool)at[18];
                    ambulante.TemLicenca = (bool)at[19];
                    ambulante.DataLicenca = (DateTime)at[20];
                    ambulante.Ativo = (bool)at[21];

                    ambulante.Contador = 1;
                }

                return ambulante;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Reports
        public ObservableCollection<mAmbulante> RCAmbulantes(List<string> _cmd)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var lista = new ObservableCollection<mAmbulante>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Atividades", _cmd[0]);
                dataAccess.AddParameters("@DescricaoNegocio", _cmd[0]);
                dataAccess.AddParameters("@Local", _cmd[1]);
                dataAccess.AddParameters("@Situacao1", _cmd[2]);
                dataAccess.AddParameters("@Situacao2", _cmd[3]);

                string sql = @"SELECT * FROM SDT_CAmbulante WHERE ((Atividades LIKE '%' + @Atividades + '%') OR (DescricaoNegocio LIKE '%' + @DescricaoNegocio + '%')) AND ([Local] LIKE '%' + @Local + '%') AND (Situacao BETWEEN @Situacao1 AND @Situacao2) AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                //System.Windows.MessageBox.Show(dataAccess.Read(sql).Rows.Count.ToString());

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var ambulante = new mAmbulante();

                    ambulante.Indice = (int)at[0];
                    ambulante.Cadastro = (string)at[1];
                    ambulante.Atendimento = (string)at[2];

                    string[] _pessoa = at[3].ToString().Split('/');
                    ambulante.Pessoa = new mCliente() { Inscricao = _pessoa[0], NomeRazao = _pessoa[1], Telefones = _pessoa[2], Email = _pessoa[3] };

                    string[] _empresa = at[4].ToString().Split('/');
                    ambulante.Empresa = new mCliente() { Inscricao = _empresa[0], NomeRazao = _empresa[1], Telefones = _empresa[2], Email = _empresa[3] };

                    ambulante.Atividades = (string)at[5];
                    ambulante.TipoInstalacoes = (string)at[6];
                    ambulante.PeridoTrabalho = (string)at[7];
                    ambulante.PessoasEnvolvidas = (int)at[8];
                    ambulante.Local = (string)at[9];
                    ambulante.EntRepresentativa = (bool)at[10];
                    ambulante.QuerFormalizar = (bool)at[11];
                    ambulante.DescricaoNegocio = (string)at[12];
                    ambulante.TempoAtividade = (int)at[13];
                    ambulante.DataCadastro = (DateTime)at[14];
                    ambulante.DataAlteracao = (DateTime)at[15];
                    ambulante.Situacao = (int)at[16];
                    ambulante.Justificativa = (string)at[17];
                    ambulante.Ativo = (bool)at[18];

                    ambulante.Contador = cont;
                    cont++;
                    lista.Add(ambulante);
                }

                return lista;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
