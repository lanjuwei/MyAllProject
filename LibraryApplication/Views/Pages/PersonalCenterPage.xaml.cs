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
using ViewModels.Login;

namespace Views.Pages
{
    /// <summary>
    /// PersonalCenterPage.xaml 的交互逻辑
    /// </summary>
    public partial class PersonalCenterPage : Page
    {
        public PersonalCenterPage()
        {
            InitializeComponent();
            Loaded += PersonalCenterPage_Loaded;
        }

        private void PersonalCenterPage_Loaded(object sender, RoutedEventArgs e)
        {
            var vm=DataContext as PersonalCenterViewModel;
            vm?.UiLoad();
            //可以触发到这里来 也就是baseviewmodel的绑定load也没有触发 这里存在着很严重的bug 这个框架
        }
    }
}
