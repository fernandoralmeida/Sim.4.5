using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Sim.Controls
{
    using Model;
    /// <summary>
    /// Siga os passos 1a ou 1b e depois 2 para usar esse controle personalizado em um arquivo XAML.
    ///
    /// Passo 1a) Usando o controle personalizado em um arquivo XAML que já existe no projeto atual.
    /// Adicione o atributo XmlNamespace ao elemento raiz do arquivo de marcação onde ele 
    /// deve ser usado:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Sim.Controls"
    ///
    ///
    /// Passo 1b) Usando o controle personalizado em um arquivo XAML que existe em um projeto diferente.
    /// Adicione o atributo XmlNamespace ao elemento raiz do arquivo de marcação onde ele 
    /// deve ser usado:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Sim.Controls;assembly=Sim.Controls"
    ///
    /// Também será necessário adicionar nesse projeto uma referência ao projeto que contém esse arquivo XAML
    /// e Recompilar para evitar erros de compilação:
    ///
    ///     No Gerenciador de Soluções, clique com o botão direito no projeto alvo e
    ///     "Add Reference"->"Projects"->[Procure e selecione o projeto]
    ///
    ///
    /// Passo 2)
    /// Vá em frente e use seu controle no arquivo XAML.
    ///
    ///     <MyNamespace:BarChartsColor/>
    ///
    /// </summary>
    [TemplatePart(Name = PartBars, Type = typeof(ItemsControl))]
    [TemplatePart(Name = PartLegendas, Type = typeof(ItemsControl))]
    public class BarChartsColor : Control
    {
        static BarChartsColor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BarChartsColor), new FrameworkPropertyMetadata(typeof(BarChartsColor)));
        }

        #region  Fields
        public const string PartBars = "PART_Bars";
        public const string PartLegendas = "PART_Leg";
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BarChartsColor), new FrameworkPropertyMetadata(string.Empty));
        //public static readonly DependencyProperty ListSourceProperty = DependencyProperty.Register("ListSource", typeof(ObservableCollection<mBarChart>), typeof(BarChart), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty RelativeWidthProperty = DependencyProperty.Register("RelativeWidth", typeof(double), typeof(BarChartsColor), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty LegendasOnOffProperty = DependencyProperty.Register("LegendasOnOff", typeof(Visibility), typeof(BarChartsColor), new FrameworkPropertyMetadata(null));
        /// <summary>
        /// Registers a dependency property as backing store for the Content property
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(object), typeof(BarChartsColor),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        private ItemsControl _partbars;
        private ItemsControl _partlegs;

        #endregion

        #region Declarations

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<mBarChart> _listsource = new ObservableCollection<mBarChart>();
        private List<KeyValuePair<string, int>> _itemssource = new List<KeyValuePair<string, int>>();

        #endregion

        #region Properties

        public string Title
        {
            get { return base.GetValue(TitleProperty) as string; }
            set
            {
                base.SetValue(TitleProperty, value);
                RaisePropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        /// <value>The Content.</value>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public ItemsControl ItemsControlBars
        {
            get { return _partbars; }
            set { _partbars = value; }
        }

        public ItemsControl ItemsControlLegs
        {
            get { return _partlegs; }
            set { _partlegs = value; }
        }

        public List<KeyValuePair<string, int>> ItemsSource
        {
            get { return _itemssource; }
            set
            {
                _itemssource = value;
                RaisePropertyChanged("ItemsSource");
            }
        }

        public Visibility LegendasOnOff
        {
            get { return (Visibility)GetValue(LegendasOnOffProperty); }
            set
            {
                SetValue(LegendasOnOffProperty, value);
                RaisePropertyChanged("LegendasOnOff");
            }
        }

        public double RelativeWidth
        {
            get { return (double)GetValue(RelativeWidthProperty); }
            set
            {
                SetValue(RelativeWidthProperty, ItemsControlBars.RenderSize.Width);
                ItemsControlBars.ItemsSource = ToListSource();
                ItemsControlLegs.ItemsSource = ToListSource();
                RaisePropertyChanged("RelativeWidth");
            }
        }

        #endregion

        #region Methods

        private ObservableCollection<mBarChart> ToListSource()
        {
            _listsource.Clear();

            double max_bar_width = 0;

            foreach (KeyValuePair<string, int> x in ItemsSource)
            {
                max_bar_width += x.Value;
            }

            foreach (KeyValuePair<string, int> x in ItemsSource)
            {
                mBarChart _chart = new mBarChart();

                double _relwidth = RelativeWidth;

                if (_relwidth == 0)
                    _relwidth = 500;

                double _current_wdth = (x.Value * (_relwidth * 0.5)) / max_bar_width;

                double _percent = x.Value / max_bar_width;

                if (_current_wdth < 1)
                    _current_wdth = 1;

                _chart.Key = x.Key;
                _chart.Width = _current_wdth;
                _chart.Value = x.Value;
                _chart.Percent = _percent;
                _chart.Total = max_bar_width;

                _listsource.Add(_chart);
            }

            if (_listsource.Count < 1)
                LegendasOnOff = Visibility.Collapsed;
            else
                LegendasOnOff = Visibility.Visible;

            return _listsource;
        }

        private void ItemsControlBars_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ItemsControlBars.ItemsSource = ToListSource();
        }

        private void ItemsControlLegs_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ItemsControlLegs.ItemsSource = ToListSource();
        }

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion

        #region Overrides

        protected override Size MeasureOverride(Size constraint)
        {
            RelativeWidth = constraint.Width;
            return base.MeasureOverride(constraint);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ItemsControlBars = Template.FindName(PartBars, this) as ItemsControl;
            ItemsControlLegs = Template.FindName(PartLegendas, this) as ItemsControl;

            ItemsControlBars.SizeChanged += ItemsControlBars_SizeChanged;

            ItemsControlLegs.SizeChanged += ItemsControlLegs_SizeChanged;
        }

        #endregion
    }
}
