using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Legislacao.ViewModel
{

    using Sim.Mvvm.Commands;
    using Sim.Mvvm.Observers;
    using Model;
    using SqlCommands;
    using Account;
    using Account.Model;

    class vmConfig : NotifyProperty
    {
        #region Declarations

        private List<mPages> _pagelist = new List<mPages>();
        private Uri _pageselected;

        #endregion

        #region Properties

        public List<mPages> PageList
        {
            get { return _pagelist; }
            set {
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

        #endregion

        public vmConfig()
        {
            GlobalNavigation.Pagina = "OPÇÕES";
            PageList.Clear();
            PageList.Add(new mPages() { Name = "Pdfs", Link = Properties.Resources.Sec_Governo_Legislacao_Config_LinkPDF });

            if (Logged.Acesso != 0)
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    PageList.Add(new mPages() { Name = "Tipos", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Tipo });
                    PageList.Add(new mPages() { Name = "Classificações L", Link = Properties.Resources.Sec_Governo_Legislacao_Config_ClassLLc });
                    PageList.Add(new mPages() { Name = "Classificações D", Link = Properties.Resources.Sec_Governo_Legislacao_Config_ClassDec  });
                    PageList.Add(new mPages() { Name = "Situações", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Situacao });
                    PageList.Add(new mPages() { Name = "Origens", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Origem });
                    PageList.Add(new mPages() { Name = "Autores", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Autor });
                    PageList.Add(new mPages() { Name = "Ações", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Acao });
                }

                else
                {

                    foreach (mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Legislacao)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Operador)
                            {
                                PageList.Add(new mPages() { Name = "Tipos", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Tipo });
                                PageList.Add(new mPages() { Name = "Classificações L", Link = Properties.Resources.Sec_Governo_Legislacao_Config_ClassLLc });
                                PageList.Add(new mPages() { Name = "Classificações D", Link = Properties.Resources.Sec_Governo_Legislacao_Config_ClassDec });
                                PageList.Add(new mPages() { Name = "Situações", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Situacao });
                                PageList.Add(new mPages() { Name = "Origens", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Origem });
                                PageList.Add(new mPages() { Name = "Autores", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Autor });
                                PageList.Add(new mPages() { Name = "Ações", Link = Properties.Resources.Sec_Governo_Legislacao_Config_Acao });
                            }
                        }
                    }
                }
            }

            PageSelected = new Uri(PageList[0].Link, UriKind.Relative);
        }

    }
}
