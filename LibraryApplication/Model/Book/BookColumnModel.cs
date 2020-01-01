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

        public double BarcodeColumnWidth
        {
            get => barcodeColumnWidth; set
            {
                Set(() => BarcodeColumnWidth, ref barcodeColumnWidth, value);
            }
        }

        public double TitleColumnWidth
        {
            get => titleColumnWidth; set
            {
                Set(() => TitleColumnWidth, ref titleColumnWidth, value);
            }
        }

        public double StatusColumnWidth
        {
            get => statusColumnWidth; set
            {
                Set(() => StatusColumnWidth, ref statusColumnWidth, value);
            }
        }

        public double RomoveColumnWidth
        {
            get => romoveColumnWidth; set
            {
                Set(() => RomoveColumnWidth, ref romoveColumnWidth, value);
            }
        }

        public double ReturnDateColumnWidth
        {
            get => _returnDateColumnWidth; set
            {
                Set(() => ReturnDateColumnWidth, ref _returnDateColumnWidth, value);
            }
        }


    }
}
