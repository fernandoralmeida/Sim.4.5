using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Sim.Controls.ViewModels
{

    using Mvvm.Observers;

    /// <summary>
    /// Classe abstrata com dos controles
    /// BlackBox, MessageBox, DialogBox, PrintBox
    /// </summary>
    public abstract class VMBase : NotifyProperty
    {
        #region Declarations
        private SolidColorBrush _msgcolor;
        private SolidColorBrush _dlgcolor;
        private Visibility _showmsg;
        private Visibility _showdlgbox;
        private Visibility _blackbox;
        private Visibility _printbox;
        private string _msgbox;
        private string _dlgbox;
        private int _reportprogress;
        private bool _startprogress;
        #endregion

        #region PrintBox
        public Visibility PrintBox
        {
            get { return _printbox; }
            set
            {
                _printbox = value;
                RaisePropertyChanged("PrintBox");
            }
        }
        #endregion

        #region BlackBox
        public Visibility BlackBox
        {
            get { return _blackbox; }
            set
            {
                _blackbox = value;
                RaisePropertyChanged("BlackBox");
            }
        }

        public bool StartProgress
        {
            get { return _startprogress; }
            set
            {
                _startprogress = value;
                RaisePropertyChanged("StartProgress");
            }
        }
        #endregion

        #region Properties MessageBox
        public Visibility ViewMessageBox
        {
            get { return _showmsg; }
            set
            {
                _showmsg = value;
                RaisePropertyChanged("ViewMessageBox");
            }
        }

        public string TextMessageBox
        {
            get { return _msgbox; }
            set
            {
                _msgbox = value;
                RaisePropertyChanged("TextMessageBox");
            }
        }

        public int ReportProgress
        {
            get { return _reportprogress; }
            set
            {
                _reportprogress = value;
                RaisePropertyChanged("ReportProgress");
            }
        }

        public SolidColorBrush ColorMessageBox
        {
            get { return _msgcolor; }
            set
            {
                _msgcolor = value;
                RaisePropertyChanged("ColorMessageBox");
            }
        }

        #endregion

        #region Properties DialogBox
        public Visibility ViewDialogBox
        {
            get { return _showdlgbox; }
            set
            {
                _showdlgbox = value;
                RaisePropertyChanged("ViewDialogBox");
            }
        }

        public string TextDialogBox
        {
            get { return _dlgbox; }
            set
            {
                _dlgbox = value;
                RaisePropertyChanged("TextDialogBox");
            }
        }

        public SolidColorBrush ColorDialogBox
        {
            get { return _dlgcolor; }
            set
            {
                _dlgcolor = value;
                RaisePropertyChanged("ColorDialogBox");
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Congela uma thread
        /// </summary>
        /// <param name="valor">delay em milisegundos</param>
        private void Freezetime(int valor)
        {
            for (int x = 100; x > 0; x--)
            {
                ReportProgress = (100 * x) / 100;
                Thread.Sleep(valor);
            }
        }

        /// <summary>
        /// Pega o cor corresponde ao enum
        /// </summary>
        /// <param name="_cor"></param>
        /// <returns></returns>
        private SolidColorBrush GetColor(DialogBoxColor _cor)
        {
            var _color = Brushes.White;
            switch (_cor)
            {
                case DialogBoxColor.Blue:
                    _color = Brushes.CornflowerBlue;
                    break;

                case DialogBoxColor.Green:
                    _color = Brushes.Green;
                    break;

                case DialogBoxColor.Orange:
                    _color = Brushes.Orange;
                    break;

                case DialogBoxColor.Red:
                    _color = Brushes.Red;
                    break;                                      
            }

            return _color;
        }

        /// <summary>
        /// Chama Messagebox assíncrono
        /// </summary>
        /// <param name="message">Texto da mensagem</param>
        /// <param name="dbgcolor">Cor da MessageBox</param>
        public async void AsyncMessageBox(string message, DialogBoxColor dbgcolor, bool _cangoback)
        {
            TextMessageBox = message;
            ColorMessageBox = GetColor(dbgcolor);            
            ViewMessageBox = Visibility.Visible;
            //StartProgress = true;
            //System.Media.SystemSounds.Exclamation.Play();
                                 
            var t = Task.Run(() => Freezetime(10));

            await t;

            if (t.IsCompleted)
            {
                //StartProgress = false;
                ViewMessageBox = Visibility.Collapsed;
                if (_cangoback)
                    GlobalNavigation.NavService.GoBack();

            }
        }

        /// <summary>
        /// Chama DialogBox síncrono
        /// </summary>
        /// <param name="message">Texto da mensagem</param>
        /// <param name="color">Cor do DialogBox</param>
        public void SyncDialogBox(string message, DialogBoxColor dbgcolor)
        {
            TextDialogBox = message;
            ColorDialogBox = GetColor(dbgcolor);
            ViewDialogBox = Visibility.Visible;
            System.Media.SystemSounds.Exclamation.Play();            
        }

        /// <summary>
        /// Chama MessageBox síncrono
        /// </summary>
        /// <param name="message">Texto da mensagem</param>
        /// <param name="color">Cor do DialogBox</param>
        public void SyncMessageBox(string message, DialogBoxColor dbgcolor)
        {
            //StartProgress = true;
            TextMessageBox = message;
            ColorMessageBox = GetColor(dbgcolor);
            ViewMessageBox = Visibility.Visible;
            //System.Media.SystemSounds.Exclamation.Play();
            Freezetime(10);
            ViewMessageBox = Visibility.Collapsed;
            //StartProgress = false;
        }
        #endregion
    }
}
