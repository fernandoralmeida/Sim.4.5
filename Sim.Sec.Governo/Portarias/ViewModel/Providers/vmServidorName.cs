using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace Sim.Sec.Governo.Portarias.ViewModel.Providers
{
    
    using Model;

    class vmServidorName : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return new mData().ListName().GetEnumerator();
        }
    }
}
