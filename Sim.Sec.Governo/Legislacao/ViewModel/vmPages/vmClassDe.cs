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

    class vmClassDe : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;

        private string _novaclassd;

        private List<mTiposGenericos> _classd = new List<mTiposGenericos>();

        private ICommand _commandblockclassificacaod;
        private ICommand _commandinsertclassd;
        #endregion

        #region Properties

        public bool  TextBoxEnabled
        {
            get { return _textboxenabled; }
            set { _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
            }
        }

        public string NovaClassD
        {
            get { return _novaclassd; }
            set { _novaclassd = value;
                RaisePropertyChanged("NovaClassD");
            }
        }

        public List<mTiposGenericos> ClassificacoesD
        {
            get { return _classd; }
            set
            {
                _classd = value;
                RaisePropertyChanged("ClassificacoesD");
            }
        }

        #endregion

        #region Commands

        public ICommand CommandBlockClassificacaoD
        {
            get
            {
                if (_commandblockclassificacaod == null)
                    _commandblockclassificacaod = new DelegateCommand(ExecuteCommandBlockClassificacaoD, null);

                return _commandblockclassificacaod;
            }
        }

        public ICommand CommandInsertClassD
        {
            get
            {
                if (_commandinsertclassd == null)
                    _commandinsertclassd = new DelegateCommand(ExecuteCommandInsertClassD, null);

                return _commandinsertclassd;
            }
        }

        #endregion

        #region Start Instance

        public vmClassDe()
        {
            ClassificacoesD = new mListaTiposGenericos().GoList(SqlCollections.Class_D_All);
            TextBoxEnabled = false;
        }

        #endregion

        #region Methods

        private void ExecuteCommandBlockClassificacaoD(object obj)
        {
            mTiposGenericos Tg = new mTiposGenericos();

            var objetos = (object[])obj;
            var bloqueado = (CheckBox)objetos[1];

            Tg.Codigo = (int)objetos[0];
            Tg.Bloqueado = (bool)bloqueado.IsChecked;

            new mData().BlockNoBlock(SqlCollections.Class_D_Block_NoBlock, Tg);
        }

        private void ExecuteCommandInsertClassD(object obj)
        {
            mTiposGenericos Tgeneric = new mTiposGenericos();

            Tgeneric.Codigo = 0;
            Tgeneric.Nome = NovaClassD;
            Tgeneric.Cadastro = DateTime.Now;
            Tgeneric.Alterado = DateTime.Now;
            Tgeneric.Bloqueado = false;

            if (MessageBox.Show("Adicionar nova Classificação?", "Sim.Apps.Alerta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                if (new mData().InsertTiposGenericos(SqlCollections.Class_D_Insert, SqlCollections.Class_D_Last_Codigo, Tgeneric) == true)
                    ClassificacoesD = new mListaTiposGenericos().GoList(SqlCollections.Class_D_All);

            NovaClassD = string.Empty;
        }

        #endregion
    }
}
