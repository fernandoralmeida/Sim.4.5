using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Animation;

namespace Sim.Sec.Desenvolvimento.Shared.View.Opcoes
{
    /// <summary>
    /// Interação lógica para pMain.xam
    /// </summary>
    public partial class pMain : Page
    {
        public pMain()
        {
            InitializeComponent();
            inframe.JournalOwnership = JournalOwnership.OwnsJournal;         
        }

        private void Inframe_Navigating(object sender, NavigatingCancelEventArgs e)
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
