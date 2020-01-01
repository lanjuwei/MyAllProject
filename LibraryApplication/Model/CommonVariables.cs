using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CommonVariables : ViewModelBase
    {
        private bool isLoading = false;
        private string loadingContent="正在加载，请稍后...";

        public bool IsLoading
        {
            get => isLoading; set
            {
                Set(() => IsLoading, ref isLoading, value);
            }
        }

        public string LoadingContent
        {
            get => loadingContent; set
            {
                Set(() => LoadingContent, ref loadingContent, value);
            }
        }
        /// <summary>
        /// 登录方法
        /// </summary>
        public Func<string,string, Task<bool>> LoginAction { get; set; }
    }
}
