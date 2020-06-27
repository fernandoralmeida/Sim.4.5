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

    class vmClassLoLc : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;

        private string _novaclassl;
                        
        private List<mTiposGenericos> _classl = new List<mTiposGenericos>();

        private ICommand _commandblockclassificacaol;
        private ICommand _commandinsertclassl;
        #endregion

        #region Properties

        public bool  TextBoxEnabled
        {
            get { return _textboxenabled; }
            set { _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
            }
        }

        public string NovaClassL
        {
            get { return _novaclassl; }
            set
            {
                _novaclassl = value;
                RaisePropertyChanged("NovaClassL");
            }
        }

        public List<mTiposGenericos> ClassificacoesL
        {
            get { return _classl; }
            set
            {
                _classl = value;
                RaisePropertyChanged("ClassificacoesL");
            }
        }

        #endregion

        #region Commands

        public ICommand CommandBlockClassificacaoL
        {
            get
            {
                if (_commandblockclassificacaol == null)
                    _commandblockclassificacaol = new DelegateCommand(ExecuteCommandBlockClassificacaoL, null);

                return _commandblockclassificacaol;
            }
        }

        public ICommand CommandInsertClassL
        {
            get
            {
                if (_commandinsertclassl == null)
                    _commandinsertclassl = new DelegateCommand(ExecuteCommandInsertClassL, null);

                return _commandinsertclassl;
            }
        }

        #endregion

        #region Start Instance

        public vmClassLoLc()
        {
            ClassificacoesL = new mListaTiposGenericos().GoList(SqlCollections.Class_L_All);

            TextBoxEnabled = false;
        }

        #endregion

        #region Methods
        
        private void ExecuteCommandBlockClassificacaoL(object obj)
        {
            mTiposGenericos Tg = new mTiposGenericos();

            var objetos = (object[])obj;
            var bloqueado = (CheckBox)objetos[1];

            Tg.Codigo = (int)objetos[0];
            Tg.Bloqueado = (bool)bloqueado.IsChecked;

            new mData().BlockNoBlock(SqlCollections.Class_L_Block_NoBlock, Tg);
        }

        private void ExecuteCommandInsertClassL(object obj)
        {
            mTiposGenericos Tgeneric = new mTiposGenericos();

            Tgeneric.Codigo = 0;
            Tgeneric.Nome = NovaClassL;
            Tgeneric.Cadastro = DateTime.Now;
            Tgeneric.Alterado = DateTime.Now;
            Tgeneric.Bloqueado = false;

            if (MessageBox.Show("Adicionar nova Classificação?", "Sim.Apps.Alerta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                if (new mData().InsertTiposGenericos(SqlCollections.Class_L_Insert, SqlCollections.Class_L_Last_Codigo, Tgeneric) == true)
                    ClassificacoesL = new mListaTiposGenericos().GoList(SqlCollections.Class_L_All);

            NovaClassL = string.Empty;
        }

        #endregion

    }
}
