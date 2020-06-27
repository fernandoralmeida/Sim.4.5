using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;

namespace Sim.Sec.Governo.Portarias.ViewModel.Providers
{
    using Model;
    using Mvvm.Observers;

    class PLastPortarias : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {            
            return new mData().LastRows().GetEnumerator();
        }
    }
}
