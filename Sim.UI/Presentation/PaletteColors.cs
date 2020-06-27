using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sim.UI.Presentation
{
    using Model;
    using Observers;

    public class PaletteColors  : NotifyPropertyChanged
    {
        #region Declarations

        private ObservableCollection<mColor> _listcolor = new ObservableCollection<mColor>();

        #endregion

        #region Properties

        private ObservableCollection<mColor> GetColors
        {
            get { return _listcolor; }
            set
            {
                _listcolor = value;
                OnPropertyChanged("GetColors");
            }
        }

        #endregion

        #region Constructor
        public PaletteColors()
        {
            /*
            Task.Factory.StartNew(() => ListColors()).ContinueWith(t => {

                if (t.IsCompleted)
                    GetColors = t.Result;

            });*/
            GetColors = ListColors();
        }
        #endregion

        #region Functions
        public ObservableCollection<mColor> ListColors()
        {
            //new mColor() { Name = "blue", Value = Color.FromRgb(0x33, 0x99, 0xff) },
            var _colors = new ObservableCollection<mColor>() {
                new mColor() { Name = "lime", Value = Color.FromRgb(0x8c, 0xbf, 0x26) },
                new mColor() { Name = "green", Value = Color.FromRgb(0x60, 0xa9, 0x17) },
                new mColor() { Name = "emerald", Value = Color.FromRgb(0x00, 0x8a, 0x00) },
                new mColor() { Name = "teal", Value = Color.FromRgb(0x00, 0xab, 0xa9) },
                new mColor() { Name = "cyan", Value = Color.FromRgb(0x1b, 0xa1, 0xe2) },

                new mColor() { Name = "cobalt", Value = Color.FromRgb(0x00, 0x50, 0xef) },
                new mColor() { Name = "indigo", Value = Color.FromRgb(0x6a, 0x00, 0xff) },
                new mColor() { Name = "violet", Value = Color.FromRgb(0xaa, 0x00, 0xff) },
                new mColor() { Name = "pink", Value = Color.FromRgb(0xf4, 0x72, 0xd0) },
                new mColor() { Name = "magenta", Value = Color.FromRgb(0xd8, 0x00, 0x73) },
                new mColor() { Name = "crimson", Value = Color.FromRgb(0xa2, 0x00, 0x25) },
                new mColor() { Name = "red", Value = Color.FromRgb(0xe5, 0x14, 0x00) },
                new mColor() { Name = "orange", Value = Color.FromRgb(0xfa, 0x68, 0x00) },
                new mColor() { Name = "amber", Value = Color.FromRgb(0xf0, 0xa3, 0x0a) },
                new mColor() { Name = "yellow", Value = Color.FromRgb(0xe3, 0xc8, 0x00) },
                new mColor() { Name = "brown", Value = Color.FromRgb(0x82, 0x5a, 0x2c) },
                new mColor() { Name = "olive", Value = Color.FromRgb(0x6d, 0x87, 0x64) },
                new mColor() { Name = "steel", Value = Color.FromRgb(0x64, 0x76, 0x87) },
                new mColor() { Name = "mauve", Value = Color.FromRgb(0x76, 0x60, 0x8a) },
                new mColor() { Name = "taupe", Value = Color.FromRgb(0x87, 0x79, 0x4e) }
            };

            return _colors;
        }
        #endregion
    }
}
