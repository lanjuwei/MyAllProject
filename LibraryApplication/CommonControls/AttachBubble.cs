using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CommonControls
{
    /// <summary>
    /// 附加气泡 适用于控件的弹出形式
    /// </summary>
    public class AttachBubble
    {
        #region 附加属性
        
        public static DataTemplate GetBubbleContent(DependencyObject dp)
        {
            return (DataTemplate)dp.GetValue(BubbleContentProperty);
        }

        public static void SetBubbleContent(DependencyObject dp, string value)
        {
            dp.SetValue(BubbleContentProperty, value);
        }

        /// <summary>
        /// 气泡的内容模板 datatemplete
        /// </summary>
        public static readonly DependencyProperty BubbleContentProperty =
         DependencyProperty.RegisterAttached("BubbleContent",
             typeof(DataTemplate), typeof(AttachBubble),
             new FrameworkPropertyMetadata(null));

        public static ShowBubbleType GetShowBubbleType(DependencyObject dp)
        {
            return (ShowBubbleType)dp.GetValue(ShowBubbleTypeProperty);
        }

        public static void SetShowBubbleType(DependencyObject dp, string value)
        {
            dp.SetValue(ShowBubbleTypeProperty, value);
        }

        public static readonly DependencyProperty ShowBubbleTypeProperty =
            DependencyProperty.RegisterAttached("ShowBubbleType",
                typeof(ShowBubbleType), typeof(AttachBubble),
                new FrameworkPropertyMetadata(ShowBubbleType.None, OnShowBubblePropertyChanged));

        public static ShowBubbleLocation GetShowBubbleLocation(DependencyObject dp)
        {
            return (ShowBubbleLocation)dp.GetValue(ShowBubbleLocationProperty);
        }

        public static void SetShowBubbleLocation(DependencyObject dp, string value)
        {
            dp.SetValue(ShowBubbleLocationProperty, value);
        }

        public static readonly DependencyProperty ShowBubbleLocationProperty =
            DependencyProperty.RegisterAttached("ShowBubbleLocation",
                typeof(ShowBubbleLocation), typeof(AttachBubble),
                new FrameworkPropertyMetadata(ShowBubbleLocation.Bottom, OnShowBubbleLocationPropertyChanged));

        public static double GetHorizontalOffset(DependencyObject dp)
        {
            return (double )dp.GetValue(HorizontalOffsetProperty);
        }

        public static void SetHorizontalOffset(DependencyObject dp, string value)
        {
            dp.SetValue(HorizontalOffsetProperty, value);
        }

        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("HorizontalOffset",
                typeof(double), typeof(AttachBubble),
                new FrameworkPropertyMetadata(0.0, OnHorizontalOffsetPropertyChanged));

        public static double GetVerticalOffset(DependencyObject dp)
        {
            return (double)dp.GetValue(VerticalOffsetProperty);
        }

        public static void SetVerticalOffset(DependencyObject dp, string value)
        {
            dp.SetValue(VerticalOffsetProperty, value);
        }

        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset",
                typeof(double), typeof(AttachBubble),
                new FrameworkPropertyMetadata(0.0, OnVerticalOffsetPropertyChanged));
        #endregion

        private static void OnVerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            verticalOffset = GetVerticalOffset(element);
        }

        private static void OnHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            horizontalOffset = GetHorizontalOffset(element);
        }

        private static void OnShowBubbleLocationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            var t = GetShowBubbleLocation(element);
            switch (t)
            {
                case ShowBubbleLocation.Top:
                    placementMode = PlacementMode.Top;
                    break;
                case ShowBubbleLocation.Bottom:
                    placementMode = PlacementMode.Bottom;
                    break;
                case ShowBubbleLocation.Right:
                    placementMode = PlacementMode.Right;
                    break;
                case ShowBubbleLocation.Left:
                    placementMode = PlacementMode.Left;
                    break;
                default:
                    placementMode = PlacementMode.Bottom;
                    break;
            }
        }

        private static void OnShowBubblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element != null)
            {
                var type = GetShowBubbleType(element);
                switch (type)
                {
                    case ShowBubbleType.Press:
                        element.MouseDown -= element_MouseDown; //点击要快于总的点击事件
                        element.MouseDown += element_MouseDown;
                        break;
                    case ShowBubbleType.Load:
                        element.Loaded -= C_Loaded;
                        element.Loaded += C_Loaded;
                        break;
                    case ShowBubbleType.None:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                element.Unloaded -= Element_Unloaded;
                element.Unloaded += Element_Unloaded;
            }
            if (Application.Current.MainWindow == null) return;
            Application.Current.MainWindow.MouseDown -= MainWindow_MouseDown;
            Application.Current.MainWindow.MouseDown += MainWindow_MouseDown;
        }

        private static void Element_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_popup?.IsOpen == true)
            {
                _popup.IsOpen = false;
            }
        }

        private static void C_Loaded(object sender, RoutedEventArgs e)
        {
            OpenBubble(sender);
        }

        //private static void MainWindow_TouchDown(object sender, TouchEventArgs e)
        //{
        //    if (e.OriginalSource != _uiElement && !isOpening && _popup?.IsOpen == true)//触摸有问题 会在PreviewTouchDown前进入TouchDown OriginalSource为最开始的点击源
        //    {
        //        _popup.IsOpen = false;
        //    }
        //}

        private static void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != _uiElement && !isOpening && _popup?.IsOpen == true)
            {
                _popup.IsOpen = false;
            }
        }

        private static Popup _popup;
        private static object _uiElement;
        private static PlacementMode placementMode= PlacementMode.Bottom;
        private static double verticalOffset;
        private static double horizontalOffset;
        private static void element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenBubble(sender);
            e.Handled = true;
        }

        private static bool isOpening = false;
        //private static void element_MouseDown(object sender, TouchEventArgs e)
        //{
        //    isOpening = true;
        //    OpenBubble(sender);
        //    isOpening = false;
        //}

        private static async void OpenBubble(object ui)
        {
            var uiElement = ui as FrameworkElement;
            if (_popup == null)
            {
                _popup = new Popup
                {
                    Placement = placementMode,
                    AllowsTransparency = true,
                    PopupAnimation = PopupAnimation.Fade,
                    Child = new ContentControl(),
                    HorizontalOffset = horizontalOffset,
                    VerticalOffset = verticalOffset,
                };
                //元素右边
                //底色透明
                //动画
                _popup.StaysOpen = true;//不一直打开
            }
            if (Equals(_uiElement, uiElement) && _popup.IsOpen)
            {
                return;
            }
            _uiElement = uiElement;
            if (_popup.IsOpen)
            {
                _popup.IsOpen = false;
                await Task.Delay(200);
            }
            _popup.PlacementTarget = uiElement;//要放置的元素

            if (_popup.Child is ContentControl contentControl)
            {
                if (contentControl.ContentTemplate==null)
                {
                    contentControl.ContentTemplate = GetBubbleContent(uiElement);
                }
                if (uiElement.DataContext!=null&&contentControl.Content!= uiElement.DataContext)
                {
                    contentControl.Content = uiElement.DataContext;
                }
            }           
            await Task.Delay(200);

            _popup.IsOpen = true;//打开气泡
        }



        

        public enum ShowBubbleType
        {
            Press,
            Load,
            None
        }

        public enum ShowBubbleLocation
        {
            Top,
            Bottom,
            Right,
            Left,
        }
    }
}
