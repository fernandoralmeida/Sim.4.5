﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Repositorio
{

    using Mvvm.Observers;
    using Shared.Model;

    public class DIA: NotifyProperty
    {
        public int Gravar(Model.DIA obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();

                string _titular = string.Format(@"{0};{1};",
                    obj.Titular.Nome,
                    obj.Titular.RG);

                string _auxiliar = string.Format(@"{0};{1};",
                    obj.Auxiliar.Nome,
                    obj.Auxiliar.RG);

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

                string _novo = @"INSERT INTO SDT_CAmbulante_DIA 
([InscricaoMunicipal], [Autorizacao], [Titular], [Auxiliar], [Atividade], [FormaAtuacao], [Veiculo], [Emissao], [Validade], [Processo], [Situacao]) 
VALUES 
(@InscricaoMunicipal, @Autorizacao, @Titular, @Auxiliar, @Atividade, @FormaAtuacao, @Veiculo, @Emissao, @Validade, @Processo, @Situacao)";


                if (dataAccess.Write(_novo))
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return 0;
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

                string sql = @"SELECT TOP 10 * FROM SDT_CAmbulante_DIA";

                //string sql = @"SELECT * FROM SDT_CAmbulante WHERE (DataCadastro BETWEEN @data1 AND @data2) AND (Cadastro LIKE @Cadastro + '%') AND (Pessoa LIKE '%' +  @Pessoa + '%') AND (Empresa LIKE '%' +  @Empresa + '%') AND (Ativo = true) ORDER BY Pessoa, DataCadastro";

                int cont = 1;
                foreach (DataRow at in dataAccess.Read(sql).Rows)
                {
                    var dia = new Model.DIA();

                    dia.Indice = (int)at[0];
                    dia.InscricaoMunicipal = (int)at[1];
                    dia.Autorizacao = (string)at[2];

                    string[] _titular = at[3].ToString().Split(';');
                    dia.Titular = new Model.Autorizados() { Nome = _titular[0], RG = _titular[1] };

                    string[] _auxiliar = at[4].ToString().Split(';');
                    dia.Auxiliar = new Model.Autorizados() { Nome = _auxiliar[0], RG = _auxiliar[1] };

                    dia.Atividade = (string)at[5];
                    dia.FormaAtuacao = (string)at[6];

                    string[] _veiculo = at[7].ToString().Split(';');
                    dia.Veiculo = new Model.Veiculo() { Modelo  = _veiculo[0], Placa = _veiculo[1], Cor = _veiculo[2] };


                    dia.Emissao = (DateTime)at[8];
                    dia.Validade = (string)at[9];
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

        public Model.DIA Consulta(Model.DIA obj)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {


                return null;
            }
            catch
            {
                return null;
            }
               
        }

        public string UltimaAutorizacao()
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);
            try
            {
                dataAccess.ClearParameters();

                string _lastrows = string.Empty;

                string sql = @"SELECT TOP 1 Indice, Autorizacao FROM SDT_CAmbulante_DIA ORDER BY Indice DESC";

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

    }
}
