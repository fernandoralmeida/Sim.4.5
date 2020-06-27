using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    using Mvvm.Observers;

    abstract class mLegislacaoBase : NotifyProperty
    {
        public void ResetValues()
        {
            this.Indice = 0;
            this.Tipo = "...";
            this.Numero = 0;
            this.Complemento = string.Empty;
            this.Data = DateTime.Now;
            this.Publicado = string.Empty;
            this.Resumo = string.Empty;
            this.Classificacao = "0";
            this.Link = string.Empty;
            this.Situacao = "0";
            this.Origem = "0";
            this.Autor = string.Empty;
            this.Cadastro = DateTime.Now;
            this.Atualizado = DateTime.Now;
            this.Excluido = false;
        }

        private int _indice;
        public int Indice { get { return _indice; }
            set
            {
                _indice = value;
                RaisePropertyChanged("Indice");
            }
        }

        private string _tipo;
        public string Tipo { get { return _tipo; }
            set
            {
                _tipo = value;
                RaisePropertyChanged("Tipo");
            }
        }

        private int _numero;
        public int Numero { get { return _numero; }
            set
            {
                _numero = value;
                RaisePropertyChanged("Numero");
            }
        }

        private string _complemento;
        public string Complemento { get { return _complemento; }
            set
            {
                _complemento = value;                
                RaisePropertyChanged("Complemento");
            }
        }

        private DateTime _data;
        public DateTime Data { get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged("Data");
            }
        }

        private string _publicado;
        public string Publicado { get { return _publicado; }
            set
            {
                _publicado = value;
                RaisePropertyChanged("Publicado");
            }
        }

        private string _resumo;
        public string Resumo { get { return _resumo; }
            set
            {
                _resumo = value;
                RaisePropertyChanged("Resumo");
            }
        }

        private string _classificacao;
        public string Classificacao { get { return _classificacao; } 
            set
            {
                _classificacao = value;
                RaisePropertyChanged("Classificacao");
            } 
        }

        private string _link;
        public string Link { get { return _link; }
            set
            {
                _link = value;
                RaisePropertyChanged("Link");
            }
        }

        private string _situação;
        public string Situacao { get{return _situação;}
            set
            {
                _situação = value;
                RaisePropertyChanged("Situacao");
            }
        }

        private string _origem;
        public string Origem { get { return _origem; }
            set
            {
                _origem = value;
                RaisePropertyChanged("Origem");
            }
        }

        private string _autor;
        public string Autor { get { return _autor; }
            set
            {
                _autor = value;
                RaisePropertyChanged("Autor");
            }
        }

        private DateTime _cadastro;
        public DateTime Cadastro { get { return _cadastro; }
            set
            {
                _cadastro = value;
                RaisePropertyChanged("Cadastro");
            }
        }

        private DateTime _atualizado;
        public DateTime Atualizado { get { return _atualizado; }
            set
            {
                _atualizado = value;
                RaisePropertyChanged("Atualizado");
            }
        }

        private bool _excluido;
        public bool Excluido { get { return _excluido; }
            set
            {
                _excluido = value;
                RaisePropertyChanged("Excluido");
            }
        }

    }
}
