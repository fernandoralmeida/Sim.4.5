using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sim.Controls.ViewModels
{
    public interface IPrintBox
    {
        ICommand CommandPrint { get; }
        ICommand CommandClosePrintBox { get; }
    }
}
