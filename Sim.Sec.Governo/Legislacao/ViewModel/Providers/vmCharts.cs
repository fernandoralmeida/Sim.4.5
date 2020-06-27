using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Legislacao.ViewModel.Providers
{

    using Model;
    using Controls;

    class vmCharts
    {
        public ObservableCollection<BarChart> Create(vmDataReports Rel, bool tipo, bool situacao, bool origem, bool classificacao, bool autor, double width)
        {
            ObservableCollection<BarChart> _chart = new ObservableCollection<BarChart>();

            _chart.Clear();

            double barchartwidth = width;

            #region chart Tipos
            if (tipo)
            {

                List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisOrdinarias().Tipo)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisComp().Tipo)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                foreach (KeyValuePair<string, int> x in Rel.InfoDecretos().Tipo)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = "Documentos : Tipos";
                    bc.ItemsSource = MyValue;

                    _chart.Add(bc);
                }
            }
            #endregion

            #region Charts Situação

            if (situacao)
            {

                List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisOrdinarias().Situacao)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue.Count > 0)
                {
                    BarChart bc = new BarChart();

                    bc.Title = "Leis : Situação";
                    bc.ItemsSource = MyValue;

                    _chart.Add(bc);
                }

                List<KeyValuePair<string, int>> MyValue1 = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisComp().Situacao)
                {
                    MyValue1.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue1.Count > 0)
                {
                    BarChart bc1 = new BarChart();

                    bc1.Title = "Leis Complementares : Situação";
                    bc1.ItemsSource = MyValue1;
                    _chart.Add(bc1);
                }

                List<KeyValuePair<string, int>> MyValue2 = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoDecretos().Situacao)
                {
                    MyValue2.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue2.Count > 0)
                {
                    BarChart bc2 = new BarChart();

                    bc2.Title = "Decretos : Situação";
                    bc2.ItemsSource = MyValue2;
                    _chart.Add(bc2);
                }
            }
            #endregion

            #region Chart Origem
            if (origem)
            {

                List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisOrdinarias().Origem)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue.Count > 0)
                {
                    BarChart bc = new BarChart();

                    bc.Title = "Leis : Origem";
                    bc.ItemsSource = MyValue;
                    _chart.Add(bc);
                }

                List<KeyValuePair<string, int>> MyValue1 = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisComp().Origem)
                {
                    MyValue1.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue1.Count > 0)
                {
                    BarChart bc1 = new BarChart();

                    bc1.Title = "Leis Complementares : Origem";
                    bc1.ItemsSource = MyValue1;
                    _chart.Add(bc1);
                }

                List<KeyValuePair<string, int>> MyValue2 = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoDecretos().Origem)
                {
                    MyValue2.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue2.Count > 0)
                {
                    BarChart bc2 = new BarChart();
                    bc2.Title = "Decretos : Origem";
                    bc2.ItemsSource = MyValue2;
                    _chart.Add(bc2);
                }
            }
            #endregion

            #region Chart Autor
            if (autor)
            {

                List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisOrdinarias().Autor)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue.Count > 0)
                {
                    BarChart bc = new BarChart();

                    bc.Title = "Leis : Autor(es)";
                    bc.ItemsSource = MyValue;
                    _chart.Add(bc);
                }

                List<KeyValuePair<string, int>> MyValue1 = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisComp().Autor)
                {
                    MyValue1.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue1.Count > 0)
                {
                    BarChart bc1 = new BarChart();

                    bc1.Title = "Leis Complementares : Autor(es)";
                    bc1.ItemsSource = MyValue1;
                    _chart.Add(bc1);
                }

                List<KeyValuePair<string, int>> MyValue2 = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoDecretos().Autor)
                {
                    MyValue2.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue2.Count > 0)
                {
                    BarChart bc2 = new BarChart();

                    bc2.Title = "Decretos : Autor(es)";
                    bc2.ItemsSource = MyValue2;
                    _chart.Add(bc2);
                }

            }
            #endregion

            #region Chart Classificao
            if (classificacao)
            {

                List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisOrdinarias().Classificacao)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue.Count > 0)
                {
                    BarChart bc = new BarChart();

                    bc.Title = "Leis : Classificação";
                    bc.ItemsSource = MyValue;
                    _chart.Add(bc);
                }

                List<KeyValuePair<string, int>> MyValue1 = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoLeisComp().Classificacao)
                {
                    MyValue1.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue1.Count > 0)
                {
                    BarChart bc1 = new BarChart();

                    bc1.Title = "Leis Complementares : Classificação";
                    bc1.ItemsSource = MyValue1;
                    _chart.Add(bc1);
                }

                List<KeyValuePair<string, int>> MyValue2 = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.InfoDecretos().Classificacao)
                {
                    MyValue2.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue2.Count > 0)
                {
                    BarChart bc2 = new BarChart();

                    bc2.Title = "Decretos : Classificação";
                    bc2.ItemsSource = MyValue2;
                    _chart.Add(bc2);
                }

                

            }
            #endregion

            return _chart;

        }
    }
}
