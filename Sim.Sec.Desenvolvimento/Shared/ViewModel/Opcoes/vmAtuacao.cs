using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Opcoes
{
    using Model;
    using Mvvm.Observers;

    class vmAtuacao : NotifyProperty
    {
        #region Declarations

        private ObservableCollection<mTiposGenericos> _listar = new ObservableCollection<mTiposGenericos>();

        #endregion

        #region Properties

        public ObservableCollection<mTiposGenericos> Listar
        {
            get { return _listar; }
            set
            {
                _listar = value;
                RaisePropertyChanged("Listar");
            }
        }
        #endregion

        #region Constructor

        public vmAtuacao()
        {
            Listar = new mData().Tipos(@"SELECT * FROM SDT_SE_PJ_UsoLocal WHERE (Ativo = True) ORDER BY Valor");
        }

        #endregion
    }
}
