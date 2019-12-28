using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PageVariables : ViewModelBase
    {
        private double rootGridWidth=1920;
        private double rootGridHeight=1080;

        public double RootGridWidth
        {
            get => rootGridWidth; set
            {
                Set(()=> RootGridWidth,ref rootGridWidth,value);
            }
        }
        public double RootGridHeight
        {
            get => rootGridHeight; set
            {
                Set(() => RootGridHeight, ref rootGridHeight, value);
            }
        }
    }
}
