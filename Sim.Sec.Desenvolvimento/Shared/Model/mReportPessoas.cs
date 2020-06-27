using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    public class mReportPessoas
    {
        public List<string> Periodo { get; set; }
        public List<KeyValuePair<string, int>> Sexo { get; set; }
        public List<KeyValuePair<string, int>> Setor { get; set; }
        public List<KeyValuePair<string, int>> Perfil { get; set; }
        public List<KeyValuePair<string, int>> Negocio { get; set; }

        public void Clear()
        {
            Periodo.Clear();
            Sexo.Clear();
            Setor.Clear();
            Perfil.Clear();
            Negocio.Clear();
        }

        public mReportPessoas()
        {
            Periodo = new List<string>();
            Sexo = new List<KeyValuePair<string, int>>();
            Setor = new List<KeyValuePair<string, int>>();
            Perfil = new List<KeyValuePair<string, int>>();
            Negocio = new List<KeyValuePair<string, int>>();
        }

        public void Show(ObservableCollection<mPF_Ext> lista_pessoas)
        {
            try
            {

                List<string> _sexo = new List<string>();
                List<string> _setor = new List<string>();
                List<string> _perfil = new List<string>();
                List<string> _negocio = new List<string>();

                foreach (mPF_Ext at in lista_pessoas)
                {
                    switch (at.Sexo)
                    {
                        case 1:
                            _sexo.Add("FEMININO");
                            break;
                        case 2:
                            _sexo.Add("MASCULINO");
                            break;
                    }

                    _setor.Add(at.Bairro);

                    _perfil.Add(at.Perfil.PerfilString);

                    if (at.Perfil.Perfil == 1)
                    {
                        if (at.Perfil.Negocio.ToString()== "True")
                            _negocio.Add("COM NEGÓCIO");
                        else
                            _negocio.Add("SEM NEGÓCIO");
                    }


                }

                var c_sexo = from x in _sexo
                                  group x by x into g
                                  let count = g.Count()
                                  orderby count descending
                                  select new { Value = g.Key, Count = count };

                foreach (var x in c_sexo)
                {
                    Sexo.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_setor = from x in _setor
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in c_setor)
                {
                    Setor.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_negocio = from x in _negocio
                                 group x by x into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count };

                foreach (var x in c_negocio)
                {
                    Negocio.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

                var c_perfil = from x in _perfil
                              group x by x into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count };

                foreach (var x in c_perfil)
                {
                    Perfil.Add(new KeyValuePair<string, int>(x.Value, x.Count));
                }

            }
            catch { }
        }
    }
}
