using Work.Core.Models;

namespace WorkActivity.WPF.ViewModels
{
    public class SprintViewModel : ViewModelBase
    {
        private Sprint _sprint;
        public Sprint Sprint
        {
            get { return _sprint; }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public string Name
        {
            get
            {
                return _sprint.Name;
            }
            set
            {
                _sprint.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Date
        {
            get
            {
                return $"{_sprint.StartDate.ToString("dd.MM.yy")} - {_sprint.EndDate.ToString("dd.MM.yy")}";
            }
        }

        public SprintViewModel(Sprint sprint)
        {
            _sprint = sprint;
        }
    }
}