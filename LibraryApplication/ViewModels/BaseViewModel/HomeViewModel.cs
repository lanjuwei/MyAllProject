using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BasicServices.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;

namespace ViewModels.Home
{
    public class HomeViewModel :ViewModelBase
    {

        public HomeViewModel()
        {
            BtnContentList = new ObservableCollection<Data>()
            {
                new Data() {BtnName= "借书",BtnNameEn = "Borrow Book",ImagePath = "/Views;component/Images/icon1.png",Tag= ButtonType.BorrowBook },
                new Data() {BtnName= "还书",BtnNameEn = "Return Book",ImagePath = "/Views;component/Images/icon2.png",Tag= ButtonType.ReturnBook },
                new Data(){ BtnName="续借",BtnNameEn="Renew Book",ImagePath = "/Views;component/Images/icon3.png",Tag= ButtonType.RenewBook},
                new Data() {BtnName= "个人中心",BtnNameEn= "Personal Center",ImagePath = "/Views;component/Images/icon4.png",Tag= ButtonType.PersonalCenter },
            };
        }

        public ICommand LoadCommand => new RelayCommand(() =>
        {
            
        });

        private ObservableCollection<Data> _btnContentList;
        public ObservableCollection<Data> BtnContentList
        {
            get => _btnContentList; set
            {
                Set(() => BtnContentList, ref _btnContentList, value);
            }
        }

        public ICommand SelectCommand => new RelayCommand<Data>(t =>
        {
            NaviService.Instance.NavigateTo(PageKey.LoginPage, t.Tag);
        });

      

        public  class Data
        {
            public string BtnName { get; set; }
            public string BtnNameEn { get; set; }
            public string ImagePath { get; set; }
            public ButtonType Tag { get; set; }
        }
    }

}
