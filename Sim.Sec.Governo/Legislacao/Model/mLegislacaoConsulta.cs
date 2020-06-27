using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    class mLegislacaoConsulta : mLegislacaoBase
    {

        /// <summary>
        /// get or set acções exercidas em outros documentos
        /// </summary>
        private List<mAcoesConsulta> _listacoesexercidas;
        public List<mAcoesConsulta> ListaAcoesExercidas
        {
            get { return _listacoesexercidas; }
            set
            {
                _listacoesexercidas = value;
                RaisePropertyChanged("ListaAcoesExercidas");
            }
        }
        /// <summary>
        /// get or set ações recebidas de outros documentos
        /// </summary>
        private List<mAcoesConsulta> _listacoesrecebidas;
        public List<mAcoesConsulta> ListaAcoesRecebidas
        {
            get { return _listacoesrecebidas; }
            set
            {
                _listacoesrecebidas = value;
                RaisePropertyChanged("ListaAcoesRecebidas");
            }
        }
    }
}
