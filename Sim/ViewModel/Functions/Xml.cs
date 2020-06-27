using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Sim.ViewModel.Functions
{
    class Xml
    {
        public static List<string> Read(string url)
        {
            List<string> lista = new List<string>();
            var docxml = XDocument.Load(url);
            var items = docxml.Element(XName.Get("System")).Elements(XName.Get("Version"));
            var nitems = items.Select(ele => ele.Element(XName.Get("Update")).Value);

            foreach (string name in nitems)
            {
                lista.Add(name);
            }

            docxml = null;
            return lista;
        }
    }
}
