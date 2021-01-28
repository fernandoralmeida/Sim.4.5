using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Repositorio
{

    using Mvvm.Observers;
    using Shared.Model;

    public class DIA : NotifyProperty
    {
        public int Gravar(Model.DIA obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();

                string _titular = string.Format(@"{0};{1};{2};{3}",
                    obj.Titular.Nome,
                    obj.Titular.CPF,
                    obj.Titular.RG,
                    obj.Titular.Tel);

                string _auxiliar = string.Format(@"{0};{1};{2};{3}",
                    obj.Auxiliar.Nome,
                    obj.Auxiliar.CPF,
                    obj.Auxiliar.RG,
                    obj.Auxiliar.Tel);

                string _veiculo = string.Format(@"{0};{1};{2};",
                    obj.Veiculo.Modelo,
                    obj.Veiculo.Placa,
                    obj.Veiculo.Cor);

                dataAccess.AddParameters("@InscricaoMunicipal", obj.InscricaoMunicipal);
                dataAccess.AddParameters("@Autorizacao", obj.Autorizacao);
                dataAccess.AddParameters("@Titular", _titular);
                dataAccess.AddParameters("@Auxiliar", _auxiliar);
                dataAccess.AddParameters("@Atividade", obj.Atividade);
                dataAccess.AddParameters("@FormaAtuacao", obj.FormaAtuacao);
                dataAccess.AddParameters("@Veiculo", _veiculo);
                dataAccess.AddParameters("@Emissao", obj.Emissao.ToShortDateString());
                dataAccess.AddParameters("@Validade", obj.Validade);
                dataAccess.AddParameters("@Processo", obj.Processo);
                dataAccess.AddParameters("@Situacao", obj.Situacao);

                string _novo = @"INSERT INTO SDT_Ambulante_DIA 
([InscricaoMunicipal], [Autorizacao], [Titular], [Auxiliar], [Atividade], [FormaAtuacao], [Veiculo], [Emissao], [Validade], [Processo], [Situacao]) 
VALUES 
(@InscricaoMunicipal, @Autorizacao, @Titular, @Auxiliar, @Atividade, @FormaAtuacao, @Veiculo, @Emissao, @Validade, @Processo, @Situacao)";


                if (dataAccess.Write(_novo))
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex )
            {               
                throw new Exception(ex.Message);
            }
        }

        public int Alterar(Model.DIA obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();

                string _titular = string.Format(@"{0};{1};{2};{3}",
                    obj.Titular.Nome,
                    obj.Titular.CPF,
                    obj.Titular.RG,
                    obj.Titular.Tel);

                string _auxiliar = string.Format(@"{0};{1};{2};{3}",
                    obj.Auxiliar.Nome,
                    obj.Auxiliar.CPF,
                    obj.Auxiliar.RG,
                    obj.Auxiliar.Tel);

                string _veiculo = string.Format(@"{0};{1};{2};",
                    obj.Veiculo.Modelo,
                    obj.Veiculo.Placa,
                    obj.Veiculo.Cor);

                dataAccess.AddParameters("@InscricaoMunicipal", obj.InscricaoMunicipal);
                dataAccess.AddParameters("@Autorizacao", obj.Autorizacao);
                dataAccess.AddParameters("@Titular", _titular);
                dataAccess.AddParameters("@Auxiliar", _auxiliar);
                dataAccess.AddParameters("@Atividade", obj.Atividade);
                dataAccess.AddParameters("@FormaAtuacao", obj.FormaAtuacao);
                dataAccess.AddParameters("@Veiculo", _veiculo);
                dataAccess.AddParameters("@Emissao", obj.Emissao.ToShortDateString());
                dataAccess.AddParameters("@Validade", obj.Validade);
                dataAccess.AddParameters("@Processo", obj.Processo);
                dataAccess.AddParameters("@Situacao", obj.Situacao);
                dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string _novo = @"INSERT INTO SDT_Ambulante_DIA 
([InscricaoMunicipal], [Autorizacao], [Titular], [Auxiliar], [Atividade], [FormaAtuacao], [Veiculo], [Emissao], [Validade], [Processo], [Situacao]) 
VALUES 
(@InscricaoMunicipal, @Autorizacao, @Titular, @Auxiliar, @Atividade, @FormaAtuacao, @Veiculo, @Emissao, @Validade, @Processo, @Situacao)";

                string _update = @"UPDATE SDT_CAmbulante_DIA SET
[InscricaoMunicipal] = @PesInscricaoMunicipalsoa, [Autorizacao] = @Autorizacao, [Titular] = @Titular, [Auxiliar] = @Auxiliar, [Atividade] = @Atividade, [FormaAtuacao] = @FormaAtuacao, [Veiculo] = @Veiculo, [Emissao] = @Emissao, [Validade] = @Validade, [Processo] = @Processo, [Situacao] = @Situacao WHERE (Indice = @Original_Indice)";


                if (dataAccess.Write(_update))
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<Model.DIA> Last_DIAs()
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var lista = new ObservableCollection<Model.DIA>();

                dataAccess.ClearParameters();

                string sql = @"SELECT TOP 10 * FROM SDT_Ambulante_DIA";

                //string sql = @"SELECT * FROM SDT_CAmbulante WHERE (DataCadastro BETWEEN @data1 AND @data2) AND (Cadastro LIKE @Cadastro + '%') AND (Pessoa LIKE '%' +  @Pessoa + '%') AND (Empresa LIKE '%' +  @Empresa + '%') AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var dia = new Model.DIA();

                    dia.Indice = (int)at[0];
                    dia.InscricaoMunicipal = (int)at[1];
                    dia.Autorizacao = (string)at[2];

                    string[] _titular = at[3].ToString().Split(';');
                    dia.Titular = new Model.Autorizados() { Nome = _titular[0], CPF = _titular[1] , RG = _titular[2], Tel = _titular[3] };

                    string[] _auxiliar = at[4].ToString().Split(';');
                    dia.Auxiliar = new Model.Autorizados() { Nome = _auxiliar[0], CPF = _auxiliar[1], RG = _auxiliar[2], Tel = _auxiliar[3] };

                    dia.Atividade = (string)at[5];
                    dia.FormaAtuacao = (string)at[6];

                    string[] _veiculo = at[7].ToString().Split(';');
                    dia.Veiculo = new Model.Veiculo() { Modelo = _veiculo[0], Placa = _veiculo[1], Cor = _veiculo[2] };


                    dia.Emissao = (DateTime)at[8];
                    dia.Validade = (DateTime?)at[9];
                    dia.Processo = (string)at[10];
                    dia.Situacao = (string)at[11];
                    dia.Contador = cont;
                    cont++;
                    lista.Add(dia);
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

        public Model.DIA GetDIA(int _indice)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var dia = new Model.DIA();

                dataAccess.AddParameters("@Indice", _indice);

                string sql = @"SELECT * FROM SDT_Ambulante_DIA WHERE (Indice = @Indice)";

                //string sql = @"SELECT * FROM SDT_CAmbulante WHERE (DataCadastro BETWEEN @data1 AND @data2) AND (Cadastro LIKE @Cadastro + '%') AND (Pessoa LIKE '%' +  @Pessoa + '%') AND (Empresa LIKE '%' +  @Empresa + '%') AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    dia.Indice = (int)at[0];
                    dia.InscricaoMunicipal = (int)at[1];
                    dia.Autorizacao = (string)at[2];

                    string[] _titular = at[3].ToString().Split(';');
                    dia.Titular = new Model.Autorizados() { Nome = _titular[0], CPF = _titular[1], RG = _titular[2], Tel = _titular[3] };

                    string[] _auxiliar = at[4].ToString().Split(';');
                    dia.Auxiliar = new Model.Autorizados() { Nome = _auxiliar[0], CPF = _auxiliar[1], RG = _auxiliar[2], Tel = _auxiliar[3] };


                    dia.Atividade = (string)at[5];
                    dia.FormaAtuacao = (string)at[6];

                    string[] _veiculo = at[7].ToString().Split(';');
                    dia.Veiculo = new Model.Veiculo() { Modelo = _veiculo[0], Placa = _veiculo[1], Cor = _veiculo[2] };

                    dia.Emissao = (DateTime)at[8];
                    dia.Validade = (DateTime?)at[9];
                    dia.Processo = (string)at[10];
                    dia.Situacao = (string)at[11];
                    dia.Contador = cont;
                    cont++;
                }

                return dia;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }

        }

        public string UltimaAutorizacao()
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                string _lastrows = string.Empty;

                string sql = @"SELECT TOP 1 Indice, Autorizacao FROM SDT_Ambulante_DIA ORDER BY Indice DESC";

                //string sql = @"SELECT * FROM SDT_CAmbulante WHERE (DataCadastro BETWEEN @data1 AND @data2) AND (Cadastro LIKE @Cadastro + '%') AND (Pessoa LIKE '%' +  @Pessoa + '%') AND (Empresa LIKE '%' +  @Empresa + '%') AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    _lastrows = (string)at[1];
                }

                return _lastrows;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<Model.DIA> Consultar(List<string> _command)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var _lista = new ObservableCollection<Model.DIA>();

                dataAccess.AddParameters("@EmissaoI", _command[0]);
                dataAccess.AddParameters("@EmissaoF", _command[1]);
                dataAccess.AddParameters("@Autorizacao", _command[2]);
                dataAccess.AddParameters("@Titular", _command[3]);
                dataAccess.AddParameters("@Auxiliar", _command[3]);
                dataAccess.AddParameters("@Atividade", _command[4]);
                dataAccess.AddParameters("@FormaAtuacao", _command[5]);
                dataAccess.AddParameters("@Situacao", _command[6]);

                string sql = @"SELECT * FROM SDT_Ambulante_DIA WHERE (Emissao BETWEEN @EmissaoI AND @EmissaoF) AND (Autorizacao LIKE @Autorizacao) AND ((Titular LIKE '%' + @Titular + '%') OR (Auxiliar LIKE '%' + @Auxiliar + '%')) AND (Atividade LIKE '%' + @Atividade + '%') AND (FormaAtuacao LIKE '%' + @FormaAtuacao + '%') AND (Situacao LIKE @Situacao) ORDER BY Emissao DESC";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var dia = new Model.DIA();
                    dia.Indice = (int)at[0];
                    dia.InscricaoMunicipal = (int)at[1];
                    dia.Autorizacao = (string)at[2];

                    string[] _titular = at[3].ToString().Split(';');
                    dia.Titular = new Model.Autorizados() { Nome = _titular[0], CPF = _titular[1], RG = _titular[2], Tel = _titular[3] };

                    string[] _auxiliar = at[4].ToString().Split(';');
                    dia.Auxiliar = new Model.Autorizados() { Nome = _auxiliar[0], CPF = _auxiliar[1], RG = _auxiliar[2], Tel = _auxiliar[3] };


                    dia.Atividade = (string)at[5];
                    dia.FormaAtuacao = (string)at[6];

                    string[] _veiculo = at[7].ToString().Split(';');
                    dia.Veiculo = new Model.Veiculo() { Modelo = _veiculo[0], Placa = _veiculo[1], Cor = _veiculo[2] };

                    dia.Emissao = (DateTime)at[8];
                    dia.Validade = (DateTime?)at[9];
                    dia.Processo = (string)at[10];
                    dia.Situacao = (string)at[11];
                    dia.Contador = cont;
                    _lista.Add(dia);
                    cont++;
                }

                return _lista;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }
        }

        #region Reports
        public ObservableCollection<Model.DIA> DIA_Ativos(List<string> _command)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var _lista = new ObservableCollection<Model.DIA>();

                dataAccess.AddParameters("@EmissaoI", _command[0]);
                dataAccess.AddParameters("@EmissaoF", _command[1]);
                dataAccess.AddParameters("@DateNow", DateTime.Now.ToShortDateString());
                dataAccess.AddParameters("@DateNow2", new DateTime(2001,1,1));

                string sql = @"SELECT * FROM SDT_Ambulante_DIA WHERE (Emissao BETWEEN @EmissaoI AND @EmissaoF) AND (Situacao LIKE 'ATIVO') AND ((Validade > @DateNow) OR (Validade = @DateNow2)) ORDER BY Emissao, Validade DESC";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var dia = new Model.DIA();
                    dia.Indice = (int)at[0];
                    dia.InscricaoMunicipal = (int)at[1];
                    dia.Autorizacao = (string)at[2];

                    string[] _titular = at[3].ToString().Split(';');
                    dia.Titular = new Model.Autorizados() { Nome = _titular[0], CPF = _titular[1], RG = _titular[2], Tel = _titular[3] };

                    string[] _auxiliar = at[4].ToString().Split(';');
                    dia.Auxiliar = new Model.Autorizados() { Nome = _auxiliar[0], CPF = _auxiliar[1], RG = _auxiliar[2], Tel = _auxiliar[3] };

                    dia.Atividade = (string)at[5];
                    dia.FormaAtuacao = (string)at[6];

                    string[] _veiculo = at[7].ToString().Split(';');
                    dia.Veiculo = new Model.Veiculo() { Modelo = _veiculo[0], Placa = _veiculo[1], Cor = _veiculo[2] };

                    dia.Emissao = (DateTime)at[8];
                    dia.Validade = (DateTime?)at[9];
                    dia.Processo = (string)at[10];
                    dia.Situacao = (string)at[11];
                    dia.Contador = cont;
                    _lista.Add(dia);
                    cont++;
                }

                return _lista;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<Model.DIA> DIA_Baixados(List<string> _command)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var _lista = new ObservableCollection<Model.DIA>();

                dataAccess.AddParameters("@EmissaoI", _command[0]);
                dataAccess.AddParameters("@EmissaoF", _command[1]);

                string sql = @"SELECT * FROM SDT_Ambulante_DIA WHERE (Emissao BETWEEN @EmissaoI AND @EmissaoF) AND (Situacao LIKE 'BAIXADO') ORDER BY Emissao DESC";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var dia = new Model.DIA();
                    dia.Indice = (int)at[0];
                    dia.InscricaoMunicipal = (int)at[1];
                    dia.Autorizacao = (string)at[2];

                    string[] _titular = at[3].ToString().Split(';');
                    dia.Titular = new Model.Autorizados() { Nome = _titular[0], CPF = _titular[1], RG = _titular[2], Tel = _titular[3] };

                    string[] _auxiliar = at[4].ToString().Split(';');
                    dia.Auxiliar = new Model.Autorizados() { Nome = _auxiliar[0], CPF = _auxiliar[1], RG = _auxiliar[2], Tel = _auxiliar[3] };

                    dia.Atividade = (string)at[5];
                    dia.FormaAtuacao = (string)at[6];

                    string[] _veiculo = at[7].ToString().Split(';');
                    dia.Veiculo = new Model.Veiculo() { Modelo = _veiculo[0], Placa = _veiculo[1], Cor = _veiculo[2] };

                    dia.Emissao = (DateTime)at[8];
                    dia.Validade = (DateTime?)at[9];
                    dia.Processo = (string)at[10];
                    dia.Situacao = (string)at[11];
                    dia.Contador = cont;
                    _lista.Add(dia);
                    cont++;
                }

                return _lista;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<Model.DIA> DIA_Vencidos(List<string> _command)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var _lista = new ObservableCollection<Model.DIA>();

                dataAccess.AddParameters("@EmissaoI", _command[0]);
                dataAccess.AddParameters("@EmissaoF", _command[1]);
                dataAccess.AddParameters("@DateNow1", new DateTime(2001, 1, 1));
                dataAccess.AddParameters("@DateNow2", DateTime.Now.ToShortDateString());
                

                string sql = @"SELECT * FROM SDT_Ambulante_DIA WHERE (Emissao BETWEEN @EmissaoI AND @EmissaoF) AND (Situacao LIKE 'ATIVO') AND ((Validade > @DateNow1) AND (Validade < @DateNow2)) ORDER BY Emissao, Validade DESC";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var dia = new Model.DIA();
                    dia.Indice = (int)at[0];
                    dia.InscricaoMunicipal = (int)at[1];
                    dia.Autorizacao = (string)at[2];

                    string[] _titular = at[3].ToString().Split(';');
                    dia.Titular = new Model.Autorizados() { Nome = _titular[0], CPF = _titular[1], RG = _titular[2], Tel = _titular[3] };

                    string[] _auxiliar = at[4].ToString().Split(';');
                    dia.Auxiliar = new Model.Autorizados() { Nome = _auxiliar[0], CPF = _auxiliar[1], RG = _auxiliar[2], Tel = _auxiliar[3] };


                    dia.Atividade = (string)at[5];
                    dia.FormaAtuacao = (string)at[6];

                    string[] _veiculo = at[7].ToString().Split(';');
                    dia.Veiculo = new Model.Veiculo() { Modelo = _veiculo[0], Placa = _veiculo[1], Cor = _veiculo[2] };

                    dia.Emissao = (DateTime)at[8];
                    dia.Validade = (DateTime?)at[9];
                    dia.Processo = (string)at[10];
                    dia.Situacao = "VENCIDO";
                    dia.Contador = cont;
                    _lista.Add(dia);
                    cont++;
                }

                return _lista;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<Model.DIA> DIA_Sem_Data_Vencimento(List<string> _command)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var _lista = new ObservableCollection<Model.DIA>();

                dataAccess.AddParameters("@EmissaoI", _command[0]);
                dataAccess.AddParameters("@EmissaoF", _command[1]);
                dataAccess.AddParameters("@DateNow", new DateTime(2001,1,1).ToShortDateString());

                string sql = @"SELECT * FROM SDT_Ambulante_DIA WHERE (Emissao BETWEEN @EmissaoI AND @EmissaoF) AND (Situacao LIKE 'ATIVO') AND (Validade = @DateNow) ORDER BY Emissao, Validade DESC";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var dia = new Model.DIA();
                    dia.Indice = (int)at[0];
                    dia.InscricaoMunicipal = (int)at[1];
                    dia.Autorizacao = (string)at[2];

                    string[] _titular = at[3].ToString().Split(';');
                    dia.Titular = new Model.Autorizados() { Nome = _titular[0], CPF = _titular[1], RG = _titular[2], Tel = _titular[3] };

                    string[] _auxiliar = at[4].ToString().Split(';');
                    dia.Auxiliar = new Model.Autorizados() { Nome = _auxiliar[0], CPF = _auxiliar[1], RG = _auxiliar[2], Tel = _auxiliar[3] };


                    dia.Atividade = (string)at[5];
                    dia.FormaAtuacao = (string)at[6];

                    string[] _veiculo = at[7].ToString().Split(';');
                    dia.Veiculo = new Model.Veiculo() { Modelo = _veiculo[0], Placa = _veiculo[1], Cor = _veiculo[2] };

                    dia.Emissao = (DateTime)at[8];
                    dia.Validade = (DateTime?)at[9];
                    dia.Processo = (string)at[10];
                    dia.Situacao = (string)at[11];
                    dia.Contador = cont;
                    _lista.Add(dia);
                    cont++;
                }

                return _lista;
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
