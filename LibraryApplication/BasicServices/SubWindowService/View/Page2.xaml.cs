using System.Windows.Controls;

namespace BasicServices.SubWindowService.View
{
    /// <summary>
    /// Page2.xaml 的交互逻辑 LJW
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            DataContext=new Page2ViewModel();
        }
    }
}
