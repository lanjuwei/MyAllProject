﻿using GalaSoft.MvvmLight;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Model.Login
{
    public class BookModel : ViewModelBase
    {
        private BookStatus bookStatus;
        private string status;
        private string describe;
        private string imagePath;
        private string currentData;
        private string returnDate;
        private bool isSlelected;

        public bool IsSlelected
        {
            get => isSlelected; set
            {
                Set(() => IsSlelected, ref isSlelected, value);
            }
        }

        /// <summary>
        /// 图书的唯一id 
        /// </summary>
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
        /// 归还日期
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ReturnDate
        {
            get => returnDate; set
            {
                returnDate = value;
                if (value is string str)
                {
                    DateTime t;
                    if (DateTime.TryParse(value.ToString(), out t))
                    {
                        var d = (t - DateTime.Now).TotalSeconds;
                        if (d < 0)
                        {
                            CurrentData = $"{str}[超期]";
                        }
                        else
                        {
                            CurrentData = value;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 借书日期
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string BorrowDate { get; set; }
        /// <summary>
        /// 图书描述
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Describe
        {
            get => describe; set
            {
                Set(() => Describe, ref describe, value);
            }
        }
        /// <summary>
        /// 结果图片地址
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ImagePath
        {
            get => imagePath; set
            {
                Set(() => ImagePath, ref imagePath, value);
            }
        }
        /// <summary>
        /// 书的日期加上特殊格式 例如已过期的要表示已超期
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CurrentData
        {
            get => currentData; set
            {
                Set(() => CurrentData, ref currentData, value);
            }
        }

        //千万不要将ui上的东西 用set通知界面啊 因为ui上的东西是主线程的 set是另一条异步线程啊 妈的 
        //就算没有set 也不能再不同set线程里面乱搞啊 吗的 必须用转换器

        //private SolidColorBrush forgroundColor = new SolidColorBrush(Colors.Black);
        //public SolidColorBrush ForgroundColor
        //{
        //    get => forgroundColor; set
        //    {
        //        forgroundColor = value;
        //        Set(() => ForgroundColor, ref forgroundColor, value);
        //    }
        //}
        ////public SolidColorBrush ForgroundColor { get; set; } = new SolidColorBrush(Colors.Red);
        public BookStatus BookStatus
        {
            get => bookStatus; set
            {
                bookStatus = value;
                switch (value)
                {
                    case BookStatus.None:
                        Status = "未知";
                        break;
                    case BookStatus.Lended:
                        Status = "已借出";
                        break;
                    case BookStatus.Returned:
                        Status = "在馆";
                        break;
                    case BookStatus.Reserved:
                        Status = "已预借";
                        break;
                    default:
                        Status = "未知";
                        break;
                }
                Set(() => BookStatus, ref bookStatus, value);
            }
        }


    }
}
