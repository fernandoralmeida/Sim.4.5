using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Portarias.ViewModel
{

    using Controls;

    class vmChartsProvider
    {
        public ObservableCollection<BarChart> Create(vmEstatisticsProvider Rel, bool ano, bool nome, bool classificacao, double width)
        {
            ObservableCollection<BarChart> _chart = new ObservableCollection<BarChart>();
            _chart.Clear();


            if (ano)
            {
                List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.Relatories.Ano)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = "Portarias : Anos";
                    bc.ItemsSource = MyValue;

                    _chart.Add(bc);
                }
            }

            if (classificacao)
            {

                List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.Relatories.Tipo)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = "Portarias : Classificações";
                    bc.ItemsSource = MyValue;

                    _chart.Add(bc);
                }

            }

            if (nome)
            {

                List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in Rel.Relatories.Servidor)
                {
                    MyValue.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (MyValue.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = "Portarias : Servidores";
                    bc.ItemsSource = MyValue;

                    _chart.Add(bc);
                }
            }

            return _chart;
        }
    }
}
