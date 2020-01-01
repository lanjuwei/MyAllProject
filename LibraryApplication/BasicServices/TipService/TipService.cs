using BaseSetting.Needs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using ToolTipUserControl = BasicServices.TipService.Views.ToolTipUserControl;
using TopUserControl = BasicServices.TipService.Views.TopUserControl;

namespace BasicServices.TipService
{
    //todo:TipService.Instance.ShowTip(ToastService.ToolTip, 1500, "账号已注册人脸");
    //todo:TipService.Instance.ShowTip(ToastService.Top, "账号已注册人脸",isNeedTimer:false);
    /// <summary>
    /// 用于各种提示用的TipService
    /// </summary>
    public class TipService
    {

        private static TipService _tipService;
        public static TipService Instance => _tipService ?? (_tipService = new TipService());
        /// <summary>
        /// 弹框提示 可await 如果不需要则不用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public void ShowTip(string key, double time = 0, object p = null, bool isNeedTimer = true)
        {
             _tipService.ShowAsync(key, time, p, isNeedTimer);
        }

        /// <summary>
        /// 关闭弹框提示
        /// </summary>
        public void CloseTip()
        {
            _tipService.CloseAsync();
        }

        public const string ToolTip = "ToolTip";
        public const string Top = "Top";

        /// <summary>
        /// 此处用于添加控件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static UserControl AddControl(string key, object p)
        {
            UserControl uiElement;
            switch (key)
            {
                case ToolTip:
                    uiElement = _dictionary.ContainsKey(key) ? _dictionary[key] : new ToolTipUserControl();//黑色提示 经常使用 存入一整个usercontrol到内存 加快响应速度
                    break;
                case Top:
                    uiElement=new TopUserControl();//示例之二 不常使用的控件 实例化即可 使用完后释放
                    break;
                default:
                    throw new Exception("No such key");
            }
            uiElement.DataContext = p;//参数传递
            return uiElement;
        }

        #region private
        private static DoubleAnimationUsingKeyFrames _showStory;
        private static DoubleAnimationUsingKeyFrames _hideStory;
        private Popup _currentPopup;
        private CancellationTokenSource _token;
        private static bool _isCompele = true;//默认是已经调用完了
        private static bool _isNeedTimer;
        /// <summary>
        /// 存入该字典的条件 必须是该UserControl经常要用到 否则不必存入字典增加内存的消耗
        /// </summary>
        private static readonly Dictionary<string, UserControl> _dictionary = new Dictionary<string, UserControl>();
        private int _time;
        private TipService()
        {

        }



        private void CloseAsync()
        {
            if (_isNeedTimer)
            {
                _token.Cancel();//取消线程
            }
            else
            {
                Application.Current.Dispatcher?.Invoke(()=> 
                {
                    _currentPopup?.Child?.BeginAnimation(UIElement.OpacityProperty, _hideStory);//动手关闭popup
                });
            }
        }

        /// <summary>
        /// 异步方法 需要和awitey一起使用 不需要等待返回结果 如果需要秩序将void 变成 task即可 然后在上一级添加async
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <param name="p"></param>
        private  void ShowAsync(string key, double time, object p, bool isNeedTimr)
        {
            if (!_isCompele)
            {
                return;
            }
            Application.Current?.Dispatcher?.BeginInvoke(new Action(async () => 
            {           
            _isCompele = false;

                _currentPopup = new Popup
                {                 
                    AllowsTransparency = true,
                    Child = new Viewbox() { Child=new Grid() { Width= IndividualNeeds.Instance.PageVariables.RootGridWidth, Height= IndividualNeeds.Instance.PageVariables.RootGridHeight } },
                    PlacementTarget = Application.Current.MainWindow,
                    Placement= PlacementMode.Center,
                };
                _currentPopup.Width = Application.Current.MainWindow.ActualWidth;
                _currentPopup.Height = Application.Current.MainWindow.ActualHeight;
                if (_currentPopup.Child is Viewbox viewbox&& viewbox.Child is Grid grid)
                {
                    grid.Children.Clear();
                    grid.Children.Add(AddControl(key, p));
                }

                _currentPopup.IsOpen = true;
           

            if (_showStory == null)
            {
                var d = new DoubleAnimationUsingKeyFrames();
                var c = new CircleEase() { EasingMode = EasingMode.EaseOut };
                d.KeyFrames.Add(new EasingDoubleKeyFrame()
                { EasingFunction = c, KeyTime = TimeSpan.FromMilliseconds(0), Value = 0 });
                d.KeyFrames.Add(new EasingDoubleKeyFrame()
                { EasingFunction = c, KeyTime = TimeSpan.FromMilliseconds(100), Value = 1 });
                _showStory = d;
                d = new DoubleAnimationUsingKeyFrames();
                d.KeyFrames.Add(new EasingDoubleKeyFrame()
                { EasingFunction = c, KeyTime = TimeSpan.FromMilliseconds(0), Value = 1 });
                d.KeyFrames.Add(new EasingDoubleKeyFrame()
                { EasingFunction = c, KeyTime = TimeSpan.FromMilliseconds(100), Value = 0 });
                _hideStory = d;
            }

            if (_hideStory != null) _hideStory.Completed += _hideStory_Completed;
            
                _currentPopup?.Child?.BeginAnimation(UIElement.OpacityProperty, _showStory);
            
            _isNeedTimer = isNeedTimr;
            if (isNeedTimr)//需要内部计时器
            {
                _token = new CancellationTokenSource(); //被使用和释放的token必须重新实例化
                await Task.Run(() =>
                {
                    while (!_token.IsCancellationRequested && _time <= time)
                    {
                        _time += 1000;
                        Thread.Sleep(1000);
                    }
                }, _token.Token);
                _time = 0;
                _token?.Dispose(); //释放当前线程
              
                    _currentPopup?.Child?.BeginAnimation(UIElement.OpacityProperty, _hideStory);
               
                await Task.Delay(150); //消除由于消失动画带来的延迟
            }
            }));
        }

        private void _hideStory_Completed(object sender, object e)
        {

            if (_currentPopup != null)
            {
                _currentPopup.IsOpen = false;
                _currentPopup.Child = null;
                _currentPopup = null;
            }
            if (_hideStory != null)
            {
                _hideStory.Completed -= _hideStory_Completed;
            }

            _isCompele = true;
        }
        #endregion
    }
}
