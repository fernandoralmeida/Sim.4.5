using System;
using System.Diagnostics;
using System.Reflection;


namespace Sim.ViewModel.Functions
{
    using Model;
    using Mvvm.Observers;

    class GetSIMInfo : NotifyProperty
    {
        private mApps _apps = new mApps();

        public mApps Apps
        {
            get { return _apps; }
            set
            {
                _apps = value;
                RaisePropertyChanged("Apps");
            }
        }

        public GetSIMInfo(string assemblyname)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assemblyname);
            Apps.Name = fvi.OriginalFilename;
            Apps.Version = "Versão: " + AssemblyName.GetAssemblyName(assemblyname).Version.ToString();
            Apps.Build = "Build: " + AssemblyName.GetAssemblyName(assemblyname).Version.Build;
            Apps.Revision = "Revisão: " + AssemblyName.GetAssemblyName(assemblyname).Version.Revision;
            Apps.Update = "Update: " + Properties.Settings.Default.Update;
            Apps.Install = "Diretório: " + AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
