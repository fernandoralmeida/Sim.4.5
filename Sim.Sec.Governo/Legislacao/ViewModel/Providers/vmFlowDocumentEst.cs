using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;

namespace Sim.Sec.Governo.Legislacao.ViewModel.Providers
{
    using Model;
    using Account;
    using Controls;

    class vmFlowDocumentEst : vmFlowDocumentBase
    {

        public FlowDocument ChartFlowDocument(ObservableCollection<BarChart> charts, List<string> filtros)
        {
            FlowDocument flow = new FlowDocument();
            flow.ColumnGap = 0;
            // Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            flow.ColumnWidth = 8.5 * 96.0;
            flow.PageWidth = 8.5 * 96.0;
            flow.PageHeight = 11.5 * 96.0;
            //flow.Background = Brushes.White;


            flow.FontFamily = new FontFamily("Arial");
            flow.FontSize = 12;
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

            #region Informe

            string f = string.Empty;
            foreach (string filtro in filtros)
                f += filtro;

            Figure lfiltro = new Figure();
            lfiltro.Width = new System.Windows.FigureLength(1000);
            lfiltro.Height = new System.Windows.FigureLength(1);
            lfiltro.Background = Brushes.Gray;
            lfiltro.Margin = new System.Windows.Thickness(0, 10, 0, 0);

            Paragraph ft = new Paragraph();
            ft.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            ft.FontSize = 10;
            ft.Inlines.Add(lfiltro);
            ft.Inlines.Add(new Run("FILTROS: "));
            ft.Inlines.Add(new Run(f));

            #endregion

            Figure lrodape = new Figure();
            lrodape.Width = new System.Windows.FigureLength(1000);
            lrodape.Height = new System.Windows.FigureLength(1);
            lrodape.Background = Brushes.Gray;
            lrodape.Margin = new System.Windows.Thickness(0, 10, 0, 0);

            Paragraph r = new Paragraph();
            r.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            r.Inlines.Add(lrodape);
            r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
            r.Inlines.Add(new Run(" / V" + version + " / "));
            r.Inlines.Add(new Bold(new Run(Logged.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));

            flow.Blocks.Add(ft);
            flow.Blocks.Add(r);

            return flow;
        }

        public FlowDocument TextFlowDoc(vmDataReports obj, bool tipo, bool situacao, bool origem, bool classificacao, bool autor, List<string> filtros)
        {
            #region Header

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
            pA.Inlines.Add(new Bold(new Run(string.Format("Estatísticas Gerais").ToUpper())));
            flow.Blocks.Add(pA);

            #endregion
            
            Table tb = new Table();

            tb.Columns.Add(new TableColumn() { Width = new GridLength(80, GridUnitType.Star) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(20, GridUnitType.Star) });
            //tb.Columns.Add(new TableColumn() { Width = new GridLength(40, GridUnitType.Star) });
            
            flow.Blocks.Add(tb);

            double mxv = 0;

            #region Tipo

            if (tipo)
            {                
                foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Tipo)
                {
                    mxv += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Tipo)
                {
                    mxv += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Tipo)
                {
                    mxv += x.Value;
                }
            }
            #endregion

            #region Leis         
   
            if (obj.InfoLeisOrdinarias().Tipo.Count > 0)
            {

                if (tipo)
                {

                    TableRowGroup rg2 = new TableRowGroup();

                    tb.RowGroups.Add(rg2);

                    TableRow rw2 = new TableRow();
                    rg2.Rows.Add(rw2);

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Tipo)
                    {

                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg2.Rows.Add(row);
                        row.Background = Brushes.Gainsboro;
                        row.Foreground = Brushes.Black;
                        row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(x.Key)))));
                        row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(string.Format("{0} [{1:P2}]", x.Value, perc))))));
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("LEIS"))) { Background = Brushes.WhiteSmoke });
                    }
                }

                #region Situação

                if (situacao)
                {
                    TableRowGroup rg = new TableRowGroup();

                    tb.RowGroups.Add(rg);

                    TableRow rw = new TableRow();
                    rg.Rows.Add(rw);
                    rw.Background = Brushes.Gainsboro;
                    rw.Foreground = Brushes.Black;
                    rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Situação")))));
                    rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Situacao)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Situacao)
                    {

                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        row.FontSize = 11;
                        rg.Rows.Add(row);
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                }
                #endregion

                #region Classificação

                if (classificacao)
                {

                    TableRowGroup rg3 = new TableRowGroup();

                    tb.RowGroups.Add(rg3);

                    TableRow rw3 = new TableRow();
                    rg3.Rows.Add(rw3);
                    rw3.Background = Brushes.Gainsboro;
                    rw3.Foreground = Brushes.Black;
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Classificação")))));
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Classificacao)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Classificacao)
                    {
                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg3.Rows.Add(row);
                        row.FontSize = 11;
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg3.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                }

                #endregion

                #region Origem
                if (origem)
                {

                    TableRowGroup rg3 = new TableRowGroup();

                    tb.RowGroups.Add(rg3);

                    TableRow rw3 = new TableRow();
                    rg3.Rows.Add(rw3);
                    rw3.Background = Brushes.Gainsboro;
                    rw3.Foreground = Brushes.Black;
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Origem")))));
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Origem)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Origem)
                    {
                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg3.Rows.Add(row);
                        row.FontSize = 11;
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg3.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));

                }
                #endregion

                #region Autor

                if (autor)
                {

                    TableRowGroup rg3 = new TableRowGroup();

                    tb.RowGroups.Add(rg3);

                    TableRow rw3 = new TableRow();
                    rg3.Rows.Add(rw3);
                    rw3.Background = Brushes.Gainsboro;
                    rw3.Foreground = Brushes.Black;
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Autor")))));
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Autor)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisOrdinarias().Autor)
                    {
                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg3.Rows.Add(row);
                        row.FontSize = 11;
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg3.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                }

                #endregion
            }
            
            #endregion

            #region Leis Complementares

            if (obj.InfoLeisComp().Tipo.Count > 0)
            {

                if (tipo)
                {

                    TableRowGroup rg2 = new TableRowGroup();

                    tb.RowGroups.Add(rg2);

                    TableRow rw2 = new TableRow();
                    rg2.Rows.Add(rw2);

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Tipo)
                    {

                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg2.Rows.Add(row);
                        row.Background = Brushes.Gainsboro;
                        row.Foreground = Brushes.Black;
                        row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(x.Key)))));
                        row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(string.Format("{0} [{1:P2}]", x.Value, perc))))));
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("LEIS COMPLEMENTARES"))) { Background = Brushes.WhiteSmoke });
                    }
                }

                #region Situação

                if (situacao)
                {
                    TableRowGroup rg = new TableRowGroup();

                    tb.RowGroups.Add(rg);

                    TableRow rw = new TableRow();
                    rg.Rows.Add(rw);
                    rw.Background = Brushes.Gainsboro;
                    rw.Foreground = Brushes.Black;
                    rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Situação")))));
                    rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Situacao)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Situacao)
                    {

                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        row.FontSize = 11;
                        rg.Rows.Add(row);
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                }
                #endregion

                #region Classificação

                if (classificacao)
                {

                    TableRowGroup rg3 = new TableRowGroup();

                    tb.RowGroups.Add(rg3);

                    TableRow rw3 = new TableRow();
                    rg3.Rows.Add(rw3);
                    rw3.Background = Brushes.Gainsboro;
                    rw3.Foreground = Brushes.Black;
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Classificação")))));
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Classificacao)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Classificacao)
                    {
                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg3.Rows.Add(row);
                        row.FontSize = 11;
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg3.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                }

                #endregion

                #region Origem
                if (origem)
                {

                    TableRowGroup rg3 = new TableRowGroup();

                    tb.RowGroups.Add(rg3);

                    TableRow rw3 = new TableRow();
                    rg3.Rows.Add(rw3);
                    rw3.Background = Brushes.Gainsboro;
                    rw3.Foreground = Brushes.Black;
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Origem")))));
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Origem)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Origem)
                    {
                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg3.Rows.Add(row);
                        row.FontSize = 11;
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg3.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));

                }
                #endregion

                #region Autor

                if (autor)
                {

                    TableRowGroup rg3 = new TableRowGroup();

                    tb.RowGroups.Add(rg3);

                    TableRow rw3 = new TableRow();
                    rg3.Rows.Add(rw3);
                    rw3.Background = Brushes.Gainsboro;
                    rw3.Foreground = Brushes.Black;
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Autor")))));
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Autor)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoLeisComp().Autor)
                    {
                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg3.Rows.Add(row);
                        row.FontSize = 11;
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg3.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                }

                #endregion
            }
            #endregion

            #region Decretos

            if (obj.InfoDecretos().Tipo.Count > 0)
            {

                if (tipo)
                {

                    TableRowGroup rg2 = new TableRowGroup();

                    tb.RowGroups.Add(rg2);

                    TableRow rw2 = new TableRow();
                    rg2.Rows.Add(rw2);

                    foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Tipo)
                    {

                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg2.Rows.Add(row);
                        row.Background = Brushes.Gainsboro;
                        row.Foreground = Brushes.Black;
                        row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(x.Key)))));
                        row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(string.Format("{0} [{1:P2}]", x.Value, perc))))));
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("DECRETOS"))) { Background = Brushes.WhiteSmoke });
                    }
                }

                #region Situação

                if (situacao)
                {
                    TableRowGroup rg = new TableRowGroup();

                    tb.RowGroups.Add(rg);

                    TableRow rw = new TableRow();
                    rg.Rows.Add(rw);
                    rw.Background = Brushes.Gainsboro;
                    rw.Foreground = Brushes.Black;
                    rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Situação")))));
                    rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Situacao)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Situacao)
                    {

                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        row.FontSize = 11;
                        rg.Rows.Add(row);
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                }
                #endregion

                #region Classificação

                if (classificacao)
                {

                    TableRowGroup rg3 = new TableRowGroup();

                    tb.RowGroups.Add(rg3);

                    TableRow rw3 = new TableRow();
                    rg3.Rows.Add(rw3);
                    rw3.Background = Brushes.Gainsboro;
                    rw3.Foreground = Brushes.Black;
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Classificação")))));
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Classificacao)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Classificacao)
                    {
                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg3.Rows.Add(row);
                        row.FontSize = 11;
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg3.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                }

                #endregion

                #region Origem
                if (origem)
                {

                    TableRowGroup rg3 = new TableRowGroup();

                    tb.RowGroups.Add(rg3);

                    TableRow rw3 = new TableRow();
                    rg3.Rows.Add(rw3);
                    rw3.Background = Brushes.Gainsboro;
                    rw3.Foreground = Brushes.Black;
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Origem")))));
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Origem)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Origem)
                    {
                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg3.Rows.Add(row);
                        row.FontSize = 11;
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg3.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));

                }
                #endregion

                #region Autor

                if (autor)
                {

                    TableRowGroup rg3 = new TableRowGroup();

                    tb.RowGroups.Add(rg3);

                    TableRow rw3 = new TableRow();
                    rg3.Rows.Add(rw3);
                    rw3.Background = Brushes.Gainsboro;
                    rw3.Foreground = Brushes.Black;
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Autor")))));
                    rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Documento(s)")))));
                    //rw3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("...")))));

                    mxv = 0;
                    foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Autor)
                    {
                        mxv += x.Value;
                    }

                    foreach (KeyValuePair<string, int> x in obj.InfoDecretos().Autor)
                    {
                        double perc = 0;
                        perc = (x.Value / mxv);

                        TableRow row = new TableRow();
                        rg3.Rows.Add(row);
                        row.FontSize = 11;
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Foreground = Brushes.DimGray }) { Background = Brushes.WhiteSmoke });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} [{1:P2}]", x.Value, perc)) { Foreground = Brushes.Navy })) { Background = Brushes.WhiteSmoke });
                        //row.Cells.Add(new TableCell(new Paragraph(new Run("Portaria(as)"))) { Background = Brushes.WhiteSmoke });
                    }

                    TableRow rowempty = new TableRow();
                    rg3.Rows.Add(rowempty);
                    rowempty.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty))));
                }

                #endregion

            }

            #endregion

            #region Informe

            string f = string.Empty;
            foreach (string filtro in filtros)
                f += filtro;

            Paragraph ft = new Paragraph();
            ft.Margin = new System.Windows.Thickness(0, 30, 0, 0);
            ft.FontSize = 10;
            ft.Inlines.Add(new Run("FILTROS: "));
            ft.Inlines.Add(new Run(f));

            #endregion

            #region Rodapé

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 10, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
            r.Inlines.Add(new Run(" / V" + version + " / "));
            r.Inlines.Add(new Bold(new Run(Logged.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));
            
            #endregion

            flow.Blocks.Add(ft);
            flow.Blocks.Add(r);

            return flow;
        }

    }
}
