using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Model
{
    public class mReportCA
    {
        #region Declarations
        private List<KeyValuePair<string, int>> _ambulante = new List<KeyValuePair<string, int>>();
        private List<KeyValuePair<string, int>> _atividade = new List<KeyValuePair<string, int>>();
        private List<KeyValuePair<string, int>> _local = new List<KeyValuePair<string, int>>();
        private List<KeyValuePair<string, int>> _instalacoes = new List<KeyValuePair<string, int>>();
        private List<KeyValuePair<string, int>> _periodos = new List<KeyValuePair<string, int>>();
        private List<KeyValuePair<string, int>> _situacao = new List<KeyValuePair<string, int>>();
        private List<KeyValuePair<string, int>> _tempoatividade = new List<KeyValuePair<string, int>>();
        #endregion

        #region Properties
        public List<KeyValuePair<string, int>> Ambulante { get { return _ambulante; } set { _ambulante = value; } }
        public List<KeyValuePair<string, int>> Atividade { get { return _atividade; } set { _atividade = value; } }
        public List<KeyValuePair<string, int>> Local { get { return _local; } set { _local = value; } }
        public List<KeyValuePair<string, int>> Instalacoes { get { return _instalacoes; } set { _instalacoes = value; } }
        public List<KeyValuePair<string, int>> Periodos { get { return _periodos; } set { _periodos = value; } }
        public List<KeyValuePair<string, int>> Situacao { get { return _situacao; } set { _situacao = value; } }
        public List<KeyValuePair<string, int>> TempoAtividade { get { return _tempoatividade; } set { _tempoatividade = value; } }
        #endregion

        #region MyRegion
        public void Clear()
        {
            Ambulante.Clear();
            Atividade.Clear();
            Local.Clear();
            Instalacoes.Clear();
            Periodos.Clear();
            Situacao.Clear();
            TempoAtividade.Clear();
        }
        #endregion
    }
}
