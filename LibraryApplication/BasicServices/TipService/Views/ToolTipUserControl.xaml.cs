using System.Windows;
using System.Windows.Controls;

namespace BasicServices.TipService.Views
{
    /// <summary>
    /// 黑色的弹框提示
    /// </summary>
    public partial class ToolTipUserControl : UserControl
    {
        public ToolTipUserControl()
        {
            InitializeComponent();
            Tb.MaxWidth = SystemParameters.PrimaryScreenWidth / 2;//默认最大宽度的1/2
        }
    }
}
