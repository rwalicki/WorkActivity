﻿using System.Windows.Input;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class DailyProgressViewModel : ViewModelBase
    {
        private readonly DailyProgressStore _dailyProgressStore;

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        private string _percentage;
        public string Percentage
        {
            get => _percentage;
            set
            {
                _percentage = value;
                OnPropertyChanged(nameof(Percentage));
            }
        }

        public ICommand OnLoadCommand { get; set; }

        public DailyProgressViewModel(DailyProgressStore dailyProgressStore)
        {
            _dailyProgressStore = dailyProgressStore;
            _dailyProgressStore.ProgressChanged += ProgressChanged;

            OnLoadCommand = new RelayCommand(Load);
        }

        private async void Load(object obj)
        {
            await _dailyProgressStore.Load();
            ProgressChanged();
        }

        private void ProgressChanged()
        {
            var hours = _dailyProgressStore.Hours;
            Text = $"{hours} / {7.5}";

            var value = (int)((hours / 7.5) * 100);
            Value = value;
            Percentage = value.ToString() + " %";
        }
    }
}