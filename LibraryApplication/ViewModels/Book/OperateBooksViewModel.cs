using Model.Login;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Home;

namespace ViewModels.Book
{
    public class OperateBooksViewModel : LibraryViewModelBase
    {
        private string _title;
        private ObservableCollection<BookModel> _bookModelList;
        private string _buttonContent;

        /// <summary>
        /// 标题
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

        public string ButtonContent
        {
            get => _buttonContent; set
            {
                Set(() => ButtonContent, ref _buttonContent, value);
            }
        }
    }
}
