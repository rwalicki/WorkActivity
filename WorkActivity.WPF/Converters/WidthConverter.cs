using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WorkActivity.WPF.Converters
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListView l = value as ListView;
            var omittingColumn = System.Convert.ToInt32(parameter);
            GridView g = l.View as GridView;

            double total = 0;

            for (int i = 0; i < g.Columns.Count; i++)
            {
                if (i == omittingColumn)
                {
                    continue;
                }

                total += g.Columns[i].Width;
            }

            return (l.ActualWidth - total - SystemParameters.VerticalScrollBarWidth - 5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}