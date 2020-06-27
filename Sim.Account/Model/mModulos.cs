
namespace Sim.Account.Model
{

    using Mvvm.Observers;
    
    public class mModulos : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _identificador;
        private int _modulo;
        private bool _acesso;
        #endregion

        #region Properties
        public int Indice
        {
            get { return _indice; }
            set
            {
                _indice = value;
                RaisePropertyChanged("Indice");
            }
        }
        public string Identificador
        {
            get { return _identificador; }
            set
            {
                _identificador = value;
                RaisePropertyChanged("Identificador");
            }
        }
        public int Modulo
        {
            get { return _modulo; }
            set
            {
                _modulo = value;
                RaisePropertyChanged("Modulo");
            }
        }
        public bool Acesso
        {
            get { return _acesso; }
            set
            {
                _acesso = value;
                RaisePropertyChanged("Acesso");
            }
        }
        #endregion

        #region Functions
        public void Clear()
        {
            Indice = 0;
            Identificador = string.Empty;
            Modulo = 0;
            Acesso = false;
        }
        #endregion
    }
}
