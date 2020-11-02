using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Sim.Sec.Desenvolvimento.Shared.Control
{
    using Model;

    public interface IAgenda 
    {
        ObservableCollection<mAgenda> ListarAgenda { get; set; }
        string EventoSelecionado { get; set; }

        ICommand CommandAgendaNavigate { get; }
        ICommand CommandEventoAtivo { get; }
        ICommand CommandEventoVencido { get; }
        ICommand CommandEventoCancelado { get; }
        ICommand CommandEventoFinalizado { get; }
        ICommand CommandDetalharEvento { get; }
        ICommand CommandEventoAlterar { get; }
        
        void AsyncListarAgenda();
        void AsyncListarAgendaAtivo();
        void AsyncListarAgendaVencida();
        void AsyncListarAgendaCancelada();
        void AsyncListarAgendaFinalizado();
    }
}
