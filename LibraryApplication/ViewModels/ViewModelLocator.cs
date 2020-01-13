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
using ViewModels.Book;
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
            SimpleIoc.Default.Register<HomeViewModel>();//主界面
            SimpleIoc.Default.Register<LoginViewModel>();//选择登录界面
            #region 登录模式
            SimpleIoc.Default.Register<HandwordLoginViewModel>();//手动登录
            SimpleIoc.Default.Register<FaceLoginViewModel>();//人脸登录
            SimpleIoc.Default.Register<PersonalCenterViewModel>();//个人中心
            SimpleIoc.Default.Register<ChangePasswordViewModel>();//修改密码
            SimpleIoc.Default.Register<RegistrateFaceViewModel>();//注册人脸
            #endregion
            #region book
            SimpleIoc.Default.Register<OperateBooksViewModel>();
            #endregion
            NaviService.Instance.Init();//初始化页面导航
            SocketHelper.Instance.InitLongSocketAsync();//初始化长连接socket
        }

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
        public HomeViewModel HomeViewModel => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public LoginViewModel LoginViewModel => ServiceLocator.Current.GetInstance<LoginViewModel>();
        public HandwordLoginViewModel HandwordLoginViewModel => ServiceLocator.Current.GetInstance<HandwordLoginViewModel>();
        public FaceLoginViewModel FaceLoginViewModel => ServiceLocator.Current.GetInstance<FaceLoginViewModel>();
        public PersonalCenterViewModel PersonalCenterViewModel => ServiceLocator.Current.GetInstance<PersonalCenterViewModel>();
        public OperateBooksViewModel OperateBooksViewModel => ServiceLocator.Current.GetInstance<OperateBooksViewModel>();
        public ChangePasswordViewModel ChangePasswordViewModel => ServiceLocator.Current.GetInstance<ChangePasswordViewModel>();
        public RegistrateFaceViewModel RegistrateFaceViewModel => ServiceLocator.Current.GetInstance<RegistrateFaceViewModel>();
    }
}
