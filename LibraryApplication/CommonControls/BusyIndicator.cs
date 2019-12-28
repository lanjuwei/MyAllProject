using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CommonControls
{
    /// <summary>
    /// A control to provide a visual indicator when an application is busy.
    /// </summary>
    /// 
    [TemplateVisualState(Name = VisualStates.StateIdle, GroupName = VisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = VisualStates.StateBusy, GroupName = VisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = VisualStates.StateVisible, GroupName = VisualStates.GroupVisibility)]
    [TemplateVisualState(Name = VisualStates.StateHidden, GroupName = VisualStates.GroupVisibility)]
    [StyleTypedProperty(Property = "OverlayStyle", StyleTargetType = typeof(Rectangle))]
    [StyleTypedProperty(Property = "ProgressBarStyle", StyleTargetType = typeof(ProgressBar))]
    public class BusyIndicator : ContentControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether the BusyContent is visible.
        /// </summary>
        protected bool IsContentVisible { get; set; }

        /// <summary>
        /// Timer used to delay the initial display and avoid flickering.
        /// </summary>
        private DispatcherTimer _displayAfterTimer;

        /// <summary>
        /// Instantiates a new instance of the BusyIndicator control.
        /// </summary>
        public BusyIndicator()
        {
            DefaultStyleKey = typeof(BusyIndicator);
            _displayAfterTimer = new DispatcherTimer();
            _displayAfterTimer.Tick += new EventHandler(DisplayAfterTimerElapsed);
            this.Animation = InitBusyContent();
            var dic = new ResourceDictionary { Source = new Uri("/CommonControls;component/AllCommonControlStyle.xaml", UriKind.RelativeOrAbsolute) };
            Style = dic["MyBusyIndicatorStyle"] as Style;
        }

        /// <summary>
        /// Overrides the OnApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        /// <summary>
        /// Handler for the DisplayAfterTimer.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        /// Changes the control's visual state(s).
        /// </summary>
        /// <param name="useTransitions">True if state transitions should be used.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the busy indicator should show.
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// Identifies the IsBusy dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
             typeof(bool),
             typeof(BusyIndicator),
             new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

        /// <summary>
        /// IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="d">BusyIndicator that changed its IsBusy.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // Go visible now
                    IsContentVisible = true;
                }
                else
                {
                    // Set a timer to go visible
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // No longer visible
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }
            ChangeVisualState(true);
        }

        /// Gets or sets a value indicating the busy content to display to the user.
        /// </summary>
        public object Animation
        {
            get { return (object)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        /// <summary>
        /// Identifies the BusyContent dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationProperty = DependencyProperty.Register(
            "Animation",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        public object BusyContent
        {
            get { return (object)GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        /// <summary>
        /// Identifies the BusyContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets a value indicating the template to use for displaying the busy content to the user.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate)GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the BusyTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating how long to delay before displaying the busy content.
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        /// <summary>
        /// Identifies the DisplayAfter dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof(TimeSpan),
            typeof(BusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        /// Gets or sets a value indicating the style to use for the overlay.
        /// </summary>
        public Style OverlayStyle
        {
            get { return (Style)GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the OverlayStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating the style to use for the progress bar.
        /// </summary>
        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the ProgressBarStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        #region BusyContent

        private Grid InitBusyContent()
        {
            var g = new Grid();
            #region AddPaths
            int counter = 12;
            for (int i = 0; i < counter; i++)
            {
                Path path = new Path();
                path.Data = Geometry.Parse("M 0,0 L -1,0 L -1,-50 L 0,-51 L 1,-12 L 1,0 Z");
                path.Stroke = new SolidColorBrush(Colors.SkyBlue);
                path.Fill = new SolidColorBrush(Colors.SkyBlue);
                path.Opacity = 0.1;

                TransformGroup tg = new TransformGroup();
                path.RenderTransform = tg;
                TranslateTransform tt = new TranslateTransform();
                tt.Y = -40; tt.X = 1;

                RotateTransform rt = new RotateTransform();
                rt.Angle = i * 30;
                tg.Children.Add(tt);
                tg.Children.Add(rt);

                DoubleAnimation da = new DoubleAnimation();
                da.From = 1.0;
                da.To = 0.1;
                da.Duration = new Duration(TimeSpan.FromSeconds(1));

                da.BeginTime = TimeSpan.FromMilliseconds(i * 1000 / counter);
                da.RepeatBehavior = RepeatBehavior.Forever;

                path.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                path.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                g.Children.Add(path);
                path.BeginAnimation(Path.OpacityProperty, da);
            }
            return g;

            #endregion
        }

        #endregion
    }
}
