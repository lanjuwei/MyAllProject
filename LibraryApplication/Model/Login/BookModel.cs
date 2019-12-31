using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Model.Login
{
    public class BookModel : ViewModelBase
    {
        private BookStatus bookStatus;
        private string status;
        private SolidColorBrush forgroundColor = new SolidColorBrush(Colors.Black);
        private string imagePath;
        private int index;

        public int Index
        {
            get => index; set
            {
                Set(() => Index, ref index, value);
            }
        }
        public string BarCode { get; set; }
        public string Title { get; set; }



        public string Status
        {
            get => status; set
            {
                status = value;
                Set(() => Status, ref status, value);
            }
        }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string ImagePath
        {
            get => imagePath; set
            {
                Set(() => ImagePath, ref imagePath, value);
            }
        }

        public SolidColorBrush ForgroundColor
        {
            get => forgroundColor; set
            {
                forgroundColor = value;
                Set(() => ForgroundColor, ref forgroundColor, value);
            }
        }

        public BookStatus BookStatus
        {
            get => bookStatus; set
            {
                bookStatus = value;
                switch (value)
                {
                    case BookStatus.None:
                        Status = "未知";
                        ForgroundColor.Color = Colors.Red;
                        break;
                    case BookStatus.Lended:
                        Status = "已借出";
                        ForgroundColor.Color = (Color)ColorConverter.ConvertFromString("#ff7256");
                        break;
                    case BookStatus.Returned:
                        Status = "在馆";
                        ForgroundColor.Color = (Color)ColorConverter.ConvertFromString("#ffaa56");
                        break;
                    case BookStatus.Reserved:
                        Status = "已预借";
                        ForgroundColor.Color = (Color)ColorConverter.ConvertFromString("#2db36c");
                        break;
                    default:
                        Status = "未知";
                        ForgroundColor.Color = Colors.Red;
                        break;
                }
                Set(() => BookStatus, ref bookStatus, value);
            }
        }


    }
}
