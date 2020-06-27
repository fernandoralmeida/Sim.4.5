using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Opcoes
{
    using Model;
    using Mvvm.Observers;

    class vmCnae : NotifyProperty
    {
        #region Declarations

        private ObservableCollection<mCNAE> _lista = new ObservableCollection<mCNAE>();

        #endregion

        #region Properties

        public ObservableCollection<mCNAE> Listar
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

        public vmCnae()
        {
            Listar = new mData().CNAES(@"SELECT * FROM SDT_SE_PJ_CNAE_MEI WHERE (Ativo = True) ORDER BY Ocupacao");
        }

        #endregion
    }
}
