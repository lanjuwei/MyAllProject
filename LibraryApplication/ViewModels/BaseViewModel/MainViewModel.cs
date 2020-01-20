using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BasicFunction.Helper;
using BasicFunction.Log;
using BasicServices.Navigation;
using BasicServices.SubWindowService.ViewService;
using BasicServices.SugarDbService;
using GalaSoft.MvvmLight.Command;
using Model;
using SqlSugar;
using ViewModels.Properties;

namespace ViewModels.Home
{
    /// <summary>
    /// 之前遇到过框架不一致的问题 更新下框架既可 在每个项目的属性
    /// </summary>
    public class MainViewModel : LibraryViewModelBase
    {
        public MainViewModel()
        {
            InitializeDatabase();
        }


        private void InitializeDatabase()
        {
            try
            {
                if (Settings.Default.IsFirstStart == false)
                {
                    DbService.InitDb();//初始化数据库
                    Settings.Default.IsFirstStart = true;
                    Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected override void Load()
        {
            NaviService.Instance.NavigateTo(PageKey.MainPage);//move to page
            SubWindowsService.Instance.OpenWindow(SubWindowsService.UpdatePage);//wait all data complete
            if (!Directory.Exists(@"Images"))//创建目录
            {
                Directory.CreateDirectory(@"Images/Gif");
            }
            var s = DbService.GetDbService();//data running
            SocketHelper.Instance.SocketDisconnectCallBack += Instance_SocketDisconnectCallBack;
            //SoundHelper.Instance.TransformTextToVideo("welcome");
        }

        protected override void UnLoad()
        {
            SocketHelper.Instance.SocketDisconnectCallBack -= Instance_SocketDisconnectCallBack;
            base.UnLoad();
        }

        private void Instance_SocketDisconnectCallBack(bool obj)
        {
            if (obj)//断开
            {
                CloseCommand?.Execute(null);//退回到主页
                SubWindowsService.Instance.OpenWindow(SubWindowsService.NetworkAbnormalPage);
            }
            else
            {
                if (SubWindowsService.Instance.IsAliveWindow(SubWindowsService.Instance.WindowId))
                {
                    SubWindowsService.Instance.CloseWindow(SubWindowsService.Instance.WindowId);
                }
            }
        }
    }
}
