using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Portarias.ViewModel
{

    using Mvvm.Commands;
    using Mvvm.Observers;
    using Model;
    using Sql;
    using Account;

    class vmConfig : NotifyProperty
    {
        #region Declarations

        private List<mPages> _pagelist = new List<mPages>();
        private Uri _pageselected;
        private Uri _comebackpage;

        #endregion

        #region Properties

        public List<mPages> PageList
        {
            get { return _pagelist; }
            set
            {
                _pagelist = value;
                RaisePropertyChanged("PageList");
            }
        }

        public Uri PageSelected
        {
            get { return _pageselected; }
            set
            {
                _pageselected = value;
                RaisePropertyChanged("PageSelected");
            }
        }

        public Uri ComeBackPage
        {
            get { return _comebackpage; }
            set
            {
                _comebackpage = value;
                RaisePropertyChanged("ComeBackPage");
            }
        }

        #endregion

        public vmConfig()
        {
            PageList.Clear();
            PageList.Add(new mPages() { Name = "Pdfs", Link = Properties.Resources.Sec_Governo_Portarias_Config_PDF });

            if (Logged.Acesso != 0)
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    PageList.Add(new mPages() { Name = "Classificações", Link = Properties.Resources.Sec_Governo_Portarias_Config_Classificacao });
                }

                else
                {

                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Portarias)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Operador)
                            {
                                PageList.Add(new mPages() { Name = "Classificações", Link = Properties.Resources.Sec_Governo_Portarias_Config_Classificacao });
                            }
                        }
                    }
                }
            }

            PageSelected = new Uri(PageList[0].Link, UriKind.Relative);
        }

    }
}
