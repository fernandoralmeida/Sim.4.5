using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Opcoes
{
    using Model;
    using Mvvm.Observers;

    class vmViabilidade : NotifyProperty
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

        public vmViabilidade()
        {
            Listar = new mData().Tipos(@"SELECT * FROM SDT_SE_Viabilidade_Situacao WHERE (Ativo = True) ORDER BY Valor");
        }

        #endregion
    }
}
