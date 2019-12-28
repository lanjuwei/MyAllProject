using BasicServices.SubWindowService.ViewService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BasicServices.SubWindowService.View
{
    /// <summary>
    /// PlaceBookPage.xaml 的交互逻辑
    /// </summary>
    public partial class PlaceBookPage : Page
    {
        private DispatcherTimer dispatcherTimer;
        private int count=30;
        public PlaceBookPage()
        {
            InitializeComponent();
            dispatcherTimer = new DispatcherTimer() { Interval=TimeSpan.FromSeconds(1)};
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            Loaded += PlaceBookPage_Loaded;
            Unloaded += PlaceBookPage_Unloaded;
            GifControl.GifImagePath = @"Images/Gif/放置图书.gif";

        }

        private void PlaceBookPage_Unloaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            tb.Text = "10";
            count = 10;
        }



        private void PlaceBookPage_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (count>0)
            {
                count--;
                tb.Text = count.ToString();
            }
            else
            {
                var vm = this.DataContext as SubWindowBase;
                vm?.Close.Invoke();
            }
        }

        private void CommonButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SubWindowBase;
            vm?.Close.Invoke();
        }
    }
}
