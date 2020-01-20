using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BasicServices.SubWindowService.ViewService
{
    public class SubWindowBase: ViewModelBase
    {
        /// <summary>
        /// 窗口的唯一标志
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 窗口卸载
        /// </summary>
        public virtual void UnLoaded()
        {

        }
        
        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void Loaded()
        {

        }
        /// <summary>
        /// 导航过来的参数
        /// </summary>
        public object LoadParamerter { get; set; }
        public  Action<object> CloseWithParameter { get; set; }
        public Action Close { get; set; }
        /// <summary>
        /// 关闭窗口 传递一个值回去 受保护方法 继承才可用 用于绑定
        /// </summary>
        public RelayCommand<object> CloseWithParameterCommand => new RelayCommand<object>(CloseWithParameter);
        public RelayCommand CloseCommand => new RelayCommand(Close);
        /// <summary>
        /// 拖拽窗口
        /// </summary>
        public RelayCommand DragCommand => new RelayCommand(DragAction);

        public Action DragAction { get; set; }

    }
}
