using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Opcoes
{
    using Model;
    using Mvvm.Observers;

    class vmPerfil : NotifyProperty
    {
        #region Declarations

        private ObservableCollection<mTiposGenericos> _lista = new ObservableCollection<mTiposGenericos>();

        #endregion

        #region Properties

        public ObservableCollection<mTiposGenericos> Listar
        {
            get { return _lista; }
            set
            {
                _lista = value;
                RaisePropertyChanged("Listar");
            }
        }
        #endregion

        #region Constructor

        public vmPerfil()
        {
            Listar = new mData().Tipos(@"SELECT * FROM SDT_SE_PF_Perfil_Tipos WHERE (Ativo = True) ORDER BY Valor");
        }

        #endregion
    }
}
