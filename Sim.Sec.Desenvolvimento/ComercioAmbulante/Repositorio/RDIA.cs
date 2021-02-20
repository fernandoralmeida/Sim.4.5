using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Repositorio
{

    using Mvvm.Observers;
    using Shared.Model;
    using Model;

    public class RDIA : NotifyProperty
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
                dataAccess.AddParameters("@DiaDesde", obj.DiaDesde.ToShortDateString());

                string _novo = @"INSERT INTO SDT_Ambulante_DIA 
([InscricaoMunicipal], [Autorizacao], [Titular], [Auxiliar], [Atividade], [FormaAtuacao], [Veiculo], [Emissao], [Validade], [Processo], [Situacao], [DiaDesde]) 
VALUES 
(@InscricaoMunicipal, @Autorizacao, @Titular, @Auxiliar, @Atividade, @FormaAtuacao, @Veiculo, @Emissao, @Validade, @Processo, @Situacao, @DiaDesde)";


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

                string _update = @"UPDATE SDT_Ambulante_DIA SET
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

        public int Renovar(Model.DIA obj)
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

                string _update = @"UPDATE SDT_Ambulante_DIA SET
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

        public int Baixar(Model.DIA obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Validade", DateTime.Now.ToShortDateString());
                dataAccess.AddParameters("@Situacao", "BAIXADO");
                dataAccess.AddParameters("@Original_Indice", obj.Indice);

                string _update = @"UPDATE SDT_Ambulante_DIA SET [Validade] = @Validade, [Situacao] = @Situacao WHERE (Indice = @Original_Indice)";

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

                    if (dia.Validade > DateTime.Now || dia.Validade == new DateTime(2001, 1, 1))
                        dia.Situacao = (string)at[11];
                    else
                    {
                        dia.Situacao = (string)at[11];
                        if (dia.Situacao != "BAIXADO")
                            dia.Situacao = "VENCIDO";
                    }
                    dia.DiaDesde = (DateTime)at[12];
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
                    dia.DiaDesde = (DateTime)at[12];
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

        public Model.DIA DIA_Existe(string _cpf)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var dia = new Model.DIA();

                dataAccess.AddParameters("@Titular", _cpf);

                string sql = @"SELECT * FROM SDT_Ambulante_DIA WHERE (Titular LIKE '%' + @Titular + '%') AND ((Situacao LIKE 'ATIVO') OR (Situacao LIKE 'VENCIDO')) ";

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

                string sql = @"SELECT TOP 1 Indice, Autorizacao FROM SDT_Ambulante_DIA ORDER BY Autorizacao DESC";

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

        public int GetIndice(string _cpf)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var dia = new Model.DIA();

                dataAccess.AddParameters("@Titular", _cpf);

                string sql = @"SELECT Indice FROM SDT_Ambulante_DIA WHERE (Titular LIKE '%' + @Titular + '%') AND ((Situacao LIKE 'ATIVO') OR (Situacao LIKE 'VENCIDO')) ";

                //string sql = @"SELECT * FROM SDT_CAmbulante WHERE (DataCadastro BETWEEN @data1 AND @data2) AND (Cadastro LIKE @Cadastro + '%') AND (Pessoa LIKE '%' +  @Pessoa + '%') AND (Empresa LIKE '%' +  @Empresa + '%') AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    dia.Indice = (int)at[0];
                    cont++;
                }

                return dia.Indice;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return 0;
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

                    if (dia.Validade > DateTime.Now || dia.Validade == new DateTime(2001, 1, 1))
                        dia.Situacao = (string)at[11];
                    else
                    {
                        dia.Situacao = (string)at[11];
                        if (dia.Situacao != "BAIXADO")
                            dia.Situacao = "VENCIDO";
                    }

                    dia.DiaDesde = (DateTime)at[12];

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
                dataAccess.AddParameters("@Situacao", "BAIXADO");

                string sql = @"SELECT * FROM SDT_Ambulante_DIA WHERE (Emissao BETWEEN @EmissaoI AND @EmissaoF) AND (Situacao LIKE @Situacao) ORDER BY Emissao DESC";

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

        #region Estatisticas
        public ObservableCollection<DIA> EDIAs()
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                var _lista = new ObservableCollection<DIA>();

                string sql = @"SELECT * FROM SDT_Ambulante_DIA ORDER BY Emissao DESC";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var dia = new DIA();
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

                    if (dia.Validade > DateTime.Now || dia.Validade == new DateTime(2001, 1, 1))
                        dia.Situacao = (string)at[11];
                    else
                    {
                        dia.Situacao = (string)at[11];
                        if (dia.Situacao != "BAIXADO")
                            dia.Situacao = "VENCIDO";
                    }

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

        public MReportDIA SetEDIAS(ObservableCollection<DIA> obj)
        {
            try
            {
                MReportDIA _est = new MReportDIA();

                List<string> _dia = new List<string>();
                List<string> _ativos = new List<string>();
                List<string> _ativosnd = new List<string>();
                List<string> _vencidos = new List<string>();
                List<string> _baixados = new List<string>();

                foreach (DIA ab in obj)
                {

                    _dia.Add(ab.Emissao.Year.ToString());


                    if (ab.Emissao < ab.Validade)
                        _ativos.Add(ab.Emissao.Year.ToString());

                    if (ab.Emissao > ab.Validade)
                        _vencidos.Add(ab.Emissao.Year.ToString());

                    if (ab.Validade == new DateTime(2001, 1, 1))
                        _ativosnd.Add(ab.Emissao.Year.ToString());

                    if (ab.Situacao == "BAIXADO")
                        _baixados.Add(ab.Emissao.Year.ToString());                    
                }

                var c_dia = from x in _dia
                            group x by x into g
                            let count = g.Count()
                            orderby count descending
                            select new { Value = g.Key, Count = count };

                foreach (var x in c_dia)
                {
                    _est.DIA.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_ativos = from x in _ativos
                            group x by x into g
                            let count = g.Count()
                            orderby count descending
                            select new { Value = g.Key, Count = count };

                foreach (var x in c_ativos)
                {
                    _est.Ativo.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_ativosnd = from x in _ativosnd
                            group x by x into g
                            let count = g.Count()
                            orderby count descending
                            select new { Value = g.Key, Count = count };

                foreach (var x in c_ativosnd)
                {
                    _est.AtivoND.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_vencidos = from x in _vencidos
                            group x by x into g
                            let count = g.Count()
                            orderby count descending
                            select new { Value = g.Key, Count = count };

                foreach (var x in c_vencidos)
                {
                    _est.Vencido.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_baixado = from x in _baixados
                            group x by x into g
                            let count = g.Count()
                            orderby count descending
                            select new { Value = g.Key, Count = count };

                foreach (var x in c_baixado)
                {
                    _est.Baixado.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                return _est;
            }
            catch(Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        

        #region Functions
        public Model.DIA Exist_DIA(string _cpf_titular)
        {
            var t = Task<Model.DIA>.Factory.StartNew(() => new RDIA().DIA_Existe(_cpf_titular));
            t.Wait();
            return t.Result;
        }
        #endregion
    }
}
