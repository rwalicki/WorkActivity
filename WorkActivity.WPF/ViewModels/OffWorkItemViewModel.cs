using System;
using Work.Core.Models;

namespace WorkActivity.WPF.ViewModels
{
    public class OffWorkItemViewModel : ViewModelBase
    {
        private OffWork _offWork;
        public OffWork OffWork => _offWork;

        public string StartDate => _offWork.StartDate.ToString("dd.MM.yyyy");

        public string EndDate => _offWork.EndDate.ToString("dd.MM.yyyy");

        public int DaysOff
        {
            get
            {
                return (_offWork.EndDate.Date - _offWork.StartDate.Date).Days + 1;
            }
        }

        public OffWorkItemViewModel(OffWork offWork)
        {
            _offWork = offWork;
        }
    }
}