using System.Windows;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Animation;


namespace Sim
{
       
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Declarations
        private Storyboard backgroundAnimation;
        #endregion

        #region Construtor

        public MainWindow()
        {            
            InitializeComponent();

            //var cbw = System.Windows.Media.Color.FromRgb(0x1b, 0xa1, 0xe2);
            //var brs = new System.Windows.Media.SolidColorBrush(cbw);

            //this.Background = brs;

            Mvvm.Observers.GlobalNavigation.NavService = _globalframe.NavigationService;
            //Mvvm.Observers.GlobalNavigation.ChildFrame = _settingsframe.NavigationService;

            this.DataContext = new ViewModel.vmMainWindow();

            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));

            UI.Observers.ThemeObserver.GlobalPropertyChanged += ThemeObserver_GlobalPropertyChanged;
        }

        #endregion

        #region Methods

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }


        private void ThemeObserver_GlobalPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //this.backgroundAnimation.Begin();
            // start background animation if theme has changed
            if (e.PropertyName == "ThemeSource" && backgroundAnimation != null)
            {
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new System.Action(delegate 
                {
                    backgroundAnimation.Begin();
                }));                
            }
        }

        #endregion

        #region Overridies
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        /// 
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // retrieve BackgroundAnimation storyboard
            var border = GetTemplateChild("WindowBorder") as Border;
            if (border != null)
            {
                backgroundAnimation = border.Resources["BackgroundAnimation"] as Storyboard;

                if (backgroundAnimation != null)
                {
                    backgroundAnimation.Begin();
                }
            }
        }

        #endregion

        private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                ShowFadingContent("FadingPage", (sender as Frame));
                ShowFadingContent("ShowPage", (sender as Frame));
            }

            if (e.NavigationMode == NavigationMode.Back)
            {
                ShowFadingContent("FadingPage", (sender as Frame));
                ShowFadingContent("ShowPage", (sender as Frame));
            }

            
        }

        private void ShowFadingContent(string Storyboard, Frame pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);            
        }

        private void Lateral_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as Grid).Visibility == Visibility.Visible)
            {
                Storyboard sb = Resources["BrowseBack"] as Storyboard;
                sb.Begin((sender as Grid));
            }
        }

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as StackPanel).Visibility == Visibility.Visible)
            {
                Storyboard sb = Resources["ShowTopMenu"] as Storyboard;
                sb.Begin((sender as StackPanel));
            }

        }

    }
}
