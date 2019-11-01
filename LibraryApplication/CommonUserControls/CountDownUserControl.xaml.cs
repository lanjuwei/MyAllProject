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
    /// CountDownUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class CountDownUserControl : UserControl
    {
        public CountDownUserControl()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty CountProperty;
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }
        static CountDownUserControl()
        {
            CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(CountDownUserControl),new PropertyMetadata(60));
        }
    }
}
