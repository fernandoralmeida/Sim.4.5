using System.Windows.Media;

namespace Sim.UI.Model
{
    public class mColor
    {
        private string _name = string.Empty;
        public string Name
        {
            get { return _name.ToUpper(); }
            set { _name = value; }
        }
        public Color Value { get; set; }
    }
}
