using BaseSetting.Needs;
using BasicServices.Navigation;
using GalaSoft.MvvmLight.Command;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Home;

namespace ViewModels.Login
{
    public class RegistrateFaceViewModel : LibraryViewModelBase
    {
        private bool isShotsFace;

        /// <summary>
        /// 步骤显示的内容和颜色
        /// </summary>
        public ObservableCollection<NormalModel> ErrorImageList => new ObservableCollection<NormalModel>()
        {
            new NormalModel(){ ImagePath="/Views;component/Images/vagueFace.png",Name="模糊"},
            new NormalModel(){ ImagePath="/Views;component/Images/sideFace.png",Name="侧脸"},
            new NormalModel(){ ImagePath="/Views;component/Images/incompleteFace.png",Name="残缺"},
        };

        public bool IsShotsFace
        {
            get => isShotsFace; set
            {
                Set(() => IsShotsFace, ref isShotsFace, value);
            }
        }

        public RegistrateFaceViewModel()
        {
            IndividualNeeds.Instance.CommonVariables.UploadImageAction = UploadImage;
        }

        protected override void Load()
        {
            base.Load();
        }

        public ICommand ConfirmUploadCommand => new RelayCommand(() =>
        {
            IsWorkingLock = true;
            IsShotsFace = true;//拍照
        });

        public async Task<bool> UploadImage(byte[] imageData)
        {           
            try
            {
                var result = await SocektInterface.UploadImage(imageData);
                if (result.IsSuccess)
                {
                    if (IndividualNeeds.Instance.CommonVariables.User!=null)
                    {
                        IndividualNeeds.Instance.CommonVariables.User.FaceImage = Convert.ToBase64String(imageData);
                    }
                    NavigateInterface.GoBack();
                }
                return result.IsSuccess;
            }
            finally
            {
                IsWorkingLock = false;
            }
            
        }
    }
}
