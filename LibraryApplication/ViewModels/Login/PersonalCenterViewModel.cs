using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Home;

namespace ViewModels.Login
{
    public class PersonalCenterViewModel : LibraryViewModelBase
    {
        protected override void Load()
        {
            Time = 300;//个人中心300s
            base.Load();
        }
    }
}
