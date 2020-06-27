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

    class vmSituacao : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;

        private string _novasituacao;

        private List<mTiposGenericos> _situacao = new List<mTiposGenericos>();

        private ICommand _commandblocksituacao;
        private ICommand _commandinsertsituacao;
        #endregion

        #region Properties

        public bool  TextBoxEnabled
        {
            get { return _textboxenabled; }
            set { _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
            }
        }

        public string NovaSituacao
        {
            get { return _novasituacao; }
            set
            {
                _novasituacao = value;
                RaisePropertyChanged("NovaSituacao");
            }
        }
  
        public List<mTiposGenericos> Situacoes
        {
            get { return _situacao; }
            set
            {
                _situacao = value;
                RaisePropertyChanged("Situacoes");
            }
        }

        #endregion

        #region Commands

        public ICommand CommandBlockSituacao
        {
            get
            {
                if (_commandblocksituacao == null)
                    _commandblocksituacao = new DelegateCommand(ExecuteCommandBlockSituacao, null);

                return _commandblocksituacao;
            }
        }

        public ICommand CommandInsertSituacao
        {
            get
            {
                if (_commandinsertsituacao == null)
                    _commandinsertsituacao = new DelegateCommand(ExecuteCommandInsertSituacao, null);

                return _commandinsertsituacao;
            }
        }

        #endregion

        #region Start Instance

        public vmSituacao()
        {
            Situacoes = new mListaTiposGenericos().GoList(SqlCollections.Situacao_All);
            TextBoxEnabled = false;
        }

        #endregion

        #region Methods

        private void ExecuteCommandBlockSituacao(object obj)
        {
            mTiposGenericos Tg = new mTiposGenericos();

            var objetos = (object[])obj;
            var bloqueado = (CheckBox)objetos[1];

            Tg.Codigo = (int)objetos[0];
            Tg.Bloqueado = (bool)bloqueado.IsChecked;

            new mData().BlockNoBlock(SqlCollections.Situacao_Block_NoBlock, Tg);
        }

        private void ExecuteCommandInsertSituacao(object obj)
        {
            mTiposGenericos Tgeneric = new mTiposGenericos();

            Tgeneric.Codigo = 0;
            Tgeneric.Nome = NovaSituacao;
            Tgeneric.Cadastro = DateTime.Now;
            Tgeneric.Alterado = DateTime.Now;
            Tgeneric.Bloqueado = false;

            if (MessageBox.Show("Adicionar nova Situação?", "Sim.Apps.Alerta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                if (new mData().InsertTiposGenericos(SqlCollections.Situacao_Insert, SqlCollections.Situacao_Last_Codigo, Tgeneric) == true)
                    Situacoes = new mListaTiposGenericos().GoList(SqlCollections.Situacao_All);

            NovaSituacao = string.Empty;
        }

        #endregion
    }
}
