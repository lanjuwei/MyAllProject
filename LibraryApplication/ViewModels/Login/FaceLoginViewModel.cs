using BaseSetting.Needs;
using BasicFunction.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Home;

namespace ViewModels.Login
{
    public class FaceLoginViewModel : LibraryViewModelBase
    {
        public string GifPath { get; set; } = @"Images/Gif/刷脸动画.gif";
        public FaceLoginViewModel()
        {
            IndividualNeeds.Instance.CommonVariables.LoginAction = base.LoginIn;
        }

    }
}
