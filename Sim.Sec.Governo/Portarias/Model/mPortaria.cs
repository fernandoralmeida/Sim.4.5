using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Portarias.Model
{

    using Mvvm.Observers;

    public class mPortaria : NotifyProperty
    {
        private int _indice;
        public int Indice
        {
            get { return _indice; }
            set
            {
                _indice = value;
                RaisePropertyChanged("Indice");
            }
        }

        private int _numero;
        public int Numero
        {
            get { return _numero; }
            set
            {
                _numero = value;
                RaisePropertyChanged("Numero");
            }
        }

        private DateTime _data;
        public DateTime Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged("Data");
            }
        }

        private string _classificacao;
        public string Tipo
        {
            get { return _classificacao; }
            set
            {
                _classificacao = value;
                RaisePropertyChanged("Tipo");
            }
        }

        private string _resumo;
        public string Resumo
        {
            get { return _resumo; }
            set
            {
                _resumo = value;
                RaisePropertyChanged("Resumo");
            }
        }

        private ObservableCollection<mServidor> _servidor = new ObservableCollection<mServidor>();
        public ObservableCollection<mServidor> Servidor
        {
            get { return _servidor; }
            set
            {
                _servidor = value;
                RaisePropertyChanged("Servidor");
            }
        }

        private string _pdf;
        public string Pdf
        {
            get { return _pdf; }
            set
            {
                _pdf = value;
                RaisePropertyChanged("Pdf");
            }
        }

        private string _publicada;
        public string Publicada
        {
            get { return _publicada; }
            set
            {
                _publicada = value;
                RaisePropertyChanged("Publicada");
            }
        }

        private DateTime _cadastrado;
        public DateTime Cadastrado
        {
            get { return _cadastrado; }
            set
            {
                _cadastrado = value;
                RaisePropertyChanged("Cadastrado");
            }
        }

        private DateTime _atualizado;
        public DateTime Atualizado
        {
            get { return _atualizado; }
            set
            {
                _atualizado = value;
                RaisePropertyChanged("Atualizado");
            }
        }

        private bool _excluido;
        public bool Excluido
        {
            get { return _excluido; }
            set
            {
                _excluido = value;
                RaisePropertyChanged("Excluido");
            }
        }
        
        public string IndiceLink { get; set; }

        public void ResetValues()
        {
            Indice = 0;
            Numero = 0;
            Data = DateTime.Now;
            Tipo = "0";
            Resumo = string.Empty;
            Pdf = string.Empty;
            Publicada = string.Empty;
            Cadastrado = DateTime.Now;
            Atualizado = DateTime.Now;
            Servidor.Clear();
            Excluido = false;
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.Indice.ToString());
            sb.AppendLine(this.Numero.ToString());
            sb.AppendLine(this.Data.ToShortDateString());
            sb.AppendLine(this.Tipo);
            sb.AppendLine(this.Resumo);
            sb.AppendLine(this.Pdf);
            sb.AppendLine(this.Publicada);
            sb.AppendLine(this.Cadastrado.ToShortDateString());
            sb.AppendLine(this.Atualizado.ToShortDateString());
            sb.AppendLine(this.Excluido.ToString());
            sb.AppendLine("[Servidor(es)]");

            if (Servidor != null)
            {

                foreach (mServidor ac in Servidor)
                {
                    sb.AppendLine("-> " + ac.Nome);
                }
            }
            return sb.ToString();
        }

    }
}
