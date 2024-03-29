﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.ViewModel
{
    using Mvvm.Commands;
    using Mvvm.Observers;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Account;
    using Shared.Model;
    using Model;
    using Shared.Control;
    public class vmMenuDIA : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        ObservableCollection<DIA> _dialist = new ObservableCollection<DIA>();
        #endregion

        #region Properties
        public ObservableCollection<DIA> DIAList
        {
            get { return _dialist; }
            set { _dialist = value; RaisePropertyChanged(DIAList.ToString()); }
        }
        #endregion

        #region Commands
        public ICommand CommandGoDIA => new RelayCommand(p =>
        {
            var t = Task<Model.DIA>.Run(() => new Repositorio.RDIA().GetDIA((int)p));            
            t.Wait();
            AreaTransferencia.Objeto = t.Result;
            ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/PreviewDIA.xaml", UriKind.RelativeOrAbsolute));
        }); 
        #endregion

        #region Constructor
        public vmMenuDIA()
        {
            GlobalNavigation.SubModulo = "COMÉRCIO AMBULANTE";
            GlobalNavigation.Parametro = "1";
            GlobalNavigation.Pagina = "D.I.A";
            ns = GlobalNavigation.NavService;
            //BlackBox = Visibility.Collapsed;
            //PreviewBox = Visibility.Collapsed;
            //RetVisible = Visibility.Collapsed;
            //StartProgress = false;
            AreaTransferencia.Limpar();

            //EventoSelecionado = "Ativos";
            //CommandEventoAtivo.Execute(null);
            
            Get_Last_10_DIAList();
        }
        #endregion

        #region Functions
        public void Get_Last_10_DIAList()
        {
            var t = Task<ObservableCollection<Model.DIA>>.Run(() => new Repositorio.RDIA().Last_DIAs());
            t.Wait();
            DIAList = t.Result;
        }
        #endregion
    }
}
