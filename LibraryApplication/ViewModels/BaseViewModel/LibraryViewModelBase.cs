using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using BasicServices.Navigation;
using BasicServices.SocketService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ViewModels.Home
{
    /// <summary>
    /// 抽象基类
    /// </summary>
    public abstract class LibraryViewModelBase : ViewModelBase
    {
        
        public LibraryViewModelBase()
        {
            dispatcherTimer.Tick -= DispatcherTimer_Tick;
            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Time > 0)
            {
                Time--;
            }
            else
            {
                MoveToNextPage();
            }
        }

        public ICommand LoadCommand => new RelayCommand(Load);
        public ICommand UnLoadCommand => new RelayCommand(UnLoad);
        public ICommand GoBackCommand => new RelayCommand(()=>{ NavigateInterface.GoBack(); });
        public ICommand CloseCommand => new RelayCommand(() => { MoveToNextPage(); });
        protected static ISocektInterface SocektInterface { get; set; }= new SocketService();
        protected static INaviServiceInterface NavigateInterface { get; set; } = NaviService.Instance;
        protected static DispatcherTimer dispatcherTimer=new DispatcherTimer() { Interval= TimeSpan.FromSeconds(1)};
        private int time;
        public  int Time{get => time; set{Set(()=> Time,ref time,value);}}

        protected virtual void Load()
        {
            Time = 60;
            dispatcherTimer?.Start();
        }
        protected virtual void UnLoad()
        {
            dispatcherTimer?.Stop();
        }

        /// <summary>
        /// 倒计时的时间到了 需要移动到下一个界面 默认为是退出到主界面 可重写移动到你需要的地方
        /// </summary>
        protected virtual void MoveToNextPage(object parameter =null)
        {
            NavigateInterface.NavigateTo(PageKey.MainPage, parameter);
        }
    }
}
