using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Legislacao.ViewModel.vmPages
{

    using Sim.Mvvm.Commands;
    using Sim.Mvvm.Observers;
    using Model;
    using SqlCommands;

    class vmAcoes : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;
        private string _novaacao;

        private List<mTiposGenericos> _acao = new List<mTiposGenericos>();

        private ICommand _commandinsertacao;
        private ICommand _commandblockacao;

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

        public string NovaAcao
        {
            get { return _novaacao; }
            set
            {
                _novaacao = value;
                RaisePropertyChanged("NovaAcao");
            }
        }

        public List<mTiposGenericos> Acoes
        {
            get { return _acao; }
            set
            {
                _acao = value;
                RaisePropertyChanged("Acoes");
            }
        }

        #endregion

        #region Commands

        public ICommand CommandBlockAcao
        {
            get
            {
                if (_commandblockacao == null)
                    _commandblockacao = new DelegateCommand(ExecuteCommandBlockAcao, null);

                return _commandblockacao;
            }
        }

        public ICommand CommandInsertAcao
        {
            get
            {
                if (_commandinsertacao == null)
                    _commandinsertacao = new DelegateCommand(ExecuteCommandInsertAcao, null);

                return _commandinsertacao;
            }
        }

        #endregion

        #region Methods

        private void ExecuteCommandBlockAcao(object obj)
        {
            mTiposGenericos Tg = new mTiposGenericos();

            var objetos = (object[])obj;
            var bloqueado = (CheckBox)objetos[1];

            Tg.Codigo = (int)objetos[0];
            Tg.Bloqueado = (bool)bloqueado.IsChecked;

            new mData().BlockNoBlock(SqlCollections.Acao_Tipos_Block_NoBlock, Tg);
        }

        private void ExecuteCommandInsertAcao(object obj)
        {
            mTiposGenericos Tgeneric = new mTiposGenericos();

            Tgeneric.Codigo = 0;
            Tgeneric.Nome = NovaAcao;
            Tgeneric.Cadastro = DateTime.Now;
            Tgeneric.Alterado = DateTime.Now;
            Tgeneric.Bloqueado = false;

            if (MessageBox.Show("Adicionar nova Ação?", "Sim.Apps.Alerta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                if (new mData().InsertTiposGenericos(SqlCollections.Acao_Insert, SqlCollections.Acao_Last_Codigo, Tgeneric) == true)
                    Acoes = new mListaTiposGenericos().GoList(SqlCollections.Acao_All);

            NovaAcao = string.Empty;
        }

        #endregion

        #region Constructor

        public vmAcoes()
        {
            Acoes = new mListaTiposGenericos().GoList(SqlCollections.Acao_All);
            TextBoxEnabled = false;
        }

        #endregion
    }
}
