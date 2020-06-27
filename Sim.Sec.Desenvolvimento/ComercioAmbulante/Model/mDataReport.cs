using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Model
{
    public class mDataReport
    {
        public mReportCA ComercioAmbulante(ObservableCollection<mAmbulante> obj)
        {
            try
            {

                mReportCA _rpt = new mReportCA();

                List<string> _ambulante = new List<string>();
                List<string> _atividade = new List<string>();
                List<string> _local = new List<string>();
                List<string> _tempoatividade = new List<string>();
                List<string> _instalacao = new List<string>();
                List<string> _periodos = new List<string>();
                List<string> _situacao = new List<string>();

                foreach (mAmbulante ab in obj)
                {

                    

                    if (ab.Empresa.Inscricao != string.Empty)
                        _ambulante.Add("PJ");
                    else
                        _ambulante.Add("PF");

                    //_ambulante.Add("TOTAL");

                    _atividade.Add(ab.Atividades.TrimEnd());

                    _tempoatividade.Add(ab.TempoAtividade.ToString().TrimEnd());

                    if (ab.Local == "")
                        ab.Local = "NÃO INFORMADO";

                    _local.Add(ab.Local.TrimEnd());


                    string[] _twk = ab.PeridoTrabalho.Split('/');

                    foreach (string wk in _twk)
                    {
                        if (wk != string.Empty)
                        {
                            _periodos.Add(wk);
                            //string[] iwk = wk.Split('-');
                            //_dias.Add(iwk[0].TrimEnd());
                            //_periodos.Add(iwk[1].TrimStart());
                        }
                    }

                    _instalacao.Add(ab.TipoInstalacoes);

                    switch (ab.Situacao)
                    {
                        case 0:
                            _situacao.Add("...");
                            break;

                        case 1:
                            _situacao.Add("EM ANÁLISE");
                            break;

                        case 2:
                            _situacao.Add("DEFERIDO");
                            break;

                        case 3:
                            _situacao.Add("INDEFERIDO");
                            break;
                    }
                }

                var c_ambulante = from x in _ambulante
                                    group x by x into g
                                    let count = g.Count()
                                    orderby count descending
                                    select new { Value = g.Key, Count = count };

                foreach (var x in c_ambulante)
                {
                    _rpt.Ambulante.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_atividade = from x in _atividade
                           group x by x into g
                           let count = g.Count()
                           orderby count descending
                           select new { Value = g.Key, Count = count };

                foreach (var x in c_atividade)
                {
                    _rpt.Atividade.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_local = from x in _local
                                  group x by x into g
                                  let count = g.Count()
                                  orderby count descending
                                  select new { Value = g.Key, Count = count };

                foreach (var x in c_local)
                {
                    _rpt.Local.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_dias = from x in _instalacao
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in c_dias)
                {
                    _rpt.Instalacoes.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_periodos = from x in _periodos
                             group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in c_periodos)
                {
                    _rpt.Periodos.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_situacao = from x in _situacao
                                 group x by x into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count };

                foreach (var x in c_situacao)
                {
                    _rpt.Situacao.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_tempoatividade = from x in _tempoatividade
                                 group x by x into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count };

                foreach (var x in c_tempoatividade)
                {
                    _rpt.TempoAtividade.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                return _rpt;
            }
            catch(Exception ex)
            {                
                return null;
                throw new Exception(ex.Message);
            }
        }
    }
}
