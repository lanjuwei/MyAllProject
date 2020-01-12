using BasicFunction.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

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
            Configure(FrameKey.MainFrame, PageKey.FaceLoginPage, "Views;component/Pages/FaceLoginPage.xaml");
            Configure(FrameKey.MainFrame, PageKey.PersonalCenterPage, "Views;component/Pages/PersonalCenterPage.xaml");
            Configure(FrameKey.MainFrame, PageKey.OperateBooksPage, "Views;component/Pages/OperateBooksPage.xaml");
            Configure(FrameKey.MainFrame, PageKey.ChangePasswordPage, "Views;component/Pages/ChangePasswordPage.xaml");
            Configure(FrameKey.MainFrame, PageKey.RegistrateFacePage, "Views;component/Pages/RegistrateFacePage.xaml");
        }

        #region
        
        public object Parameter { get; set; }

        public void NavigateTo(PageKey pageKey, object parameter = null, FrameKey frameKey = FrameKey.MainFrame)
        {
            lock (navigateObject)
            {
                Application.Current?.Dispatcher?.Invoke( () =>
                {
                    var item = naviModels.FirstOrDefault(x => x.FrameKey == frameKey);
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
                    if (item.MyFrame.Content != null)
                    {
                        item.LastPage = item.MyFrame.Content as Page;//记录上一个界面
                    }
                    if (item.PageDic.ContainsKey(pageKey))
                    {
                        item.MyFrame.Content = null;
                        item.MyFrame.Content = item.PageDic[pageKey];
                        if (pageKey != PageKey.MainPage)//首页不需要
                        {
                            LeftAndRightAllAnimation(item.PageDic[pageKey],true);//page animation
                        }
                    }
                    else
                    {
                        item.MyFrame.Navigate(item.UrlDic[pageKey]);
                    }
                });
            }
        }

        public void GoBack(FrameKey frameKey = FrameKey.MainFrame)
        {
            Application.Current?.Dispatcher?.Invoke(()=> 
            {
                var item = naviModels.FirstOrDefault(x => x.FrameKey == frameKey);
                if (item != null)
                {
                    if (item.LastPage != null)
                    {
                        item.MyFrame.Content = item.LastPage;
                        LeftAndRightAllAnimation(item.LastPage,false);
                    }
                }
                else
                {
                    throw new ArgumentException("can not find frame");
                }
            });
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
                item.PageDic.Add(d.Key, page);//add page         
                if (d.Key!= PageKey.MainPage)//首页不需要
                {
                    LeftAndRightAllAnimation(page,true);//page animation
                }
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

        private static bool _isLeft;
        private static Storyboard _allStoryboard;
        private static DoubleAnimation _allAnimation;

        private void LeftAndRightAllAnimation(FrameworkElement frameworkElement, bool isLeft ) 
        {
            _isLeft = isLeft;
            if (!(frameworkElement.RenderTransform is TransformGroup))
            {
                var transformGroup = new TransformGroup();
                transformGroup.Children.Add(new TranslateTransform() { X = 0, Y = 0 });
                frameworkElement.RenderTransformOrigin = new Point(0.5, 0.5);
                frameworkElement.RenderTransform = transformGroup;
                frameworkElement.Loaded +=(sender,e)=>
                {
                    if (_isLeft)
                    {
                        _allAnimation.From = frameworkElement.ActualWidth * 2 / 3;
                        _allAnimation.To = 0;
                    }
                    else
                    {
                        _allAnimation.From = -frameworkElement.ActualWidth * 2 / 3;
                        _allAnimation.To =0;
                    }
                    _allStoryboard.Begin();
                };
            }
            if (_allStoryboard==null)
            {
                _allStoryboard = new Storyboard();
                _allAnimation = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(0.8), EasingFunction = new CubicEase() };
                _allStoryboard.Children.Add(_allAnimation);
            }
            Storyboard.SetTarget(_allAnimation, frameworkElement);
            Storyboard.SetTargetProperty(_allAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
        }
        //public BitmapImage DrawElement( FrameworkElement element)
        //{
        //    int width = (int)element.ActualWidth; 
        //    int height = (int)element.ActualHeight; 
        //    RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Pbgra32); 
        //    bmp.Render(element); 
        //    PngBitmapEncoder encoder = new PngBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(bmp)); 
        //    BitmapImage bitmapImage = new BitmapImage();
        //    using (var memoryStream = new MemoryStream()) 
        //    { 
        //        encoder.Save(memoryStream); 
        //        memoryStream.Seek(0, SeekOrigin.Begin); 
        //        bitmapImage.BeginInit(); 
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad; 
        //        bitmapImage.StreamSource = memoryStream; 
        //        bitmapImage.EndInit(); 
        //    }
        //    return bitmapImage;
        //}

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
        HandwordLoginPage,
        FaceLoginPage,
        PersonalCenterPage,
        OperateBooksPage,
        ChangePasswordPage,
        RegistrateFacePage
    }
}
