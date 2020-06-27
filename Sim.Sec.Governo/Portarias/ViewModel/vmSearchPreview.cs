using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;

namespace Sim.Sec.Governo.Portarias.ViewModel
{
    using Mvvm.Commands;
    using Mvvm.Observers;

    class vmSearchPreview
    {
        #region Declarations

        private ICommand _commandclose;

        #endregion

        #region Properties

        public IDocumentPaginatorSource DocumentPreview { get; set; }
        public Window WindowParent { get; set; }
        #endregion

        #region Commands

        public ICommand CommandClose
        {
            get
            {
                if (_commandclose == null)
                    _commandclose = new DelegateCommand(ExecuteCommandClose, null);

                return _commandclose;
            }
        }

        #endregion

        public vmSearchPreview(string fileXps)
        {

        }

        #region Methods

        private void ExecuteCommandClose(object obj)
        {
            //WindowParent.Close();
            //vmPageObserver.SearchClear();
        }

        #endregion
    }
}
