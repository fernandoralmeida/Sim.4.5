using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{

    using Mvvm.Observers;

    public class mFormalizada : NotifyProperty
    {
        #region Declarations

        private int _indice;
        private string _cnpj = string.Empty;
        private string _inscmunicipal = string.Empty;
        private int _porte;
        private int _usolocal;
        private bool _formalizado_se;
        private DateTime _data;
        private bool _ativo;

        #endregion

        #region Properties

        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string CNPJ { get { return _cnpj; } set { _cnpj = value; RaisePropertyChanged("CNPJ"); } }
        public string InscricaoMunicipal { get { return _inscmunicipal; } set { _inscmunicipal = value; RaisePropertyChanged("InscricaoMunicipal"); } }
        public int Porte { get { return _porte; } set { _porte = value; RaisePropertyChanged("Porte"); } }
        public int UsoLocal { get { return _usolocal; } set { _usolocal = value; RaisePropertyChanged("UsoLocal"); } }
        public bool FormalizadoSE { get { return _formalizado_se; } set { _formalizado_se = value; RaisePropertyChanged("FormalizadoSE"); } }
        public DateTime Data { get { return _data; } set { _data = value; RaisePropertyChanged("Data"); } }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }

        #endregion

        #region Functions

        public void Clear()
        {
            Indice = 0;
            CNPJ = string.Empty;
            Porte = 0;
            UsoLocal = 0;
            FormalizadoSE = true;
            Data = DateTime.Now;
            Ativo = true;
        }

        #endregion
    }
}
