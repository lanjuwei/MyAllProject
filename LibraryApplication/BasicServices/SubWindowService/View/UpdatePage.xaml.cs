using BasicServices.SubWindowService.ViewService;
using System;
using System.Collections.Generic;
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

namespace BasicServices.SubWindowService.View
{
    /// <summary>
    /// UpdatePage.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePage : Page
    {
        public UpdatePage()
        {
            InitializeComponent();
            Loaded += UpdatePage_Loaded;

        }



        private void UpdatePage_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(()=> 
            {
                for (int i = 1; i <= 100; i++)
                {
                    this.Dispatcher?.Invoke(()=> 
                    {
                        MyProgressBar.Value = i;
                    });                   
                    Thread.Sleep(20);
                }
                this.Dispatcher?.Invoke(() =>
                {
                    var vm = this.DataContext as SubWindowBase;
                    vm?.Close.Invoke();
                });
            });
        }
    }
}
