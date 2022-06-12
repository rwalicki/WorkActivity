using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using Work.Core.Models;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class SprintListViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly ISprintRepository _sprintRepository;
        private readonly TaskStore _taskStore;
        private readonly NavigationService<AddSprintViewModel> _addSprintNavigationService;

        private readonly ObservableCollection<Sprint> _sprints;
        public IEnumerable<Sprint> Sprints => _sprints;

        public ICommand OnLoadCommand { get; set; }
        public ICommand AddSprintCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public SprintListViewModel(ISnackbarService snackbarService,
            ISprintRepository sprintRepository,
            TaskStore taskStore,
            NavigationService<AddSprintViewModel> addSprintNavigationService)
        {
            _sprints = new ObservableCollection<Sprint>();

            _snackbarService = snackbarService;
            _sprintRepository = sprintRepository;
            _taskStore = taskStore;
            _addSprintNavigationService = addSprintNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            AddSprintCommand = new RelayCommand(AddSprintNavigate);
            DeleteCommand = new RelayCommand(Delete);
        }

        private async void Load(object sender)
        {
            _sprints.Clear();

            var result = await _sprintRepository.GetAll();
            if (result.Success)
            {
                var data = result.Data.ToList().OrderByDescending(x => x.StartDate);
                foreach (var sprint in data)
                {
                    _sprints.Add(sprint);
                }
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
                await _taskStore.Load();
                if (_taskStore.Tasks.Any(x => x.Sprints.Exists(x => x?.Id.Equals(sprint.Id) ?? false)))
                {
                    _snackbarService.ShowMessage($"Cannot remove {sprint.Name}. It has tasks attached.");
                    return;
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