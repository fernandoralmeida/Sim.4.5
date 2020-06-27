using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;

namespace Sim.Sec.Governo.Portarias.ViewModel
{
    using Sim.Controls;
    using Model;
    using Account;

    class vmFlowDocumentEst : vmFlowDocumentBase
    {

        public FlowDocument ChartFlowDoc(ObservableCollection<BarChart> charts, List<string> filtros)
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

            foreach (BarChart bc in charts)
            {
                flow.Blocks.Add(new BlockUIContainer(bc));
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
            flow.Blocks.Add(r);

            return flow;
        }

        public FlowDocument TextFlowDoc(mEstatistics obj, bool ano, bool nome, bool classificacao, List<string> filtros)
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
            flow.TextAlignment = TextAlignment.Justify;
            flow.PagePadding = new Thickness(50);

            LineBreak lb = new LineBreak();

            Paragraph pH = new Paragraph(new Run(NameOrg));
            pH.Typography.Capitals = FontCapitals.SmallCaps;
            pH.FontSize = 20;
            pH.FontWeight = FontWeights.Bold;
            pH.Margin = new Thickness(0);

            Paragraph pH1 = new Paragraph(new Run(SloganOrg));
            pH1.FontSize = 9;
            pH1.Margin = new Thickness(1, 0, 0, 0);

            Paragraph pH2 = new Paragraph(new Run(DepOrg));
            pH2.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.FontWeight = FontWeights.Bold;
            pH2.FontSize = 14;
            pH2.Margin = new Thickness(0, 10, 0, 0);

            Paragraph pH3 = new Paragraph(new Run(SetorOrg));
            pH3.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.FontWeight = FontWeights.Bold;
            pH3.FontSize = 12;
            pH3.Margin = new Thickness(0, 0, 0, 40);

            flow.Blocks.Add(pH);
            flow.Blocks.Add(pH1);
            flow.Blocks.Add(pH2);
            flow.Blocks.Add(pH3);

            Paragraph pA = new Paragraph();
            pA.Margin = new Thickness(0, 20, 0, 0);
            pA.Inlines.Add(new Bold(new Run(string.Format("Relatório de Portarias").ToUpper())));
            flow.Blocks.Add(pA);

            Table tb = new Table();
            
            tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(20, GridUnitType.Star) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(10, GridUnitType.Star) });

            flow.Blocks.Add(tb);

            #region Classificacao

            if (classificacao)
            {
                TableRowGroup rg2 = new TableRowGroup();

                tb.RowGroups.Add(rg2);

                TableRow rw2 = new TableRow();
                rg2.Rows.Add(rw2);
                rw2.Background = Brushes.LightSalmon;
                rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Por Classificação")))));
                rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Quantidade")))));
                rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                double mxv = 0;
                foreach (KeyValuePair<string, int> x in obj.Tipo)
                {
                    mxv += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.Tipo)
                {

                    double perc = 0;
                    perc = (x.Value / mxv);

                    TableRow row = new TableRow();
                    rg2.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.Navy }) { Background = Brushes.WhiteSmoke });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.DarkViolet })) { Background = Brushes.WhiteSmoke });
                    row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                }
                /*
                TableRow rowtotal = new TableRow();
                rg2.Rows.Add(rowtotal);
                rowtotal.Cells.Add(new TableCell(new Paragraph(new Run("Total"))));
                rowtotal.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", mxv, (mxv / mxv))) { Foreground = Brushes.DarkViolet })) { Background = Brushes.WhiteSmoke });
                rowtotal.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                */
                TableRow rowempty1 = new TableRow();
                rg2.Rows.Add(rowempty1);
                rowempty1.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
            }
            #endregion

            #region Exercicio

            if (ano)
            {
                TableRowGroup rg = new TableRowGroup();

                tb.RowGroups.Add(rg);

                TableRow rw = new TableRow();
                rg.Rows.Add(rw);
                rw.Background = Brushes.LightSkyBlue;
                rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Por Exercício")))));
                rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Quantidade")))));
                rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                double mxv = 0;
                foreach (KeyValuePair<string, int> x in obj.Tipo)
                {
                    mxv += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.Ano)
                {

                    double perc = 0;
                    perc = (x.Value / mxv);

                    TableRow row = new TableRow();
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.Navy }) { Background = Brushes.WhiteSmoke });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.DarkViolet })) { Background = Brushes.WhiteSmoke });
                    row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                }
                /*
                TableRow rowtotal = new TableRow();
                rg.Rows.Add(rowtotal);
                rowtotal.Cells.Add(new TableCell(new Paragraph(new Run("Total"))));
                rowtotal.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", mxv, (mxv / mxv))) { Foreground = Brushes.DarkViolet })) { Background = Brushes.WhiteSmoke });
                rowtotal.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                */
                TableRow rowempty = new TableRow();
                rg.Rows.Add(rowempty);
                rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
            }
            #endregion            

            #region Servidor
            if (nome)
            {

                TableRowGroup rg3 = new TableRowGroup();

                tb.RowGroups.Add(rg3);

                TableRow rw3 = new TableRow();
                rg3.Rows.Add(rw3);
                rw3.Background = Brushes.LightGreen;
                rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Por Servidor/Funcionário")))));
                rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Quantidade")))));
                rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                double mxv = 0;
                foreach (KeyValuePair<string, int> x in obj.Tipo)
                {
                    mxv += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.Servidor)
                {
                    double perc = 0;
                    perc = (x.Value / mxv);

                    TableRow row = new TableRow();
                    rg3.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.Navy }) { Background = Brushes.WhiteSmoke });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.DarkViolet })) { Background = Brushes.WhiteSmoke });
                    row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                }
                /*
                TableRow rowtotal = new TableRow();
                rg3.Rows.Add(rowtotal);
                rowtotal.Cells.Add(new TableCell(new Paragraph(new Run("Total"))));
                rowtotal.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", mxv, (mxv / mxv))) { Foreground = Brushes.DarkViolet })) { Background = Brushes.WhiteSmoke });
                rowtotal.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                */
            }
            #endregion

            #region Filtros

            string f = string.Empty;
            foreach (string filtro in filtros)
                f += filtro;

            Paragraph ft = new Paragraph();
            ft.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            ft.FontSize = 10;
            ft.Inlines.Add(new Run("FILTROS: "));
            ft.Inlines.Add(new Run(f));

            #endregion

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 0, 0, 0);
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
            flow.Blocks.Add(r);

            return flow;
        }
    }
}
