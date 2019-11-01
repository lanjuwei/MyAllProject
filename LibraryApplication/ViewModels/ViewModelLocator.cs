using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicServices.Navigation;
using BasicServices.SubWindowService.ViewService;
using BasicServices.TipService;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using ViewModels.Home;
using ViewModels.Login;

namespace ViewModels
{
    public class ViewModelLocator
    {
        private static ViewModelLocator _viewModelLocator;
        public static ViewModelLocator Instance => _viewModelLocator ?? (_viewModelLocator = new ViewModelLocator());

        public ViewModelLocator()
        {
            //主窗口
            MainViewModel = new MainViewModel();
            //主页
            HomeViewModel = new HomeViewModel();
            //登录
            LoginViewModel = new LoginViewModel();


        }

        //主窗口
        public MainViewModel MainViewModel { get; set; }
        //主页
        public HomeViewModel HomeViewModel { get; set; }
        //登录
        public LoginViewModel LoginViewModel { get; set; }
    }
}
