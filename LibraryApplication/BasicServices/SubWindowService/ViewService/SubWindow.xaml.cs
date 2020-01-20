using BaseSetting.Needs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BasicServices.SubWindowService.ViewService
{
    /// <summary>
    /// SubWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SubWindow : Window
    {

        public string Id { get; } = Guid.NewGuid().ToString();

        public Point? CurrrentPoint { get; set; }

        public bool IsHaveBackground { get; set; }


        public SubWindow()
        {
            InitializeComponent();
            Loaded += SubWindow_Loaded;
            Unloaded += SubWindow_Unloaded;
            Width = Application.Current.MainWindow.ActualWidth;
            Height = Application.Current.MainWindow.ActualHeight;
            //RootGrid.Width = SystemParameters.PrimaryScreenWidth;//可配置 比例 viewbox会以一定的比例缩放
            //RootGrid.Height = SystemParameters.PrimaryScreenHeight;
        }



        private void SubWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Frame.Navigated -= _frame_Navigated;
            var page = Frame.Content as Page;
            if (page != null)
            {
                var vm = page.DataContext as SubWindowBase;
                if (vm != null)
                {
                    vm.UnLoaded();
                }
            }
            var closeStoryboard = this.FindResource("CloseStoryboard") as Storyboard;
            if (closeStoryboard != null)
            {
                closeStoryboard.Completed -= CloseStoryboard_Completed;
            }
            //Frame.SizeChanged -= Page_SizeChanged;
        }

        private void SubWindow_Loaded(object sender, RoutedEventArgs e)
        {
         
            Frame.Navigated += _frame_Navigated;
        }

        private void _frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var page = Frame.Content as Page;
            if (page != null)
            {
                var vm = page.DataContext as SubWindowBase; //有vm的page vm必须继承SubWindowBase
                if (vm == null)
                {
                    vm = new SubWindowBase();
                    page.DataContext = vm;
                }

                vm.DragAction = DragWindow;
                vm.CloseWithParameter = CloseWindow;
                vm.Close = NormalCloseWindow;
                vm.LoadParamerter = _parameter;
                vm.Loaded();
                vm.Id = this.Id;

                //Frame.SizeChanged += Page_SizeChanged; 除非需要窗口拖动 否则不须计算宽高
            }
            else
            {
                throw new ArgumentException("Content of Frame is not a Page");
            }
        }

        //private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (!e.NewSize.IsEmpty)
        //    {
        //        if (CurrrentPoint.HasValue)
        //        {
        //            this.Left = CurrrentPoint.Value.X;
        //            this.Top = CurrrentPoint.Value.Y;
        //        }
        //        else
        //        {
        //            double workHeight = SystemParameters.WorkArea.Height;
        //            double workWidth = SystemParameters.WorkArea.Width;
        //            if (e.NewSize.Width < workWidth && e.NewSize.Height < workHeight)
        //            {
        //                Left = (workWidth - e.NewSize.Width) / 2;
        //                Top = (workHeight - e.NewSize.Height) / 2;
        //            }
        //            else
        //            {
        //                Left = 0;
        //                Top = 0;
        //            }
        //        }

        //    }
        //}

        public bool CanGoBack => Frame.CanGoBack;

        public bool CanGoForward => Frame.CanGoForward;

        public void GoBack()
        {
            Frame.GoBack();
        }

        public void GoForward()
        {
            Frame.GoForward();
        }

        private object _parameter;
        private object _result;

        /// <summary>
        /// frame导航
        /// </summary>
        /// <param name="sourcePageType"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool Navigate(string sourcePageType, object parameter = null)
        {
            _parameter = parameter;
            return Frame.Navigate(new Uri(sourcePageType, UriKind.RelativeOrAbsolute), parameter);
        }

        /// <summary>
        /// Show的形式展示窗口
        /// </summary>
        public new object Show()
        {
            base.Show();
            return _result;
        }

        /// <summary>
        /// ShowDialog的形式展示窗口
        /// </summary>
        public new object ShowDialog()
        {
            base.ShowDialog();
            return _result;
        }

        private void CloseWindow(object result)
        {
            _result = result;
            var closeStoryboard = this.FindResource("CloseStoryboard") as Storyboard;
            if (closeStoryboard != null)
            {
                closeStoryboard.Completed += CloseStoryboard_Completed;
                closeStoryboard.Begin(this);//开始动画
            }
            else
            {
                this.Close();
            }
        }

        private void NormalCloseWindow()
        {
            var closeStoryboard = this.FindResource("CloseStoryboard") as Storyboard;
            if (closeStoryboard != null)
            {
                closeStoryboard.Completed += CloseStoryboard_Completed;
                closeStoryboard.Begin(this);//开始动画
            }
            else
            {
                this.Close();
            }
        }

        private void CloseStoryboard_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 拖拽窗口
        /// </summary>
        private void DragWindow()
        {
            this.DragMove();
        }
    }
}
