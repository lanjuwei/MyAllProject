using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryApplication
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;
        //LierdaCracker cracker = new LierdaCracker();
        protected override void OnStartup(StartupEventArgs e)
        {
            //cracker.Cracker(100);//垃圾回收黑科技 
            base.OnStartup(e);
            bool ret;
            mutex = new System.Threading.Mutex(true, "ElectronicNeedleTherapySystem", out ret);
            if (!ret)
            {
                Environment.Exit(0);
            }
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;//ui线程意外错误捕获处理

        }
        /// <summary>
        /// 处理全局意外异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Logger.Log("程序发生意外错误！！");
            //Logger.LogError(e.Exception);
            e.Handled = true;//使得程序不能崩溃
        }
     

    }
}
