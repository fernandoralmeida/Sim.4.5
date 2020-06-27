
namespace Sim.Model
{
    using Mvvm.Observers;
    class mApps : NotifyProperty
    {
        private string _name;
        private string _version;
        private string _build;
        private string _revision;
        private string _update;
        private string _install;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                RaisePropertyChanged("Version");
            }
        }

        public string Build
        {
            get { return _build; }
            set
            {
                _build = value;
                RaisePropertyChanged("Build");
            }
        }

        public string Revision
        {
            get { return _revision; }
            set
            {
                _revision = value;
                RaisePropertyChanged("Revision");
            }
        }

        public string Update
        {
            get { return _update; }
            set
            {
                _update = value;
                RaisePropertyChanged("Update");
            }
        }

        public string Install
        {
            get { return _install; }
            set
            {
                _install = value;
                RaisePropertyChanged("Install");
            }
        }
    }
}
