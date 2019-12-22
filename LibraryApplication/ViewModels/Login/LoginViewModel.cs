using BasicFunction.Helper;
using BasicFunction.Log;
using GalaSoft.MvvmLight.Command;
using Model;
using Model.Login;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Home;

namespace ViewModels.Login
{
    public class LoginViewModel : LibraryViewModelBase
    {
        public LoginViewModel()
        {
        }

        private ObservableCollection<LoginItem> loginItemList;

        public ObservableCollection<LoginItem> LoginItemList
        {
            get => loginItemList;
            set
            {
                Set(() => LoginItemList, ref loginItemList, value);
            }
        }

        protected override void Load()
        {
            if (LoginItemList == null)
            {
                LoginItemList = new ObservableCollection<LoginItem>()
                {
                    new LoginItem(){LoginName = "人脸识别", LoginImage = "../Images/LoginImages/人脸识别.png", LoginTag = LoginWay.Veriface },
                    new LoginItem(){LoginName = "手动输入", LoginImage = "../Images/LoginImages/手动输入.png", LoginTag = LoginWay.Handword },
                    new LoginItem(){LoginName = "刷读者证",LoginImage = "../Images/LoginImages/读者证.png",LoginTag = LoginWay.SlotPatronCard,IsEnabled=false, },
                    new LoginItem(){LoginName = "刷身份证",LoginImage = "../Images/LoginImages/身份证.png",LoginTag = LoginWay.SlotIdCard,IsEnabled=false, },
                    new LoginItem(){LoginName = "信用登录",LoginImage = "../Images/LoginImages/芝麻.png",LoginTag = LoginWay.CreditLogin,IsEnabled=false, },
                    new LoginItem(){LoginName = "扫码登录",LoginImage = "../Images/LoginImages/二维码.png",LoginTag = LoginWay.Scanner,IsEnabled=false, },
                };
            }
            //var sss = await SocektInterface.GetUserInfoAsync("441522199504231013", "12345678l");
            base.Load();
        }

        public ICommand SelectCurrentLoginItemCommand => new RelayCommand<LoginItem>(t =>
        {
            if (t != null)
            {
                SelectLoginItem(t.LoginTag);
            }
        });

        private void SelectLoginItem(LoginWay loginTag)
        {
            switch (loginTag)
            {
                case LoginWay.SlotPatronCard:
                    break;
                case LoginWay.SlotIdCard:
                    break;
                case LoginWay.ScanQrCode:
                    break;
                case LoginWay.Veriface:
                    NavigateInterface.NavigateTo(BasicServices.Navigation.PageKey.FaceLoginPage);
                    break;
                case LoginWay.Fingerprint:
                    break;
                case LoginWay.Vein:
                    break;
                case LoginWay.Handword:
                    NavigateInterface.NavigateTo( BasicServices.Navigation.PageKey.HandwordLoginPage, NavigateInterface.Parameter);
                    break;
                case LoginWay.Scanner:
                    break;
                case LoginWay.OneDimensionalCode:
                    break;
                case LoginWay.VirtualReaderCard:
                    break;
                case LoginWay.CreditLogin:
                    break;
                case LoginWay.SocialSecurityCardCheck:
                    break;
                default:
                    break;
            }
        }
    }
}