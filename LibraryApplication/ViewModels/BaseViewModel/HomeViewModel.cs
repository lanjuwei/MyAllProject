using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BaseSetting.Needs;
using BasicServices.Navigation;
using BasicServices.SubWindowService.ViewService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;

namespace ViewModels.Home
{
    public class HomeViewModel : LibraryViewModelBase
    {

        public HomeViewModel()
        {
            BtnContentList = new ObservableCollection<Data>()
            {
                new Data() {BtnName= IndividualNeeds.Instance.LanguageVariables.BorrowBook,BtnNameEn = "Borrow Book",ImagePath = "/Views;component/Images/icon1.png",Tag= ButtonType.BorrowBook,BackImagePath="/Views;component/Images/card1.png" },
                new Data() {BtnName= IndividualNeeds.Instance.LanguageVariables.RenewBook,BtnNameEn = "Return Book",ImagePath = "/Views;component/Images/icon2.png",Tag= ButtonType.ReturnBook,BackImagePath="/Views;component/Images/card2.png" },
                new Data(){ BtnName=IndividualNeeds.Instance.LanguageVariables.RenewBook,BtnNameEn="Renew Book",ImagePath = "/Views;component/Images/icon3.png",Tag= ButtonType.RenewBook,BackImagePath="/Views;component/Images/card3.png"},
                new Data() {BtnName= IndividualNeeds.Instance.LanguageVariables.PersonalCenter,BtnNameEn= "Personal Center",ImagePath = "/Views;component/Images/icon4.png",Tag= ButtonType.PersonalCenter ,BackImagePath="/Views;component/Images/card4.png"},
            };
        }



        private ObservableCollection<Data> _btnContentList;
        private ObservableCollection<LangKey> languageList=new ObservableCollection<LangKey>() 
        { 
            new LangKey() { Name="中文",LangType= LanguageType.Cn} ,
            new LangKey(){Name="English", LangType= LanguageType.En}
        };

        public ObservableCollection<Data> BtnContentList
        {
            get => _btnContentList; set
            {
                Set(() => BtnContentList, ref _btnContentList, value);
            }
        }

        public ObservableCollection<LangKey> LanguageList
        {
            get => languageList; set
            {
                Set(() => LanguageList, ref languageList, value);
            }
        }

        protected override void Load()
        {

        }

        public ICommand SelectCommand => new RelayCommand<Data>(t =>
        {
            NaviService.Instance.NavigateTo(PageKey.LoginPage, t.Tag);
        });



        public class Data
        {
            public string BtnName { get; set; }
            public string BtnNameEn { get; set; }
            public string ImagePath { get; set; }
            public ButtonType Tag { get; set; }
            public string BackImagePath { get; set; }
        }

        public class LangKey 
        {
            public string Name { get; set; }
            public LanguageType LangType { get; set; }
        }
    }

}
