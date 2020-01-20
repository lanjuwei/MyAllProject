using BaseSetting.Needs;
using BasicFunction.Helper;
using CommonUserControls;
using System;
using System.Collections.Generic;
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

namespace Views.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            IndividualNeeds.Instance.CommonVariables.CheckImage = CheckImage;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CheckImage(VolumeSizeHelper.Instance.defaultPlaybackDevice.IsMuted);
        }

        private void CheckImage(bool IsCheck) 
        {
            var bitmap = new BitmapImage (IsCheck?new Uri("pack://application:,,,/CommonUserControls;component/Images/yinliangguanbi.png") :new Uri("pack://application:,,,/CommonUserControls;component/Images/yinliangkai.png")) {CacheOption= BitmapCacheOption.None };
            VolumeBtn.NormalImageSource = bitmap;
        }
    }
}
