using BasicFunction.Helper;
using BasicServices.Navigation;
using GalaSoft.MvvmLight.Command;
using Model;
using Model.Book;
using Model.Login;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ViewModels.Home;

namespace ViewModels.Login
{
    public class PersonalCenterViewModel : LibraryViewModelBase
    {
        private ObservableCollection<BookModel> _bookList;
        private UserModel _currentUser;
        private BookColumnModel bookColumn;
        private ImageSource _faceImage;

        public ObservableCollection<BookModel> BookList
        {
            get => _bookList; set
            {
                Set(() => BookList, ref _bookList, value);
            }
        }

        public UserModel CurrentUser
        {
            get => _currentUser; set
            {
                Set(() => CurrentUser, ref _currentUser, value);
            }
        }
        /// <summary>
        /// 图书列表的宽度
        /// </summary>
        public BookColumnModel BookColumn
        {
            get => bookColumn; set
            {
                Set(() => BookColumn, ref bookColumn, value);
            }
        }

        public ImageSource FaceImage
        {
            get => _faceImage; set
            {
                Set(() => FaceImage, ref _faceImage, value);
            }
        }

        public PersonalCenterViewModel()
        {
            //初始化列表的宽度
            BookColumn = new BookColumnModel()
            {
                BlankLineWidth = 60,
                BarcodeColumnWidth = 150,
                TitleColumnWidth = 950,
                ReturnDateColumnWidth = 150
            };
        }

        public void UiLoad()
        {
            Time = 60;
            StartTimer();
            LoadUserDataAsync();
        }

        protected override void Load()
        {

        }


        public ICommand OperateBookCommand => new RelayCommand<string>(t =>
        {
            var type = (ButtonType)Enum.Parse(typeof(ButtonType), t);
            NavigateInterface.NavigateTo(PageKey.OperateBooksPage, type);
        });

        public ICommand PersonCommand => new RelayCommand<string>(t =>
        {
            switch (t)
            {
                case "ChangePassword":
                    NavigateInterface.NavigateTo(PageKey.ChangePasswordPage);
                    break;
                case "UploadFace":
                    NavigateInterface.NavigateTo(PageKey.RegistrateFacePage);
                    break;
                case "DeleteFace":
                    DeleteFaceImage();
                    break;
            }
        });

        private void DeleteFaceImage()
        {

        }

        private async void LoadUserDataAsync()
        {
            if (User?.FaceImage != null)
            {
                var data = Convert.FromBase64String(User.FaceImage);
                var bitmap = ImageHelper.Instance.ToImage(data);
                bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
                FaceImage=bitmap;
            }
            else
            {
                var bitmap = new System.Windows.Media.Imaging.BitmapImage(User?.Sex == 0 ? new Uri("/Views;component/Images/Penson/man.png") : new Uri("/Views;component/Images/Penson/woman.png"));
                bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;             
                FaceImage = bitmap;
            }
            CurrentUser = User;
            if (User != null)
            {
                var result = await SocektInterface.GetUserBookListAsync(User.Id);
                if (result.IsSuccess && result.Data.Count > 0)
                {
                    BookList = new ObservableCollection<BookModel>(result.Data);
                }
            }
        }
    }
}
