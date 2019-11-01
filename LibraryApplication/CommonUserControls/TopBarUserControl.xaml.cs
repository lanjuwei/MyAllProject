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
    /// TopBarUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class TopBarUserControl : UserControl
    {
        public TopBarUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty GoBackCommandProperty;
        public ICommand GoBackCommand
        {
            get { return (ICommand)GetValue(GoBackCommandProperty); }
            set { SetValue(GoBackCommandProperty, value); }
        }
        public static readonly DependencyProperty CloseCommandProperty;
        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        static TopBarUserControl()
        {
            GoBackCommandProperty = DependencyProperty.Register("GoBackCommand",typeof(ICommand), typeof(TopBarUserControl),new FrameworkPropertyMetadata(null));
            CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(TopBarUserControl), new FrameworkPropertyMetadata(null));
        }
    }
}
