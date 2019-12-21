using BasicFunction.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BasicServices.Navigation
{
    public class NaviService: INaviServiceInterface
    {
        private static NaviService _navigationService;
        public static NaviService Instance => _navigationService ?? (_navigationService = new NaviService());

        public void Init()
        {
            Configure(FrameKey.MainFrame, PageKey.MainPage, "Views;component/Pages/MainPage.xaml");//注册frame与frame所拥有的page
            Configure(FrameKey.MainFrame, PageKey.LoginPage, "Views;component/Pages/LoginPage.xaml");
            Configure(FrameKey.MainFrame, PageKey.HandwordLoginPage, "Views;component/Pages/HandwordLoginPage.xaml");
        }

        #region
        
        public object Parameter { get; set; }

        public void NavigateTo(PageKey pageKey, object parameter=null, FrameKey frameKey = FrameKey.MainFrame)
        {
            lock (navigateObject)
            {
                var item=naviModels.FirstOrDefault(x=>x.FrameKey== frameKey);
                if (item != null)//能找到item 并且item里面发frame为null 才去寻找frame控件
                {
                    if (item.MyFrame == null)
                    {
                        var frame = FindControlHelper.Instance.GetChildObject<Frame>(Application.Current.MainWindow, item.FrameKey.ToString());
                        if (frame != null)
                        {
                            frame.Navigated -= Frame_Navigated;
                            frame.Navigated += Frame_Navigated;
                            item.MyFrame = frame;
                        }
                        else
                        {
                            throw new ArgumentException("can not find frame");
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("can not find frame key");
                }
                if (!item.UrlDic.ContainsKey(pageKey))
                {
                    throw new ArgumentException($"No such page: {pageKey} At {frameKey}", "pageKey");
                }              
                Parameter = parameter;
                if (item.MyFrame.Content!=null)
                {
                    item.LastPage = item.MyFrame.Content as Page;//记录上一个界面
                }
                if (item.PageDic.ContainsKey(pageKey))
                {
                    item.MyFrame.Content = item.PageDic[pageKey];
                }
                else
                {
                    item.MyFrame.Navigate(item.UrlDic[pageKey]);
                }  
            }
        }

        public void GoBack(FrameKey frameKey = FrameKey.MainFrame)
        {           
            var item = naviModels.FirstOrDefault(x => x.FrameKey == frameKey);
            if (item!=null)
            {
                if (item.LastPage!=null)
                {
                    item.MyFrame.Content = item.LastPage;
                }
            }
            else
            {
                throw new ArgumentException("can not find frame");
            }
        }

        #region 不需要的内部细节

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var page = e.Content as Page;
            var frame = sender as Frame;
            var item =naviModels.FirstOrDefault(x=>x.MyFrame== frame);
            if (!item.PageDic.ContainsValue(page))
            {
                var d=item.UrlDic.FirstOrDefault(x=>x.Value==e.Uri);
                item.PageDic.Add(d.Key, page);
            }
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageUrl"></param>
        private void Configure(FrameKey frameKey, PageKey key, string pageUrl)
        {
            var item =naviModels.FirstOrDefault(x=>x.FrameKey== frameKey);
            if (item==null)
            {
                var m = new NaviModel
                {
                    FrameKey = frameKey
                };
                m.UrlDic.Add(key, new Uri(pageUrl, UriKind.RelativeOrAbsolute));
                item = m;
                naviModels.Add(item);
            }
            else
            {
                if (!item.UrlDic.ContainsKey(key))
                {
                    item.UrlDic.Add(key, new Uri(pageUrl, UriKind.RelativeOrAbsolute));
                }
            }
        }

        private class NaviModel
        {
            /// <summary>
            /// 当前key
            /// </summary>
            public FrameKey FrameKey { get; set; }
            /// <summary>
            /// //存储导航过的页面
            /// </summary>
            public Dictionary<PageKey, Page> PageDic { get; set; } = new Dictionary<PageKey, Page>();
            /// <summary>
            /// //为存储过的页面需要url
            /// </summary>
            public Dictionary<PageKey, Uri> UrlDic { get; set; } = new Dictionary<PageKey, Uri>();
            /// <summary>
            /// 当前的frmae
            /// </summary>
            public Frame MyFrame { get; set; }
            /// <summary>
            /// 上一个页面的key
            /// </summary>
            public Page LastPage { get; set; }
        }

        private List<NaviModel> naviModels = new List<NaviModel>();


        private object navigateObject = new object();//导航锁

        public NaviService()
        {

        }
        #endregion
        #endregion
    }
    /// <summary>
    /// Frame容器的key key必须为你当前frame的x:name
    /// </summary>
    public enum FrameKey
    {
        MainFrame
    }
    /// <summary>
    /// 页面的key
    /// </summary>
    public enum PageKey
    {
        MainPage,
        LoginPage,
        HandwordLoginPage
    }
}
