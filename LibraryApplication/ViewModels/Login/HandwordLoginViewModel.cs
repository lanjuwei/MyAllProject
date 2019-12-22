using BasicFunction.Log;
using GalaSoft.MvvmLight.Command;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Home;

namespace ViewModels.Login
{
    public class HandwordLoginViewModel : LibraryViewModelBase
    {
        private string _readCardId;
        private string _password;
        private bool _isFocusReadCardId;
        private bool _isFocusPassword;
        private bool _isSureButtonEnable;

        /// <summary>
        /// 读者证号码
        /// </summary>
        public string ReadCardId
        {
            get => _readCardId;
            set
            {
                Set(() => ReadCardId, ref _readCardId, value);
                IsCanClick();
            }
        }
        /// <summary>
        /// 聚焦到读者证输入框
        /// </summary>
        public bool IsFocusReadCardId
        {
            get => _isFocusReadCardId; set
            {
                Set(() => IsFocusReadCardId, ref _isFocusReadCardId, value);
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => _password; set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
                IsCanClick();
            }
        }
        /// <summary>
        /// 聚焦到密码控件
        /// </summary>
        public bool IsFocusPassword
        {
            get => _isFocusPassword; set
            {
                Set(() => IsFocusPassword, ref _isFocusPassword, value);
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
            ReadCardId = string.Empty;
            Password = string.Empty;
            IsFocusReadCardId = false;
            IsFocusReadCardId = true;
            base.Load();
        }
        /// <summary>
        /// 登录命令
        /// </summary>
        public RelayCommand SureCommand => new RelayCommand(() =>
        {
            isCanClose = false;
            try
            {
                if (NavigateInterface.Parameter is ButtonType buttonType)
                {
                    switch (buttonType)
                    {
                        case ButtonType.PersonalCenter:
                            NavigateInterface.NavigateTo(BasicServices.Navigation.PageKey.PersonalCenterPage);
                            break;
                        case ButtonType.RenewBook:
                        case ButtonType.BorrowBook:
                        case ButtonType.ReturnBook:
                            NavigateInterface.NavigateTo(BasicServices.Navigation.PageKey.OperateBooksPage);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                isCanClose = true;
            }
        });

        private void IsCanClick()
        {
            IsSureButtonEnable= !string.IsNullOrWhiteSpace(ReadCardId) && !string.IsNullOrWhiteSpace(Password) ? true : false;
        }
    }
}
