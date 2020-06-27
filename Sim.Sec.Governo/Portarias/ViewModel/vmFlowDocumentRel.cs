using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;

namespace Sim.Sec.Governo.Portarias.ViewModel
{

    using Model;
    using Mvvm.Observers;
    using Account;

    class vmFlowDocumentRel : vmFlowDocumentBase
    {

        public FlowDocument CreateFlowDocument(ObservableCollection<mPortaria> lista, List<string> filtros)
        {
            FlowDocument flow = new FlowDocument();
            flow.ColumnGap = 0;
            // Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            flow.ColumnWidth = 8.5 * 96.0;
            flow.PageWidth = 8.5 * 96.0;
            flow.PageHeight = 11.5 * 96.0;
            flow.Background = Brushes.White;
            flow.Foreground = Brushes.Black;

            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 11;
            flow.TextAlignment = System.Windows.TextAlignment.Justify;
            flow.PagePadding = new System.Windows.Thickness(50);

            LineBreak lb = new LineBreak();

            Paragraph pH = new Paragraph(new Run(NameOrg));
            pH.Typography.Capitals = System.Windows.FontCapitals.SmallCaps;
            pH.FontSize = 20;
            pH.FontWeight = System.Windows.FontWeights.Bold;
            pH.Margin = new System.Windows.Thickness(0);

            Paragraph pH1 = new Paragraph(new Run(SloganOrg));
            pH1.FontSize = 9;
            pH1.Margin = new System.Windows.Thickness(1, 0, 0, 0);

            Paragraph pH2 = new Paragraph(new Run(DepOrg));
            pH2.Typography.Capitals = System.Windows.FontCapitals.SmallCaps;
            pH2.FontWeight = System.Windows.FontWeights.Bold;
            pH2.FontSize = 14;
            pH2.Margin = new System.Windows.Thickness(0, 10, 0, 0);

            Paragraph pH3 = new Paragraph(new Run(SetorOrg));
            pH3.Typography.Capitals = System.Windows.FontCapitals.SmallCaps;
            pH2.FontWeight = System.Windows.FontWeights.Bold;
            pH3.FontSize = 12;
            pH3.Margin = new System.Windows.Thickness(0, 0, 0, 40);

            flow.Blocks.Add(pH);
            flow.Blocks.Add(pH1);
            flow.Blocks.Add(pH2);
            flow.Blocks.Add(pH3);

            int cont = 0;

            foreach (mPortaria m in lista)
            {

                Paragraph p = new Paragraph();
                p.Margin = new System.Windows.Thickness(0, 20, 0, 0);

                p.Inlines.Add(new Bold(new Run(string.Format(@"{0} Nº {1}/{2}", "PORTARIA", m.Numero, m.Data.Year))));
                p.Inlines.Add(string.Format(@" [{0}]", m.Publicada));
                p.Inlines.Add(new LineBreak());
                p.Inlines.Add(m.Tipo);
                p.Inlines.Add(new LineBreak());
                p.Inlines.Add(m.Resumo);

                List list = new List();
                list.FontSize = 11;
                list.Margin = new System.Windows.Thickness(0, 0, 20, 0);

                foreach (mServidor ae in m.Servidor)
                {
                    list.ListItems.Add(new ListItem(new Paragraph(new Run(string.Format(@"{0}", ae.Nome)))));
                }

                flow.Blocks.Add(p);
                flow.Blocks.Add(list);

                cont++;
            }

            #region Filtros

            string f = string.Empty;
            foreach (string filtro in filtros)
                f += filtro;

            Paragraph ft = new Paragraph();
            ft.Margin = new System.Windows.Thickness(0, 30, 0, 0);
            ft.FontSize = 10;
            ft.Inlines.Add(new Run("FILTROS: "));
            ft.Inlines.Add(new Run(f));

            Paragraph ft1 = new Paragraph();
            ft1.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            ft1.FontSize = 10;
            ft1.Inlines.Add(new Run("[PORTARIAS ENCONTRADAS: "));
            ft1.Inlines.Add(new Bold(new Run(cont.ToString())));
            ft1.Inlines.Add(new Run("]"));

            #endregion

            Paragraph r = new Paragraph();
            r.Margin = new System.Windows.Thickness(0, 10, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
            r.Inlines.Add(new Run(" / V" + version + " / "));
            r.Inlines.Add(new Bold(new Run(Logged.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));

            flow.Blocks.Add(ft);
            flow.Blocks.Add(ft1);
            flow.Blocks.Add(r);

            return flow;
        }
    }
}
