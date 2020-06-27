using System.Windows.Input;

namespace Sim.Controls.ViewModels
{

    public interface IDialogBox
    {
        ICommand CommandMsgYes { get; }
        ICommand CommandMsgNot { get; }
    }
}
