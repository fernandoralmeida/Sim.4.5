using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    public class mReportAtendimentos
    {
        public List<string> Periodo { get; set; }
        public List<KeyValuePair<string, int>> Ano { get; set; }
        public List<KeyValuePair<string, int>> Atendimentos { get; set; }
        public List<KeyValuePair<string, int>> Tipo { get; set; }
        public List<KeyValuePair<string, int>> Origem { get; set; }
        public List<KeyValuePair<string, int>> Canal { get; set; }
        public List<KeyValuePair<string, int>> Operador { get; set; }

        public void Clear()
        {
            Periodo.Clear();
            Ano.Clear();
            Atendimentos.Clear();
            Tipo.Clear();
            Origem.Clear();
            Operador.Clear();
            Canal.Clear();
        }

        public mReportAtendimentos()
        {
            Periodo = new List<string>();
            Ano = new List<KeyValuePair<string, int>>();
            Atendimentos = new List<KeyValuePair<string, int>>();
            Tipo = new List<KeyValuePair<string, int>>();
            Origem = new List<KeyValuePair<string, int>>();
            Canal = new List<KeyValuePair<string, int>>();
            Operador = new List<KeyValuePair<string, int>>();
        }

        public void Show(ObservableCollection<mAtendimento> lista_atendimento)
        {
            try
            {
                List<string> _ano = new List<string>();
                List<string> _periodo = new List<string>();
                List<string> _atendimento = new List<string>();
                List<string> _tipo = new List<string>();
                List<string> _origem = new List<string>();
                List<string> _canal = new List<string>();
                List<string> _operador = new List<string>();

                foreach(mAtendimento at in lista_atendimento)
                {
                    _atendimento.Add("Atendimentos".ToUpper());

                    _ano.Add(at.Data.Year.ToString());

                    string[] words = at.TipoString.ToString().Split(';');

                    foreach (string sv in words)
                    {
                        if (sv != null && sv != string.Empty)
                            _tipo.Add(sv);
                    }

                    _canal.Add(at.Canal);
                    //_tipo.Add(at.TipoString);
                    _origem.Add(at.OrigemString);
                    _operador.Add(at.Operador);
                }
                               
                var c_atendimento = from x in _atendimento
                             group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in c_atendimento)
                {
                    Atendimentos.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_ano = from x in _ano
                            group x by x into g
                            let count = g.Count()
                            orderby count descending
                            select new { Value = g.Key, Count = count };

                foreach (var x in c_ano)
                {
                    Ano.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_tipo = from x in _tipo
                             group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in c_tipo)
                {
                    Tipo.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_origem = from x in _origem
                             group x by x into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count };

                foreach (var x in c_origem)
                {
                    Origem.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_operador = from x in _operador
                               group x by x into g
                               let count = g.Count()
                               orderby count descending
                               select new { Value = g.Key, Count = count };

                foreach (var x in c_operador)
                {
                    Operador.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_canal = from x in _canal
                                 group x by x into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count };

                foreach (var x in c_canal)
                {
                    Canal.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

            }
            catch { }
        }
    }
}
