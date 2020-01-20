using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction.Helper
{
    public  class ExcelHelper
    {
        private static ExcelHelper _socketHelper;
        public static ExcelHelper Instance => _socketHelper ?? (_socketHelper = new ExcelHelper());

        /// <summary>
        /// 导出到excel表
        /// </summary>
        public void ExportToExcel<T>()
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("办证金额统计"); //创建一个Sheet 
                ws.Cells[1, 1].Value = 1;//从第1行 第一列开始
            }
        }
    }
}
