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

    class vmTipos : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;

        private string _novotipo;
                        
        private List<mTiposGenericos> _tipos = new List<mTiposGenericos>();

        private ICommand _commandblocktipo;
        private ICommand _commandinserttipo;
        #endregion

        #region Properties

        public bool  TextBoxEnabled
        {
            get { return _textboxenabled; }
            set { _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
            }
        }

        public string NovoTipo
        {
            get { return _novotipo; }
            set { _novotipo = value;
                RaisePropertyChanged("NovoTipo");
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

        #endregion

        #region Commands
        
        public ICommand CommandBlockTipo {
            get
            {
                if (_commandblocktipo == null)
                    _commandblocktipo = new DelegateCommand(ExecuteCommandBlockTipo, null);

                return _commandblocktipo;
            }
        }

        public ICommand CommandInsertTipo
        {
            get
            {
                if (_commandinserttipo == null)
                    _commandinserttipo = new DelegateCommand(ExecuteCommandInsertTipo, null);

                return _commandinserttipo;
            }
        }

        #endregion

        #region Start Instance

        public vmTipos()
        {
            Tipos = new mListaTiposGenericos().GoList(SqlCollections.Tipo_All);
            TextBoxEnabled = false;
        }

        #endregion

        #region Methods

        private void ExecuteCommandBlockTipo(object obj)
        {

            mTiposGenericos Tg = new mTiposGenericos();

            var objetos = (object[])obj;            
            var bloqueado = (CheckBox)objetos[1];

            Tg.Codigo = (int)objetos[0];
            Tg.Bloqueado = (bool)bloqueado.IsChecked;

            new mData().BlockNoBlock(SqlCollections.Tipo_Block_NoBlock, Tg);
        }

        private void ExecuteCommandInsertTipo(object obj)
        {
            mTiposGenericos Tgeneric = new mTiposGenericos();

            Tgeneric.Codigo = 0;
            Tgeneric.Nome = NovoTipo;
            Tgeneric.Cadastro = DateTime.Now;
            Tgeneric.Alterado = DateTime.Now;
            Tgeneric.Bloqueado = false;

            if (MessageBox.Show("Adicionar novo Tipo?", "Sim.Apps.Alerta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                if (new mData().InsertTiposGenericos(SqlCollections.Tipo_Inset, SqlCollections.Tipo_Last_Codigo, Tgeneric) == true)
                    Tipos = new mListaTiposGenericos().GoList(SqlCollections.Tipo_All);

            NovoTipo = string.Empty;
        }

        #endregion
    }
}
