using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

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

                if (dataAccess.Read(@"SELECT * FROM SDT_Ambulante WHERE (Cadastro = @Cadastro) ORDER BY DataCadastro DESC").Rows.Count > 0)
                    _exist = true;

                string _pessoa = string.Format(@"{0};{1};{2};{3}",
                    new mMascaras().Remove(obj.Titular.CPF),
                    obj.Titular.Nome,
                    obj.Titular.RG,
                    obj.Titular.Tel);

                string _empresa = string.Format(@"{0};{1};{2};{3}",
                       new mMascaras().Remove(obj.Auxiliar.CPF),
                       obj.Auxiliar.Nome,
                       obj.Auxiliar.RG,
                       obj.Auxiliar.Tel);
                
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
                    return dataAccess.Write(_update);
                
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

                    ambulante.Atividade = (string)at[4];
                    ambulante.Local = (string)at[5];
                    ambulante.FormaAtuacao = (string)at[6];
                    ambulante.HorarioTrabalho = (string)at[7];
                    ambulante.DataCadastro = (DateTime)at[8];
                    ambulante.UltimaAlteracao = (DateTime)at[9];
                    ambulante.Ativo = (bool)at[10];

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

        public ObservableCollection<Ambulante> Lista_Ambulantes(List<string> _cmd)
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
                dataAccess.AddParameters("@FormaAtuacao", _cmd[5]);

                //string ss = string.Empty;

                //foreach (string s in _cmd)
                    //ss = ss + "\n" + s;

                //System.Windows.MessageBox.Show(ss);

                string sql = @"SELECT * FROM SDT_Ambulante WHERE (DataCadastro BETWEEN @data1 AND @data2) AND (Cadastro LIKE @Cadastro) AND ((Titular LIKE '%' +  @Titular + '%') OR (Auxiliar LIKE '%' +  @Auxiliar + '%')) AND (Atividade LIKE '%' + @Atividade + '%') AND (FormaAtuacao LIKE '%' + @FormaAtuacao + '%') AND (Ativo = true) ORDER BY DataCadastro";

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

                    ambulante.Atividade = (string)at[4];
                    ambulante.Local = (string)at[5];
                    ambulante.FormaAtuacao = (string)at[6];
                    ambulante.HorarioTrabalho = (string)at[7];
                    ambulante.DataCadastro = (DateTime)at[8];
                    ambulante.UltimaAlteracao = (DateTime)at[9];
                    ambulante.Ativo = (bool)at[10];

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

                string sql = @"SELECT * FROM SDT_Ambulante WHERE ((Cadastro LIKE @Cadastro) OR (Titular LIKE '%' +  @Titular + '%')) AND (Ativo = true) ORDER BY Titular, DataCadastro";

                var ambulante = new Ambulante();

                //string ss = string.Empty;

                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    /*
                    for (int i = 0; i < 11; i++)
                        ss = ss + at[i].ToString();*/
                    

                    ambulante.Indice = (int)at[0];
                    ambulante.Cadastro = (string)at[1];

                    string[] _pessoa = at[2].ToString().Split(';');
                    ambulante.Titular = new Autorizados() { CPF = _pessoa[0], Nome = _pessoa[1], RG = _pessoa[2], Tel = _pessoa[3] };

                    string[] _empresa = at[3].ToString().Split(';');
                    ambulante.Auxiliar = new Autorizados() { CPF = _empresa[0], Nome = _empresa[1], RG = _empresa[2], Tel = _empresa[3] };

                    ambulante.Atividade = (string)at[4];
                    ambulante.Local = (string)at[5];
                    ambulante.FormaAtuacao = (string)at[6];
                    ambulante.HorarioTrabalho = (string)at[7];
                    ambulante.DataCadastro = (DateTime)at[8];
                    ambulante.UltimaAlteracao = (DateTime)at[9];
                    ambulante.Ativo = (bool)at[10];

                    ambulante.Contador = 1;
                }

                //System.Windows.MessageBox.Show(ss);
                

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
        /// <summary>
        /// Retorna todos os ambulantes conforme os parametros
        /// </summary>
        /// <param name="_cmd"></param>
        /// <returns></returns>
        public ObservableCollection<Ambulante> RAmbulantes(List<string> _cmd)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var lista = new ObservableCollection<Ambulante>();

                dataAccess.AddParameters("@Atividade", _cmd[0]);
                dataAccess.AddParameters("@Local", _cmd[1]);
                dataAccess.AddParameters("@FormaAtuacao", _cmd[2]);
                dataAccess.AddParameters("@HorarioTrabalho", _cmd[3]);

                string ss = string.Empty;

                foreach (string s in _cmd)
                    ss = ss + "\n" + s;

                System.Windows.MessageBox.Show(ss);

                string sql = @"SELECT * FROM SDT_Ambulante WHERE ([Atividade] LIKE '%' + @Atividade + '%') AND ([Local] LIKE '%' + @Local + '%') AND ([FormaAtuacao] LIKE '%' + @FormaAtuacao + '%') AND ([HorarioTrabalho] LIKE '%' + @HorarioTrabalho + '%') AND (Ativo = true) ORDER BY DataCadastro";

                System.Windows.MessageBox.Show(dataAccess.Read(sql).Rows.Count.ToString());

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

                    ambulante.Atividade = (string)at[4];
                    ambulante.Local = (string)at[5];
                    ambulante.FormaAtuacao = (string)at[6];
                    ambulante.HorarioTrabalho = (string)at[7];
                    ambulante.DataCadastro = (DateTime)at[8];
                    ambulante.UltimaAlteracao = (DateTime)at[9];
                    ambulante.Ativo = (bool)at[10];

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

        /// <summary>
        /// Retorna todos os ambulantes com cadastro ativo
        /// </summary>
        /// <param name="_cmd"></param>
        /// <returns></returns>
        public ObservableCollection<Ambulante> RAAmbulantes(List<string> _cmd)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var lista = new ObservableCollection<Ambulante>();

                string ss = string.Empty;

                foreach (string s in _cmd)
                    ss = ss + "\n" + s;

                System.Windows.MessageBox.Show(ss);

                string sql = @"SELECT * FROM SDT_Ambulante WHERE (Ativo = true) ORDER BY DataCadastro DESC";

                System.Windows.MessageBox.Show(dataAccess.Read(sql).Rows.Count.ToString());

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

                    ambulante.Atividade = (string)at[4];
                    ambulante.Local = (string)at[5];
                    ambulante.FormaAtuacao = (string)at[6];
                    ambulante.HorarioTrabalho = (string)at[7];
                    ambulante.DataCadastro = (DateTime)at[8];
                    ambulante.UltimaAlteracao = (DateTime)at[9];
                    ambulante.Ativo = (bool)at[10];

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

        #region Functions
        public Ambulante Exist_Ambulante_Cadastrado(string _cpf_titular)
        {
            var t = Task<Ambulante>.Factory.StartNew(() => GetAmbulante(_cpf_titular));
            t.Wait();
            return t.Result;
        }
        #endregion
    }
}
