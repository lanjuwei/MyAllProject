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
    public class MainViewModel
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
                //Logger.Error("Could not initialize db: " + ex);
            }
        }

        public ICommand LoadCommand => new RelayCommand(()=> 
        {
            NaviService.Instance.NavigateTo(PageKey.MainPage);
            //SoundHelper.Instance.TransformTextToVideo("welcome");
            var s=DbService.GetDbService();
        });

    }
}
