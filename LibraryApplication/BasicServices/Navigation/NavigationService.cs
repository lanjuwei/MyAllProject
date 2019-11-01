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
    public class NavigationService
    {
        private static NavigationService _navigationService;
        public static NavigationService Instance => _navigationService ?? (_navigationService = new NavigationService());

        private  readonly Dictionary<string,Frame> _frameDic=new Dictionary<string, Frame>();
        private  readonly Dictionary<string, Page> _pageDic = new Dictionary<string, Page>();
        private  readonly Dictionary<string, Uri> _urlDic = new Dictionary<string, Uri>();
        private string _currentKey  ;
        private readonly Dictionary<string, Page> _lastPageDic= new Dictionary<string, Page>();

        public NavigationService()
        {
            Configure(FrameName.MainFrame,PageName.MainPage, "Views;component/Pages/MainPage.xaml");//注册frame与frame所拥有的page
            Configure(FrameName.MainFrame, PageName.LoginPage, "Views;component/Pages/LoginPage.xaml");
        }

        public object Parameter { get; set; }

        public void NavigateTo(string pageKey, object parameter=null, string frameKey = FrameName.MainFrame)
        {
            Frame frame = null;
            lock (_frameDic)
            {
                if (!_frameDic.ContainsKey(frameKey))
                {
                    throw new ArgumentException($"No such frame: {pageKey} ", "frameKey");
                }
                frame = _frameDic[frameKey];
                frame.Navigated -= Frame_Navigated;
                frame.Navigated += Frame_Navigated;
            }
            lock (_urlDic)
            {
                if (!_urlDic.ContainsKey(pageKey))
                {
                    throw new ArgumentException($"No such page: {pageKey} ", "pageKey");
                }
                _currentKey = pageKey;
                if (frame.Content!=null)
                {
                    _lastPageDic[frameKey] = frame.Content as Page;
                }
                Parameter = parameter;
                if (_pageDic.ContainsKey(pageKey))
                {
                    frame.Content = _pageDic[pageKey];
                }
                else
                {
                    frame.Navigate(_urlDic[pageKey]);
                }
    
            }

        }

        public void GoBack(string frameKey = FrameName.MainFrame)
        {
            if (_lastPageDic.ContainsKey(frameKey))
            {
                _frameDic[frameKey].Content = _lastPageDic[frameKey];
            }
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var page = e.Content as Page;
            if (!_pageDic.ContainsValue(page))
            {
                _pageDic.Add(_currentKey, page);
            }
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageUrl"></param>
        private void Configure(string frameKey,string key, string pageUrl)
        {
            lock (_frameDic)
            {
                if (!_frameDic.ContainsKey(frameKey))
                {
                    var frame = FindControlHelper.Instance.GetChildObject<Frame>(Application.Current.MainWindow, frameKey);
                    if (frame!=null)
                    {
                        _frameDic.Add(frameKey, frame);
                        _lastPageDic.Add(frameKey,null);
                    }
                    else
                    {
                        throw new ArgumentException("can not find frame");
                    }
                }
            }
            var url=new Uri(pageUrl,UriKind.RelativeOrAbsolute);
            lock (_urlDic)
            {
                if (!_urlDic.ContainsKey(key))
                {
                    _urlDic.Add(key, url);
                }
            }
        }
    }
}
