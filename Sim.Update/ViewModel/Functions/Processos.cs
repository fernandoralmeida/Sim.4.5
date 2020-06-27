using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sim.Update.ViewModel.Functions
{
    using Common;

    class Processos
    {
        public void ExecuteProcess(string processname)
        {
            Process[] pname = Process.GetProcessesByName(processname);

            if (pname.Length == 0)
            {
                ProcessStartInfo pInfo = new ProcessStartInfo(Folders.AppData_Sim + processname + ".exe");
                pInfo.WorkingDirectory = Folders.AppData_Sim;
                Process p = Process.Start(pInfo);

                System.Windows.Application.Current.Shutdown();
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
