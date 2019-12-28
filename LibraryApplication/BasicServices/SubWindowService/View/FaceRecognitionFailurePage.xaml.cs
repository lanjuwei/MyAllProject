using System.Windows;
using System.Windows.Controls;
using BasicServices.SubWindowService.ViewService;
using Model;

namespace BasicServices.SubWindowService.View
{
    /// <summary>
    /// FaceRecognitionFailurePage.xaml 的交互逻辑
    /// </summary>
    public partial class FaceRecognitionFailurePage  : Page
    {
        public FaceRecognitionFailurePage()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SubWindowBase;
            vm?.CloseWithParameter(ResultType.Close);
        }
        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SubWindowBase;
            vm?.CloseWithParameter(ResultType.RecogineAgian);
        }

        private void ButtonBase_OnClick3(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SubWindowBase;
            vm?.CloseWithParameter(ResultType.ToLogin);
        }
    }
}
