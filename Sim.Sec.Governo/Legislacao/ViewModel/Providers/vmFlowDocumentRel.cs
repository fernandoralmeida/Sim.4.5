using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;

namespace Sim.Sec.Governo.Legislacao.ViewModel.Providers
{
    using Model;
    using Account;

    class vmFlowDocumentRel : vmFlowDocumentBase
    {
        
        public FlowDocument FlowPrint { get; set; }

        public FlowDocument CreateFlowDocument(ObservableCollection<mLegislacaoConsulta> lista, List<string> filtros)
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
            pH1.Margin = new System.Windows.Thickness(1,0,0,0);

            Paragraph pH2 = new Paragraph(new Run(DepOrg));
            pH2.Typography.Capitals = System.Windows.FontCapitals.SmallCaps;
            pH2.FontWeight = System.Windows.FontWeights.Bold;
            pH2.FontSize = 14;
            pH2.Margin = new System.Windows.Thickness(0,10,0,0);

            Paragraph pH3 = new Paragraph(new Run(SetorOrg));
            pH3.Typography.Capitals = System.Windows.FontCapitals.SmallCaps;
            pH2.FontWeight = System.Windows.FontWeights.Bold;
            pH3.FontSize = 12;
            pH3.Margin = new System.Windows.Thickness(0,0,0,40);

            flow.Blocks.Add(pH);
            flow.Blocks.Add(pH1);
            flow.Blocks.Add(pH2);
            flow.Blocks.Add(pH3);

            int l = 0;
            int d = 0;
            int lc = 0;

            foreach (mLegislacaoConsulta m in lista)
            {

                if (m.Tipo == "LEI")
                    l++;

                if (m.Tipo == "LEI COMPLEMENTAR")
                    lc++;

                if (m.Tipo == "DECRETO")
                    d++;

                Paragraph p = new Paragraph();
                p.Margin = new System.Windows.Thickness(0, 20, 0, 0);

                p.Inlines.Add(new Bold(new Run(string.Format(@"{0} Nº {1}{2}/{3} ", m.Tipo, m.Numero, m.Complemento, m.Data.Year))));
                p.Inlines.Add(string.Format(@" {0}", m.Publicado));

                SolidColorBrush fc = new SolidColorBrush();

                if (m.Situacao == "INALTERADO(A)")
                    fc = Brushes.Green;

                if (m.Situacao == "ALTERADO(A)")
                    fc = Brushes.Orange;

                if (m.Situacao == "REVOGADO(A)")
                    fc = Brushes.Red;

                p.Inlines.Add(new Run(string.Format(@" {0}", m.Situacao)) { Foreground = fc });
                p.Inlines.Add(new LineBreak());
                p.Inlines.Add(m.Autor);
                p.Inlines.Add(new LineBreak());
                p.Inlines.Add(m.Classificacao);
                p.Inlines.Add(new LineBreak());
                p.Inlines.Add(m.Resumo);

                List list = new List();
                list.FontSize = 10;
                list.Margin = new System.Windows.Thickness(0, 0, 20, 0);

                foreach (mAcoesConsulta ae in m.ListaAcoesExercidas)
                {
                    list.ListItems.Add(new ListItem(new Paragraph(new Run(
                        string.Format(@"{0} {1} Nº {2}{3}/{4}", ae.Acao, ae.TipoAlvo, ae.NumeroAlvo, ae.ComplementoAlvo, ae.DataAlvo.Year))
                        { Foreground = Brushes.SlateBlue })));
                }
                                
                foreach (mAcoesConsulta ar in m.ListaAcoesRecebidas)
                {
                    list.ListItems.Add(new ListItem(new Paragraph(new Run(
                        string.Format(@"{0} {1} Nº {2}{3}/{4}", ar.Acao, ar.TipoAlvo, ar.NumeroAlvo, ar.ComplementoAlvo, ar.DataAlvo.Year)) 
                        { Foreground = Brushes.SteelBlue})));
                }

                flow.Blocks.Add(p);
                flow.Blocks.Add(list);
            }

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
            ft1.Inlines.Add(new Run("RESULTADO: "));
            ft1.Inlines.Add(new Run(" LEIS: " + l));
            ft1.Inlines.Add(new Run(" LEIS COMPLEMENTARES: " + lc));
            ft1.Inlines.Add(new Run(" DECRETOS: " + d));
            ft1.Inlines.Add(new Bold(new Run(" TOTAL: " + (l + lc + d))));

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

            //FlowPrint = flow;
            return flow;
        }
    }
}
