using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CommonControls
{
    /// <summary>
    /// GifImagePlayerUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class GifImagePlayerUserControl : UserControl
    {
        #region 成员变量
        /// <summary>
        /// gif动画的System.Drawing.Bitmap
        /// </summary>
        private Bitmap gifBitmap;

        /// <summary>
        /// 用于显示每一帧的BitmapSource
        /// </summary>
        private BitmapSource bitmapSource;

        #endregion

        #region 构造器
        public GifImagePlayerUserControl()
        {
            InitializeComponent();
            this.Loaded -= GifImagePlayerUserControl_Loaded;
            this.Loaded += GifImagePlayerUserControl_Loaded;
            this.Unloaded -= GifImagePlayerUserControl_Unloaded;
            this.Unloaded += GifImagePlayerUserControl_Unloaded;
        }
        static GifImagePlayerUserControl()
        {
            GifImagePathProperty = DependencyProperty.Register("GifImagePath", typeof(string), typeof(GifImagePlayerUserControl), new FrameworkPropertyMetadata(""
, OnImagePathChanged));
        }
        #endregion

        #region 依赖属性
        /// <summary>
        /// 图片路径
        /// </summary>
        public static readonly DependencyProperty GifImagePathProperty = null;
        /// <summary>
        /// 图片路径
        /// </summary>
        public string GifImagePath
        {
            get { return (string)GetValue(GifImagePathProperty); }
            set
            {
                SetValue(GifImagePathProperty, value);
            }
        }

        #endregion


        #region 依赖属性变更事件
        private static void OnImagePathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GifImagePlayerUserControl gifImageUc = sender as GifImagePlayerUserControl;
            if (gifImageUc == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(gifImageUc.GifImagePath) || string.IsNullOrWhiteSpace(gifImageUc.GifImagePath))
            {
                return;
            }
            if (!File.Exists(gifImageUc.GifImagePath))
            {
                return;
            }
            gifImageUc.StopAnimate();
            gifImageUc.InitGifImage(gifImageUc.GifImagePath);//初始化控件
            gifImageUc.StartAnimate();//播放Gif图片
        }

        #endregion

        #region 控件事件
        private void GifImagePlayerUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.StartAnimate();//加载控件，播放gif图片
        }
        private void GifImagePlayerUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.StopAnimate();//当控件卸载，停止播放gif图片
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 初始化控件
        /// </summary>
        public void InitGifImage(string gifPath)
        {
            if (gifBitmap!=null)
            {
                gifBitmap.Dispose();
            }
            this.gifBitmap = new Bitmap(gifPath);
            this.bitmapSource = this.GetBitmapSource();
            this.img.Source = this.bitmapSource;
        }
        /// <summary>
        /// 从System.Drawing.Bitmap中获得用于显示的那一帧图像的BitmapSource
        /// </summary>
        /// <returns></returns>
        private BitmapSource GetBitmapSource()
        {
            IntPtr handle = IntPtr.Zero;

            try
            {
                handle = this.gifBitmap.GetHbitmap();
                this.bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    DeleteObject(handle);
                }
            }
            return this.bitmapSource;
        }

        /// <summary>
        /// Start animation
        /// </summary>
        public void StartAnimate()
        {
            if (this.gifBitmap != null)
            {
                ImageAnimator.Animate(this.gifBitmap, this.OnFrameChanged);
            }
        }

        /// <summary>
        /// Stop animation
        /// </summary>
        public void StopAnimate()
        {
            if (this.gifBitmap != null)
            {
                ImageAnimator.StopAnimate(this.gifBitmap, this.OnFrameChanged);
            }
        }
        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                ImageAnimator.UpdateFrames(); // 更新到下一帧
                if (this.bitmapSource != null)
                {
                    this.bitmapSource.Freeze();
                }
                this.bitmapSource = this.GetBitmapSource();
                this.img.Source = this.bitmapSource;
                this.InvalidateVisual();
            });
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject(IntPtr hObject);
        #endregion

    }
}
