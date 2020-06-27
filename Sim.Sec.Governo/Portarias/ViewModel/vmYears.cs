using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sim.Sec.Governo.Portarias.ViewModel
{
    using System.Collections;

    class vmYears : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            List<KeyValuePair<string, string>> ano = new List<KeyValuePair<string, string>>();

            ano.Add(new KeyValuePair<string, string>("...", "0001"));

            for (int i = DateTime.Now.Year; i > 1852; i--)
                ano.Add(new KeyValuePair<string, string>(i.ToString(), i.ToString())); ;            

            return ano.GetEnumerator();
        }
    }
}
