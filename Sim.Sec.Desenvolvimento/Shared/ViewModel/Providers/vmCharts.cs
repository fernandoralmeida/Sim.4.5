using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Providers
{
    using Controls;
    using Model;

    class vmCharts
    {
        public ObservableCollection<BarChart> Atendimento(mReportAtendimentos reports, bool periodo)
        {
            ObservableCollection<BarChart> _chart = new ObservableCollection<BarChart>();
            _chart.Clear();

            if (periodo)
            {

                List<KeyValuePair<string, int>> Atendimentos = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in reports.Atendimentos)
                {
                    Atendimentos.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                List<KeyValuePair<string, int>> Anos = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in reports.Ano)
                {
                    Anos.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                List<KeyValuePair<string, int>> Origens = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in reports.Origem)
                {
                    Origens.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                List<KeyValuePair<string, int>> Tipos = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in reports.Tipo)
                {
                    Tipos.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                List<KeyValuePair<string, int>> Operadores = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in reports.Operador)
                {
                    Operadores.Add(new KeyValuePair<string, int>(x.Key, x.Value));
                }

                if (Atendimentos.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Total de Atendimentos");
                    bc.ItemsSource = Atendimentos;
                    _chart.Add(bc);

                    if (Anos.Count > 0)
                    {
                        BarChart bci = new BarChart();

                        bci.Title = string.Format("Atendimentos / Anos");
                        bci.ItemsSource = Anos;

                        _chart.Add(bci);
                    }

                    if (Origens.Count > 0)
                    {
                        BarChart bci = new BarChart();

                        bci.Title = string.Format("Atendimentos / Origem");
                        bci.ItemsSource = Origens;

                        _chart.Add(bci);
                    }


                    if (Tipos.Count > 0)
                    {
                        BarChart bci = new BarChart();

                        bci.Title = string.Format("Atendimentos / Serviços");
                        bci.ItemsSource = Tipos;

                        _chart.Add(bci);
                    }

                    if (Operadores.Count > 0)
                    {
                        BarChart bci = new BarChart();

                        bci.Title = string.Format("Atendimentos / Operador", reports.Periodo[0], reports.Periodo[1]);
                        bci.ItemsSource = Operadores;

                        //_chart.Add(bci);
                    }

                }

            }

            return _chart;
        }

        public ObservableCollection<BarChart> Viabilidade(mReportViabilidade reports, bool periodo)
        {
            ObservableCollection<BarChart> _chart = new ObservableCollection<BarChart>();
            _chart.Clear();

            if (periodo)
            {
                List<KeyValuePair<string, int>> Viabilidade = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> Atividades = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> Bairro = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> Situacao = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in reports.Viabilidade)
                {
                    Viabilidade.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in reports.Atividades)
                {
                    Atividades.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in reports.Bairro)
                {
                    Bairro.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in reports.Situacao)
                {
                    Situacao.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                if (Viabilidade.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Periodo entre {0} e {1}", reports.Periodo[0], reports.Periodo[1]);
                    bc.ItemsSource = Viabilidade;

                    _chart.Add(bc);
                }

                if (Bairro.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Viabilidade / Setores");
                    bc.ItemsSource = Bairro;

                    _chart.Add(bc);
                }

                if (Atividades.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Viabilidade / Atividades");
                    bc.ItemsSource = Atividades;

                    _chart.Add(bc);
                }


                if (Situacao.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Situação");
                    bc.ItemsSource = Situacao;

                    _chart.Add(bc);
                }

            }

            return _chart;
        }

        public ObservableCollection<BarChart> Empresas(mReportEmpresas reports, bool periodo)
        {
            ObservableCollection<BarChart> _chart = new ObservableCollection<BarChart>();
            _chart.Clear();

            if (periodo)
            {
                List<KeyValuePair<string, int>> Atividade = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> Usolocal = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> Setor = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> Porte = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in reports.Atividade)
                {
                    Atividade.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in reports.UsoLocal)
                {
                    Usolocal.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in reports.Setor)
                {
                    Setor.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in reports.Porte)
                {
                    Porte.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                if (Porte.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Periodo entre {0} e {1}", reports.Periodo[0], reports.Periodo[1]);
                    bc.ItemsSource = Porte;

                    _chart.Add(bc);
                }

                if (Setor.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Empresas / Setor");
                    bc.ItemsSource = Setor;

                    _chart.Add(bc);
                }

                if (Atividade.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Empresas / Atividade Principal");
                    bc.ItemsSource = Atividade;

                    _chart.Add(bc);
                }

                if (Usolocal.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Empresas / Atividade no Endereço");
                    bc.ItemsSource = Usolocal;

                    _chart.Add(bc);
                }

            }

            return _chart;
        }

        public ObservableCollection<BarChart> Pessoas(mReportPessoas reports, bool periodo)
        {
            ObservableCollection<BarChart> _chart = new ObservableCollection<BarChart>();
            _chart.Clear();

            if (periodo)
            {
                List<KeyValuePair<string, int>> Sexo = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> Perfil = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> Negocio = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> Setor = new List<KeyValuePair<string, int>>();

                foreach (KeyValuePair<string, int> x in reports.Sexo)
                {
                    Sexo.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in reports.Perfil)
                {
                    Perfil.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in reports.Negocio)
                {
                    Negocio.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in reports.Setor)
                {
                    Setor.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                if (Perfil.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Periodo entre {0} e {1}", reports.Periodo[0], reports.Periodo[1]);
                    bc.ItemsSource = Perfil;

                    _chart.Add(bc);
                }

                if (Negocio.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Pessoas / Potencial Empreendedor");
                    bc.ItemsSource = Negocio;

                    _chart.Add(bc);
                }

                if (Sexo.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Pessoas / Sexo");
                    bc.ItemsSource = Sexo;

                    _chart.Add(bc);
                }

                if (Setor.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("Pessoas / Setor");
                    bc.ItemsSource = Setor;

                    _chart.Add(bc);
                }

            }

            return _chart;
        }
    }
}
