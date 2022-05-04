using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using Work.Core.Models;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class SprintListViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly ISprintRepository _sprintRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly NavigationService<AddSprintViewModel> _addSprintNavigationService;

        private ObservableCollection<Sprint> _sprints;
        public ObservableCollection<Sprint> Sprints
        {
            get { return _sprints; }
            set
            {
                _sprints = value;
                OnPropertyChanged(nameof(Sprints));
            }
        }

        public ICommand OnLoadCommand { get; set; }
        public ICommand AddSprintCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public SprintListViewModel(ISnackbarService snackbarService,
            ISprintRepository sprintRepository,
            ITaskRepository taskRepository,
            NavigationService<AddSprintViewModel> addSprintNavigationService)
        {
            _snackbarService = snackbarService;
            _sprintRepository = sprintRepository;
            _taskRepository = taskRepository;
            _addSprintNavigationService = addSprintNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            AddSprintCommand = new RelayCommand(AddSprintNavigate);
            DeleteCommand = new RelayCommand(Delete);
        }

        private async void Load(object sender)
        {
            var result = await _sprintRepository.GetAll();
            if (result.Success)
            {
                Sprints = new ObservableCollection<Sprint>(result.Data);
            }
        }

        private void AddSprintNavigate(object sender)
        {
            _addSprintNavigationService.Navigate();
        }

        private async void Delete(object sender)
        {
            var sprint = sender as Work.Core.Models.Sprint;
            if (sprint != null)
            {
                var taskResult = await _taskRepository.GetAll();
                if (taskResult.Success)
                {
                    if (taskResult.Data.Any(x => x.Sprints.Exists(x => x?.Id.Equals(sprint.Id) ?? false)))
                    {
                        _snackbarService.ShowMessage($"Cannot remove {sprint.Name}. It has tasks attached.");
                        return;
                    }
                }

                var result = await _sprintRepository.Delete(sprint.Id);
                if (result.Success)
                {
                    _snackbarService.ShowMessage($"Sprint id {result.Data.Id} removed.");
                    OnLoadCommand.Execute(null);
                }
            }
        }
    }
}