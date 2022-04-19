using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Interfaces;
using Shared.Services;
using System.Windows;
using Work.API.Repositories;
using Work.Core.Interfaces;
using WorkActivity.WPF.ViewModels;
using WorkActivity.WPF.Views;

namespace WorkActivity.WPF
{
    public partial class App : Application
    {
        private IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton<ITaskRepository, TaskFileRepository>();
                services.AddSingleton<IFileService<Work.Core.DTOs.Task>>(new FileRepository<Work.Core.DTOs.Task>());

                services.AddSingleton<IWorkRepository, WorkFileRepository>();
                services.AddSingleton<IFileService<Work.Core.DTOs.Work>>(new FileRepository<Work.Core.DTOs.Work>());

                services.AddSingleton<ISprintService, SprintService>();
                services.AddSingleton<IFileService<Work.Core.Models.Sprint>>(new FileRepository<Work.Core.Models.Sprint>());

                services.AddSingleton<IDailyWorkService, DailyWorkService>();

                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            })
            .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            using (_host)
            {
                _host.Start();
                var mainWindow = _host.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
        }

        private async void CloseApplication(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
        }
    }
}