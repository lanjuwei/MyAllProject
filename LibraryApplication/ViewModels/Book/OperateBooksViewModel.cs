using BasicServices.SubWindowService.ViewService;
using GalaSoft.MvvmLight.Command;
using Model.Book;
using Model.Login;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Home;

namespace ViewModels.Book
{
    public class OperateBooksViewModel : LibraryViewModelBase
    {
        private string _title;
        private ObservableCollection<BookModel> _bookModelList;
        private string _buttonContent;
        private ObservableCollection<StepModel> stepList;
        private BookColumnModel bookColumn;

        /// <summary>
        /// 界面标题
        /// </summary>
        public string Title
        {
            get => _title; set
            {
                Set(() => Title, ref _title, value);
            }
        }
        /// <summary>
        /// 图书集合
        /// </summary>
        public ObservableCollection<BookModel> BookModelList
        {
            get => _bookModelList; set
            {
                Set(() => BookModelList, ref _bookModelList, value);
            }
        }
        /// <summary>
        /// 界面底层按钮显示内容
        /// </summary>
        public string ButtonContent
        {
            get => _buttonContent; set
            {
                Set(() => ButtonContent, ref _buttonContent, value);
            }
        }
        /// <summary>
        /// 步骤显示的内容和颜色
        /// </summary>
        public ObservableCollection<StepModel> StepList
        {
            get => stepList; set
            {
                Set(() => StepList, ref stepList, value);
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

        public OperateBooksViewModel()
        {
            Title = "还书";
            ButtonContent = "归还";
            //初始化列表的宽度
            BookColumn = new BookColumnModel() 
            { 
                BlankLineWidth = 60, 
                BarcodeColumnWidth = 150,
                NumberColumnWidth = 120,
                TitleColumnWidth = 650, 
                RomoveColumnWidth = double.NaN,
                StatusColumnWidth = 150
            };
            StepList = new ObservableCollection<StepModel>()
            {
                new StepModel(){  StepContent="放置图书",StepIndex=1,IsNextStep=true,IsRectVisible=false },
                new StepModel(){StepContent="还书处理",StepIndex=2,IsRectVisible=true },
                new StepModel(){ StepContent="打印凭条",StepIndex=3,IsRectVisible=true},
            };
            BookModelList = new ObservableCollection<BookModel>()
            {
                new BookModel() { BarCode="125001",Title="平凡的世界",BookStatus= Model.BookStatus.Lended},
                new BookModel() { BarCode="125008",Title="善的脆弱性",BookStatus= Model.BookStatus.Returned},
                new BookModel() { BarCode="125009",Title="元尊",BookStatus= Model.BookStatus.Returned},
                new BookModel() { BarCode="125010",Title="简爱",BookStatus= Model.BookStatus.Returned},
                new BookModel() { BarCode="125007",Title="关于物理学家彼得罗夫在圣彼得堡外科医学院借助有时由4200个铜环与锌环构成的巨大电池组所作的伽伐尼——伏打实验的消息",BookStatus= Model.BookStatus.Lended},
                new BookModel() { BarCode="125006",Title="贫穷的本质",BookStatus= Model.BookStatus.Lended},
                new BookModel() { BarCode="125003",Title="穿过生命中的泥泞时刻",BookStatus= Model.BookStatus.Lended},
                new BookModel() { BarCode="125002",Title="不可思议的图书馆",BookStatus= Model.BookStatus.Reserved},
                new BookModel() { BarCode="125011",Title="雪落香杉树",BookStatus= Model.BookStatus.Reserved},
                new BookModel() { BarCode="125004",Title="大地蓝的像一只橙子",BookStatus= Model.BookStatus.Returned},
                new BookModel() { BarCode="125005",Title="斗罗大陆之蓝银寒魂",BookStatus= Model.BookStatus.None},
            };
        }

        protected override void Load()
        {
            //SubWindowsService.Instance.OpenWindow(SubWindowsService.PlaceBookPage);
            base.Load();
        }


        public ICommand ReCommand => new RelayCommand<BookModel>(t =>
        {
            if (t!=null)
            {
                BookModelList.Remove(t);
            }
        });
        public ICommand OperateCommand => new RelayCommand<string>(t =>
        {
            switch (t)
            {
                case "归还":
                    break;
                case "借阅":
                    break;
                case "续借":
                    break;
                case "完成":
                    break;
                default:
                    break;
            }
        });
    }

    
}
