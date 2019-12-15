using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using BasicServices.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ViewModels.Home
{
    /// <summary>
    /// 抽象基类
    /// </summary>
    public abstract class LibraryViewModelBase : ViewModelBase
    {
        protected DispatcherTimer dispatcherTimer;
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
                MoveToNextPage();
            }
        }

        public ICommand LoadCommand => new RelayCommand(Load);
        public ICommand UnLoadCommand => new RelayCommand(UnLoad);
        public ICommand GoBackCommand => new RelayCommand(()=>{ NavigationService.Instance.GoBack(); });
        public ICommand CloseCommand => new RelayCommand(() => { MoveToNextPage(); });
        public  int Time
        {
            get => time; set
            {
                time = value;
                RaisePropertyChanged(()=> Time);
            }
        }


        protected virtual void Load()
        {
            Time = 60;//默认是60
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            //dispatcherTimer.Start();
        }
        protected virtual void UnLoad()
        {
            if (dispatcherTimer!=null)
            {
                dispatcherTimer.Stop();
                dispatcherTimer.Tick -= DispatcherTimer_Tick;
                dispatcherTimer = null;
                Time = 0;
            }
        }

        /// <summary>
        /// 倒计时的时间到了 需要移动到下一个界面 默认为是退出到主界面 可重写移动到你需要的地方
        /// </summary>
        protected virtual void MoveToNextPage()
        {
            NavigationService.Instance.NavigateTo(PageName.MainPage);
        }



        private int time ;
    
    }
}
