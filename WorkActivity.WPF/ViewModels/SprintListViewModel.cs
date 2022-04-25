using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Work.Core.Interfaces;
using Work.Core.Models;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class SprintListViewModel : ViewModelBase
    {
        private ISprintRepository _sprintService;
        private MainWindowViewModel _mainWindowViewModel;

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
        public ICommand OnDeleteCommand { get; set; }

        public SprintListViewModel(ISprintRepository sprintService, MainWindowViewModel mainWindowViewModel)
        {
            _sprintService = sprintService;
            _mainWindowViewModel = mainWindowViewModel;

            OnLoadCommand = new RelayCommand(async (obj) =>
            {
                var result = await _sprintService.GetAll();
                if (result.Success)
                {
                    Sprints = new ObservableCollection<Sprint>(result.Data);
                }
            });

            AddSprintCommand = new RelayCommand((obj) =>
            {
                _mainWindowViewModel.CurrentViewModel = new AddSprintViewModel(_sprintService, _mainWindowViewModel);
            });

            OnDeleteCommand = new RelayCommand(async (obj) =>
            {
                var sprint = obj as Work.Core.Models.Sprint;
                if (sprint != null)
                {
                    var result = await _sprintService.Delete(sprint.Id);
                    if (result.Success)
                    {
                        _mainWindowViewModel.SnakbarMessageQueue.Enqueue($"Sprint id {result.Data.Id} removed.");
                        OnLoadCommand.Execute(null);
                    }
                }
            });
        }
    }
}