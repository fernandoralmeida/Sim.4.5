using System;
using System.ComponentModel;

namespace Sim.Mvvm.Observers
{

    /// <summary>
    /// A implementação base do INotifyPropertyChanged 
    /// </summary>
    public abstract class NotifyProperty : INotifyPropertyChanged
    {

        /// <summary>
        /// Ocorre quando um valor de propriedade é alterado.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gera o evento PropertyChanged.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));                       
        }
    }
}
