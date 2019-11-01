using System.Windows;
using System.Windows.Controls;
using BasicServices.SubWindowService.ViewService;

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
            AddProportion();
        }

        private void AddProportion()
        {
            RootGrid.Width = SystemParameters.PrimaryScreenWidth;
            RootGrid.Height = SystemParameters.PrimaryScreenHeight;
            CurrentGrid.Width = 600 / 1920 * SystemParameters.PrimaryScreenWidth;//按比例赋值
            CurrentGrid.Height = 498 / 1080 * SystemParameters.PrimaryScreenHeight;
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

        public enum ResultType
        {
            RecogineAgian,
            ToLogin,
            Close
        }


    }
}
