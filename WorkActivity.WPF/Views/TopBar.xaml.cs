using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkActivity.WPF.ViewModels;

namespace WorkActivity.WPF.Views
{
    public partial class TopBar : UserControl
    {
        public DailyProgressViewModel DailyProgressViewModel
        {
            get { return (DailyProgressViewModel)GetValue(DailyProgressViewModelProperty); }
            set { SetValue(DailyProgressViewModelProperty, value); }
        }

        public static readonly DependencyProperty DailyProgressViewModelProperty =
            DependencyProperty.Register("DailyProgressViewModel", typeof(DailyProgressViewModel), typeof(TopBar));


        public bool IsMaximized
        {
            get { return (bool)GetValue(IsMaximizedProperty); }
            set { SetValue(IsMaximizedProperty, value); }
        }

        public static readonly DependencyProperty IsMaximizedProperty =
            DependencyProperty.Register("IsMaximized", typeof(bool), typeof(TopBar));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TopBar));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(TopBar));

        public ICommand WindowMaximizeCommand
        {
            get { return (ICommand)GetValue(WindowMaximizeCommandProperty); }
            set { SetValue(WindowMaximizeCommandProperty, value); }
        }

        public static readonly DependencyProperty WindowMaximizeCommandProperty =
            DependencyProperty.Register("WindowMaximizeCommand", typeof(ICommand), typeof(TopBar));

        public ICommand WindowMinimizeCommand
        {
            get { return (ICommand)GetValue(WindowMinimizeCommandProperty); }
            set { SetValue(WindowMinimizeCommandProperty, value); }
        }

        public static readonly DependencyProperty WindowMinimizeCommandProperty =
            DependencyProperty.Register("WindowMinimizeCommand", typeof(ICommand), typeof(TopBar));

        public ICommand WindowRestoreCommand
        {
            get { return (ICommand)GetValue(WindowRestoreCommandProperty); }
            set { SetValue(WindowRestoreCommandProperty, value); }
        }

        public static readonly DependencyProperty WindowRestoreCommandProperty =
            DependencyProperty.Register("WindowRestoreCommand", typeof(ICommand), typeof(TopBar));

        public TopBar()
        {
            InitializeComponent();
        }
    }
}