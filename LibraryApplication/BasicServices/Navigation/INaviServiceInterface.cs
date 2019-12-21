using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicServices.Navigation
{
    public interface INaviServiceInterface
    {
        /// <summary>
        /// 传递到一个页面的参数
        /// </summary>
        object Parameter { get; set; }
        /// <summary>
        /// 页面回退
        /// </summary>
        /// <param name="frameKey"></param>
        void GoBack(FrameKey frameKey = FrameKey.MainFrame);
        /// <summary>
        /// 导航到指定的页面
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="parameter"></param>
        /// <param name="frameKey"></param>
        void NavigateTo(PageKey pageKey, object parameter = null, FrameKey frameKey = FrameKey.MainFrame);
    }
}
