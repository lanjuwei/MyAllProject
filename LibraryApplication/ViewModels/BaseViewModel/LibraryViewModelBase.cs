using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using BaseSetting.Needs;
using BasicServices.Navigation;
using BasicServices.SocketService;
using BasicServices.SubWindowService.ViewService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;
using Model.Login;

namespace ViewModels.Home
{
    /// <summary>
    /// 抽象基类
    /// </summary>
    public abstract class LibraryViewModelBase : ViewModelBase
    {
        
        public LibraryViewModelBase()
        {
                    
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Time > 0)
            {
                Time--;
            }
            else
            {
                MoveToNextPage();//时间一到不停的调用
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
        /// <summary>
        /// 当前用户 登录的时候赋予值 用来保存用户信息 以及用唯一主键id来调用其他的接口
        /// </summary>
        protected static UserModel User { get; set; }
        /// <summary>
        /// 能否回到主界面
        /// </summary>
        protected static bool isCanClose = true;

        protected virtual void Load()
        {
            Time = 60;
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer?.Start();
        }
        protected virtual void UnLoad()
        {           
            dispatcherTimer?.Stop();
            dispatcherTimer.Tick -= DispatcherTimer_Tick;
            isCanClose = true;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        protected bool LoginIn(string id,string password) 
        {
            IndividualNeeds.Instance.CommonVariables.IsLoading = true;
            try
            {
                var task = SocektInterface.GetUserInfoAsync(id, password);
                task.Wait();//阻塞
                if (task.Result.IsSuccess)
                {
                    User = task.Result.Data;
                    if (NavigateInterface.Parameter is ButtonType buttonType)
                    {
                        switch (buttonType)
                        {
                            case ButtonType.PersonalCenter:
                                NavigateInterface.NavigateTo(PageKey.PersonalCenterPage);
                                break;
                            case ButtonType.RenewBook:
                            case ButtonType.BorrowBook:
                            case ButtonType.ReturnBook:
                                NavigateInterface.NavigateTo(PageKey.OperateBooksPage, buttonType);
                                break;
                        }
                    }
                }
                return task.Result.IsSuccess;
            } 
            finally            
            {
                IndividualNeeds.Instance.CommonVariables.IsLoading = false;
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        protected void LoginOut()
        {
            User = null;
        }
        /// <summary>
        /// 倒计时的时间到了 需要移动到下一个界面 默认为是退出到主界面 可重写移动到你需要的地方
        /// </summary>
        protected virtual void MoveToNextPage(object parameter =null)
        {
            if (isCanClose)
            {
                if (SubWindowsService.Instance.IsAliveWindow(SubWindowsService.Instance.WindowId))
                {
                    SubWindowsService.Instance.CloseWindow(SubWindowsService.Instance.WindowId);
                }
                NavigateInterface.NavigateTo(PageKey.MainPage, parameter);
                LoginOut();
            }
        }
    }
}
