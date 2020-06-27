using System.ComponentModel;

namespace Sim.Mvvm.Observers
{
    public abstract class GlobalNotifyProperty : NotifyProperty
    {
        public static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };

        public static void OnGlobalPropertyChanged(string propertyName)
        {
            GlobalPropertyChanged(
                typeof(GlobalNotifyProperty),
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
