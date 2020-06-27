using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.NomesPublicos.Model
{
    using Sim.Data;
    using Model;

    class mData
    {
        #region Includes
        public bool AddDenominacao(mObjeto obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.NomePublicos);
            try
            {
                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Tipo", obj.Tipo);
                dataAccess.AddParameters("@Nome", obj.Nome);
                dataAccess.AddParameters("@Bairro", obj.Bairro);
                dataAccess.AddParameters("@Cep", obj.CEP);
                dataAccess.AddParameters("@Inicio", obj.PontoInicial);
                dataAccess.AddParameters("@Nome_Anterior", obj.NomeAnterior);
                dataAccess.AddParameters("@Origem", obj.Origem);
                dataAccess.AddParameters("@Numero_Origem", obj.NumeroOrigem);
                dataAccess.AddParameters("@Ano_Origem", obj.AnoOrigem);
                dataAccess.AddParameters("@Especie", obj.Especie);
                dataAccess.AddParameters("@Observacoes", obj.Observacoes);
                dataAccess.AddParameters("@Cadastro", obj.Cadastro.ToShortDateString());
                dataAccess.AddParameters("@Alteracao", obj.Cadastro.ToShortDateString());
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                string Novo = @"INSERT INTO Denominacoes (Tipo, Nome, Bairro, Cep, Inicio, Nome_Anterior, Origem, Numero_Origem, Ano_Origem, Especie, Observacoes, Cadastro, Alteracao, Ativo)
                              VALUES
                             (@Tipo, @Nome, @Bairro, @Cep, @Inicio, @Nome_Anterior, @Origem, @Numero_Origem, @Ano_Origem, @Especie, @Observacoes, @Cadastro, @Alteracao, @Ativo)";

                return dataAccess.Write(Novo);
            }
            catch
            {
                return false;
            }
        }

        public bool AddTipo(mTipos obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.NomePublicos);

            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@tipo", obj.Tipo);
                dataAccess.AddParameters("@ativo", true);

                string Novo = @"INSERT INTO Tipos (Tipo, Ativo) VALUES (@tipo, @ativo)";

                return dataAccess.Write(Novo);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool AddEspecie(mEspecie obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.NomePublicos);

            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@codigo", obj.Codigo);
                dataAccess.AddParameters("@especie", obj.Especie);
                dataAccess.AddParameters("@ativo", true);

                string Novo = @"INSERT INTO Especies (Codigo, Especie, Ativo) VALUES (@codigo, @especie, @ativo)";

                return dataAccess.Write(Novo);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion

        #region Listas
        public ObservableCollection<mEspecie> Especies()
        {
            try
            {
                ObservableCollection<mEspecie> _list = new ObservableCollection<mEspecie>();
                var mdata = Data.Factory.Connecting(DataBase.Base.NomePublicos);

                foreach (DataRow dr in mdata.Read("SELECT * FROM Especies ORDER BY Codigo").Rows)
                {
                    mEspecie e = new mEspecie();
                    e.Indice = (int)dr[0];
                    e.Codigo = (int)dr[1];
                    e.Especie = (string)dr[2];
                    e.Ativo = (bool)dr[3];
                    _list.Add(e);
                }

                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<string> Tipos()
        {
            try
            {
                ObservableCollection<string> _list = new ObservableCollection<string>();
                var mdata = Data.Factory.Connecting(DataBase.Base.NomePublicos);

                foreach (DataRow dr in mdata.Read("SELECT * FROM Tipos ORDER BY Tipo").Rows)
                {
                    string t = (string)dr[1];
                    _list.Add(t);
                }

                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);                
            }
        }

        public ObservableCollection<string> Origens()
        {
            try
            {
                ObservableCollection<string> _list = new ObservableCollection<string>();

                _list.Add("LEI");
                _list.Add("DECRETO");
                _list.Add("LEI COMPLEMENTAR");

                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public DataTable Denominacoes(string sqlcommand)
        {
            try
            {
                var dataAccess = Data.Factory.Connecting(DataBase.Base.NomePublicos);

                return dataAccess.Read(sqlcommand);
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public DataTable LastRows()
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.NomePublicos);
            try
            {
                var list = new DataTable();

                string sqlport = @"SELECT TOP 20 Tipo, Nome, Origem, Numero_Origem, Ano_Origem FROM Denominacoes ORDER BY Cadastro DESC, Ano_Origem DESC, Nome DESC";

                return dataAccess.Read(sqlport);
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
            finally
            {
                dataAccess = null;
            }
        }
        #endregion

        #region Objetos

        #endregion
    }
}
