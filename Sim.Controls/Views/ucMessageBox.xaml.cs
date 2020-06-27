using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sim.Controls.Views
{
    /// <summary>
    /// Interação lógica para ucMessageBox.xam
    /// </summary>
    public partial class ucMessageBox : UserControl
    {
        public ucMessageBox()
        {
            InitializeComponent();
        }

        private void ShowFadingContent(string Storyboard, Grid pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);
        }

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as Grid).Visibility == Visibility.Visible)
            {
                //ShowFadingContent("FadingContent", (sender as Grid));
                //ShowFadingContent("SlideContent", (sender as Grid));
            }
        }
    }
}
