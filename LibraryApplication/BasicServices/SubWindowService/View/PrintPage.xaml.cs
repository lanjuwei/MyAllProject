using BasicServices.SubWindowService.ViewService;
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

namespace BasicServices.SubWindowService.View
{
    /// <summary>
    /// PrintPage.xaml 的交互逻辑
    /// </summary>
    public partial class PrintPage : Page
    {
        public PrintPage()
        {
            InitializeComponent();
            Loaded += PrintPage_Loaded;
            //this.NavigationService.LoadCompleted += NavigationService_LoadCompleted;
        }

        private void PrintPage_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SubWindowBase;
            if (vm.LoadParamerter!=null)
            {
                run.Text = vm.LoadParamerter.ToString();
            }
        }

        private void CommonButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SubWindowBase;
            vm?.Close.Invoke();
        }
    }
}
