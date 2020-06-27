using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace Sim.Sec.Governo.Portarias.ViewModel.Pages
{
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Model;
    using Sql;

    class vmClassf : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;
        
        private string _novaclass;
        
        private List<mClassificacao> _class = new List<mClassificacao>();
        
        private ICommand _commandblockclassificacao;
        private ICommand _commandinsertclassificacao;
        #endregion

        #region Properties

        public bool TextBoxEnabled
        {
            get { return _textboxenabled; }
            set
            {
                _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
            }
        }
        
        public string NovaClassificacao
        {
            get { return _novaclass; }
            set
            {
                _novaclass = value;
                RaisePropertyChanged("NovaClassificacao");
            }
        }

        public List<mClassificacao> Classificacoes
        {
            get { return _class; }
            set
            {
                _class = value;
                RaisePropertyChanged("Classificacoes");
            }
        }
        
        #endregion

        #region Commands

        public ICommand CommandBlockClassificacao
        {
            get
            {
                if (_commandblockclassificacao == null)
                    _commandblockclassificacao = new DelegateCommand(ExecuteCommandBlockClassificacao, null);

                return _commandblockclassificacao;
            }
        }
        
        public ICommand CommandInsertClassificacao
        {
            get
            {
                if (_commandinsertclassificacao == null)
                    _commandinsertclassificacao = new DelegateCommand(ExecuteCommandInsertClassificacao, null);

                return _commandinsertclassificacao;
            }
        }

        #endregion

        #region Start Instance

        public vmClassf()
        {
            Classificacoes = new mData().ListaGenerica(SqlCollections.Classi_With_Blocked); 
            TextBoxEnabled = false;        
        }

        #endregion

        #region Methods
        
        private void ExecuteCommandBlockClassificacao(object obj)
        {
            mClassificacao generic = new mClassificacao();

            var objetos = (object[])obj;
            var bloqueado = (CheckBox)objetos[1];

            generic.Codigo = (int)objetos[0];
            generic.Bloqueado = (bool)bloqueado.IsChecked;

            new mData().BlockNoBlockClassificacao(SqlCollections.Classi_Block_Non_Block, generic);
        }

        private void ExecuteCommandInsertClassificacao(object obj)
        {
            mClassificacao generic = new mClassificacao();

            generic.Codigo = 0;
            generic.Nome = NovaClassificacao;
            generic.Cadastro = DateTime.Now;
            generic.Alterado = DateTime.Now;
            generic.Bloqueado = false;

            if (MessageBox.Show("Adicionar nova Classificação?", "Sim.Apps.Alerta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                if (new mData().InsertClassificacao(SqlCollections.Insert_Classi, SqlCollections.Classi_Last_Row, generic) == true)
                    Classificacoes = new mData().ListaGenerica(SqlCollections.Classi_With_Blocked);

            NovaClassificacao = string.Empty;
        }

        #endregion
    }
}
