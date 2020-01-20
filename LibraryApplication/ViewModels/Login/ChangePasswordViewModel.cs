using BaseSetting.Needs;
using BasicServices.TipService;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Home;

namespace ViewModels.Login
{
    public  class ChangePasswordViewModel : LibraryViewModelBase
    {
        private string _oldPassword;
        private bool _isFocusOldPassword;
        private string _newPassword;
        private bool _isFocusNewPassword;
        private string _confirmPassword;
        private bool _isFocusConfirmPassword;
        private bool _isSureButtonEnable;

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                Set(() => OldPassword, ref _oldPassword, value);
                IsCanClick();
            }
        }
        /// <summary>
        /// 聚焦到旧密码输入框
        /// </summary>
        public bool IsFocusOldPassword
        {
            get => _isFocusOldPassword; set
            {
                Set(() => IsFocusOldPassword, ref _isFocusOldPassword, value);
            }
        }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword
        {
            get => _newPassword; set
            {
                Set(() => NewPassword, ref _newPassword, value);
                IsCanClick();
            }
        }
        /// <summary>
        /// 聚焦到新密码控件
        /// </summary>
        public bool IsFocusNewPassword
        {
            get => _isFocusNewPassword; set
            {
                Set(() => IsFocusNewPassword, ref _isFocusNewPassword, value);
            }
        }

        /// <summary>
        /// 新密码
        /// </summary>
        public string ConfirmPassword
        {
            get => _confirmPassword; set
            {
                Set(() => ConfirmPassword, ref _confirmPassword, value);
                IsCanClick();
            }
        }
        /// <summary>
        /// 聚焦到新密码控件
        /// </summary>
        public bool IsFocusConfirmPassword
        {
            get => _isFocusConfirmPassword; set
            {
                Set(() => IsFocusConfirmPassword, ref _isFocusConfirmPassword, value);
            }
        }

        /// <summary>
        /// 老外的框架有漏洞 不能用rasiecommand了 吗的
        /// </summary>
        public bool IsSureButtonEnable
        {
            get => _isSureButtonEnable; set
            {
                Set(() => IsSureButtonEnable, ref _isSureButtonEnable, value);
            }
        }
        protected override void Load()
        {
            IsFocusOldPassword = false;
            IsFocusOldPassword = true;
            Time = 300;
            StartTimer();
        }

        public RelayCommand SureCommand => new RelayCommand(async () =>
        {
            if (User.Password!= OldPassword)
            {
                OldPassword = string.Empty;
                IsFocusOldPassword = false;
                IsFocusOldPassword = true;
                TipService.Instance.ShowTip(TipService.ToolTip, 500, "旧密码不正确");
                return;
            }
            if (NewPassword!= ConfirmPassword)
            {
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
                IsFocusNewPassword = false;
                IsFocusNewPassword = true;
                TipService.Instance.ShowTip(TipService.ToolTip, 500, "新密码与确认密码不匹配");
                return;
            }
            IndividualNeeds.Instance.CommonVariables.IsLoading = true;
            var result=await SocektInterface.ChangePassword(ConfirmPassword);
            if (result.IsSuccess)
            {
                if (IndividualNeeds.Instance.CommonVariables.User!=null)
                {
                    IndividualNeeds.Instance.CommonVariables.User.Password = ConfirmPassword;
                    NavigateInterface.GoBack();
                }
            }
            IndividualNeeds.Instance.CommonVariables.IsLoading = false;
        });

        private void IsCanClick()
        {
            IsSureButtonEnable = !string.IsNullOrWhiteSpace(OldPassword) && !string.IsNullOrWhiteSpace(NewPassword)&& !string.IsNullOrWhiteSpace(ConfirmPassword) ? true : false;
        }
    }
}
