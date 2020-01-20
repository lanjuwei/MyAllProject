using BasicServices.SubWindowService.ViewService;
using BasicServices.TipService;
using GalaSoft.MvvmLight.Command;
using Model;
using Model.Book;
using Model.Login;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Home;

namespace ViewModels.Book
{
    public class OperateBooksViewModel : LibraryViewModelBase
    {
        private string _title;
        private ObservableCollection<BookModel> _bookModelList = new ObservableCollection<BookModel>();
        private string _buttonContent;
        private ObservableCollection<StepModel> stepList;
        private BookColumnModel bookColumn;
        private bool isSureButtonEnabled = true;
        private bool isAllSlelected;
        private int selectCount;
        private int successCount;
        private int failCount;
        private bool isFirstStep;


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
        /// 确定按钮是否可以点击
        /// </summary>
        public bool IsSureButtonEnabled
        {
            get => isSureButtonEnabled; set
            {
                Set(() => IsSureButtonEnabled, ref isSureButtonEnabled, value);
            }
        }

        public bool IsAllSlelected
        {
            get => isAllSlelected; set
            {
                Set(() => IsAllSlelected, ref isAllSlelected, value);
            }
        }

        public int SelectCount
        {
            get => selectCount; set
            {
                Set(() => SelectCount, ref selectCount, value);
            }
        }

        public int SuccessCount
        {
            get => successCount; set
            {
                Set(() => SuccessCount, ref successCount, value);
            }
        }

        public int FailCount
        {
            get => failCount; set
            {
                Set(() => FailCount, ref failCount, value);
            }
        }

        public bool IsFirstStep
        {
            get => isFirstStep; set
            {
                Set(() => IsFirstStep, ref isFirstStep, value);
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
            //初始化列表的宽度
            LoadColumnWidth();
            StepList = new ObservableCollection<StepModel>()
            {
                new StepModel(){  StepContent="放置图书",StepIndex=1,IsNextStep=true,IsRectVisible=false },
                new StepModel(){StepContent="处理图书",StepIndex=2,IsRectVisible=true },
                new StepModel(){ StepContent="打印凭条",StepIndex=3,IsRectVisible=true},
            };
        }

        protected override async void Load()
        {
            IsSureButtonEnabled = false;
            IsFirstStep = true;
            if (NavigateInterface.Parameter != null)
            {
                var type = (ButtonType)Enum.Parse(typeof(ButtonType), NavigateInterface.Parameter.ToString());
                switch (type)
                {
                    case ButtonType.BorrowBook:
                        if (User.CanBorrowCount == User.LendCount)
                        {
                            TipService.Instance.ShowTip(TipService.ToolTip, 1000, "借书数量已满");
                            await Task.Delay(1050);
                            CloseCommand.Execute(null);
                            return;
                        }
                        Title = "借书";
                        ButtonContent = "借阅";
                        BookColumn.DescribeColumnWidth = 120;
                        BookColumn.DateColumnWidth = 0;
                        var result1 = await SocektInterface.GetAllBooks();
                        if (result1.IsSuccess)
                        {
                            base.Load();
                            SubWindowsService.Instance.OpenWindow(SubWindowsService.PlaceBookPage);
                            if (result1.Data.Count > 0)
                            {
                                //var list1 = new List<BookModel>();
                                foreach (var item in result1.Data)
                                {
                                    if (item.BookStatus == BookStatus.Returned)
                                    {
                                        item.Describe = "可借出";
                                        BookModelList.Add(item);
                                    }
                                    else
                                    {
                                        item.Describe = "不可借";
                                        //list1.Add(item);
                                    }
                                }
                                //foreach (var item in list1)
                                //{
                                //    BookModelList.Add(item);
                                //}
                            }
                        }
                        else
                        {
                            TipService.Instance.ShowTip(TipService.ToolTip, 1000, result1.Message);
                            await Task.Delay(1050);
                            CloseCommand.Execute(null);//退回到主页
                            return;
                        }
                        break;
                    case ButtonType.ReturnBook:
                        Title = "还书";
                        ButtonContent = "归还";
                        var result = await SocektInterface.GetUserBookListAsync(User.Id);
                        if (result.IsSuccess)
                        {
                            base.Load();
                            SubWindowsService.Instance.OpenWindow(SubWindowsService.PlaceBookPage);
                            if (result.Data.Count > 0)
                            {
                                foreach (var item in result.Data)
                                {
                                    BookModelList.Add(item);
                                }
                            }
                        }
                        else
                        {
                            TipService.Instance.ShowTip(TipService.ToolTip, 1000, result.Message);
                            await Task.Delay(1050);
                            CloseCommand.Execute(null);//退回到主页
                            return;
                        }
                        break;
                    case ButtonType.RenewBook:
                        Title = "续借";
                        ButtonContent = "续借";
                        var result3 = await SocektInterface.GetUserBookListAsync(User.Id);
                        if (result3.IsSuccess)
                        {
                            base.Load();
                            SubWindowsService.Instance.OpenWindow(SubWindowsService.PlaceBookPage);
                            if (result3.Data.Count > 0)
                            {
                                foreach (var item in result3.Data)
                                {
                                    BookModelList.Add(item);
                                }
                            }
                        }
                        else
                        {
                            TipService.Instance.ShowTip(TipService.ToolTip, 1000, result3.Message);
                            await Task.Delay(1050);
                            CloseCommand.Execute(null);//退回到主页
                            return;
                        }
                        break;
                }
            }
        }



        //public ICommand ReCommand => new RelayCommand<BookModel>(t =>
        //{
        //    if (t != null)
        //    {
        //        BookModelList.Remove(t);
        //    }
        //});

        /// <summary>
        /// checkbox 千万不要用checkbox的ischeck来判断触发  用command 因为ischekc是用来触发ui的
        /// </summary>
        public ICommand SelectItemCommand => new RelayCommand<bool>(t =>
        {
            SelectCount = BookModelList.Count(x => x.IsSlelected);
            IsAllSlelected = SelectCount == BookModelList.Count;
            IsSureButtonEnabled = SelectCount != 0;
        });

        public ICommand SelectAllItem => new RelayCommand<bool>(t =>
        {
            BookModelList.Count(x => x.IsSlelected = t);//list.all 这个方法只是用来判断的 不是用来给属性全部赋值是 全选或者全不选
            if (t)
            {
                SelectCount = BookModelList.Count;
                IsSureButtonEnabled = true;
            }
            else
            {
                IsSureButtonEnabled = false;
                SelectCount = 0;
            }

        });

        protected override void UnLoad()
        {
            base.UnLoad();
            LoadColumnWidth();
            BookModelList?.Clear();
            IsFirstStep = false;
            IsAllSlelected = false;
            SuccessCount = 0;
            StepList[1].IsNextStep = false;
            StepList[2].IsNextStep = false;
            SelectCount = 0;
        }

        private void LoadColumnWidth()
        {
            if (BookColumn == null)
            {
                BookColumn = new BookColumnModel()
                {
                    BlankLineWidth = 60,
                    BarcodeColumnWidth = 150,
                    TitleColumnWidth = 650,
                    DescribeColumnWidth = 0,
                    RomoveColumnWidth = double.NaN,//自适应的
                    StatusColumnWidth = 100,
                    DateColumnWidth = 150,
                    ResultColumnWidth = 0,
                    SelectedColumnWidth = 60,
                };
            }
            else
            {
                BookColumn.BlankLineWidth = 60;
                BookColumn.BarcodeColumnWidth = 150;
                BookColumn.TitleColumnWidth = 650;
                BookColumn.DescribeColumnWidth = 0;
                BookColumn.RomoveColumnWidth = double.NaN;//自适应的
                BookColumn.StatusColumnWidth = 100;
                BookColumn.DateColumnWidth = 150;
                BookColumn.ResultColumnWidth = 0;
                BookColumn.SelectedColumnWidth = 60;
            }
        }

        public ICommand OperateCommand => new RelayCommand<string>(async t =>
        {
            
            for (int i = BookModelList.Count - 1; i >= 0; i--)
            {
                BookModel item = BookModelList[i];
                if (item.IsSlelected == false)
                {
                    BookModelList.Remove(item);
                }
            }
            var list1 = BookModelList.Where(x => x.IsSlelected).ToList();//生成新的列表 但里面的元素依旧未改变
            switch (t)
            {
                case "归还":
                    IsFirstStep = false;
                    IsWorkingLock = true;
                    IsSureButtonEnabled = false;
                    StepList[1].IsNextStep = true;
                    BookColumn.SelectedColumnWidth = 0;
                    BookColumn.StatusColumnWidth = 0;
                    BookColumn.DescribeColumnWidth = 350;
                    BookColumn.RomoveColumnWidth = 0;
                    BookColumn.ResultColumnWidth = double.NaN;
                    BookColumn.DateColumnWidth = 0;
                    foreach (var item in list1)
                    {
                        item.Describe = "归还中...";
                        item.ImagePath = @"Images/Gif/waitting.gif";
                    }
                    await Task.Delay(800);
                    var result = await SocektInterface.RetrueBooks(list1);
                    if (result.IsSuccess)
                    {
                        foreach (var item in list1)
                        {
                            item.Describe = "归还成功";
                            item.ImagePath = @"Images/Gif/true.png";
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        foreach (var item in list1)
                        {
                            item.Describe = $"归还失败：{result.Message}";
                            item.ImagePath = @"Images/Gif/false.png";
                            Thread.Sleep(200);
                        }
                    }
                    break;
                case "借阅":
                    //var list = BookModelList.Where(x => x.BookStatus == BookStatus.Returned).ToList();//获取在馆的列表
                    //if (list.Count == 0)
                    //{
                    //    TipService.Instance.ShowTip(TipService.ToolTip, 1000, "无可借图书");
                    //    return;
                    //}
                    if (list1.Count + User.LendCount - User.CanBorrowCount > 0)//超出可借的数量
                    {
                        TipService.Instance.ShowTip(TipService.ToolTip, 1000, $"您可再借{User.CanBorrowCount - User.LendCount}本，请删减");
                        return;
                    }
                    IsFirstStep = false;
                    IsWorkingLock = true;
                    IsSureButtonEnabled = false;
                    BookColumn.SelectedColumnWidth = 0;
                    StepList[1].IsNextStep = true;
                    BookColumn.StatusColumnWidth = 0;
                    BookColumn.DescribeColumnWidth = 350;
                    BookColumn.RomoveColumnWidth = 0;
                    BookColumn.ResultColumnWidth = double.NaN;
                    BookColumn.DateColumnWidth = 150;
                    foreach (var item in list1)
                    {
                        item.Describe = "借书中...";
                        item.ImagePath = @"Images/Gif/waitting.gif";
                    }
                    await Task.Delay(800);
                    var result2 = await SocektInterface.BorrowBooks(list1.ToList());
                    if (result2.IsSuccess)
                    {
                        foreach (var item in list1)
                        {
                            item.Describe = "借书成功";
                            item.ImagePath = @"Images/Gif/true.png";
                            item.ReturnDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        foreach (var item in list1)
                        {
                            item.Describe = $"借书失败：{result2.Message}";
                            item.ImagePath = @"Images/Gif/false.png";
                            Thread.Sleep(200);
                        }
                    }
                    break;
                case "续借":
                    IsFirstStep = false;
                    IsWorkingLock = true;
                    IsSureButtonEnabled = false;
                    StepList[1].IsNextStep = true;
                    BookColumn.SelectedColumnWidth = 0;
                    BookColumn.StatusColumnWidth = 0;
                    BookColumn.DescribeColumnWidth = 350;
                    BookColumn.RomoveColumnWidth = 0;
                    BookColumn.ResultColumnWidth = double.NaN;
                    foreach (var item in list1)
                    {
                        item.Describe = "续借中...";
                        item.ImagePath = @"Images/Gif/waitting.gif";
                    }
                    await Task.Delay(800);
                    var result3 = await SocektInterface.RenewBooks(list1);
                    if (result3.IsSuccess)
                    {
                        foreach (var item in list1)
                        {
                            item.Describe = "续借成功";
                            item.ImagePath = @"Images/Gif/true.png";
                            if (item.ReturnDate is string returnData)
                            {
                                DateTime t1 = DateTime.Parse(returnData);
                                item.ReturnDate = t1.AddMonths(1).ToString("yyyy-MM-dd");
                            }
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        foreach (var item in list1)
                        {
                            item.Describe = $"续借失败：{result3.Message}";
                            item.ImagePath = @"Images/Gif/false.png";
                            Thread.Sleep(200);
                        }
                    }
                    break;
                case "打印":
                    SubWindowsService.Instance.OpenWindow(SubWindowsService.PrintPage);
                    //CloseCommand.Execute(null);//退回到主页
                    break;
                default:
                    break;
            }
            StepList[2].IsNextStep = true;
            ButtonContent = "打印";
            IsSureButtonEnabled = true;
            IsWorkingLock = false;
            SuccessCount = list1.Count(x=>x.Describe.Contains("成功"));
            FailCount = list1.Count - SuccessCount;
            Time = 10;//重置10秒给与打印时间
        });
    }


}
