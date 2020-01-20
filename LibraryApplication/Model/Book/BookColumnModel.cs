using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Book
{
    public class BookColumnModel : ViewModelBase
    {
        private double blankLineWidth;
        private double numberColumnWidth;
        private double barcodeColumnWidth;
        private double titleColumnWidth;
        private double statusColumnWidth;
        private double romoveColumnWidth;
        private double _returnDateColumnWidth;
        private double describeColumnWidth;
        private double resultColumnWidth;
        private string returnDataColumnTitle;
        private double selectedColumnWidth;

        /// <summary>
        /// 空白列
        /// </summary>
        public double BlankLineWidth
        {
            get => blankLineWidth; set
            {
                Set(() => BlankLineWidth, ref blankLineWidth, value);
            }
        }

        public double SelectedColumnWidth
        {
            get => selectedColumnWidth; set
            {
                Set(() => SelectedColumnWidth, ref selectedColumnWidth, value);
            }
        }

        /// <summary>
        /// 编号
        /// </summary>
        public double NumberColumnWidth
        {
            get => numberColumnWidth; set
            {
                numberColumnWidth = value;
                Set(() => NumberColumnWidth, ref numberColumnWidth, value);
            }
        }
        /// <summary>
        /// 条码号
        /// </summary>
        public double BarcodeColumnWidth
        {
            get => barcodeColumnWidth; set
            {
                Set(() => BarcodeColumnWidth, ref barcodeColumnWidth, value);
            }
        }
        /// <summary>
        /// 书名
        /// </summary>
        public double TitleColumnWidth
        {
            get => titleColumnWidth; set
            {
                Set(() => TitleColumnWidth, ref titleColumnWidth, value);
            }
        }
        public double DescribeColumnWidth
        {
            get => describeColumnWidth; set
            {
                Set(() => DescribeColumnWidth, ref describeColumnWidth, value);
            }
        }
        public double ResultColumnWidth
        {
            get => resultColumnWidth; set
            {
                Set(() => ResultColumnWidth, ref resultColumnWidth, value);
            }
        }
        /// <summary>
        /// 图书状态
        /// </summary>
        public double StatusColumnWidth
        {
            get => statusColumnWidth; set
            {
                Set(() => StatusColumnWidth, ref statusColumnWidth, value);
            }
        }
        /// <summary>
        /// Delete按钮的所在列宽
        /// </summary>
        public double RomoveColumnWidth
        {
            get => romoveColumnWidth; set
            {
                Set(() => RomoveColumnWidth, ref romoveColumnWidth, value);
            }
        }

        public string ReturnDataColumnTitle
        {
            get => returnDataColumnTitle; set
            {
                Set(() => ReturnDataColumnTitle, ref returnDataColumnTitle, value);
            }
        }
        /// <summary>
        /// 日期宽度
        /// </summary>
        public double DateColumnWidth
        {
            get => _returnDateColumnWidth; set
            {
                Set(() => DateColumnWidth, ref _returnDateColumnWidth, value);
            }
        }


    }
}
