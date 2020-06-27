using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    public class mReportEmpresas
    {
        public List<string> Periodo { get; set; }
        public List<KeyValuePair<string, int>> Atividade { get; set; }
        public List<KeyValuePair<string, int>> Setor { get; set; }
        public List<KeyValuePair<string, int>> Porte { get; set; }
        public List<KeyValuePair<string, int>> UsoLocal { get; set; }

        public void Clear()
        {
            Periodo.Clear();
            Atividade.Clear();
            Setor.Clear();
            Porte.Clear();
            UsoLocal.Clear();
        }

        public mReportEmpresas()
        {
            Periodo = new List<string>();
            Atividade = new List<KeyValuePair<string, int>>();
            Setor = new List<KeyValuePair<string, int>>();
            Porte = new List<KeyValuePair<string, int>>();
            UsoLocal = new List<KeyValuePair<string, int>>();
        }

        public void Show(ObservableCollection<mPJ_Ext> lista_empresas, List<object> commands)
        {
            try
            {
                List<string> _periodo = new List<string>();
                List<string> _atividade = new List<string>();
                List<string> _setor = new List<string>();
                List<string> _porte = new List<string>();
                List<string> _usolocal = new List<string>();

                foreach (mPJ_Ext at in lista_empresas)
                {

                    _atividade.Add(at.AtividadePrincipal.ToUpper().TrimEnd());

                    if (at.Segmentos.Agronegocio == true)
                        _setor.Add("AGRONEGOCIO");
                    
                    if (at.Segmentos.Comercio == true)
                        _setor.Add("COMERCIO");

                    if (at.Segmentos.Industria == true)
                        _setor.Add("INDUSTRIA");

                    if (at.Segmentos.Servicos == true)
                        _setor.Add("SERVICOS");

                    if (at.Formalizada.Porte == 1)
                        _porte.Add("MEI");

                    if (at.Formalizada.Porte == 2)
                        _porte.Add("EIRELI");

                    if (at.Formalizada.Porte == 3)
                        _porte.Add("ME");

                    if (at.Formalizada.Porte == 4)
                        _porte.Add("EPP");

                    if (at.Formalizada.Porte == 5)
                        _porte.Add("OUTROS");

                    if (at.Formalizada.UsoLocal == 1)
                        _usolocal.Add("ESTABELECIMENTO");

                    if (at.Formalizada.UsoLocal == 2)
                        _usolocal.Add("SOMENTE PARA CORRESPONDENCIA");

                }


                var c_atividade = from x in _atividade
                                    group x by x into g
                                    let count = g.Count()
                                    orderby count descending
                                    select new { Value = g.Key, Count = count };

                foreach (var x in c_atividade)
                {
                    Atividade.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_setor = from x in _setor
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach(var x in c_setor)
                {
                    Setor.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_usolocal = from x in _usolocal
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in c_usolocal)
                {
                    UsoLocal.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_porte = from x in _porte
                                 group x by x into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count };

                foreach (var x in c_porte)
                {
                    Porte.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

            }
            catch { }
        }
    }

}
