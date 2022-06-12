using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorkActivity.WPF.Views
{
    public partial class AddWorkView : UserControl
    {
        public AddWorkView()
        {
            InitializeComponent();
        }

        private void HoursPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:[\.\,][0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}