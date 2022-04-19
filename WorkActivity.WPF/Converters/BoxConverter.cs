using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WorkActivity.WPF.ViewModels;
using WorkActivity.WPF.Views;

namespace WorkActivity.WPF.Converters
{
    internal class BoxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is bool)
            {
                var selected = (bool)values[0];
                if (selected)
                {
                    ((values[2] as AddTaskView).DataContext as AddTaskViewModel).SelectedSprints.Add((int)values[1]);
                }
                else
                {
                    ((values[2] as AddTaskView).DataContext as AddTaskViewModel).SelectedSprints.Remove((int)values[1]);
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
