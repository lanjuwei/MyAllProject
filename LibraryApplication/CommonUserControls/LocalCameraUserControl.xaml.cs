using AForge.Controls;
using AForge.Video.DirectShow;
using BaseSetting.Needs;
using BasicFunction.Helper;
using BasicFunction.Log;
using BasicServices.Navigation;
using BasicServices.SubWindowService.ViewService;
using BasicServices.TipService;
using Model;
using Model.Entity;
using Model.SDKModels;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Face;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonUserControls
{
    /// <summary>
    /// LocalCameraUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LocalCameraUserControl : UserControl
    {
        //private static VideoCapture _rgbVideoCapture;
        //private static VideoCapture _irVideoCapture;
        private FilterInfoCollection filterInfoCollection;
        private static bool isDoubleShot;
        private CameraStatus _cameraStatus;
        private bool _isShotsFace;
        private CascadeClassifier _cascadeClassifier;
        private LBPHFaceRecognizer _iBPHFaceRecognizer;
        private Dictionary<int, string> _labelsToId = new Dictionary<int, string>();
        private bool _isDetectFace = false;
        private bool _isRecognitionFace = false;
        private bool isLoad = false;
        /// <summary>
        /// 图片最大大小
        /// </summary>
        private long maxSize = 1024 * 1024 * 2;
        private float threshold = 0.8f;
        /// <summary>
        /// 允许误差范围
        /// </summary>
        private int allowAbleErrorRange = 40;
        /// <summary>
        /// 本地人脸特征列表还有对应的ID：id_password
        /// </summary>
        private static Dictionary<IntPtr, string> imagesFeatureList = new Dictionary<IntPtr, string>();
        /// <summary>
        /// 引擎Handle
        /// </summary>
        private  IntPtr pImageEngine = IntPtr.Zero;
        /// <summary>
        /// 视频引擎Handle
        /// </summary>
        private  IntPtr pVideoEngine = IntPtr.Zero;
        /// <summary>
        /// RGB视频引擎 FR Handle 处理   FR和图片引擎分开，减少强占引擎的问题
        /// </summary>
        private  IntPtr pVideoRGBImageEngine = IntPtr.Zero;
        /// <summary>
        /// IR视频引擎 FR Handle 处理   FR和图片引擎分开，减少强占引擎的问题
        /// </summary>
        private  IntPtr pVideoIRImageEngine = IntPtr.Zero;
        /// <summary>
        /// RGB 摄像头索引
        /// </summary>
        private  int rgbCameraIndex = 0;
        /// <summary>
        /// IR 摄像头索引
        /// </summary>
        private  int irCameraIndex = 0;
        /// <summary>
        /// RGB摄像头设备
        /// </summary>
        private static VideoCaptureDevice rgbDeviceVideo;
        /// <summary>
        /// IR摄像头设备
        /// </summary>
        private static VideoCaptureDevice irDeviceVideo;
        private FaceTrackUnit trackRGBUnit = new FaceTrackUnit();
        private FaceTrackUnit trackIRUnit = new FaceTrackUnit();
        private Font font = new Font(System.Drawing.FontFamily.GenericSerif, 10f, System.Drawing.FontStyle.Bold);
        private SolidBrush redBrush = new SolidBrush(System.Drawing.Color.Red);
        private SolidBrush greenBrush = new SolidBrush(System.Drawing.Color.Green);
        private bool isRGBLock = false;
        private bool isIRLock = false;
        private MRECT allRect = new MRECT();
        private bool isRectLock = false;
        private static VideoSourcePlayer rgbVideoSource= new VideoSourcePlayer() { BorderColor = System.Drawing.Color.White, BackColor = System.Drawing.Color.White }; 
        private static VideoSourcePlayer irVideoSource = new VideoSourcePlayer() { BorderColor = System.Drawing.Color.White, BackColor = System.Drawing.Color.White };
        private static WindowsFormsHost windowsFormsHost = new WindowsFormsHost() { Background = new SolidColorBrush(Colors.Transparent) };
        private static WindowsFormsHost windowsFormsHost1 = new WindowsFormsHost() { Background = new SolidColorBrush(Colors.Transparent) };
        private bool isIrLive = false;
        public LocalCameraUserControl()
        {
            InitializeComponent();
            Loaded += LocalCameraUserControl_Loaded;
            Unloaded += LocalCameraUserControl_Unloaded;
            Application.Current.MainWindow.Closed += MainWindow_Closed;
            //_cascadeClassifier = new CascadeClassifier(@"haarcascade_frontalface_alt.xml");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                //销毁引擎
                int retCode = ASFFunctions.ASFUninitEngine(pImageEngine);
                Console.WriteLine("UninitEngine pImageEngine Result:" + retCode);
                //销毁引擎
                retCode = ASFFunctions.ASFUninitEngine(pVideoEngine);
                Console.WriteLine("UninitEngine pVideoEngine Result:" + retCode);

                //销毁引擎
                retCode = ASFFunctions.ASFUninitEngine(pVideoRGBImageEngine);
                Console.WriteLine("UninitEngine pVideoImageEngine Result:" + retCode);

                //销毁引擎
                retCode = ASFFunctions.ASFUninitEngine(pVideoIRImageEngine);
                Console.WriteLine("UninitEngine pVideoIRImageEngine Result:" + retCode);
                foreach (var item in imagesFeatureList)
                {
                    MemoryUtil.Free(item.Key);//释放内存
                }
                irVideoSource?.Dispose();//释放
                rgbVideoSource?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("UninitEngine pImageEngine Error:" + ex.Message);
            }
        }

        public static readonly DependencyProperty IsShotsFaceProperty;
        /// <summary>
        /// 是否截取人脸图片
        /// </summary>
        public bool IsShotsFace
        {
            get { return (bool)GetValue(IsShotsFaceProperty); }
            set { SetValue(IsShotsFaceProperty, value); }
        }

        public static readonly DependencyProperty IsDetectFaceProperty;
        /// <summary>
        /// 是否检测人脸图片
        /// </summary>
        public bool IsDetectFace
        {
            get { return (bool)GetValue(IsDetectFaceProperty); }
            set { SetValue(IsDetectFaceProperty, value); }
        }

        public static readonly DependencyProperty IsRecognitionFaceProperty;
        /// <summary>
        /// 是否识别人脸
        /// </summary>
        public bool IsRecognitionFace
        {
            get { return (bool)GetValue(IsRecognitionFaceProperty); }
            set { SetValue(IsRecognitionFaceProperty, value); }
        }

        public static readonly DependencyProperty CameraStatusProperty;
        /// <summary>
        /// 视频的播放状态
        /// </summary>
        public CameraStatus Status
        {
            get { return (CameraStatus)GetValue(CameraStatusProperty); }
            set { SetValue(CameraStatusProperty, value); }
        }



        static LocalCameraUserControl()
        {
            IsShotsFaceProperty =DependencyProperty.Register("IsShotsFace", typeof(bool), typeof(LocalCameraUserControl), new PropertyMetadata(false, IsCaptureVideoCallBack));
            CameraStatusProperty =DependencyProperty.Register("Status", typeof(CameraStatus), typeof(LocalCameraUserControl), new PropertyMetadata(CameraStatus.Stop, CameraStatusCallBack));
            IsDetectFaceProperty =DependencyProperty.Register("IsDetectFace", typeof(bool), typeof(LocalCameraUserControl), new PropertyMetadata(false, IsDetectFaceCallBack));
            IsRecognitionFaceProperty =DependencyProperty.Register("IsRecognitionFace", typeof(bool), typeof(LocalCameraUserControl), new PropertyMetadata(false, IsRecognitionFaceCallBack));      
            
        }

        private static void IsRecognitionFaceCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as LocalCameraUserControl;
            c._isRecognitionFace = (bool)e.NewValue;
        }

        private static void IsDetectFaceCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as LocalCameraUserControl;
            c._isDetectFace = (bool)e.NewValue;
        }

        private static void CameraStatusCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as LocalCameraUserControl;
            c._cameraStatus = (CameraStatus)e.NewValue;
        }

        private static void IsCaptureVideoCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as LocalCameraUserControl;
            c._isShotsFace = (bool)e.NewValue;
        }


        private void LocalCameraUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BusyIndicator1.IsBusy = true;
            Task.Run(()=> 
            {
                if (!isLoad)
                {
                    InitEngines();//加载引擎一次就好
                    isLoad = true;
                }
                GetLocalFaceImage();//获取本地图片
                this.Dispatcher?.Invoke(()=> 
                {
                    initVideo();
                });
                
            });
        }
        private  void LocalCameraUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
           
                videoGrid.Children.Clear();//在windowform控件未被释放掉之前移除
                UnResgistPaint();
                if (rgbVideoSource?.IsRunning == true)
                {
                    rgbVideoSource.SignalToStop();
                    rgbVideoSource?.Stop();
                }
                if (irVideoSource?.IsRunning == true)
                {
                    irVideoSource.SignalToStop();
                    irVideoSource?.Stop();
                }
                trackRGBUnit.message = string.Empty;
            

        }
        /// <summary>
        /// 初始化引擎 一次就好
        /// </summary>
        private  void InitEngines()
        {
            //读取配置文件
            AppSettingsReader reader = new AppSettingsReader();
            string appId = (string)reader.GetValue("APP_ID", typeof(string));
            string sdkKey64 = (string)reader.GetValue("SDKKEY64", typeof(string));
            string sdkKey32 = (string)reader.GetValue("SDKKEY32", typeof(string));
            rgbCameraIndex = (int)reader.GetValue("RGB_CAMERA_INDEX", typeof(int));
            irCameraIndex = (int)reader.GetValue("IR_CAMERA_INDEX", typeof(int));
            //判断CPU位数
            var is64CPU = Environment.Is64BitProcess;
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(is64CPU ? sdkKey64 : sdkKey32))
            {
                TipService.Instance.ShowTip(TipService.ToolTip, 1000, string.Format("请在App.config配置文件中先配置APP_ID和SDKKEY{0}!", is64CPU ? "64" : "32"));
                return;
            }
            //在线激活引擎    如出现错误，1.请先确认从官网下载的sdk库已放到对应的bin中，2.当前选择的CPU为x86或者x64
            int retCode = 0;
            try
            {
                retCode = ASFFunctions.ASFActivation(appId, is64CPU ? sdkKey64 : sdkKey32);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("无法加载 DLL"))
                {
                    TipService.Instance.ShowTip(TipService.ToolTip, 1000, "请将sdk相关DLL放入bin对应的x86或x64下的文件夹中!");
                }
                else
                {
                    TipService.Instance.ShowTip(TipService.ToolTip, 1000, "激活引擎失败!");
                }
                return;
            }
            Logger.Info("Activate Result:" + retCode);
            //初始化引擎
            uint detectMode = DetectionMode.ASF_DETECT_MODE_IMAGE;
            //Video模式下检测脸部的角度优先值
            int videoDetectFaceOrientPriority = ASF_OrientPriority.ASF_OP_0_HIGHER_EXT;
            //Image模式下检测脸部的角度优先值
            int imageDetectFaceOrientPriority = ASF_OrientPriority.ASF_OP_0_ONLY;
            //人脸在图片中所占比例，如果需要调整检测人脸尺寸请修改此值，有效数值为2-32
            int detectFaceScaleVal = 16;
            //最大需要检测的人脸个数
            int detectFaceMaxNum = 5;
            //引擎初始化时需要初始化的检测功能组合
            int combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_AGE | FaceEngineMask.ASF_GENDER | FaceEngineMask.ASF_FACE3DANGLE;
            //初始化引擎，正常值为0，其他返回值请参考http://ai.arcsoft.com.cn/bbs/forum.php?mod=viewthread&tid=19&_dsign=dbad527e
            retCode = ASFFunctions.ASFInitEngine(detectMode, imageDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pImageEngine);
            Logger.Info("InitEngine Result:" + retCode);
            if (retCode != 0)
            {
                TipService.Instance.ShowTip(TipService.ToolTip, 1000, "引擎初始化失败!");
                return;
            }
            //初始化视频模式下人脸检测引擎
            uint detectModeVideo = DetectionMode.ASF_DETECT_MODE_VIDEO;
            int combinedMaskVideo = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION;
            retCode = ASFFunctions.ASFInitEngine(detectModeVideo, videoDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMaskVideo, ref pVideoEngine);
            //RGB视频专用FR引擎
            detectFaceMaxNum = 1;
            combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_LIVENESS;
            retCode = ASFFunctions.ASFInitEngine(detectMode, imageDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pVideoRGBImageEngine);

            //IR视频专用FR引擎
            combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_IR_LIVENESS;
            retCode = ASFFunctions.ASFInitEngine(detectMode, imageDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pVideoIRImageEngine);
            Logger.Info("InitVideoEngine Result:" + retCode);           
        }
        private void initVideo()
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (filterInfoCollection.Count == 0)
            {
                TipService.Instance.ShowTip(TipService.ToolTip, 1000, "未检测到摄像头，请确保已安装摄像头或驱动!");
                return;
            }
            //获取filterInfoCollection的总数
            int maxCameraCount = filterInfoCollection.Count;
            //如果配置了两个不同的摄像头索引
            if (rgbCameraIndex != irCameraIndex && maxCameraCount >= 2)
            {
                //RGB摄像头加载
                if (rgbDeviceVideo==null)
                {
                    rgbDeviceVideo = new VideoCaptureDevice(filterInfoCollection[rgbCameraIndex < maxCameraCount ? rgbCameraIndex : 0].MonikerString);
                    rgbDeviceVideo.VideoResolution = rgbDeviceVideo.VideoCapabilities[0];
                    rgbVideoSource.VideoSource = rgbDeviceVideo;
                    windowsFormsHost.Child = rgbVideoSource;                   
                }
                //IR摄像头
                if (irDeviceVideo == null)
                {
                    irDeviceVideo = new VideoCaptureDevice(filterInfoCollection[irCameraIndex < maxCameraCount ? irCameraIndex : 0].MonikerString);
                    irDeviceVideo.VideoResolution = irDeviceVideo.VideoCapabilities[0];
                    irVideoSource.VideoSource = irDeviceVideo;
                    windowsFormsHost1.Child = irVideoSource;
                }
                rgbVideoSource.Paint += RgbVideoSource_Paint;
                irVideoSource.Paint += IrVideoSource_Paint;
                //双摄标志设为true
                isDoubleShot = true;
                StartCamera();
                isIrLive = false;
            }
            else
            {
                //仅打开RGB摄像头，IR摄像头控件隐藏
                if (rgbDeviceVideo==null)
                {
                    rgbDeviceVideo = new VideoCaptureDevice(filterInfoCollection[rgbCameraIndex <= maxCameraCount ? rgbCameraIndex : 0].MonikerString);
                    rgbDeviceVideo.VideoResolution = rgbDeviceVideo.VideoCapabilities[0];
                    rgbVideoSource.VideoSource = rgbDeviceVideo;
                    windowsFormsHost.Child = rgbVideoSource;
                }
                rgbVideoSource.Paint += RgbVideoSource_Paint;
                StartCamera();
                irVideoSource.Hide();
                isIrLive = true;
            }
            isRGBLock = false;
            isIRLock = false;
            isRectLock = false;          
            AddControlAsync();
        }

        private async void AddControlAsync()
        {
            Grid.SetRowSpan(windowsFormsHost, 2);
            Grid.SetColumnSpan(windowsFormsHost, 2);
            Grid.SetRow(windowsFormsHost1, 0);
            Grid.SetColumn(windowsFormsHost1, 1);
            await Task.Delay(1000);
            videoGrid.Children.Add(windowsFormsHost);
            videoGrid.Children.Add(windowsFormsHost1);
        }

        private void IrVideoSource_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (isDoubleShot && irVideoSource.IsRunning)
            {
                //如果双摄，且IR摄像头工作，获取IR摄像头图片
                Bitmap irBitmap = irVideoSource.GetCurrentVideoFrame();
                if (irBitmap == null)
                {
                    return;
                }
                if (isRectLock == false)
                {
                    isRectLock = true;
                    //rgb的矩形框可能会被小概率的被改变 从而影响ir的矩形框
                    var rect = new MRECT
                    {
                        left = allRect.left,
                        right = allRect.right,
                        top = allRect.top,
                        bottom = allRect.bottom
                    };
                    if (rect.left != 0 && rect.right != 0 && rect.top != 0 && rect.bottom != 0)
                    {
                        float irOffsetX = irVideoSource.Width * 1f / irBitmap.Width;
                        float irOffsetY = irVideoSource.Height * 1f / irBitmap.Height;
                        float offsetX = irVideoSource.Width * 1f / rgbVideoSource.Width;
                        float offsetY = irVideoSource.Height * 1f / rgbVideoSource.Height;
                        //检测IR摄像头下最大人脸
                        Graphics g = e.Graphics;
                        float x = rect.left * offsetX;
                        float width = rect.right * offsetX - x;
                        float y = rect.top * offsetY;
                        float height = rect.bottom * offsetY - y;
                        //根据Rect进行画框
                        g.DrawRectangle(Pens.Yellow, x, y, width, height);
                        if (trackIRUnit.message != "" && x > 0 && y > 0)
                        {
                            //将上一帧检测结果显示到页面上
                            g.DrawString(trackIRUnit.message, font, trackIRUnit.message.Contains("活体") ? greenBrush : redBrush, x, y - 15);
                        }
                        //保证只检测一帧，防止页面卡顿以及出现其他内存被占用情况
                        if (isIRLock == false)
                        {
                            isIRLock = true;
                            var currentirBitmap = irBitmap.Clone() as Bitmap;
                            var currentRect = new MRECT
                            {
                                left = allRect.left,
                                right = allRect.right,
                                top = allRect.top,
                                bottom = allRect.bottom
                            };
                            //异步处理提取特征值和比对，不然页面会比较卡
                            Task.Run(() =>
                            {
                                bool isLiveness = false;
                                try
                                {
                                    if (currentRect.left != 0 && currentRect.right != 0 && currentRect.top != 0 && currentRect.bottom != 0)
                                    {
                                        //得到当前摄像头下的图片
                                        if (currentirBitmap != null)
                                        {
                                            //检测人脸，得到Rect框
                                            ASF_MultiFaceInfo irMultiFaceInfo = FaceUtil.DetectFace(pVideoIRImageEngine, currentirBitmap);
                                            if (irMultiFaceInfo.faceNum <= 0)
                                            {
                                                return;
                                            }
                                            //得到最大人脸
                                            ASF_SingleFaceInfo irMaxFace = FaceUtil.GetMaxFace(irMultiFaceInfo);
                                            //得到Rect
                                            MRECT irRect = irMaxFace.faceRect;
                                            //判断RGB图片检测的人脸框与IR摄像头检测的人脸框偏移量是否在误差允许范围内
                                            if (isInAllowErrorRange(currentRect.left * offsetX / irOffsetX, irRect.left) && isInAllowErrorRange(currentRect.right * offsetX / irOffsetX, irRect.right)
                                                    && isInAllowErrorRange(currentRect.top * offsetY / irOffsetY, irRect.top) && isInAllowErrorRange(currentRect.bottom * offsetY / irOffsetY, irRect.bottom))
                                            {
                                                int retCode_Liveness = -1;
                                                //将图片进行灰度转换，然后获取图片数据
                                                ImageInfo irImageInfo = ImageUtil.ReadBMP_IR(currentirBitmap);
                                                if (irImageInfo == null)
                                                {
                                                    return;
                                                }
                                                //IR活体检测
                                                ASF_LivenessInfo liveInfo = FaceUtil.LivenessInfo_IR(pVideoIRImageEngine, irImageInfo, irMultiFaceInfo, out retCode_Liveness);
                                                //判断检测结果
                                                if (retCode_Liveness == 0 && liveInfo.num > 0)
                                                {
                                                    int isLive = MemoryUtil.PtrToStructure<int>(liveInfo.isLive);
                                                    isIrLive=isLiveness = (isLive == 1) ? true : false;
                                                }
                                                MemoryUtil.Free(irImageInfo.imgData);//释放当前指针所指向的内存
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                finally
                                {
                                    trackIRUnit.message = string.Format("IR{0}", isLiveness ? "活体" : "假体");
                                    currentirBitmap?.Dispose();
                                    isIRLock = false;
                                }
                            });
                        }
                    }
                    else
                    {
                        trackIRUnit.message = string.Empty;
                    }
                    irBitmap?.Dispose();//释放
                    isRectLock = false;
                }
            }
        }

        private void RgbVideoSource_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (rgbVideoSource.IsRunning)
            {
                //得到当前RGB摄像头下的图片
                Bitmap bitmap = rgbVideoSource.GetCurrentVideoFrame();
                if (bitmap == null)
                {
                    return;
                }
                this.Dispatcher?.Invoke(() =>
                {
                    if (BusyIndicator1.IsBusy)
                    {
                        BusyIndicator1.IsBusy = false;
                        //AddControl();//播放后将winform控件添加到集合里面
                    }              
                });
                //检测人脸，得到Rect框
                ASF_MultiFaceInfo multiFaceInfo = FaceUtil.DetectFace(pVideoEngine, bitmap);
                if (multiFaceInfo.faceNum > 0)//检测到人脸才开始画框
                {
                    //得到最大人脸
                    ASF_SingleFaceInfo maxFace = FaceUtil.GetMaxFace(multiFaceInfo);
                    //得到Rect
                    MRECT rect = maxFace.faceRect;
                    //检测RGB摄像头下最大人脸
                    Graphics g = e.Graphics;
                    float offsetX = rgbVideoSource.Width * 1f / bitmap.Width;
                    float offsetY = rgbVideoSource.Height * 1f / bitmap.Height;
                    float x = rect.left * offsetX;
                    float width = rect.right * offsetX - x;
                    float y = rect.top * offsetY;
                    float height = rect.bottom * offsetY - y;
                    //根据Rect进行画框
                    g.DrawRectangle(Pens.Yellow, x, y, width, height);
                    g.DrawRectangle(Pens.Yellow, x, y, width, height);                    
                    if (trackRGBUnit.message != "" && x > 0 && y > 0)
                    {
                        //将上一帧检测结果显示到页面上 同步人脸图片和边框
                        g.DrawString(trackRGBUnit.message, font, trackRGBUnit.message.Contains("活体") ? greenBrush : redBrush, x, y - 15);
                    }
                    //保证只检测一帧，防止页面卡顿以及出现其他内存被占用情况
                    if (isRGBLock == false)
                    {
                        isRGBLock = true;
                        var currentBitmap = bitmap.Clone() as  Bitmap;//复制一帧
                        allRect.left = (int)(rect.left * offsetX);//复制矩形
                        allRect.top = (int)(rect.top * offsetY);
                        allRect.right = (int)(rect.right * offsetX);
                        allRect.bottom = (int)(rect.bottom * offsetY);
                        //异步处理提取特征值和比对，不然页面会比较卡
                        Task.Run(async ()=>
                        {
                            try
                            {
                                if (allRect.left != 0 && allRect.right != 0 && allRect.top != 0 && allRect.bottom != 0)
                                {
                                    bool isLiveness = false;
                                    //调整图片数据，非常重要
                                    ImageInfo imageInfo = ImageUtil.ReadBMP(currentBitmap);//值传递
                                    if (imageInfo == null)
                                    {
                                        return;
                                    }
                                    int retCode_Liveness = -1;
                                    //RGB活体检测
                                    ASF_LivenessInfo liveInfo = FaceUtil.LivenessInfo_RGB(pVideoRGBImageEngine, imageInfo, multiFaceInfo, out retCode_Liveness);
                                    //判断检测结果
                                    if (retCode_Liveness == 0 && liveInfo.num > 0)
                                    {
                                        int isLive = MemoryUtil.PtrToStructure<int>(liveInfo.isLive);
                                        isLiveness = (isLive == 1) ? true : false;
                                    }
                                    if (imageInfo != null)
                                    {
                                        MemoryUtil.Free(imageInfo.imgData);
                                    }
                                    if (isLiveness&& _isRecognitionFace&& isIrLive)//是否识别人脸
                                    {
                                        isIrLive = false;//重置
                                        //提取人脸特征
                                        IntPtr feature = FaceUtil.ExtractFeature(pVideoRGBImageEngine, currentBitmap, maxFace);
                                        float similarity = 0f;
                                        //得到比对结果
                                        var result = compareFeature(feature, out similarity);
                                        MemoryUtil.Free(feature);
                                        if (!string.IsNullOrEmpty(result))
                                        {
                                            //将比对结果放到显示消息中，用于最新显示
                                            trackRGBUnit.message = string.Format(" 分数 {0},{1}", similarity, string.Format("RGB{0}", isLiveness ? "活体" : "假体"));
                                            var str = result.Split('_');
                                            if (str.Length == 2)
                                            {
                                                UnResgistPaint();
                                                var isSuccessLogin = await IndividualNeeds.Instance.CommonVariables.LoginAction.Invoke(str[0], str[1]);//同步
                                                if (!isSuccessLogin)
                                                {
                                                    ResgistPaint();
                                                }
                                            }
                                            else
                                            {
                                                Logger.Info("人脸传过来的id解析不对啊");
                                            }
                                        }
                                        else
                                        {
                                            //弹框 3种选择 dialog
                                            this.Dispatcher?.Invoke(() =>
                                            {
                                                UnResgistPaint();
                                                SubWindowsService.Instance.OpenWindow(SubWindowsService.FaceRecognitionFailurePage, IsDialog: true);
                                                ResultType resultType;
                                                if (SubWindowsService.Instance.Result!=null&&Enum.TryParse(SubWindowsService.Instance.Result.ToString(), out resultType))
                                                {
                                                    switch (resultType)
                                                    {
                                                        case ResultType.RecogineAgian:
                                                            ResgistPaint();
                                                            break;
                                                        case ResultType.ToLogin:
                                                            NaviService.Instance.GoBack();
                                                            break;
                                                        case ResultType.Close:
                                                            NaviService.Instance.NavigateTo(PageKey.MainPage);
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    Logger.Info("人脸失败弹窗无结果返回");
                                                }
                                            });
                                        }
                                    }
                                    else
                                    {
                                        //显示消息
                                        trackRGBUnit.message = string.Format("RGB{0}", isLiveness ? "活体" : "假体");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                            finally 
                            {
                                currentBitmap?.Dispose();
                                isRGBLock = false;
                            }
                        });
                    }
                }
                else
                {
                    allRect.left = 0;
                    allRect.right = 0;
                    allRect.top = 0;
                    allRect.bottom = 0;
                }
                bitmap.Dispose();
            }
        }

        private string compareFeature(IntPtr feature, out float similarity)
        {
            string result =string.Empty;
            similarity = 0f;
            //如果人脸库不为空，则进行人脸匹配
            if (imagesFeatureList != null && imagesFeatureList.Count > 0)
            {
                foreach (var item in imagesFeatureList)
                {
                    //调用人脸匹配方法，进行匹配
                    ASFFunctions.ASFFaceFeatureCompare(pVideoRGBImageEngine, feature, item.Key, ref similarity);
                    if (similarity >= threshold)
                    {
                        result = item.Value;
                        break;
                    }
                }
            }
            return result;
        }

        private void GetLocalFaceImage()
        {

            var dirStr = @"Faces";
            DirectoryInfo directoryInfo;
            if (!Directory.Exists(dirStr))
            {
                directoryInfo = Directory.CreateDirectory(dirStr);
            }
            else
            {
                directoryInfo = new DirectoryInfo(dirStr);
            }
            foreach (FileInfo fi in directoryInfo.GetFiles())
            {
                if (imagesFeatureList.ContainsValue(fi.Name))
                {
                    continue;
                }
                var image = ImageUtil.readFromFile(fi.FullName);//全路径
                if (image != null && fi.Length >= 2 && fi.Length <= maxSize )
                {
                    if (image.Width > 1536 || image.Height > 1536)
                    {
                        image = ImageUtil.ScaleImage(image, 1536, 1536);
                    }
                    if (image == null)
                    {
                        Logger.Info($"图片{fi.Name}压缩出错");
                        continue;
                    }
                    if (image.Width % 4 != 0)
                    {
                        image = ImageUtil.ScaleImage(image, image.Width - (image.Width % 4), image.Height);
                    }
                    if (image == null)
                    {
                        Logger.Info($"图片{fi.Name}压缩出错");
                        continue;
                    }
                    //人脸检测
                    ASF_MultiFaceInfo multiFaceInfo = FaceUtil.DetectFace(pImageEngine, image);
                    //判断检测结果
                    if (multiFaceInfo.faceNum > 0)
                    {
                        MRECT rect = MemoryUtil.PtrToStructure<MRECT>(multiFaceInfo.faceRects);
                        image = ImageUtil.CutImage(image, rect.left, rect.top, rect.right, rect.bottom);//裁剪出人脸
                        ASF_SingleFaceInfo singleFaceInfo = new ASF_SingleFaceInfo();
                        IntPtr feature = FaceUtil.ExtractFeature(pImageEngine, image, out singleFaceInfo);
                        if (singleFaceInfo.faceRect.left == 0 && singleFaceInfo.faceRect.right == 0)
                        {
                            Logger.Info($"图片{fi.Name}检测不到特征值");
                        }
                        else
                        {
                            var id = fi.Name.Replace(fi.Extension, "");
                            imagesFeatureList.Add(feature, id);//name 也是key
                        }
                    }
                    else
                    {
                        image?.Dispose();
                        Logger.Info($"图片{fi.Name}检测不到人脸");
                        continue;
                    }
                }
                else
                {
                    Logger.Info($"图片{fi.Name}不符合标准或者已添加");
                }
                image?.Dispose();
            }
        }

        private void StartCamera() 
        {
            if (rgbVideoSource?.IsRunning==false)
            {
                rgbVideoSource.Start();
            }
            if (isDoubleShot&&irVideoSource?.IsRunning==false)
            {
                irVideoSource.Start();
            }         
        }

        private void StopCamera() 
        {
            if (rgbVideoSource?.IsRunning == true)
            {
                rgbVideoSource.Stop();
            }
            if (isDoubleShot && irVideoSource?.IsRunning == true)
            {
                irVideoSource.Stop();
            }
        }

        /// <summary>
        /// 判断参数0与参数1是否在误差允许范围内
        /// </summary>
        /// <param name="arg0">参数0</param>
        /// <param name="arg1">参数1</param>
        /// <returns></returns>
        private bool isInAllowErrorRange(float arg0, float arg1)
        {
            bool rel = false;
            if (arg0 > arg1 - allowAbleErrorRange && arg0 < arg1 + allowAbleErrorRange)
            {
                rel = true;
            }
            return rel;
        }
        private void UnResgistPaint() 
        {
            if (rgbVideoSource!=null)
            {
                rgbVideoSource.Paint -= RgbVideoSource_Paint;
            }
            if (irVideoSource!=null)
            {
                irVideoSource.Paint -= IrVideoSource_Paint;
            }
        }
        private void ResgistPaint()
        {
            if (rgbVideoSource != null)
            {
                rgbVideoSource.Paint += RgbVideoSource_Paint;
            }
            if (irVideoSource != null)
            {
                irVideoSource.Paint += IrVideoSource_Paint;
            }
        }

        //#region opencvsharp       


        //private void LoadVideoData()
        //{
        //    AppSettingsReader reader = new AppSettingsReader();
        //    var rgbCameraIndex = (int)reader.GetValue("RGB_CAMERA_INDEX", typeof(int));
        //    var irCameraIndex = (int)reader.GetValue("IR_CAMERA_INDEX", typeof(int));
        //    Task.Run(() =>
        //    {
        //        if (_rgbVideoCapture == null)
        //        {
        //            _rgbVideoCapture = new VideoCapture(rgbCameraIndex);//获取rgb摄像机
        //            if (!_rgbVideoCapture.IsOpened())
        //            {
        //                this.Dispatcher?.Invoke(() =>
        //                {
        //                    BusyIndicator1.IsBusy = false;
        //                });
        //                TipService.Instance.ShowTip(TipService.ToolTip, 1000, "摄像头故障");
        //                return;
        //            }
        //            _rgbVideoCapture.Set(CaptureProperty.FrameWidth, this.ActualWidth);//宽度 根据控件的大小来决定           
        //            _rgbVideoCapture.Set(CaptureProperty.FrameHeight, this.ActualHeight);//高度     
        //        }
        //        if (_irVideoCapture == null)
        //        {
        //            _irVideoCapture = new VideoCapture(irCameraIndex);
        //            if (!_irVideoCapture.IsOpened())
        //            {
        //                _irVideoCapture.Release();//释放
        //                _irVideoCapture = null;
        //            }
        //            else
        //            {
        //                _isDoubleCapture = true;
        //                _irVideoCapture.Set(CaptureProperty.FrameWidth, IrImage.ActualWidth);
        //                _irVideoCapture.Set(CaptureProperty.FrameWidth, IrImage.ActualHeight);
        //            }
        //        }
        //        GetFaceRecognizer();
        //        PlayRgbCamera();
        //        _cameraStatus = CameraStatus.Palying;
        //    });
        //}
        //private Task PlayRgbCamera()
        //{
        //    return Task.Run(async () =>
        //      {
        //          Mat irFrame = null;
        //          Mat rgbFrame = null;
        //          try
        //          {
        //              while (_cameraStatus!= CameraStatus.Stop)
        //              {
        //                  if (_cameraStatus== CameraStatus.Suspend)
        //                  {
        //                      Thread.Sleep(1000);
        //                      continue;
        //                  }
        //                  rgbFrame = new Mat();
        //                  _rgbVideoCapture.Read(rgbFrame);//获取frame从摄像头或者视频文件
        //                  if (_isDoubleCapture)
        //                  {
        //                      irFrame =  new Mat();
        //                      _irVideoCapture.Read(rgbFrame);
        //                  }
        //                  int sleepTime = (int)Math.Round(1000 / _rgbVideoCapture.Fps);//帧数，Thread.Sleep(40);
        //                  Cv2.WaitKey(sleepTime);//沉睡
        //                  if (rgbFrame.Empty()||(_isDoubleCapture&& (rgbFrame.Empty()|| irFrame.Empty())))//单目 双目
        //                  {
        //                      continue;//获取不到则继续
        //                  }
        //                  this.Dispatcher?.InvokeAsync(() =>
        //                  {
        //                      if (BusyIndicator1.IsBusy)
        //                      {
        //                          BusyIndicator1.IsBusy = false;
        //                      }
        //                  }); 
        //                  //私有属性是否开启检测 由于是异步的 依赖性属性不能放入
        //                  if (_isDetectFace)
        //                  {
        //                      //绘制指定区域(人脸框) minNeighbors:邻居像素得符合多少个才能被认为是人脸 minSize:最小50 50才能被检测到
        //                      var rect = _cascadeClassifier.DetectMultiScale(rgbFrame, minNeighbors: 5, minSize: new OpenCvSharp.Size(50, 50));
        //                      if (rect.Length > 0)//捕捉到人脸
        //                      {
        //                          foreach (var item in rect)
        //                          {
        //                              Scalar color = Scalar.FromRgb(255, 242, 0);
        //                              Cv2.Rectangle(rgbFrame, item, color, 2);
        //                          }
        //                          ShotFace(rgbFrame, rect);//嵌入功能 截图
        //                          var result=await DistinguishFaceAsync(rgbFrame, rect);//同步运行 阻塞 引用传递
        //                          if (result!= ResultType.RecogineAgian)
        //                          {
        //                              _cameraStatus = CameraStatus.Stop;
        //                              return;
        //                          }
        //                      }
        //                  }
        //                  //Cv2.Flip(cFrame, cFrame, FlipMode.Y);
        //                  Application.Current.Dispatcher?.Invoke(() =>
        //                  {
        //                      var image = ImageHelper.Instance.ConvertBitmapToBitmapImage(rgbFrame.ToBitmap());
        //                      image.CacheOption = BitmapCacheOption.None;//创建完后 要是没有引用就释放
        //                      VideoImage.Source = image;
        //                      if (_isDoubleCapture)
        //                      {
        //                          var irImage = ImageHelper.Instance.ConvertBitmapToBitmapImage(irFrame.ToBitmap());
        //                          irImage.CacheOption = BitmapCacheOption.None;//创建完后 要是没有引用就释放
        //                          IrImage.Source = irImage;
        //                      }
        //                  });
        //              }
        //          }
        //          catch (Exception ex)
        //          {
        //              TipService.Instance.ShowTip(TipService.ToolTip, 1000, ex.Message);
        //          }
        //          finally
        //          {
        //              rgbFrame?.Release();//释放
        //              irFrame?.Release();
        //          }
        //      });
        //}
        //private async Task<ResultType> DistinguishFaceAsync( Mat cFrame, OpenCvSharp.Rect[] rect) 
        //{
        //    var faceRecognitionResult = ResultType.RecogineAgian;
        //    try
        //    {
        //        //是否开启识别
        //        if (_isRecognitionFace)
        //        {
        //            if (rect.Length > 2)
        //            {
        //                TipService.Instance.ShowTip(TipService.ToolTip, 1000, "检测到多张人脸");
        //                Thread.Sleep(1000);
        //            }
        //            else
        //            {
        //                Mat cFace = new Mat(cFrame, rect[0]);
        //                Cv2.Resize(cFace, cFace, new OpenCvSharp.Size(100, 100));
        //                Cv2.CvtColor(cFace, cFace, ColorConversionCodes.BGR2GRAY);
        //                Cv2.EqualizeHist(cFace, cFace);
        //                var label = _iBPHFaceRecognizer.Predict(cFace);
        //                cFace.Release();
        //                string id;
        //                if (_labelsToId.TryGetValue(label, out id))//识别到了返回id
        //                {
        //                    //开始登录 返回bool 
        //                    //Cv2.PutText(cFrame, name, new OpenCvSharp.Point(rect[0].X, rect[0].Y), HersheyFonts.Italic, 1, color);
        //                    var str = id.Split('_');
        //                    if (str.Length == 2)
        //                    {
        //                        var isSuccessLogin = await IndividualNeeds.Instance.CommonVariables.LoginAction.Invoke(str[0], str[1]);//同步
        //                        if (isSuccessLogin)
        //                        {
        //                            faceRecognitionResult = ResultType.Success;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Logger.Info("人脸传过来的id解析不对啊");
        //                    }
        //                }
        //                else
        //                {
        //                    //弹框 3种选择 dialog
        //                    this.Dispatcher?.Invoke(() =>
        //                    {
        //                        SubWindowsService.Instance.OpenWindow(SubWindowsService.FaceRecognitionFailurePage, IsDialog: true);
        //                        ResultType resultType;
        //                        if (Enum.TryParse(SubWindowsService.Instance.Result.ToString(), out resultType))
        //                        {
        //                            faceRecognitionResult = resultType;
        //                        }
        //                        else
        //                        {
        //                            Logger.Info("人脸失败弹窗无结果返回");
        //                        }
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    catch ( Exception ex) 
        //    {
        //        Logger.Error(ex);
        //    }
        //    finally
        //    {

        //    }
        //    return faceRecognitionResult;
        //}

        //private void ShotFace(Mat cFrame, OpenCvSharp.Rect[] rect) 
        //{
        //    //是否截取人脸图片 只截取一张
        //    if (_isShotsFace)
        //    {
        //        try
        //        {
        //            if (rect.Length > 2)
        //            {
        //                TipService.Instance.ShowTip(TipService.ToolTip, 1000, "检测到多张人脸");
        //                Thread.Sleep(1000);
        //            }
        //            else
        //            {
        //                Application.Current.Dispatcher?.Invoke(() =>
        //                {
        //                    Mat cHead = new Mat(cFrame, rect[0]);//截图区域   
        //                    Cv2.Resize(cHead, cHead, new OpenCvSharp.Size(200, 200));//图片过大 缩放图片为100 100
        //                    var path = $"{AppDomain.CurrentDomain.BaseDirectory}Faces\\12506_123456.png";
        //                    Cv2.ImWrite(path, cHead); //写入文件
        //                    cHead.Release();
        //                });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Error(ex);
        //        }
        //        finally
        //        {
        //            Application.Current.Dispatcher?.Invoke(() =>
        //            {
        //                IsShotsFace = false;
        //            });
        //        }
        //    }
        //}

        //private void GetFaceRecognizer()
        //{
        //    //获得Debug下的人脸图片 进行灰度和亮度处理
        //    string path = @"Faces";
        //    DirectoryInfo root = new DirectoryInfo(path);
        //    FileInfo[] files = root.GetFiles();
        //    var imageList = new List<Mat>();
        //    var idList = new List<int>();
        //    int i = 0;
        //    _labelsToId.Clear();
        //    foreach (var item in files)
        //    {

        //        var image = Cv2.ImRead(item.FullName);
        //        Cv2.CvtColor(image, image, ColorConversionCodes.BGR2GRAY);
        //        //Cv2.EqualizeHist(image, image);
        //        imageList.Add(image);
        //        i++;
        //        idList.Add(i);
        //        _labelsToId.Add(i, item.Name);//字典 可以根据id获取自己需要的 FaceId

        //    }
        //    if (imageList.Count>0)
        //    {
        //        //使用了LBPHFaceRecognizer类型的人脸识别器
        //        if (_iBPHFaceRecognizer == null)
        //        {
        //            _iBPHFaceRecognizer = LBPHFaceRecognizer.Create();
        //        }
        //        //进行人脸数据的训练 每张图片及它的标识Id
        //        _iBPHFaceRecognizer.Train(imageList, idList);
        //    }

        //}
        //#endregion

    }
}
