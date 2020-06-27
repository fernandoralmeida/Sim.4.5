using System;
using System.Diagnostics;

namespace Sim.ViewModel.Functions
{
    using Model;

    class Processos
    {
        public void ExecuteProcess(string processname)
        {
            Process[] pname = Process.GetProcessesByName(processname);

            if (pname.Length == 0)
            {
                ProcessStartInfo pInfo = new ProcessStartInfo(processname + ".exe");
                pInfo.WorkingDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                Process p = Process.Start(pInfo);
                //System.Windows.Application.Current.Shutdown();
            }

        }

        public void StopProcess(string processname)
        {
            Process[] pname = Process.GetProcessesByName(processname);

            if (pname.Length > 0)
                pname[0].CloseMainWindow();
        }
    }
}
