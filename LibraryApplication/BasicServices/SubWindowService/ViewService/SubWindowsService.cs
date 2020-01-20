using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BasicServices.SubWindowService.ViewService
{
    /// <summary>
    /// 弹窗服务 容器 LJW
    /// </summary>
    public class SubWindowsService: ISubWindowsService
    {

        #region Page注册

        //public const string Loginview = "Page1";//界面的key
        //public const string Loginview1 = "Page2";
        public const string FaceRecognitionFailurePage = "FaceRecognitionFailurePage";
        public const string UpdatePage = "UpdatePage";
        public const string PlaceBookPage = "PlaceBookPage";
        public const string PrintPage = "PrintPage";
        public const string NetworkAbnormalPage = "NetworkAbnormalPage";
        private void RegisterNormalPage()
        {
            //Configure(Loginview, "pack://application:,,,/BasicServices;component/SubWindowService/View/Page1.xaml");//注册界面 可使用绝对路径或者相对路径 WpfApplication2为项目名称
            //Configure(Loginview1, "pack://application:,,,/BasicServices;component/SubWindowService/View/Page2.xaml");
            Configure(UpdatePage, "pack://application:,,,/BasicServices;component/SubWindowService/View/UpdatePage.xaml");
            Configure(FaceRecognitionFailurePage, "pack://application:,,,/BasicServices;component/SubWindowService/View/FaceRecognitionFailurePage.xaml");
            Configure(PlaceBookPage, "pack://application:,,,/BasicServices;component/SubWindowService/View/PlaceBookPage.xaml");
            Configure(PrintPage, "pack://application:,,,/BasicServices;component/SubWindowService/View/PrintPage.xaml");
            Configure(NetworkAbnormalPage, "pack://application:,,,/BasicServices;component/SubWindowService/View/NetworkAbnormalPage.xaml");
        }

        #endregion

        private static SubWindowsService _subWindowsService;
        public static ISubWindowsService Instance => _subWindowsService ?? (_subWindowsService = new SubWindowsService());

        #region 不需要关注的内部细节
        /// <summary>
        /// 构造器 页面注册
        /// </summary>
        public SubWindowsService()
        {
            RegisterNormalPage();
        }


        /// <summary>
        /// 返回的结果
        /// </summary>
        public object Result { get; set; }

        public string WindowId { get; set; } = string.Empty;

        private readonly Dictionary<string, string> _pages = new Dictionary<string, string>();
        private readonly Dictionary<string, SubWindow> _subWindows = new Dictionary<string, SubWindow>();

        /// <summary>
        /// 打开窗口 除非你要在外界通过窗口返回的id控制窗口 否则请用viewmodel里的窗口id（可用来导航）
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="parameter"></param>
        /// <param name="IsDialog"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public string OpenWindow(string pageKey, object parameter = null, bool IsDialog=false,Point? position = null)
        {
            string page;
            var key = pageKey;
            lock (_pages)
            {
                if (!_pages.TryGetValue(key, out page))
                {
                    throw new ArgumentException($"Page not found: {key}. Did you forget to call SubWindowsService.Configure?", nameof(key));
                }
            }
            var subWindow = new SubWindow {Owner = Application.Current.MainWindow, CurrrentPoint = position};
            subWindow.Navigate(page, parameter);
            subWindow.Closed += SubWindow_Closed;
            _subWindows.Add(subWindow.Id, subWindow);
            WindowId = subWindow.Id;
            Result = IsDialog ? subWindow.ShowDialog() : subWindow.Show();
            return subWindow.Id;
        }


        /// <summary>
        /// 在子窗口上导航 
        /// </summary>
        /// <param name="subWinKey">子窗口id</param>
        /// <param name="pageKey">ViewModel</param>
        /// <param name="parameter">传入参数</param>
        public void Navigate(string subWinKey,string pageKey, object parameter = null)
        {
            string key = pageKey;
            if (_subWindows.ContainsKey(subWinKey))
            {
                var subWin = _subWindows[subWinKey];
                string page;
                lock (_pages)
                {
                    if (!_pages.TryGetValue(key, out page))
                    {
                        throw new ArgumentException(
                            $"Page not found: {key}. Did you forget to call SubWindowsService.Configure?",
                            nameof(key));
                    }
                }
                subWin.Navigate(page, parameter);
            }
        }

        /// <summary>
        /// 判断窗口是否存在
        /// </summary>
        /// <param name="subWinKey"></param>
        /// <returns></returns>
        public bool IsAliveWindow(string subWinKey)
        {
            return _subWindows.ContainsKey(subWinKey);
        }

        /// <summary>
        /// 获取窗口坐标
        /// </summary>
        /// <param name="subWinKey"></param>
        /// <returns></returns>
        public Point GetWindowPosition(string subWinKey)
        {
            if (_subWindows.ContainsKey(subWinKey))
            {
                var subWin = _subWindows[subWinKey];
                return new Point(subWin.Left, subWin.Top);
            }
            return new Point();
        }

        /// <summary>
        /// 隐藏单个窗口 以subWinKey为参数
        /// </summary>
        /// <param name="subWinKey"></param>
        public void HideWindow(string subWinKey)
        {
            if (_subWindows.ContainsKey(subWinKey))
            {
                var subWin = _subWindows[subWinKey];
                subWin.Hide();
            }
        }

        /// <summary>
        /// 隐藏所有窗口
        /// </summary>
        public void HideAllWindows()
        {
            foreach (var subWin in _subWindows.Values)
            {
                subWin.Hide();
            }
        }


        /// <summary>
        /// 展示所有窗口
        /// </summary>
        public void ShowAllWindows()
        {
            foreach (var subWin in _subWindows.Values)
            {
                subWin.Show();
            }
        }

        /// <summary>
        /// 关闭单个窗口 以subWinKey为参数
        /// </summary>
        /// <param name="subWinKey"></param>
        public void CloseWindow(string subWinKey)
        {
            if (_subWindows.ContainsKey(subWinKey))
            {
                var subWin = _subWindows[subWinKey];
                subWin.Close();
            }
        }

        /// <summary>
        /// 关闭所有窗口
        /// </summary>
        public void CloseAllWindows()
        {
            while (_subWindows.Count > 0)
            {
                var subWin = _subWindows.First().Value;
                subWin.Close();
            }
        }

        /// <summary>
        /// frame 后退
        /// </summary>
        /// <param name="subWinKey"></param>
        public void GoBack(string subWinKey)
        {
            if (_subWindows.ContainsKey(subWinKey))
            {
                var subWin = _subWindows[subWinKey];
                if (subWin.CanGoBack)
                {
                    subWin.GoBack();
                }
            }
        }

        /// <summary>
        /// frame 前进
        /// </summary>
        /// <param name="subWinKey"></param>
        public void GoForward(string subWinKey)
        {
            if (_subWindows.ContainsKey(subWinKey))
            {
                var subWin = _subWindows[subWinKey];
                if (subWin.CanGoForward)
                {
                    subWin.GoForward();
                }
            }
        }

        /// <summary>
        /// 普通提示page注册 不含vm 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageType"></param>
        private void Configure(string key, string pageType)
        {
            lock (_pages)
            {
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException(string.Format("The key {0} is already configured in SubWindowsService", key));
                }

                if (_pages.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException(string.Format("This type is already configured with key {0}", _pages.First(p => p.Value == pageType).Key));
                }

                _pages.Add(key, pageType);
            }
        }

        private void SubWindow_Closed(object sender, EventArgs e)
        {
            var subWindow = sender as SubWindow;
            if (subWindow!=null&& _subWindows.ContainsKey(subWindow.Id))
            {
                subWindow.Closed -= SubWindow_Closed;
                _subWindows.Remove(subWindow.Id);
            }
        }

        #endregion



    }
}
