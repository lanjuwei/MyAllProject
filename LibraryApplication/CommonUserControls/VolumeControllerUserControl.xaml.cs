using AudioSwitcher.AudioApi.CoreAudio;
using BaseSetting.Needs;
using BasicFunction.Helper;
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

namespace CommonUserControls
{
    /// <summary>
    /// VolumeControllerUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class VolumeControllerUserControl : UserControl
    {
        public VolumeControllerUserControl()
        {
            InitializeComponent();
            Loaded += VolumeControllerUserControl_Loaded;
            Application.Current.MainWindow.Unloaded += MainWindow_Unloaded;          
        }


        private void VolumeControllerUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (VolumeSizeHelper.Instance.defaultPlaybackDevice.State == AudioSwitcher.AudioApi.DeviceState.Disabled)//声音控件不可用时 不让点击
            {
                this.IsEnabled = false;
            }
            slider.Value = VolumeSizeHelper.Instance.defaultPlaybackDevice.Volume;
            tb1.Text = ((int)slider.Value).ToString();
            CheckBox1.IsChecked = VolumeSizeHelper.Instance.defaultPlaybackDevice.IsMuted;
            IndividualNeeds.Instance.CommonVariables.CheckImage?.Invoke(VolumeSizeHelper.Instance.defaultPlaybackDevice.IsMuted);
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            VolumeSizeHelper.Instance.defaultPlaybackDevice?.Dispose();//释放音量句柄
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (VolumeSizeHelper.Instance.defaultPlaybackDevice?.IsMuted==true)
            {
                VolumeSizeHelper.Instance.defaultPlaybackDevice.Mute(false);
                CheckBox1.IsChecked = false;
                IndividualNeeds.Instance.CommonVariables.CheckImage?.Invoke(false);
            }
            VolumeSizeHelper.Instance.defaultPlaybackDevice.Volume = e.NewValue;
            tb1.Text = ((int)e.NewValue).ToString();
        }

        private void CommonCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox1.IsChecked.HasValue)
            {
                VolumeSizeHelper.Instance.defaultPlaybackDevice.Mute(CheckBox1.IsChecked.Value);
                IndividualNeeds.Instance.CommonVariables.CheckImage?.Invoke(CheckBox1.IsChecked.Value);
            }
        }
    }
}
