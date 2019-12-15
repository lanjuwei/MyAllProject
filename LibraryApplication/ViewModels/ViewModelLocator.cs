using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicFunction.Helper;
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

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            NaviService.Instance.Init();
            SocketHelper.Instance.InitLongSocketAsync();
        }

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
        public HomeViewModel HomeViewModel => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public LoginViewModel LoginViewModel => ServiceLocator.Current.GetInstance<LoginViewModel>();

    }
}
