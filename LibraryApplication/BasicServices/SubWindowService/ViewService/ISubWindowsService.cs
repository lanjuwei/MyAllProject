using System.Windows;

namespace BasicServices.SubWindowService.ViewService
{
    public interface ISubWindowsService
    {

        /// <summary>
        /// 窗口关闭 返回结果
        /// </summary>
        object Result { get; set; }
        /// <summary>
        /// 当前窗口的id
        /// </summary>
        string WindowId { get; set; }

        /// <summary>
        /// 打开窗口 除非你要在外界通过窗口返回的id控制窗口 否则请用viewmodel里的窗口id（可用来导航）
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="parameter"></param>
        /// <param name="IsDialog"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        string OpenWindow(string pageKey, object parameter = null, bool IsDialog = false, Point? position = null);
        /// <summary>
        /// 在子窗口上导航 
        /// </summary>
        /// <param name="subWinKey">子窗口id</param>
        /// <param name="pageKey">ViewModel</param>
        /// <param name="parameter">传入参数</param>
        void Navigate(string subWinKey, string pageKey, object parameter = null);
        bool IsAliveWindow(string subWinKey);
        Point GetWindowPosition(string subWinKey);
        void HideWindow(string subWinKey);
        void HideAllWindows();
        void ShowAllWindows();
        void CloseWindow(string subWinKey);
        void CloseAllWindows();
        void GoBack(string subWinKey);
        void GoForward(string subWinKey);
    }


}
