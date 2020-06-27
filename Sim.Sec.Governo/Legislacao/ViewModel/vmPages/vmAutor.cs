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

    class vmAutor : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;

        private string _novoautor;
                        
        private List<mTiposGenericos> _autor = new List<mTiposGenericos>();

        private ICommand _commandblockautor;
        private ICommand _commandinsertautor;
        #endregion

        #region Properties


        public bool  TextBoxEnabled
        {
            get { return _textboxenabled; }
            set { _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
            }
        }

        public string NovoAutor
        {
            get { return _novoautor; }
            set
            {
                _novoautor = value;
                RaisePropertyChanged("NovoAutor");
            }
        }

        public List<mTiposGenericos> Autores
        {
            get { return _autor; }
            set
            {
                _autor = value;
                RaisePropertyChanged("Autores");
            }
        }

        #endregion

        #region Commands

        public ICommand CommandBlockAutor
        {
            get
            {
                if (_commandblockautor == null)
                    _commandblockautor = new DelegateCommand(ExecuteCommandBlockAutor, null);

                return _commandblockautor;
            }
        }

        public ICommand CommandInsertAutor
        {
            get
            {
                if (_commandinsertautor == null)
                    _commandinsertautor = new DelegateCommand(ExecuteCommandInsertAutor, null);

                return _commandinsertautor;
            }
        }

        #endregion

        #region Start Instance

        public vmAutor()
        {
            Autores = new mListaTiposGenericos().GoList(SqlCollections.Autor_All);
            TextBoxEnabled = false;
        }

        #endregion

        #region Methods

        private void ExecuteCommandBlockAutor(object obj)
        {
            mTiposGenericos Tg = new mTiposGenericos();

            var objetos = (object[])obj;
            var bloqueado = (CheckBox)objetos[1];

            Tg.Codigo = (int)objetos[0];
            Tg.Bloqueado = (bool)bloqueado.IsChecked;

            new mData().BlockNoBlock(SqlCollections.Autor_Block_NoBlock, Tg);
        }

        private void ExecuteCommandInsertAutor(object obj)
        {
            mTiposGenericos Tgeneric = new mTiposGenericos();

            Tgeneric.Codigo = 0;
            Tgeneric.Nome = NovoAutor;
            Tgeneric.Cadastro = DateTime.Now;
            Tgeneric.Alterado = DateTime.Now;
            Tgeneric.Bloqueado = false;

            if (MessageBox.Show("Adicionar novo Autor?", "Sim.Apps.Alerta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                if (new mData().InsertTiposGenericos(SqlCollections.Autor_Insert, SqlCollections.Autor_Last_Codigo, Tgeneric) == true)
                    Autores = new mListaTiposGenericos().GoList(SqlCollections.Autor_All);

            NovoAutor = string.Empty;
        }

        #endregion
    }
}
