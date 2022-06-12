using System;
using WorkActivity.WPF.ViewModels;

namespace WorkActivity.WPF.Stores
{
    public class NavigationStore
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (!_currentViewModel?.GetType().Equals(value?.GetType() ?? null) ?? true)
                {
                    _currentViewModel?.Dispose();
                    _currentViewModel = value;
                    OnCurrentViewModelChanged();
                }
            }
        }

        public event Action CurrentViewModelChanged;

        public void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}