using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    public class mReportViabilidade
    {
        public List<string> Periodo { get; set; }
        public List<KeyValuePair<string, int>> Viabilidade { get; set; }
        public List<KeyValuePair<string, int>> Bairro { get; set; }
        public List<KeyValuePair<string, int>> Situacao { get; set; }
        public List<KeyValuePair<string, int>> Atividades { get; set; }

        public void Clear()
        {
            Periodo.Clear();
            Viabilidade.Clear();
            Bairro.Clear();
            Situacao.Clear();
            Atividades.Clear();
        }

        public mReportViabilidade()
        {
            Periodo = new List<string>();
            Viabilidade = new List<KeyValuePair<string, int>>();
            Bairro = new List<KeyValuePair<string, int>>();
            Situacao = new List<KeyValuePair<string, int>>();
            Atividades = new List<KeyValuePair<string, int>>();
        }

        public void Show(ObservableCollection<mViabilidade> lista_viabilidade)
        {
            try
            {
                List<string> _periodo = new List<string>();
                List<string> _viabilidade = new List<string>();
                List<string> _bairro = new List<string>();
                List<string> _situacao = new List<string>();
                List<string> _atividades = new List<string>();

                foreach (mViabilidade at in lista_viabilidade)
                {

                    _viabilidade.Add("VIABILIDADE");

                    _bairro.Add(at.Bairro);

                    switch(at.Perecer)
                    {
                        case 1:
                            _situacao.Add("EM ANÁLISE");
                            break;
                        case 2:
                            _situacao.Add("DEFERIDO");
                            break;
                        case 3:
                            _situacao.Add("INDEFERIDO");
                            break;
                        case 4:
                            _situacao.Add("EIV");
                            break;
                        case 5:
                            _situacao.Add("CANCELADO");
                            break;
                    }

                    _atividades.Add(at.Atividades);

                }


                var c_viabilidade = from x in _viabilidade
                                  group x by x into g
                                  let count = g.Count()
                                  orderby count descending
                                  select new { Value = g.Key, Count = count };

                foreach (var x in c_viabilidade)
                {
                    Viabilidade.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_bairro = from x in _bairro
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in c_bairro)
                {
                    Bairro.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_atividades = from x in _atividades
                                 group x by x into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count };

                foreach (var x in c_atividades)
                {
                    Atividades.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_situacao = from x in _situacao
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in c_situacao)
                {
                    Situacao.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

            }
            catch { }
        }
    }
}
