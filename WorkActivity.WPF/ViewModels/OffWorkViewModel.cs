using System.Collections.ObjectModel;

namespace WorkActivity.WPF.ViewModels
{
    public class OffWorkViewModel : ViewModelBase
    {
        public ObservableCollection<OffWorkItemViewModel> OffWorkList { get; set; }

        public OffWorkViewModel()
        {
            OffWorkList = new ObservableCollection<OffWorkItemViewModel>();
            OffWorkList.Add(new OffWorkItemViewModel(new Work.Core.Models.OffWork() { Id= 0, StartDate = new System.DateTime(2022,05,10), EndDate = new System.DateTime(2022,05,13)}));
            OffWorkList.Add(new OffWorkItemViewModel(new Work.Core.Models.OffWork() { Id= 1, StartDate = new System.DateTime(2022,06,10), EndDate = new System.DateTime(2022,06,11)}));
            OffWorkList.Add(new OffWorkItemViewModel(new Work.Core.Models.OffWork() { Id= 2, StartDate = new System.DateTime(2022,07,10), EndDate = new System.DateTime(2022,07,10)}));
            OffWorkList.Add(new OffWorkItemViewModel(new Work.Core.Models.OffWork() { Id= 3, StartDate = new System.DateTime(2022,08,12), EndDate = new System.DateTime(2022,08,15)}));
        }
    }
}