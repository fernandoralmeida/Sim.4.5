using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    class mLegislacao : mLegislacaoBase
    {
        private List<mAcoes> _listacoes;
        public List<mAcoes> ListaAcoes
        {
            get { return _listacoes; }
            set
            {
                _listacoes = value;
                RaisePropertyChanged("ListaAcoes");
            }
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.Indice.ToString());
            sb.AppendLine(this.Tipo);
            sb.AppendLine(this.Numero.ToString());
            sb.AppendLine(this.Complemento);
            sb.AppendLine(this.Data.ToShortDateString());
            sb.AppendLine(this.Publicado);
            sb.AppendLine(this.Resumo);
            sb.AppendLine(this.Classificacao);
            sb.AppendLine(this.Link);
            sb.AppendLine(this.Situacao);
            sb.AppendLine(this.Origem);
            sb.AppendLine(this.Autor);
            sb.AppendLine(this.Cadastro.ToShortDateString());
            sb.AppendLine(this.Atualizado.ToShortDateString());
            sb.AppendLine(this.Excluido.ToString());
            sb.AppendLine("[Lista Ações]");

            if (ListaAcoes != null)
            {

                foreach (mAcoes ac in ListaAcoes)
                {
                    string sbl = string.Format("-> {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}",
                        ac.Indice,
                        ac.TipoOrigem,
                        ac.NumeroOrigem,
                        ac.ComplementoOrigem,
                        ac.DataOrigem.ToShortDateString(),
                        ac.Acao,
                        ac.TipoAlvo,
                        ac.NumeroAlvo,
                        ac.ComplementoAlvo,
                        ac.DataAlvo.ToShortDateString(),
                        ac.Incluido.ToShortDateString());

                    sb.AppendLine(sbl);
                }
            }
            return sb.ToString();
        }
    }
}
