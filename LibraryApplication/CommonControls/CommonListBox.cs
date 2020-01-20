using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommonControls
{
    //xmlns:listbox="clr-namespace:PCL.CommonControl.ListBox;assembly=PCL.CommonControl"
    /*
     <listbox:CommonListBox  NormalItemOpacity="0.3"
                                               ItemsSource="{Binding ElementName=page,Path=MyList}"

                                               HorizontalContentAlignment="Center"
            VerticalContentAlignment="Center"
            
            ItemCornerRadius="16"
            
            NormalItemOpacity="0.3"
            NormalItemBackground="#4b585f"
            NormalItemThickness="2"           
            NormalItemBorderBrush="BlanchedAlmond"


            ItemContainerStyle="{StaticResource CommonListBoxItemStyle}"


            MouseOverItemBackground="#4b585f"
            MouseOverItemBorderBrush="#2dd1ff"
            MouseOverItemOpacity="0.5"
            
            PressedItemBorderBrush="#2dd1ff"
            PressedItemBackground="Pink"
            
            ItemPadding="30"
            ItemMargin="10">
                        </listbox:CommonListBox>
     */

    /// <summary>
    /// 通用ListBox 用来改变listboxitem背景颜色 边框 透明度等
    /// </summary>
    public class CommonListBox : ListBox
    {
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CommonListBox), new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius ItemCornerRadius
        {
            get { return (CornerRadius)GetValue(ItemCornerRadiusProperty); }
            set { SetValue(ItemCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty ItemCornerRadiusProperty =
             DependencyProperty.Register("ItemCornerRadius", typeof(CornerRadius), typeof(CommonListBox), new PropertyMetadata(new CornerRadius(0,0,0,0)));

        public double NormalItemOpacity
        {
            get { return (double)GetValue(ItemOpacityProperty); }
            set { SetValue(ItemOpacityProperty, value); }
        }
        public static readonly DependencyProperty ItemOpacityProperty =
             DependencyProperty.Register("NormalItemOpacity", typeof(double), typeof(CommonListBox), new PropertyMetadata(1.0));

        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }
        public static readonly DependencyProperty ItemMarginProperty =
             DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(CommonListBox), new PropertyMetadata(new Thickness(0,0,0,0)));

        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }
        public static readonly DependencyProperty ItemPaddingProperty =
             DependencyProperty.Register("ItemPadding", typeof(Thickness), typeof(CommonListBox), new PropertyMetadata(new Thickness(0,0,0,0)));

        #region 正常颜色


        public Color NormalItemBackground
        {
            get { return (Color)GetValue(NormalBackgroundProperty); }
            set { SetValue(NormalBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NormalBackgroundProperty =
             DependencyProperty.Register("NormalItemBackground", typeof(Color), typeof(CommonListBox), new PropertyMetadata(Colors.Transparent));

        public Color NormalItemBorderBrush
        {
            get { return (Color)GetValue(NormalBorderBrushProperty); }
            set { SetValue(NormalBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty NormalBorderBrushProperty =
             DependencyProperty.Register("NormalItemBorderBrush", typeof(Color), typeof(CommonListBox), new PropertyMetadata(Colors.Transparent));


        public Thickness NormalItemThickness
        {
            get { return (Thickness)GetValue(NormalThicknessProperty); }
            set { SetValue(NormalThicknessProperty, value); }
        }
        public static readonly DependencyProperty NormalThicknessProperty =
             DependencyProperty.Register("NormalItemThickness", typeof(Thickness), typeof(CommonListBox), new PropertyMetadata(new Thickness(0,0,0,0)));

        #endregion

        #region 鼠标滑过颜色


        public Color? MouseOverItemBackground
        {
            get { return (Color?)GetValue(MouseOverColorProperty); }
            set { SetValue(MouseOverColorProperty, value); }
        }
        public static readonly DependencyProperty MouseOverColorProperty =
             DependencyProperty.Register("MouseOverItemBackground", typeof(Color?), typeof(CommonListBox), new PropertyMetadata(null));

        public Color? MouseOverItemBorderBrush
        {
            get { return (Color?)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty MouseOverBorderBrushProperty =
             DependencyProperty.Register("MouseOverItemBorderBrush", typeof(Color?), typeof(CommonListBox), new PropertyMetadata(null));

        public Thickness? MouseOverItemThickness
        {
            get { return (Thickness?)GetValue(MouseOverThicknessProperty); }
            set { SetValue(MouseOverThicknessProperty, value); }
        }
        public static readonly DependencyProperty MouseOverThicknessProperty =
             DependencyProperty.Register("MouseOverItemThickness", typeof(Thickness?), typeof(CommonListBox), new PropertyMetadata(null));

        public double? MouseOverItemOpacity
        {
            get { return (double?)GetValue(MouseOverItemOpacityProperty); }
            set { SetValue(MouseOverItemOpacityProperty, value); }
        }
        public static readonly DependencyProperty MouseOverItemOpacityProperty =
             DependencyProperty.Register("MouseOverItemOpacity", typeof(double?), typeof(CommonListBox), new PropertyMetadata(null));

        #endregion

        #region 鼠标选定的颜色


        public Color? PressedItemBackground
        {
            get { return (Color?)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty PressedBackgroundProperty =
             DependencyProperty.Register("PressedItemBackground", typeof(Color?), typeof(CommonListBox), new PropertyMetadata(null));

        public Color? PressedItemBorderBrush
        {
            get { return (Color?)GetValue(PressedBorderBrushProperty); }
            set { SetValue(PressedBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty PressedBorderBrushProperty =
             DependencyProperty.Register("PressedItemBorderBrush", typeof(Color?), typeof(CommonListBox), new PropertyMetadata(null));

        public Thickness? PressedItemThickness
        {
            get { return (Thickness?)GetValue(PressedThicknessProperty); }
            set { SetValue(PressedThicknessProperty, value); }
        }
        public static readonly DependencyProperty PressedThicknessProperty =
             DependencyProperty.Register("PressedItemThickness", typeof(Thickness?), typeof(CommonListBox), new PropertyMetadata(null));

        public double? PressedItemOpacity
        {
            get { return (double?)GetValue(PressedItemOpacityProperty); }
            set { SetValue(PressedItemOpacityProperty, value); }
        }
        public static readonly DependencyProperty PressedItemOpacityProperty =
             DependencyProperty.Register("PressedItemOpacity", typeof(double?), typeof(CommonListBox), new PropertyMetadata(null));


        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(CommonListBox), new PropertyMetadata(0));

        public ICommand ScrollToCommond
        {
            get { return (ICommand)GetValue(ScrollToCommondProperty); }
            set { SetValue(ScrollToCommondProperty, value); }
        }
        public static readonly DependencyProperty ScrollToCommondProperty =
            DependencyProperty.Register("ScrollToCommond", typeof(ICommand), typeof(CommonListBox), new PropertyMetadata(null));

        public ICommand LoadMoreCommond
        {
            get { return (ICommand)GetValue(LoadMoreCommondProperty); }
            set { SetValue(LoadMoreCommondProperty, value); }
        }
        public static readonly DependencyProperty LoadMoreCommondProperty =
            DependencyProperty.Register("LoadMoreCommond", typeof(ICommand), typeof(CommonListBox), new PropertyMetadata(null));

        private ScrollViewer _scrollViewer;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged -= _scrollViewer_ScrollChanged;
            }
            _scrollViewer = this.GetTemplateChild("ScrollViewer") as ScrollViewer;
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged += _scrollViewer_ScrollChanged;
            }
        }

        private void _scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            var dvo = scrollViewer.VerticalOffset;
            var dvh = scrollViewer.ViewportHeight;
            var deh = scrollViewer.ExtentHeight;
            if (dvo != 0 && dvo + dvh >= deh - 3 && dvo + dvh <= deh)
            {
                ScrollToCommond?.Execute(null);
            }
            if (scrollViewer.ScrollableHeight <= 0 && this.Items?.Count < Total)
            {
                LoadMoreCommond?.Execute(null);
            }
        }

        #endregion

        public CommonListBox() 
        {
            var dic = new ResourceDictionary { Source = new Uri("/CommonControls;component/AllCommonControlStyle.xaml", UriKind.RelativeOrAbsolute) };
            this.Style = dic["CommonListBoxStyle"] as Style;
            this.ItemContainerStyle = dic["CommonListBoxItemStyle"] as Style;//有点特殊 不能讲这style放入总的listboxstyle
        }
        static CommonListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommonListBox), new FrameworkPropertyMetadata(typeof(CommonListBox)));
        }
    }
}
