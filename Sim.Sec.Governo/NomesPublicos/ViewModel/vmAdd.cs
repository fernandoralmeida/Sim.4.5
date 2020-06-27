using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Sim.Sec.Governo.NomesPublicos.ViewModel
{
    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;

    class vmAdd: NotifyProperty
    {
        #region Declarations
        private mObjeto _denomi = new mObjeto();

        private ICommand _commandsave;
        private ICommand _commandcancel;
        #endregion

        #region Properties
        public mObjeto Denominacao
        {
            get { return _denomi; }
            set
            {
                _denomi = value;
                RaisePropertyChanged("Denominacao");
            }
        }

        public ObservableCollection<string> Tipos
        {
            get { return new mData().Tipos(); }
        }

        public ObservableCollection<mEspecie> Especies
        {
            get { return new mData().Especies(); }
        }

        public ObservableCollection<string> Origens
        {
            get { return new mData().Origens(); }
        }
        #endregion

        #region Commands
        public ICommand CommandSave
        {
            get
            {
                if (_commandsave == null)
                    _commandsave = new DelegateCommand(ExecCommandSave, null);
                return _commandsave;
            }
        }

        private void ExecCommandSave(object obj)
        {
            try
            {
                Gravar(Denominacao);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        public ICommand CommandCancel
        {
            get
            {
                if (_commandcancel == null)
                    _commandcancel = new DelegateCommand(ExecCommandCancel, null);
                return _commandcancel;
            }
        }

        private void ExecCommandCancel(object obj)
        {
            try
            {
                Gravar(Denominacao);
            }
            catch(Exception ex) {
                System.Windows.MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }
        #endregion

        #region Constructor
        public vmAdd()
        {
            Denominacao.Cadastro = DateTime.Now;
            Denominacao.Atualizado = DateTime.Now;
            Denominacao.Ativo = true;
            GlobalNavigation.Pagina = "INCLUSÃO";
        }
        #endregion

        #region Functions

        private void Gravar(mObjeto obj)
        {
            try
            {
                var mdata = new mData();

                mdata.AddTipo(new mTipos() { Tipo = obj.Tipo, Ativo = true });

                if (mdata.AddDenominacao(Denominacao))
                {
                    System.Windows.MessageBox.Show("Registro incluído com sucesso!", "Sim.Aviso!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    Denominacao.Clear();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
