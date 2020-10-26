using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Sim.Publisher.ViewModel
{
    public class Version
    {
        public static void Major(string valor)
        {
            Version.w_Xml(valor, "Major");
        }

        public static void Minor(string valor)
        {
            Version.w_Xml(valor, "Minor");
        }

        public static void Build(string valor)
        {
            Version.w_Xml(valor, "Build");
        }

        public static void Revision(string valor)
        {
            Version.w_Xml(valor, "Revision");
        }

        public static void Update(string valor)
        {
            Version.w_Xml(valor, "Update");
        }

        private static void w_Xml(string valor, string chave)
        {
            XmlDocument docxml = new XmlDocument();
            docxml.Load(string.Format(@"{0}\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, "sim_update"));

            XmlNode node;
            node = docxml.DocumentElement;

            foreach (XmlNode node1 in node.ChildNodes)
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == chave)
                        node2.InnerText = valor;

            docxml.Save(string.Format(@"{0}\source\repos\fernandoralmeida\Sim.4.5\Sim.Publisher\{1}.xml", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "sim_update"));
            docxml.Save(string.Format(@"{0}\{1}\{2}.xml", Directories.SimTEMP, Folders.Publish, "sim_update"));
        }

        public static List<string> Listar(string file, string elemento, string elementos, string item)
        {
            List<string> lista = new List<string>();
            var docxml = XDocument.Load(string.Format(@"{0}\source\repos\fernandoralmeida\Sim.4.5\Sim.Publisher\{1}.xml", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), file));
            var items = docxml.Element(XName.Get(elemento)).Elements(XName.Get(elementos));
            var nitems = items.Select(ele => ele.Element(XName.Get(item)).Value);

            foreach (string name in nitems)
            {
                lista.Add(name);
            }

            docxml = null;
            return lista;
        }
    }
}
