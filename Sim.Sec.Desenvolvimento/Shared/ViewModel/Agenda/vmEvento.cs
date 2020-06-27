using System;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Agenda
{
    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Controls.ViewModels;

    class vmEvento : VMBase
    {
        #region Declarations
        NavigationService ns;
        mData mdata = new mData();
        mAgenda _agd = new mAgenda();
        #endregion

        #region Properties
        public ObservableCollection<mTiposGenericos> Tipos
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Tipo"); }
        }

        public ObservableCollection<mTiposGenericos> Setores
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Setores WHERE (Ativo = True) ORDER BY Setor"); }
        }

        public ObservableCollection<mTiposGenericos> Estados
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Estado WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public mAgenda Agenda
        {
            get { return _agd; }
            set
            {
                _agd = value;
                RaisePropertyChanged("Agenda");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandGravarAgenda => new RelayCommand(async p =>
                    {
                        Agenda.Ativo = true;
                        Agenda.Criacao = DateTime.Now;
                        if(await GravarAgenda()) ns.GoBack();
                    });

        public ICommand CommandCancelar => new RelayCommand(p =>
                    {
                        ns.GoBack();
                    });
        #endregion

        #region Constructor
        public vmEvento()
        {
            ns = GlobalNavigation.NavService;
            Agenda.Data = DateTime.Now;
            BlackBox = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;
            StartProgress = false;
            Agenda.Codigo = Codigo();
        }
        #endregion

        #region Methods

        #endregion

        #region Functions
        private string Codigo()
        {
            string r = string.Empty;

            string a = DateTime.Now.Year.ToString("0000");
            string m = DateTime.Now.Month.ToString("00");
            string d = DateTime.Now.Day.ToString("00");

            string hs = DateTime.Now.Hour.ToString("00");
            string mn = DateTime.Now.Minute.ToString("00");
            string ss = DateTime.Now.Second.ToString("00");

            r = string.Format(@"EV{0}{1}{2}{3}{4}{5}",
                a, m, d, hs, mn, ss);

            return r;
        }

        private Task<bool> GravarAgenda()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            return Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    if(mdata.GravarEvento(Agenda))
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                        SyncMessageBox("Evento criado com sucesso!", DialogBoxColor.Green);
                        return true;
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                        SyncMessageBox("Evento NÃO criado!", DialogBoxColor.Orange);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    BlackBox = Visibility.Collapsed;
                    StartProgress = false;
                    SyncMessageBox(ex.Message, DialogBoxColor.Red);
                    return false;
                }
            });
        }

        #endregion
    }
}
