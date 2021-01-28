using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Repositorio
{

    using Mvvm.Observers;
    using Shared.Model;
    using Model;

    public class RAmbulante : NotifyProperty
    {
        #region Inserção / Edição / Remoção
        public bool GravarAmbulante(Ambulante obj)
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
                    new mMascaras().Remove(obj.Titular.CPF),
                    obj.Titular.Nome,
                    obj.Titular.RG,
                    obj.Titular.Tel);

                string _empresa = string.Empty;
                if (obj.Auxiliar.CPF != null)
                {
                    _empresa = string.Format(@"{0}/{1}/{2}/{3}",
                        new mMascaras().Remove(obj.Auxiliar.CPF),
                        obj.Auxiliar.Nome,
                        obj.Auxiliar.RG,
                        obj.Auxiliar.Tel);
                }

                
                dataAccess.ClearParameters();

                if (_exist == false)
                {
                    dataAccess.AddParameters("@Cadastro", obj.Cadastro);
                }

                dataAccess.AddParameters("@Titular", _pessoa);
                dataAccess.AddParameters("@Auxiliar", _empresa);
                dataAccess.AddParameters("@Atividade", obj.Atividade);
                dataAccess.AddParameters("@Local", obj.Local);
                dataAccess.AddParameters("@FormaAtuacao", obj.FormaAtuacao);
                dataAccess.AddParameters("@HorarioTrabalho", obj.HorarioTrabalho);
                dataAccess.AddParameters("@DataCadastro", obj.DataCadastro.ToShortDateString());
                dataAccess.AddParameters("@UltimaAlteracao", obj.UltimaAlteracao.ToShortDateString());
                dataAccess.AddParameters("@Ativo", obj.Ativo);


                if (_exist == true)
                    dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string _novo = @"INSERT INTO SDT_Ambulante 
([Cadastro], [Titular], [Auxiliar], [Atividade], [Local], [FormaAtuacao], [HorarioTrabalho], [DataCadastro], [UltimaAlteracao], [Ativo]) 
VALUES 
(@Cadastro, @Titular, @Auxiliar, @Atividade, @Local, @FormaAtuacao, @HorarioTrabalho, @DataCadastro, @DataAlteracao, @Ativo)";

                string _update = @"UPDATE SDT_Ambulante SET
[Titular] = @Titular, [Auxiliar] = @Auxiliar, [Atividade] = @Atividade, [Local] = @Local, [FormaAtuacao] = @FormaAtuacao, [HorarioTrabalho] = @HorarioTrabalho, [DataCadastro] = @DataCadastro, [UltimaAlteracao] = @UltimaAlteracao, [Ativo] = @Ativo WHERE (Indice = @Original_Indice)";

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

        #endregion

        #region Consultas / Listas
        public ObservableCollection<Ambulante> Top10Ambulantes()
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var lista = new ObservableCollection<Ambulante>();

                dataAccess.ClearParameters();

                string sql = @"SELECT TOP 10 * FROM SDT_Ambulante WHERE (Ativo = true) ORDER BY DataCadastro DESC";

                //string sql = @"SELECT * FROM SDT_CAmbulante WHERE (DataCadastro BETWEEN @data1 AND @data2) AND (Cadastro LIKE @Cadastro + '%') AND (Pessoa LIKE '%' +  @Pessoa + '%') AND (Empresa LIKE '%' +  @Empresa + '%') AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var ambulante = new Ambulante();

                    ambulante.Indice = (int)at[0];
                    ambulante.Cadastro = (string)at[1];

                    string[] _pessoa = at[2].ToString().Split(';');
                    ambulante.Titular = new Autorizados() { CPF = _pessoa[0], Nome = _pessoa[1], RG = _pessoa[2], Tel = _pessoa[3] };

                    string[] _empresa = at[3].ToString().Split(';');
                    ambulante.Auxiliar = new Autorizados() { CPF = _empresa[0], Nome = _empresa[1], RG = _empresa[2], Tel = _empresa[3] };

                    ambulante.Atividade = (string)at[5];
                    ambulante.Local = (string)at[6];
                    ambulante.FormaAtuacao = (string)at[7];
                    ambulante.HorarioTrabalho = (string)at[8];
                    ambulante.Local = (string)at[9];
                    ambulante.DataCadastro = (DateTime)at[14];
                    ambulante.UltimaAlteracao = (DateTime)at[15];
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

        public ObservableCollection<Ambulante> Lista_Ambulantes(List<object> _cmd)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var lista = new ObservableCollection<Ambulante>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@data1", _cmd[0]);
                dataAccess.AddParameters("@data2", _cmd[1]);
                dataAccess.AddParameters("@Cadastro", _cmd[2]);
                dataAccess.AddParameters("@Titular", _cmd[3]);
                dataAccess.AddParameters("@Auxiliar", _cmd[3]);
                dataAccess.AddParameters("@Atividade", _cmd[4]);
                dataAccess.AddParameters("@Local", _cmd[5]);
                dataAccess.AddParameters("@FormaAtuacao", _cmd[6]);

                //System.Windows.MessageBox.Show(sb.ToString());

                string sql = @"SELECT * FROM SDT_CAmbulante WHERE (DataCadastro BETWEEN @data1 AND @data2) AND ((Titular LIKE '%' +  @Titular + '%') OR (Auxiliar LIKE '%' +  @Auxiliar + '%')) AND ((Atividade LIKE '%' + @Atividade + '%') OR (Local LIKE '%' + @Local + '%')) AND (FormaAtuacao LIKE '%' + @FormaAtuacao + '%') AND (Ativo = true) ORDER BY Titular, DataCadastro";

                //System.Windows.MessageBox.Show(dataAccess.Read(sql).Rows.Count.ToString());

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var ambulante = new Ambulante();

                    ambulante.Indice = (int)at[0];
                    ambulante.Cadastro = (string)at[1];

                    string[] _pessoa = at[2].ToString().Split(';');
                    ambulante.Titular = new Autorizados() { CPF = _pessoa[0], Nome = _pessoa[1], RG = _pessoa[2], Tel = _pessoa[3] };

                    string[] _empresa = at[3].ToString().Split(';');
                    ambulante.Auxiliar = new Autorizados() { CPF = _empresa[0], Nome = _empresa[1], RG = _empresa[2], Tel = _empresa[3] };

                    ambulante.Atividade = (string)at[5];
                    ambulante.Local = (string)at[6];
                    ambulante.FormaAtuacao = (string)at[7];
                    ambulante.HorarioTrabalho = (string)at[8];
                    ambulante.Local = (string)at[9];
                    ambulante.DataCadastro = (DateTime)at[14];
                    ambulante.UltimaAlteracao = (DateTime)at[15];
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

        #endregion

        #region Consulta / Objeto
        public Ambulante GetAmbulante(string _cpf_or_ca)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var _amb = new Ambulante();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Cadastro", _cpf_or_ca);
                dataAccess.AddParameters("@Titular", _cpf_or_ca);

                string sql = @"SELECT * FROM SDT_CAmbulante WHERE ((Cadastro LIKE @Cadastro) OR (Titular LIKE '%' +  @Titular + '%')) AND (Ativo = true) ORDER BY Titular, DataCadastro";

                var ambulante = new Ambulante();

                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {

                    ambulante.Indice = (int)at[0];
                    ambulante.Cadastro = (string)at[1];

                    string[] _pessoa = at[2].ToString().Split(';');
                    ambulante.Titular = new Autorizados() { CPF = _pessoa[0], Nome = _pessoa[1], RG = _pessoa[2], Tel = _pessoa[3] };

                    string[] _empresa = at[3].ToString().Split(';');
                    ambulante.Auxiliar = new Autorizados() { CPF = _empresa[0], Nome = _empresa[1], RG = _empresa[2], Tel = _empresa[3] };

                    ambulante.Atividade = (string)at[5];
                    ambulante.Local = (string)at[6];
                    ambulante.FormaAtuacao = (string)at[7];
                    ambulante.HorarioTrabalho = (string)at[8];
                    ambulante.Local = (string)at[9];
                    ambulante.DataCadastro = (DateTime)at[14];
                    ambulante.UltimaAlteracao = (DateTime)at[15];
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
        public ObservableCollection<Ambulante> RAmbulantes(List<string> _cmd)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var lista = new ObservableCollection<Ambulante>();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Atividade", _cmd[0]);
                dataAccess.AddParameters("@Local", _cmd[1]);
                dataAccess.AddParameters("@FormaAtuacao", _cmd[2]);
                dataAccess.AddParameters("@HorarioTrabalho", _cmd[3]);

                string sql = @"SELECT * FROM SDT_CAmbulante WHERE (Atividade LIKE '%' + @Atividade + '%') AND (Local LIKE '%' + @Local + '%') AND (FormaAtuacao LIKE '%' + @FormaAtuacao + '%') AND (HorarioTrabalho LIKE '%' + @HorarioTrabalho + '%') AND (Ativo = true) ORDER BY Titular, DataCadastro";

                //System.Windows.MessageBox.Show(dataAccess.Read(sql).Rows.Count.ToString());

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var ambulante = new Ambulante();

                    ambulante.Indice = (int)at[0];
                    ambulante.Cadastro = (string)at[1];

                    string[] _pessoa = at[2].ToString().Split(';');
                    ambulante.Titular = new Autorizados() { CPF = _pessoa[0], Nome = _pessoa[1], RG = _pessoa[2], Tel = _pessoa[3] };

                    string[] _empresa = at[3].ToString().Split(';');
                    ambulante.Auxiliar = new Autorizados() { CPF = _empresa[0], Nome = _empresa[1], RG = _empresa[2], Tel = _empresa[3] };

                    ambulante.Atividade = (string)at[5];
                    ambulante.Local = (string)at[6];
                    ambulante.FormaAtuacao = (string)at[7];
                    ambulante.HorarioTrabalho = (string)at[8];
                    ambulante.Local = (string)at[9];
                    ambulante.DataCadastro = (DateTime)at[14];
                    ambulante.UltimaAlteracao = (DateTime)at[15];
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
        #endregion
    }
}
