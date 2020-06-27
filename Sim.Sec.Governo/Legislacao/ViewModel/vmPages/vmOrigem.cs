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

    class vmOrigem : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;

        private string _novaorigem;

        private List<mTiposGenericos> _origem = new List<mTiposGenericos>();

        private ICommand _commandblockorigem;
        private ICommand _commandinsertorigem;
        #endregion

        #region Properties

        public bool  TextBoxEnabled
        {
            get { return _textboxenabled; }
            set { _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
            }
        }

        public string NovaOrigem
        {
            get { return _novaorigem; }
            set
            {
                _novaorigem = value;
                RaisePropertyChanged("NovaOrigem");
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

        #endregion

        #region Commands

        public ICommand CommandBlockOrigem
        {
            get
            {
                if (_commandblockorigem == null)
                    _commandblockorigem = new DelegateCommand(ExecuteCommandBlockOrigem, null);

                return _commandblockorigem;
            }
        }
        
        public ICommand CommandInsertOrigem
        {
            get
            {
                if (_commandinsertorigem == null)
                    _commandinsertorigem = new DelegateCommand(ExecuteCommandInsertOrigem, null);

                return _commandinsertorigem;
            }
        }

        #endregion

        #region Start Instance

        public vmOrigem()
        {
            Origem = new mListaTiposGenericos().GoList(SqlCollections.Origem_All);
            TextBoxEnabled = false;
        }

        #endregion

        #region Methods
        
        private void ExecuteCommandBlockOrigem(object obj)
        {
            mTiposGenericos Tg = new mTiposGenericos();

            var objetos = (object[])obj;
            var bloqueado = (CheckBox)objetos[1];

            Tg.Codigo = (int)objetos[0];
            Tg.Bloqueado = (bool)bloqueado.IsChecked;

            new mData().BlockNoBlock(SqlCollections.Origem_Block_NoBlock, Tg);
        }
        
        private void ExecuteCommandInsertOrigem(object obj)
        {
            mTiposGenericos Tgeneric = new mTiposGenericos();

            Tgeneric.Codigo = 0;
            Tgeneric.Nome = NovaOrigem;
            Tgeneric.Cadastro = DateTime.Now;
            Tgeneric.Alterado = DateTime.Now;
            Tgeneric.Bloqueado = false;

            if (MessageBox.Show("Adicionar nova Origem?", "Sim.Apps.Alerta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                if (new mData().InsertTiposGenericos(SqlCollections.Origem_Insert, SqlCollections.Origem_Last_Codigo, Tgeneric) == true)
                    Origem = new mListaTiposGenericos().GoList(SqlCollections.Origem_All);

            NovaOrigem = string.Empty;
        }

        #endregion
    }
}
