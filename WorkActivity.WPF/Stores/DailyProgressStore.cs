using System;

namespace WorkActivity.WPF.Stores
{
    public class DailyProgressStore
    {
        private float _hours;
        public float Hours
        {
            get => _hours;
            set
            {
                _hours = value;
                ProgressChanged?.Invoke();
            }
        }

        public event Action ProgressChanged;
    }
}