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

        public bool Login(string id)
        {
            var str = id.Split('_');
            if (str.Length==2)
            {
                return LoginIn(str[0], str[1]);
            }
            else
            {
                Logger.Info("人脸传过来的id解析不对啊"); 
            }
            return false;
        }
    }
}
