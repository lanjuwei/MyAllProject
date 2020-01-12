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
    public partial class OpencvCameraUserControl : UserControl
    {
        private static VideoCapture _rgbVideoCapture;
        private static VideoCapture _irVideoCapture;
        private CameraStatus _cameraStatus;
        private bool _isShotsFace;
        private CascadeClassifier _cascadeClassifier;
        private bool _isDetectFace = false;
        private bool _isRecognitionFace = false;
        private static bool isDoubleShot;
        private LBPHFaceRecognizer _iBPHFaceRecognizer;
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
        private IntPtr pImageEngine = IntPtr.Zero;
        /// <summary>
        /// 视频引擎Handle
        /// </summary>
        private IntPtr pVideoEngine = IntPtr.Zero;
        /// <summary>
        /// RGB视频引擎 FR Handle 处理   FR和图片引擎分开，减少强占引擎的问题
        /// </summary>
        private IntPtr pVideoRGBImageEngine = IntPtr.Zero;
        /// <summary>
        /// IR视频引擎 FR Handle 处理   FR和图片引擎分开，减少强占引擎的问题
        /// </summary>
        private IntPtr pVideoIRImageEngine = IntPtr.Zero;
        /// <summary>
        /// RGB 摄像头索引
        /// </summary>
        private int rgbCameraIndex = 0;
        /// <summary>
        /// IR 摄像头索引
        /// </summary>
        private int irCameraIndex = 0;
        /// <summary>
        /// IR摄像头设备
        /// </summary>
        private FaceTrackUnit trackRGBUnit = new FaceTrackUnit();
        private FaceTrackUnit trackIRUnit = new FaceTrackUnit();
        private Font font = new Font(System.Drawing.FontFamily.GenericSerif, 10f, System.Drawing.FontStyle.Bold);
        private SolidBrush redBrush = new SolidBrush(System.Drawing.Color.Red);
        private SolidBrush greenBrush = new SolidBrush(System.Drawing.Color.Green);
        private bool isRGBLock = false;
        private bool isLiveLock = false;
        private bool isRecogniteLock = false;
        private bool isIRLock = false;
        private MRECT allRect = new MRECT();
        private int _rectX;
        private int _rectY;
        private int _rectHeight;
        private int _rectWidth;
        private string loginId;
        private bool isRectLock = false;
        private bool isIrLive = false;//ir是否过关
        private object _trackLock = new object();
        private FaceResult faceResult= FaceResult.NotStrat;
        public OpencvCameraUserControl()
        {
            InitializeComponent();
            Loaded += LocalCameraUserControl_Loaded;
            Unloaded += LocalCameraUserControl_UnloadedAsync;
            _cascadeClassifier = new CascadeClassifier(@"haarcascade_frontalface_alt.xml");

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



        static OpencvCameraUserControl()
        {
            IsShotsFaceProperty = DependencyProperty.Register("IsShotsFace", typeof(bool), typeof(OpencvCameraUserControl), new PropertyMetadata(false, IsCaptureVideoCallBack));
            CameraStatusProperty = DependencyProperty.Register("Status", typeof(CameraStatus), typeof(OpencvCameraUserControl), new PropertyMetadata(CameraStatus.Stop, CameraStatusCallBack));
            IsDetectFaceProperty = DependencyProperty.Register("IsDetectFace", typeof(bool), typeof(OpencvCameraUserControl), new PropertyMetadata(false, IsDetectFaceCallBack));
            IsRecognitionFaceProperty = DependencyProperty.Register("IsRecognitionFace", typeof(bool), typeof(OpencvCameraUserControl), new PropertyMetadata(false, IsRecognitionFaceCallBack));
        }

        ~OpencvCameraUserControl()
        {

        }

        private static void IsRecognitionFaceCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as OpencvCameraUserControl;
            c._isRecognitionFace = (bool)e.NewValue;
        }

        private static void IsDetectFaceCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as OpencvCameraUserControl;
            c._isDetectFace = (bool)e.NewValue;
        }

        private static void CameraStatusCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as OpencvCameraUserControl;
            c._cameraStatus = (CameraStatus)e.NewValue;
        }

        private static void IsCaptureVideoCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as OpencvCameraUserControl;
            c._isShotsFace = (bool)e.NewValue;
        }


        private void LocalCameraUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BusyIndicator1.IsBusy = true;
            _cameraStatus = CameraStatus.Palying;
            faceResult = FaceResult.NotStrat;
            loginId = string.Empty;
            isIrLive = false;
            _rgbVideoCapture?.RetrieveMat();
            _irVideoCapture?.RetrieveMat();
            Task.Run(() =>
            {
                if (!isLoad)
                {
                    InitEngines();//加载引擎一次就好
                    isLoad = true;
                }
                GetLocalFaceImage();//获取本地图片检测人脸
                if (_rgbVideoCapture == null)
                {
                    if (rgbCameraIndex != irCameraIndex)
                    {
                        _rgbVideoCapture = new VideoCapture(rgbCameraIndex);
                        _irVideoCapture = new VideoCapture(irCameraIndex);
                        var isRgbOpen = _rgbVideoCapture.IsOpened();
                        if (!isRgbOpen)
                        {
                            this.Dispatcher?.Invoke(() =>
                            {
                                BusyIndicator1.IsBusy = false;
                            });
                            TipService.Instance.ShowTip(TipService.ToolTip, 1000, "未检测到彩色摄像头!");
                        }
                        var isIrOpen = _irVideoCapture.IsOpened();
                        if (!isIrOpen)
                        {
                            this.Dispatcher?.Invoke(() =>
                            {
                                BusyIndicator1.IsBusy = false;
                            });
                            TipService.Instance.ShowTip(TipService.ToolTip, 1000, "未检测到红外摄像头!");
                        }
                        if (!isIrOpen|| !isRgbOpen)
                        {
                            return;
                        }
                        isDoubleShot = true;
                    }
                    else
                    {
                        _rgbVideoCapture = new VideoCapture(0);
                        if (!_rgbVideoCapture.IsOpened())
                        {
                            this.Dispatcher?.Invoke(() =>
                            {
                                BusyIndicator1.IsBusy = false;
                            });
                            TipService.Instance.ShowTip(TipService.ToolTip, 1000, "未检测到摄像头，请确保已安装摄像头或驱动!");
                            return;
                        }
                        _rgbVideoCapture.Set(CaptureProperty.FrameWidth, this.ActualWidth);//宽度 根据控件的大小来决定           
                        _rgbVideoCapture.Set(CaptureProperty.FrameHeight, this.ActualHeight);//高度   
                    }  
                }
                PlayLacalCamera();

            });
        }



        private  void LocalCameraUserControl_UnloadedAsync(object sender, RoutedEventArgs e)
        {
            _cameraStatus = CameraStatus.Stop;
            VideoImage.Source = null;
            IrImage.Source = null;
        }



        private Task PlayLacalCamera()
        {
            return Task.Run(async () =>
            {
                Mat cFrame = null;
                Mat cIrFrame = null;
                try
                {
                    while (_cameraStatus != CameraStatus.Stop)
                    {
                        if (_cameraStatus == CameraStatus.Suspend)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        cFrame = new Mat();
                        _rgbVideoCapture.Read(cFrame);//获取frame从摄像头或者视频文件
                        if (isDoubleShot)
                        {
                            cIrFrame = new Mat();
                            _irVideoCapture.Read(cIrFrame);
                        }
                        int sleepTime = (int)Math.Round(1000 / _rgbVideoCapture.Fps);//帧数，Thread.Sleep(40);
                        Cv2.WaitKey(sleepTime);//沉睡
                        if (cFrame?.Empty()==true||(isDoubleShot&& (cFrame?.Empty()==true|| cIrFrame?.Empty()==true)))
                        {
                            continue;//获取不到则继续
                        }
                        this.Dispatcher?.InvokeAsync(() =>
                        {
                            if (BusyIndicator1.IsBusy)
                            {
                                BusyIndicator1.IsBusy = false;
                            }
                        });
                        
                        //私有属性是否开启检测 由于是异步的 依赖性属性不能放入
                        if (_isDetectFace)
                        {
                            if (isIRLock==false&&isDoubleShot)
                            {
                                isIRLock = true;
                                var irBitmap = cIrFrame.ToBitmap();
                                Task.Run(()=> 
                                {
                                    try
                                    {
                                        ASF_MultiFaceInfo irMultiFaceInfo = FaceUtil.DetectFace(pVideoIRImageEngine, irBitmap);
                                        int retCode_Liveness = -1;
                                        //将图片进行灰度转换，然后获取图片数据
                                        ImageInfo irImageInfo = ImageUtil.ReadBMP_IR(irBitmap);
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
                                            trackIRUnit.message = isLive==1?"Ir Yes": "Ir No";
                                            isIrLive = isLive == 1;
                                        }
                                        MemoryUtil.Free(irImageInfo.imgData);//释放当前指针所指向的内存
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    finally 
                                    {
                                        irBitmap?.Dispose();
                                        isIRLock = false;
                                    }
                                });
                            }
                            if (isRGBLock==false)
                            {
                                isRGBLock = true;
                                var bitmap = cFrame.ToBitmap();
                                Task.Run(()=>
                                {
                                    try
                                    {
                                        _rectX = 0;
                                        _rectY = 0;
                                        _rectWidth = 0;
                                        _rectHeight = 0;
                                        ASF_MultiFaceInfo multiFaceInfo = FaceUtil.DetectFace(pVideoEngine, bitmap);                        
                                        if (multiFaceInfo.faceNum > 0)
                                        {
                                            ASF_SingleFaceInfo maxFace = FaceUtil.GetMaxFace(multiFaceInfo);
                                            _rectX = maxFace.faceRect.left;
                                            _rectY = maxFace.faceRect.top;
                                            _rectWidth = maxFace.faceRect.right - maxFace.faceRect.left;
                                            _rectHeight = maxFace.faceRect.bottom - maxFace.faceRect.top;//将耗时的识别结果返回搞到上个一个线程 识别结果并不一定要等待才有 不等待什么时候完成返回也可以
                                            if (isLiveLock == false && _rectWidth != 0 && _rectHeight != 0)
                                            {
                                                isLiveLock = true;
                                                var liveBitmap = bitmap.Clone() as Bitmap;
                                                Task.Run(() =>
                                                {
                                                    try
                                                    {
                                                        //调整图片数据，非常重要
                                                        ImageInfo imageInfo = ImageUtil.ReadBMP(liveBitmap);//值传递
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
                                                            if (isDoubleShot && !isIrLive)//是双目 但是活体没过
                                                            {
                                                                return;
                                                            }
                                                            if (isLive == 1)
                                                            {
                                                                if (isRecogniteLock == false)
                                                                {
                                                                    isRecogniteLock = true;
                                                                    var reBitmap = liveBitmap.Clone() as Bitmap;
                                                                    Task.Run(() =>
                                                                    {
                                                                        try
                                                                        {
                                                                            //提取人脸特征
                                                                            IntPtr feature = FaceUtil.ExtractFeature(pVideoRGBImageEngine, reBitmap, maxFace);
                                                                            float similarity = 0f;
                                                                            //得到比对结果
                                                                            var result = compareFeature(feature, out similarity);
                                                                            MemoryUtil.Free(feature);
                                                                            if (!string.IsNullOrEmpty(result))
                                                                            {
                                                                                //将比对结果放到显示消息中，用于最新显示
                                                                                trackRGBUnit.message = string.Format(" Socre {0},{1}", similarity, "RGB Yes");
                                                                                faceResult = FaceResult.Success;
                                                                                loginId = result;
                                                                            }
                                                                            else
                                                                            {
                                                                                faceResult = FaceResult.Fail;
                                                                                trackRGBUnit.message = "RGB  Yes";
                                                                            }                                                                           
                                                                        }
                                                                        catch (Exception)
                                                                        {
                                                                        }
                                                                        finally
                                                                        {
                                                                            reBitmap?.Dispose();
                                                                            isRecogniteLock = false;
                                                                        }
                                                                    });
                                                                }
                                                            }
                                                            else
                                                            {
                                                                trackRGBUnit.message = "RGB  No";
                                                            }
                                                        }
                                                        if (imageInfo != null)
                                                        {
                                                            MemoryUtil.Free(imageInfo.imgData);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    finally
                                                    {
                                                        liveBitmap?.Dispose();
                                                        isLiveLock = false;
                                                    }

                                                });
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        //当页面关闭的时候cFrame被真正的释放到了 用lock也没有用的 虽然两条线程操作了同一个对象cframe
                                    }
                                    finally
                                    {
                                        bitmap.Dispose();
                                        isRGBLock = false;
                                    }
                                });
                            }
                        }
                        if (_rectX != 0&& _rectY != 0&& _rectWidth != 0&& _rectHeight != 0)
                        {
                            Cv2.Rectangle(cFrame, new OpenCvSharp.Rect(_rectX, _rectY, _rectWidth, _rectHeight), Scalar.Yellow, 2);
                            if (cIrFrame!=null)
                            {
                                Cv2.Rectangle(cIrFrame, new OpenCvSharp.Rect(_rectX, _rectY, _rectWidth, _rectHeight), Scalar.Yellow, 2);
                            }                          
                            if (trackRGBUnit.message != "" && _rectX > 0 && _rectY > 0)
                            {
                                Cv2.PutText(cFrame, trackRGBUnit.message, new OpenCvSharp.Point(_rectX, _rectY - 15), HersheyFonts.Italic, 0.8, trackRGBUnit.message.Contains("Yes") ? Scalar.Green : Scalar.Red);
                                if (cIrFrame != null)
                                {
                                    Cv2.PutText(cIrFrame, trackIRUnit.message, new OpenCvSharp.Point(_rectX, _rectY - 15), HersheyFonts.Italic, 2.5, trackRGBUnit.message.Contains("Yes") ? Scalar.Green : Scalar.Red);
                                }
                            }
                        }
                        //switch (faceResult)
                        //{
                        //    case FaceResult.Success:
                        //        if (!string.IsNullOrEmpty(loginId))
                        //        {
                        //            var str = loginId.Split('_');
                        //            if (str.Length == 2)
                        //            {
                        //                var isSuccessLogin = await IndividualNeeds.Instance.CommonVariables.LoginAction.Invoke(str[0], str[1]);//同步
                        //                if (isSuccessLogin)
                        //                {
                        //                    _cameraStatus = CameraStatus.Stop;
                        //                }
                        //            }
                        //            else
                        //            {
                        //                Logger.Info("人脸传过来的id解析不对啊");
                        //            }
                        //        }
                        //        break;
                        //    case FaceResult.Fail:
                        //        //弹框 3种选择 dialog
                        //        this.Dispatcher?.Invoke(() =>
                        //        {
                        //            SubWindowsService.Instance.OpenWindow(SubWindowsService.FaceRecognitionFailurePage, IsDialog: true);
                        //            ResultType resultType;
                        //            if (SubWindowsService.Instance.Result != null && Enum.TryParse(SubWindowsService.Instance.Result.ToString(), out resultType))
                        //            {
                        //                switch (resultType)
                        //                {
                        //                    case ResultType.RecogineAgian:
                        //                        faceResult = FaceResult.NotStrat;
                        //                        break;
                        //                    case ResultType.ToLogin:
                        //                        _cameraStatus = CameraStatus.Stop;
                        //                        NaviService.Instance.GoBack();
                        //                        break;
                        //                    case ResultType.Close:
                        //                        _cameraStatus = CameraStatus.Stop;
                        //                        NaviService.Instance.NavigateTo(PageKey.MainPage);
                        //                        break;
                        //                }
                        //            }
                        //            else
                        //            {
                        //                Logger.Info("人脸失败弹窗无结果返回");
                        //            }
                        //        });
                        //        break;
                        //    case FaceResult.NotStrat:
                        //        break;
                        //}
                   
                        Application.Current.Dispatcher?.Invoke(() =>
                        {
                           // var image = ImageHelper.Instance.ConvertBitmapToBitmapImage(cFrame.ToBitmap());
                            //image.CacheOption = BitmapCacheOption.None;//创建完后 要是没有引用就释放
                            VideoImage.Source = cFrame.ToBitmapSource();
                            if (cIrFrame!=null)
                            {
                                IrImage.Source = cIrFrame.ToBitmapSource();
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    TipService.Instance.ShowTip(TipService.ToolTip, 1000, ex.Message);
                }
                finally
                {
                    cFrame?.Release();//释放
                    cIrFrame?.Release();
                }
            });
        }


        //private async Task<ResultType> DistinguishFaceAsync(Mat cFrame, OpenCvSharp.Rect[] rect)
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

        //                bool isLiveness = false;
        //                //调整图片数据，非常重要
        //                ImageInfo imageInfo = ImageUtil.ReadBMP(cFrame.ToBitmap());//值传递
        //                int retCode_Liveness = -1;
        //                //RGB活体检测
        //                ASF_LivenessInfo liveInfo = FaceUtil.LivenessInfo_RGB(pVideoRGBImageEngine, imageInfo, multiFaceInfo, out retCode_Liveness);
        //                //判断检测结果
        //                if (retCode_Liveness == 0 && liveInfo.num > 0)
        //                {
        //                    int isLive = MemoryUtil.PtrToStructure<int>(liveInfo.isLive);
        //                    isLiveness = (isLive == 1) ? true : false;
        //                }
        //                if (imageInfo != null)
        //                {
        //                    MemoryUtil.Free(imageInfo.imgData);
        //                }
        //                //if (_labelsToId.TryGetValue(label, out id))//识别到了返回id
        //                //{
        //                //    //开始登录 返回bool 
        //                //    //Cv2.PutText(cFrame, name, new OpenCvSharp.Point(rect[0].X, rect[0].Y), HersheyFonts.Italic, 1, color);
        //                //    var str = id.Split('_');
        //                //    if (str.Length == 2)
        //                //    {
        //                //        var isSuccessLogin = await IndividualNeeds.Instance.CommonVariables.LoginAction.Invoke(str[0], str[1]);//同步
        //                //        if (isSuccessLogin)
        //                //        {
        //                //            faceRecognitionResult = ResultType.Success;
        //                //        }
        //                //    }
        //                //    else
        //                //    {
        //                //        Logger.Info("人脸传过来的id解析不对啊");
        //                //    }
        //                //}
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
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //    finally
        //    {

        //    }
        //    return faceRecognitionResult;
        //}
        private string compareFeature(IntPtr feature, out float similarity)
        {
            string result = string.Empty;
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

        private void ShotFace(Mat cFrame, OpenCvSharp.Rect[] rect)
        {
            //是否截取人脸图片 只截取一张
            if (_isShotsFace)
            {
                try
                {
                    if (rect.Length > 2)
                    {
                        TipService.Instance.ShowTip(TipService.ToolTip, 1000, "检测到多张人脸");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Application.Current.Dispatcher?.Invoke(() =>
                        {
                            Mat cHead = new Mat(cFrame, rect[0]);//截图区域   
                            Cv2.Resize(cHead, cHead, new OpenCvSharp.Size(200, 200));//图片过大 缩放图片为100 100
                            var path = $"{AppDomain.CurrentDomain.BaseDirectory}Faces\\12506_123456.png";
                            Cv2.ImWrite(path, cHead); //写入文件
                            cHead.Release();
                        });
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
                finally
                {
                    Application.Current.Dispatcher?.Invoke(() =>
                    {
                        IsShotsFace = false;
                    });
                }
            }
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
                if (image != null && fi.Length >= 2 && fi.Length <= maxSize)
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

        /// <summary>
        /// 初始化引擎 一次就好
        /// </summary>
        private void InitEngines()
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

    }
}
