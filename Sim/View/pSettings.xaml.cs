using System.Windows;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Sim.View
{
    /// <summary>
    /// Interação lógica para pSetings.xam
    /// </summary>
    public partial class pSettings : Page
    {
        public pSettings()
        {
            InitializeComponent();
            //stg_innerframe.JournalOwnership = JournalOwnership.OwnsJournal;
        }

        /*
        private void Stg_innerframe_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                BeginAnimation("FadingPage", (sender as Frame));
                BeginAnimation("ShowPage", (sender as Frame));
            }

            if (e.NavigationMode == NavigationMode.Back)
            {
                BeginAnimation("FadingPage", (sender as Frame));
                BeginAnimation("ShowPage", (sender as Frame));
            }
        }

        private void BeginAnimation(string Storyboard, Frame pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);
        }
        */
    }
}
