using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Book
{
    public class StepModel : ViewModelBase
    {
        private bool isNextStep;
        private bool isRectVisible = true;

        public bool IsNextStep
        {
            get => isNextStep; set
            {
                Set(() => IsNextStep, ref isNextStep, value);
            }
        }

        public string StepContent { get; set; }

        public bool IsRectVisible
        {
            get => isRectVisible; set
            {
                Set(() => IsRectVisible, ref isRectVisible, value);
            }
        }
        public int StepIndex { get; set; }

    }
}
