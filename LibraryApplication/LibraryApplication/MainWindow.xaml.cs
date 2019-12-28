using BaseSetting.Needs;
using BasicFunction.Helper;
using LibraryApplication.Properties;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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

namespace LibraryApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //RootGrid.Width = IndividualNeeds.Instance.PageVariables.RootGridWidth;//可配置 比例 viewbox会以一定的比例缩放
            //RootGrid.Height = IndividualNeeds.Instance.PageVariables.RootGridHeight;
            TestImage();
        }

        private void TestImage()
        {
            //Directory.CreateDirectory(@"CashLog");
            //using (File.Create(@"CashLog\my.txt"))
            //{
            //}

            //ssssss.Source = new BitmapImage(new Uri(@"NewFolder1\my.png", UriKind.RelativeOrAbsolute));
            //PathName = @"NewFolder1/my.png";

            //var path = @"Faces\my.png";
            //var image = Cv2.ImRead(path);
            //Cv2.CvtColor(image, image, ColorConversionCodes.BGR2GRAY);//灰度处理
            //Cv2.Normalize(image, image,alpha:0,beta:255,normType: NormTypes.MinMax);//归一化处理
            //var byte1=image.ToBytes();
            //var ss=ImageHelper.Instance.ToImage(byte1);
            //ssssss.Source = ss;
        }
    }
}
