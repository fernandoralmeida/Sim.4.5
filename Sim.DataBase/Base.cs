using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.DataBase
{
    public class Base
    {
        public static string Login
        {
            get
            {
                return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}SimDataBase1_N.mdb;Persist Security Info=True", Properties.Settings.Default.ConnectionString);
            }
        }

        public static string Governo
        {
            get
            {
                return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}SimDataBase2.mdb;Persist Security Info=True", Properties.Settings.Default.ConnectionString);
            }
        }

        public static string Desenvolvimento
        {
            get
            {
                return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}SimDataBase3.mdb;Persist Security Info=True", Properties.Settings.Default.ConnectionString);
            }
        }

        public static string NomePublicos
        {
            get
            {
                return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}SimDataBase4.mdb;Persist Security Info=True", Properties.Settings.Default.ConnectionString);
            }
        }

    }
}
