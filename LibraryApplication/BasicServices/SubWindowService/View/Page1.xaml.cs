using System.Windows;
using System.Windows.Controls;
using BasicServices.SubWindowService.ViewService;

namespace BasicServices.SubWindowService.View
{
    /// <summary>
    /// Page1.xaml 的交互逻辑 LJW
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SubWindowBase;
            vm?.Close.Invoke();
        }
        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SubWindowBase;
            vm?.CloseWithParameter(true);
        }
        private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
        {
            //var vm = this.DataContext as SubWindowBase;
            //if (vm != null) SubWindowsService.Instance.Navigate(vm.Id, SubWindowsService.Loginview1);
        }
    }
}
