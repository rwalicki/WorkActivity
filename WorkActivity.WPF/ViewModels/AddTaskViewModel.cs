using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        private ITaskRepository _taskService;
        private IWorkRepository _workService;
        private MainWindowViewModel _mainWindowViewModel;

        public string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public ICommand AddTaskCommand { get; set; }


        public AddTaskViewModel(ITaskRepository taskService, IWorkRepository workService, MainWindowViewModel mainWindowViewModel)
        {
            _taskService = taskService;
            _workService = workService;
            _mainWindowViewModel = mainWindowViewModel;
            AddTaskCommand = new RelayCommand(async(obj) =>
            {
                var result = await _taskService.Create(new Work.Core.Models.Task() { Number = int.Parse(Number), Title = Title, Date = System.DateTime.Now });
                if (result.Success)
                {
                    _mainWindowViewModel.CurrentViewModel = new TaskListViewModel(_taskService, _workService, _mainWindowViewModel);
                }
                else
                {
                    _mainWindowViewModel.SnakbarMessageQueue.Enqueue(result.Message);
                }
            });
        }
    }
}