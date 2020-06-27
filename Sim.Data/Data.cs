using System;
using System.Data;
using System.Data.OleDb;

namespace Sim.Data
{
    class Data : IData
    {
        private OleDbParameterCollection ParameterCollection = new OleDbCommand().Parameters;

        public Data(string connection)
        {
            Conectar = new OleDbConnection(connection);
        }

        private OleDbConnection Conectar { get; set; }

        public void ClearParameters()
        {
            ParameterCollection.Clear();
        }

        public void AddParameters(string parameterName, object parameterValue)
        {
            ParameterCollection.Add(new OleDbParameter(parameterName, parameterValue));
        }

        public DataTable Read(string sqlcommandOrStoredProcedure)
        {
            OleDbConnection dbConnection = Conectar;

            try
            {
                dbConnection.Open();

                OleDbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = sqlcommandOrStoredProcedure;

                foreach (OleDbParameter p in ParameterCollection)
                {
                    dbCommand.Parameters.Add(new OleDbParameter(p.ParameterName, p.Value));
                }

                DataTable dt = new DataTable();

                new OleDbDataAdapter(dbCommand).Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public bool Write(string sqlcommandOrStoredProcedure)
        {
            OleDbConnection dbConnection = Conectar;                       

            dbConnection.Open();

            //OleDbTransaction dbTransaction = dbConnection.BeginTransaction();

            try
            {

                OleDbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = sqlcommandOrStoredProcedure;
                dbCommand.CommandType = CommandType.Text;

                //dbCommand.Transaction = dbTransaction;

                foreach (OleDbParameter p in ParameterCollection)
                {
                    dbCommand.Parameters.Add(new OleDbParameter(p.ParameterName, p.Value));
                }

                dbCommand.ExecuteNonQuery();
                //dbTransaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //dbTransaction.Rollback();
                return false;
                throw new Exception(ex.Message);
            }
            finally
            {
                //dbTransaction.Dispose();
                //dbTransaction = null;
                dbConnection.Close();
                dbConnection.Dispose();
                //dbConnection = null;
                this.Dispose();
            }
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
