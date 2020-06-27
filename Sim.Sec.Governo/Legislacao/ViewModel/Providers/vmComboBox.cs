using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.ViewModel.Providers
{

    using Model;
    using Sim.Mvvm.Observers;
    using SqlCommands;

    class vmComboBox : NotifyProperty
    {
        #region Declarations

        private string _tipo;
        private string _classificacao;
        private string _selectedorigem;
        private string _selectedautor;

        private List<mTiposGenericos> _tipos = new List<mTiposGenericos>();
        private List<mTiposGenericos> _classificacoes;
        private List<mTiposGenericos> _situacao = new List<mTiposGenericos>();
        private List<mTiposGenericos> _origem = new List<mTiposGenericos>();
        private List<mTiposGenericos> _autor;
        private List<mTiposGenericos> _acao = new List<mTiposGenericos>();

        #endregion

        #region Properties

        public string Classificacao
        {
            get { return _classificacao; }
            set
            {
                _classificacao = value;
                RaisePropertyChanged("Classificacao");
            }
        }

        public string Tipo { get { return _tipo; }
            set { _tipo = value;

                if (_tipo == "...")
                    this.Classificacoes = new mListaTiposGenericos().GoList(null);

                if (_tipo == "LEI" || _tipo == "LEI COMPLEMENTAR")
                    this.Classificacoes = new mListaTiposGenericos().GoList(SqlCollections.Class_L_Only_Non_Blocked);

                if (_tipo == "DECRETO")
                    this.Classificacoes = new mListaTiposGenericos().GoList(SqlCollections.Class_D_Only_Non_Blocked);

                Classificacao = "...";
                RaisePropertyChanged("Tipo");
            }
        }

        public string SelectedOrigem
        {
            get { return _selectedorigem; }
            set
            {
                _selectedorigem = value;

                if (_selectedorigem == "..." || _selectedorigem.ToLower() == "executivo")
                    Autor = new mListaTiposGenericos().GoList(null);

                if (_selectedorigem.ToLower() == "legislativo")
                    Autor = new mListaTiposGenericos().GoList(SqlCollections.Autor_Only_Non_Blocked);

                SelectedAutor = "...";
                RaisePropertyChanged("SelectedOrigem");
            }
        }

        public string SelectedAutor
        {
            get { return _selectedautor; }
            set
            {
                _selectedautor = value;
                RaisePropertyChanged("SelectedAutor");
            }
        }

        public List<mTiposGenericos> Tipos
        {
            get { return _tipos; }
            set
            {
                _tipos = value;
                RaisePropertyChanged("Tipos");
            }
        }

        public List<mTiposGenericos> Classificacoes
        {
            get { return _classificacoes; }
            set
            {
                _classificacoes = new List<mTiposGenericos>();
                _classificacoes = value;
                RaisePropertyChanged("Classificacoes");
            }
        }

        public List<mTiposGenericos> Origem
        {
            get { return _origem; }
            set
            {
                _origem = value;
                RaisePropertyChanged("Origem");
            }
        }

        public List<mTiposGenericos> Situacao
        {
            get { return _situacao; }
            set
            {
                _situacao = value;
                RaisePropertyChanged("Situacao");
            }
        }

        public List<mTiposGenericos> Acao
        {
            get { return _acao; }
            set
            {
                _acao = value;
                RaisePropertyChanged("Acao");
            }
        }

        public List<mTiposGenericos> Autor
        {
            get { return _autor; }
            set
            {
                _autor = new List<mTiposGenericos>();
                _autor = value;
                RaisePropertyChanged("Autor");
            }
        }

        #endregion

        public vmComboBox()
        {
            this.Tipos = new mListaTiposGenericos().GoList(SqlCollections.Tipo_Only_Non_Blocked);
            this.Origem = new mListaTiposGenericos().GoList(SqlCollections.Origem_Only_Non_Blocked);
            this.Situacao = new mListaTiposGenericos().GoList(SqlCollections.Situacao_Only_Non_Blocked);
            this.Acao = new mListaTiposGenericos().GoList(SqlCollections.Acao_Tipos_Only_Non_Blocked);
            this.Autor = new mListaTiposGenericos().GoList(SqlCollections.Autor_Only_Non_Blocked);
        }
        
    }
}
