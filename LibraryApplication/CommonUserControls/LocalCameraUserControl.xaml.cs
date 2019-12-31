using BasicFunction.Helper;
using BasicFunction.Log;
using BasicServices.SubWindowService.ViewService;
using BasicServices.TipService;
using Model;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Face;
using System;
using System.Collections.Generic;
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
    public partial class LocalCameraUserControl : UserControl
    {
        private static VideoCapture _videoCapture;
        private CameraStatus _cameraStatus;
        private bool _isShotsFace;
        private CascadeClassifier _cascadeClassifier;
        private LBPHFaceRecognizer _iBPHFaceRecognizer;
        private Dictionary<int, string> _labelsToId = new Dictionary<int, string>();
        private bool _isDetectFace = false;
        private bool _isRecognitionFace = false;
        public LocalCameraUserControl()
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

 
        /// <summary>
        /// 外部命令
        /// </summary>
        public Func<string, bool> ExternalAction
        {
            get;set;
        }

        static LocalCameraUserControl()
        {
            IsShotsFaceProperty =DependencyProperty.Register("IsShotsFace", typeof(bool), typeof(LocalCameraUserControl), new PropertyMetadata(false, IsCaptureVideoCallBack));
            CameraStatusProperty =DependencyProperty.Register("Status", typeof(CameraStatus), typeof(LocalCameraUserControl), new PropertyMetadata(CameraStatus.Stop, CameraStatusCallBack));
            IsDetectFaceProperty =DependencyProperty.Register("IsDetectFace", typeof(bool), typeof(LocalCameraUserControl), new PropertyMetadata(false, IsDetectFaceCallBack));
            IsRecognitionFaceProperty =DependencyProperty.Register("IsRecognitionFace", typeof(bool), typeof(LocalCameraUserControl), new PropertyMetadata(false, IsRecognitionFaceCallBack));
        }

        ~LocalCameraUserControl()
        {
            //_videoCapture?.Release();这种会自己释放
            _iBPHFaceRecognizer?.Dispose();//关闭时释放 避免内存泄漏
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
                if (_videoCapture==null)
                {
                    _videoCapture = new VideoCapture(CaptureDevice.Any);
                    if (!_videoCapture.IsOpened())
                    {
                        this.Dispatcher?.Invoke(() =>
                        {
                            BusyIndicator1.IsBusy = false;
                        });
                        TipService.Instance.ShowTip(TipService.ToolTip, 1000, "摄像头故障");
                        return;
                    }
                    _videoCapture.Set(CaptureProperty.FrameWidth, this.ActualWidth);//宽度 根据控件的大小来决定           
                    _videoCapture.Set(CaptureProperty.FrameHeight, this.ActualHeight);//高度     
                }
                GetFaceRecognizer();
                PlayLacalCamera();
                _cameraStatus = CameraStatus.Palying;
            });
        }



        private async void LocalCameraUserControl_UnloadedAsync(object sender, RoutedEventArgs e)
        {        
            _cameraStatus = CameraStatus.Stop;
            await PlayLacalCamera();
            //_backgroundTask.Wait();//等待线程结束 这里会存在着问题 unloaded代表卸载后 如果里面含有ui线程会直接卡死
            //_backgroundTask.Dispose();//释放线程
            //_videoCapture?.Release();
            //_isDetectFace = false;//检测
            //_isShotsFace = false;//截取
            //_isRecognitionFace = false;//识别
           
            VideoImage.Source = null;
        }
       
        private Task PlayLacalCamera()
        {
            return Task.Run(() =>
              {
                  Mat cFrame = null;
                  try
                  {
                      while (_cameraStatus!= CameraStatus.Stop)
                      {
                          if (_cameraStatus== CameraStatus.Suspend)
                          {
                              Thread.Sleep(1000);
                              continue;
                          }
                          cFrame = new Mat();
                          _videoCapture.Read(cFrame);//获取frame从摄像头或者视频文件
                          int sleepTime = (int)Math.Round(1000 / _videoCapture.Fps);//帧数，Thread.Sleep(40);
                          Cv2.WaitKey(sleepTime);//沉睡
                          if (cFrame.Empty())
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
                              //绘制指定区域(人脸框) minNeighbors:邻居像素得符合多少个才能被认为是人脸 minSize:最小50 50才能被检测到
                              var rect = _cascadeClassifier.DetectMultiScale(cFrame, minNeighbors: 5, minSize: new OpenCvSharp.Size(50, 50));
                              if (rect.Length > 0)//捕捉到人脸
                              {
                                  foreach (var item in rect)
                                  {
                                      Scalar color = Scalar.FromRgb(255, 242, 0);
                                      Cv2.Rectangle(cFrame, item, color, 2);
                                  }
                                  ShotFace(cFrame, rect);//嵌入功能 截图
                                  var result=DistinguishFace(cFrame, rect);//同步运行 阻塞 引用传递
                                  if (result!= ResultType.RecogineAgian)
                                  {
                                      _cameraStatus = CameraStatus.Stop;
                                      return;
                                  }
                              }
                          }
                          //Cv2.Flip(cFrame, cFrame, FlipMode.Y);
                          Application.Current.Dispatcher?.Invoke(() =>
                          {
                              var image = ImageHelper.Instance.ConvertBitmapToBitmapImage(cFrame.ToBitmap());
                              image.CacheOption = BitmapCacheOption.None;//创建完后 要是没有引用就释放
                              VideoImage.Source = image;
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
                  }
              });
        }


        private ResultType DistinguishFace( Mat cFrame, OpenCvSharp.Rect[] rect) 
        {
            var faceRecognitionResult = ResultType.RecogineAgian;
            try
            {
                //是否开启识别
                if (_isRecognitionFace)
                {
                    if (rect.Length > 2)
                    {
                        TipService.Instance.ShowTip(TipService.ToolTip, 1000, "检测到多张人脸");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Mat cFace = new Mat(cFrame, rect[0]);
                        Cv2.Resize(cFace, cFace, new OpenCvSharp.Size(100, 100));
                        Cv2.CvtColor(cFace, cFace, ColorConversionCodes.BGR2GRAY);
                        Cv2.EqualizeHist(cFace, cFace);
                        var label = _iBPHFaceRecognizer.Predict(cFace);
                        cFace.Release();
                        string id;
                        if (_labelsToId.TryGetValue(label, out id))//识别到了返回id
                        {
                            //开始登录 返回bool 
                            //Cv2.PutText(cFrame, name, new OpenCvSharp.Point(rect[0].X, rect[0].Y), HersheyFonts.Italic, 1, color);
                            if (ExternalAction!=null)
                            {
                                var isSuccessLogin=ExternalAction.Invoke(id);//同步
                                if (isSuccessLogin)
                                {
                                    faceRecognitionResult = ResultType.Success;
                                }
                            }                           
                        }
                        else
                        {
                            //弹框 3种选择 dialog
                            this.Dispatcher?.Invoke(()=>
                            {
                                SubWindowsService.Instance.OpenWindow(SubWindowsService.FaceRecognitionFailurePage,IsDialog:true);
                                ResultType resultType;
                                if (Enum.TryParse(SubWindowsService.Instance.Result.ToString(),out resultType))
                                {
                                    faceRecognitionResult = resultType;
                                }
                                else
                                {
                                    Logger.Info("人脸失败弹窗无结果返回");
                                }
                            });
                        }
                    }
                }
            }
            catch ( Exception ex) 
            {
                Logger.Error(ex);
            }
            finally
            {
                
            }
            return faceRecognitionResult;
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

       
   
        private void GetFaceRecognizer()
        {
            //获得Debug下的人脸图片 进行灰度和亮度处理
            string path = @"Faces";
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] files = root.GetFiles();
            var imageList = new List<Mat>();
            var idList = new List<int>();
            int i = 0;
            _labelsToId.Clear();
            foreach (var item in files)
            {

                var image = Cv2.ImRead(item.FullName);
                Cv2.CvtColor(image, image, ColorConversionCodes.BGR2GRAY);
                //Cv2.EqualizeHist(image, image);
                imageList.Add(image);
                i++;
                idList.Add(i);
                _labelsToId.Add(i, item.Name);//字典 可以根据id获取自己需要的 FaceId
            
            }
            if (imageList.Count>0)
            {
                //使用了LBPHFaceRecognizer类型的人脸识别器
                if (_iBPHFaceRecognizer == null)
                {
                    _iBPHFaceRecognizer = LBPHFaceRecognizer.Create();
                }
                //进行人脸数据的训练 每张图片及它的标识Id
                _iBPHFaceRecognizer.Train(imageList, idList);
            }
       
        }

    }
}
