using Shared.Interfaces;
using System;
using System.Windows;
using System.Windows.Input;

namespace WorkActivity.WPF.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IWindowOperations vm)
            {
                vm.Minimize += () => WindowState = WindowState.Minimized;
                vm.Maximize += () => WindowState = WindowState.Maximized;
                vm.Restore += () => WindowState = WindowState.Normal;
                vm.Close += () => Close();
                vm.SetWindowTitle(Title);
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (DataContext is IWindowOperations vm)
            {
                if (WindowState == WindowState.Maximized)
                {
                    vm.WindowMaximized(true);
                }
                else
                {
                    vm.WindowMaximized(false);
                }
            }
        }
    }
}