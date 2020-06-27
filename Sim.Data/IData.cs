using System.Data;

namespace Sim.Data
{
    public interface IData
    {
        void ClearParameters();

        void AddParameters(string parameterName, object parameterValue);

        DataTable Read(string sqlcommandOrStoredProcedure);

        bool Write(string sqlcommandOrStoredProcedure);
    }
}
