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
        private bool isTakePhotosVisible = true;
        private bool isTakePhotosAgainVisible;
        private bool isShotsFace;
        private CameraStatus status= CameraStatus.Palying;

        /// <summary>
        /// 步骤显示的内容和颜色
        /// </summary>
        public ObservableCollection<NormalModel> ErrorImageList => new ObservableCollection<NormalModel>()
        {
            new NormalModel(){ ImagePath="/Views;component/Images/vagueFace.png",Name="模糊"},
            new NormalModel(){ ImagePath="/Views;component/Images/sideFace.png",Name="侧脸"},
            new NormalModel(){ ImagePath="/Views;component/Images/incompleteFace.png",Name="残缺"},
        };

        public bool IsTakePhotosVisible
        {
            get => isTakePhotosVisible; set
            {
                Set(() => IsTakePhotosVisible, ref isTakePhotosVisible, value);
            }
        }
        public bool IsTakePhotosAgainVisible
        {
            get => isTakePhotosAgainVisible; set
            {
                Set(() => IsTakePhotosAgainVisible, ref isTakePhotosAgainVisible, value);
            }
        }

        public bool IsShotsFace
        {
            get => isShotsFace; set
            {
                Set(() => IsShotsFace, ref isShotsFace, value);
            }
        }

        public CameraStatus Status
        {
            get => status; set
            {
                Set(() => Status, ref status, value);
            }
        }

        protected override void Load()
        {
            IsTakePhotosVisible = true;
            IsTakePhotosAgainVisible = false;
            base.Load();
        }

        protected override void MoveToNextPage(object parameter = null)
        {
            NavigateInterface.NavigateTo(PageKey.PersonalCenterPage);
        }

        public ICommand TakePhotosCommand => new RelayCommand(() =>
        {
            Status = CameraStatus.Suspend;
            IsTakePhotosVisible = false;
            IsTakePhotosAgainVisible = true;
        });

        public ICommand TakePhotosAgainCommand => new RelayCommand(() =>
        {
            IsTakePhotosVisible = true;
            IsTakePhotosAgainVisible = false;
            Status = CameraStatus.Palying;
        });

        public ICommand ConfirmUploadCommand => new RelayCommand(() =>
        {

        });

    }
}
