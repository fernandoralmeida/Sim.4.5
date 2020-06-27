using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Portarias.ViewModel
{

    using Model;
    using Mvvm.Observers;
    using Mvvm.Commands;

    class vmEdit : NotifyProperty
    {
        #region Declarações

        private mData mdata = new mData();
        private mPortaria _portaria = new mPortaria();

        private string _names;
        private int _selectedrow;

        private ICommand _commandsave;
        private ICommand _commandcancel;
        private ICommand _commandincname;
        private ICommand _commandexcname;

        #endregion

        #region Propriedades

        public Window WindowParent { get; set; }

        public mPortaria Portaria
        {
            get { return _portaria; }
            set
            {
                _portaria = value;
                RaisePropertyChanged("Portaria");
            }
        }

        public string Names
        {
            get { return _names; }
            set
            {
                _names = value;
                RaisePropertyChanged("Names");
            }
        }

        public int SelectedRow
        {
            get { return _selectedrow; }
            set
            {
                _selectedrow = value;
                RaisePropertyChanged("SelectedRow");
            }
        }

        #endregion

        #region Commandos

        public ICommand CommandSave
        {
            get
            {
                if (_commandsave == null)
                    _commandsave = new DelegateCommand(ExecuteCommandSave, null);
                return _commandsave;
            }
        }

        private void ExecuteCommandSave(object obj)
        {
            bool gravado = false;
            try
            {
                Portaria.Atualizado = DateTime.Now;
                gravado = mdata.Update(Portaria);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Apps.Alerta!");
            }
            finally
            {
                if (gravado == true)
                    NavigationCommands.BrowseBack.Execute(null, null);
            }
        }

        public ICommand CommandCancel
        {
            get { if (_commandcancel == null)
                    _commandcancel = new DelegateCommand(ExecuteCommandCancel, null);
                return _commandcancel;
            }
        }

        private void ExecuteCommandCancel(object obj)
        {
            NavigationCommands.BrowseBack.Execute(null, null);
        }

        public ICommand CommandIncName
        {
            get
            {
                if (_commandincname == null)
                    _commandincname = new DelegateCommand(ExecuteCommandIncName, null);
                return _commandincname;
            }
        }

        private void ExecuteCommandIncName(object obj)
        {
            Portaria.Servidor.Add(new mServidor() { Nome = Names });
            Names = string.Empty;
        }

        public ICommand CommandExcName
        {
            get
            {
                if (_commandexcname == null)
                    _commandexcname = new DelegateCommand(ExecuteCommandExcName, null);
                return _commandexcname;
            }
        }

        private void ExecuteCommandExcName(object obj)
        {
            if (Portaria.Servidor.Count > 0)
                Portaria.Servidor.RemoveAt(SelectedRow);
        }

        #endregion

        public vmEdit()
        {
            try
            {
                GlobalNavigation.Pagina = "EDITAR";

                var obj = GlobalNavigation.Parametro;

                Portaria = mdata.ListaParaEdicao(Convert.ToInt32(obj))[0];
            }
            catch
            { Portaria.ResetValues(); }
        }

    }
}
