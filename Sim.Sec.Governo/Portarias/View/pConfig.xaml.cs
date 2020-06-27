using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Animation;

namespace Sim.Sec.Governo.Portarias.View
{
    /// <summary>
    /// Interação lógica para pConfig.xam
    /// </summary>
    public partial class pConfig : Page
    {
        public pConfig()
        {
            InitializeComponent();
            innerframe.JournalOwnership = JournalOwnership.OwnsJournal;
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                ShowFadingContent("FadingContent", (sender as Frame));
                ShowFadingContent("ShowRightContent", (sender as Frame));
            }

            if (e.NavigationMode == NavigationMode.Back)
            {
                ShowFadingContent("FadingContent", (sender as Frame));
                ShowFadingContent("ShowRightContent", (sender as Frame));
            }
        }

        private void ShowFadingContent(string Storyboard, Frame pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);
        }

    }
}
