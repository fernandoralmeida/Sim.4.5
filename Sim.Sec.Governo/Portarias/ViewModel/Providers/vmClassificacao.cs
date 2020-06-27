using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Portarias.ViewModel.Providers
{

    using Model;
    using Sql;

    public class vmClassificacao : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return new mData().ListaGenerica(SqlCollections.Classi_Only_Non_Blocked).GetEnumerator();
        }

    }
}
