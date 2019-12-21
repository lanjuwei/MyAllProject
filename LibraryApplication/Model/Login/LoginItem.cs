using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Login
{
    public class LoginItem : ViewModelBase
    {
        private string loginName = string.Empty;
        /// <summary>
        /// 登录的名称
        /// </summary>
        public string LoginName
        {
            get
            {
                return this.loginName;
            }
            set
            {
                if (this.loginName != value)
                {
                    this.loginName = value;
                    base.RaisePropertyChanged("LoginName");
                }
            }
        }
        /// <summary>
        /// 登录的图片
        /// </summary>
        public string LoginImage { get; set; }
        /// <summary>
        /// 标志用于选择登录方式
        /// </summary>
        public LoginWay LoginTag { get; set; }
        /// <summary>
        /// 序号用于排序
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 可否点击
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
