using System.Windows.Input;
using System.Threading.Tasks;

namespace Sim.ViewModel
{

    using Mvvm.Commands;
    using Controls.ViewModels;

    class vmToolsHPrint : VMBase, IDialogBox
    {
        #region Declarations

        private string _nameorg;
        private string _slogan;
        private string _deporg;
        private string _setororg;

        #endregion

        #region Properties

        public string NameOrg
        {
            get { return _nameorg; }
            set
            {
                _nameorg = value;
                RaisePropertyChanged("NameOrg");
            }
        }

        public string SloganOrg
        {
            get { return _slogan; }
            set
            {
                _slogan = value;
                RaisePropertyChanged("SloganOrg");
            }
        }

        public string DepOrg
        {
            get { return _deporg; }
            set
            {
                _deporg = value;
                RaisePropertyChanged("DepOrg");
            }
        }

        public string SetorOrg
        {
            get { return _setororg; }
            set
            {
                _setororg = value;
                RaisePropertyChanged("SetorOrg");
            }
        }

        #endregion

        #region Commands

        public ICommand CommandUpdateRelatory => new RelayCommand(p => 
        {
            SyncDialogBox("Gravar alterações no Cabeçalho de Impressão?", DialogBoxColor.Red);
        });

        public ICommand CommandMsgYes => new RelayCommand(p => AsyncGravar());

        public ICommand CommandMsgNot => new RelayCommand(p => { ViewDialogBox = System.Windows.Visibility.Collapsed; });

        #endregion

        #region Constructor

        public vmToolsHPrint()
        {
            Mvvm.Observers.GlobalNavigation.Pagina = "IMPRESSÃO";
            NameOrg = Services.Properties.Settings.Default.Prefeitura;
            SloganOrg = Services.Properties.Settings.Default.Fundada;
            DepOrg = Services.Properties.Settings.Default.Secretaria;
            SetorOrg = Services.Properties.Settings.Default.Setor;
            ViewDialogBox = System.Windows.Visibility.Collapsed;
            ViewMessageBox = System.Windows.Visibility.Collapsed;
        }

        #endregion

        #region Functions
        private async void AsyncGravar()
        {
            ViewDialogBox = System.Windows.Visibility.Collapsed;

            var t = Task.Run(() => {
                Services.Properties.Settings.Default.Prefeitura = NameOrg;
                Services.Properties.Settings.Default.Fundada = SloganOrg;
                Services.Properties.Settings.Default.Secretaria = DepOrg;
                Services.Properties.Settings.Default.Setor = SetorOrg;
                Services.Properties.Settings.Default.Save();
            });

            await t;

            if(t.IsCompleted)
            {
                AsyncMessageBox("Alterações gravadas!", DialogBoxColor.Green, false);
            }
        }
        #endregion
    }
}
