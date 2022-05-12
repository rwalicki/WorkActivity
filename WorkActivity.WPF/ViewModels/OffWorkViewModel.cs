﻿using Shared.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using System.Threading.Tasks;

namespace WorkActivity.WPF.ViewModels
{
    public class OffWorkViewModel : ViewModelBase
    {
        private readonly IFileService<Work.Core.Models.OffWork> _offWorkService;
        private readonly NavigationService<AddOffWorkViewModel> _addOffWorkNavigationService;

        public ObservableCollection<OffWorkItemViewModel> OffWorkList { get; set; }

        public ICommand OnLoadCommand { get; }
        public ICommand AddOffWorkCommand { get; }
        public ICommand DeleteCommand { get; }

        public OffWorkViewModel(IFileService<Work.Core.Models.OffWork> offWorkService, NavigationService<AddOffWorkViewModel> addOffWorkNavigationService)
        {
            _offWorkService = offWorkService;
            _addOffWorkNavigationService = addOffWorkNavigationService;

            OnLoadCommand = new RelayCommand(async (sender) => await Load(sender));
            AddOffWorkCommand = new NavigateCommand(_addOffWorkNavigationService);
            DeleteCommand = new RelayCommand(async (sender) => await Delete(sender));
        }

        private async Task Load(object sender)
        {
            OffWorkList = new ObservableCollection<OffWorkItemViewModel>();

            var offWorkList = await _offWorkService.GetAll();
            foreach (var offWork in offWorkList)
            {
                OffWorkList.Add(new OffWorkItemViewModel(offWork));
            }

            OnPropertyChanged(nameof(OffWorkList));
        }

        private async Task Delete(object sender)
        {
            var offWorkItem = sender as OffWorkItemViewModel;
            await _offWorkService.Delete(offWorkItem.OffWork.Id);
            await Load(sender);
        }
    }
}