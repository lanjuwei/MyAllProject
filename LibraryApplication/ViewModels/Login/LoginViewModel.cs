using BasicFunction.Helper;
using System;
using ViewModels.Home;

namespace ViewModels.Login
{
    public class LoginViewModel : LibraryViewModelBase
    {
        public LoginViewModel()
        {
        }

        protected override void MoveToNextPage()
        {
            base.MoveToNextPage();
        }

        protected override async void Load()
        {
            var sss=await SocektInterface.GetUserInfoAsync("441522199504231013","12345678l");
            base.Load();
        }

    }
}