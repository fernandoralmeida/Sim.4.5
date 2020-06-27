
namespace Sim.Model
{
    using Mvvm.Observers;
    class mMemory : NotifyProperty
    {
        private string _used;
        private string _pagined;
        private string _private;
        private string _virtual;
        private string _physic;
        private string _peakpagined;
        private string _peakvirtual;
        private string _peakphysic;

        public string PeakPagined
        {
            get { return _peakpagined; }
            set
            {
                _peakpagined = value;
                RaisePropertyChanged("PeakPagined");
            }
        }

        public string PeakVirtual
        {
            get { return _peakvirtual; }
            set
            {
                _peakvirtual = value;
                RaisePropertyChanged("PeakVirtual");
            }
        }

        public string PeakPhysic
        {
            get { return _peakphysic; }
            set
            {
                _peakphysic = value;
                RaisePropertyChanged("PeakPhysic");
            }
        }

        public string Private
        {
            get { return _private; }
            set
            {
                _private = value;
                RaisePropertyChanged("Private");
            }
        }

        public string Physic
        {
            get { return _physic; }
            set
            {
                _physic = value;
                RaisePropertyChanged("Physic");
            }
        }

        public string Used
        {
            get { return _used; }
            set
            {
                _used = value;
                RaisePropertyChanged("Used");
            }
        }

        public string Pagined
        {
            get { return _pagined; }
            set
            {
                _pagined = value;
                RaisePropertyChanged("Pagined");
            }
        }

        public string Virtual
        {
            get { return _virtual; }
            set
            {
                _virtual = value;
                RaisePropertyChanged("Virtual");
            }
        }
    }
}
