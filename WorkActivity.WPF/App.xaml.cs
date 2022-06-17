using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Interfaces;
using Shared.Services;
using System;
using System.Windows;
using Work.API.Repositories;
using Work.Core.Interfaces;
using Work.Core.Models;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Services.Renderer;
using WorkActivity.WPF.Stores;
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
                services.AddSingleton<IConfigurationService, ConfigurationService>();
                services.AddSingleton<TopBarViewModel>();
                services.AddSingleton<DailyProgressViewModel>();
                services.AddSingleton<SideBarViewModel>();
                services.AddSingleton<ITableBuilder, HTMLTableBuilder>();

                services.AddSingleton<IMenuService, MenuService>();
                services.AddSingleton<IWorkReportService, WorkReportService>();

                services.AddSingleton<ITaskRepository, TaskFileRepository>();
                services.AddSingleton<IFileService<Work.Core.DTOs.Task>>(s => CreateTaskFileService(s));

                services.AddSingleton<IWorkRepository, WorkFileRepository>();
                services.AddSingleton<IFileService<Work.Core.DTOs.Work>>(s => CreateWorkFileService(s));

                services.AddSingleton<ISprintRepository, SprintFileRepository>();
                services.AddSingleton<IFileService<Sprint>>(s => CreateSprintFileService(s));

                services.AddSingleton<IOffWorkRepository, OffWorkRepository>();
                services.AddSingleton<IFileService<OffWork>>(s => CreateOffWorkFileService(s));

                services.AddSingleton<IDailyWorkService, DailyWorkService>();
                services.AddSingleton<IReport, MonthReport>();
                services.AddSingleton<IOffWorkService, OffWorkService>();

                services.AddSingleton<ISnackbarService, SnackbarService>();

                services.AddSingleton<NavigationStore>();
                services.AddSingleton<DailyProgressStore>();
                services.AddSingleton<TaskStore>();
                services.AddSingleton<WorkStore>();
                services.AddSingleton<DailyWorkStore>();

                services.AddSingleton<IFilterService<TaskViewModel>, FilterTaskService>();
                services.AddSingleton<TaskListViewStore>();

                services.AddTransient<NavigationService<SprintListViewModel>>((s) => CreateSprintListNavigationService(s));
                services.AddTransient<NavigationService<AddSprintViewModel>>((s) => CreateAddSprintNavigationService(s));
                services.AddTransient<NavigationService<TaskListViewModel>>((s) => CreateTaskListNavigationService(s));
                services.AddTransient<NavigationService<WorkListViewModel>>((s) => CreateWorkListNavigationService(s));
                services.AddTransient<NavigationService<DailyWorkListViewModel>>((s) => CreateDailyWorkListNavigationService(s));
                services.AddTransient<NavigationService<OffWorkViewModel>>((s) => CreateOffWorkNavigationService(s));
                services.AddTransient<NavigationService<AddOffWorkViewModel>>((s) => CreateAddOffWorkNavigationService(s));
                services.AddTransient<NavigationService<ReportsViewModel>>((s) => CreateReportsNavigationService(s));
                services.AddTransient<SprintListViewModel>((s) => CreateSprintListViewModel(s));
                services.AddTransient<AddSprintViewModel>((s) => CreateAddSprintViewModel(s));
                services.AddTransient<TaskListViewModel>((s) => CreateTaskListViewModel(s));
                services.AddTransient<WorkListViewModel>((s) => CreateWorkListViewModel(s));
                services.AddTransient<DailyWorkListViewModel>((s) => CreateDailyWorkListViewModel(s));

                services.AddSingleton<ModalNavigationStore>();

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
            _host.Start();
            _host.Services.GetRequiredService<NavigationService<TaskListViewModel>>().Navigate();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private async void CloseApplication(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
        }

        private NavigationService<SprintListViewModel> CreateSprintListNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<SprintListViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), () => CreateSprintListViewModel(serviceProvider));
        }

        private NavigationService<AddSprintViewModel> CreateAddSprintNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<AddSprintViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), () => CreateAddSprintViewModel(serviceProvider));
        }

        private NavigationService<TaskListViewModel> CreateTaskListNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<TaskListViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), () => CreateTaskListViewModel(serviceProvider));
        }

        private NavigationService<AddTaskViewModel> CreateAddTaskNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<AddTaskViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), () => CreateAddTaskViewModel(serviceProvider));
        }

        private NavigationService<WorkListViewModel> CreateWorkListNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<WorkListViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), () => CreateWorkListViewModel(serviceProvider));
        }

        private NavigationService<DailyWorkListViewModel> CreateDailyWorkListNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<DailyWorkListViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), () => CreateDailyWorkListViewModel(serviceProvider));
        }

        private NavigationService<OffWorkViewModel> CreateOffWorkNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<OffWorkViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), () => CreateOffWorkViewModel(serviceProvider));
        }

        private NavigationService<AddOffWorkViewModel> CreateAddOffWorkNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<AddOffWorkViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), () => CreateAddOffWorkViewModel(serviceProvider));
        }

        private NavigationService<ReportsViewModel> CreateReportsNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<ReportsViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), () => CreateReportsViewModel(serviceProvider));
        }

        private ParameterNavigationService<object, AddWorkViewModel> CreateAddWorkNavigationService(IServiceProvider serviceProvider)
        {
            return new ParameterNavigationService<object, AddWorkViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), (parameter) => CreateAddWorkViewModel(serviceProvider, parameter));
        }

        private ParameterNavigationService<object, EditTaskViewModel> CreateEditTaskNavigationService(IServiceProvider serviceProvider)
        {
            return new ParameterNavigationService<object, EditTaskViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), (parameter) => CreateEditTaskViewModel(serviceProvider, parameter));
        }

        private ParameterNavigationService<object, DailyWorkDetailsListViewModel> CreateDailyWorkDetailsNavigationService(IServiceProvider serviceProvider)
        {
            return new ParameterNavigationService<object, DailyWorkDetailsListViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), (parameter) => CreateDailyWorkDetailsViewModel(serviceProvider, parameter));
        }

        private ParameterNavigationService<object, AttachedWorkListViewModel> CreateAttachedWorkListNavigationService(IServiceProvider serviceProvider)
        {
            return new ParameterNavigationService<object, AttachedWorkListViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), (parameter) => CreateAttachedWorkListViewModel(serviceProvider, parameter));
        }

        private SprintListViewModel CreateSprintListViewModel(IServiceProvider serviceProvider)
        {
            return new SprintListViewModel(serviceProvider.GetRequiredService<ISnackbarService>(),
                serviceProvider.GetRequiredService<ISprintRepository>(),
                serviceProvider.GetRequiredService<TaskStore>(),
                CreateAddSprintNavigationService(serviceProvider));
        }

        private AddSprintViewModel CreateAddSprintViewModel(IServiceProvider serviceProvider)
        {
            return new AddSprintViewModel(serviceProvider.GetRequiredService<ISnackbarService>(), serviceProvider.GetRequiredService<ISprintRepository>(), CreateSprintListNavigationService(serviceProvider));
        }

        private TaskListViewModel CreateTaskListViewModel(IServiceProvider serviceProvider)
        {
            return new TaskListViewModel(serviceProvider.GetRequiredService<ISnackbarService>(),
                serviceProvider.GetRequiredService<TaskStore>(),
                serviceProvider.GetRequiredService<ISprintRepository>(),
                serviceProvider.GetRequiredService<WorkStore>(),
                serviceProvider.GetRequiredService<IFilterService<TaskViewModel>>(),
                serviceProvider.GetRequiredService<TaskListViewStore>(),
                CreateAddTaskNavigationService(serviceProvider),
                CreateAddWorkNavigationService(serviceProvider),
                CreateEditTaskNavigationService(serviceProvider),
                CreateAttachedWorkListNavigationService(serviceProvider),
                serviceProvider.GetRequiredService<ModalNavigationStore>());
        }

        private AddTaskViewModel CreateAddTaskViewModel(IServiceProvider serviceProvider)
        {
            return new AddTaskViewModel(serviceProvider.GetRequiredService<ISnackbarService>(),
                serviceProvider.GetRequiredService<TaskStore>(),
                serviceProvider.GetRequiredService<ISprintRepository>(),
                CreateTaskListNavigationService(serviceProvider));
        }

        private EditTaskViewModel CreateEditTaskViewModel(IServiceProvider serviceProvider, object parameter)
        {
            return new EditTaskViewModel(serviceProvider.GetRequiredService<ISnackbarService>(),
                serviceProvider.GetRequiredService<TaskStore>(),
                serviceProvider.GetRequiredService<ISprintRepository>(),
                CreateTaskListNavigationService(serviceProvider),
                parameter);
        }

        private WorkListViewModel CreateWorkListViewModel(IServiceProvider serviceProvider)
        {
            return new WorkListViewModel(serviceProvider.GetRequiredService<IConfigurationService>(),
                serviceProvider.GetRequiredService<ISnackbarService>(),
                serviceProvider.GetRequiredService<WorkStore>(),
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                CreateAddWorkNavigationService(serviceProvider));
        }

        private AddWorkViewModel CreateAddWorkViewModel(IServiceProvider serviceProvider, object parameter)
        {
            return new AddWorkViewModel(serviceProvider.GetRequiredService<WorkStore>(), serviceProvider.GetRequiredService<TaskStore>(), CreateWorkListNavigationService(serviceProvider), parameter);
        }

        private DailyWorkListViewModel CreateDailyWorkListViewModel(IServiceProvider serviceProvider)
        {
            return new DailyWorkListViewModel(serviceProvider.GetRequiredService<DailyWorkStore>(), serviceProvider.GetRequiredService<WorkStore>(), CreateDailyWorkDetailsNavigationService(serviceProvider), serviceProvider.GetRequiredService<DailyProgressStore>());
        }

        private ReportsViewModel CreateReportsViewModel(IServiceProvider serviceProvider)
        {
            return new ReportsViewModel(serviceProvider.GetRequiredService<IReport>(), serviceProvider.GetRequiredService<IConfigurationService>(), serviceProvider.GetRequiredService<WorkStore>());
        }

        private OffWorkViewModel CreateOffWorkViewModel(IServiceProvider serviceProvider)
        {
            return new OffWorkViewModel(serviceProvider.GetRequiredService<IOffWorkRepository>(), CreateAddOffWorkNavigationService(serviceProvider));
        }

        private AddOffWorkViewModel CreateAddOffWorkViewModel(IServiceProvider serviceProvider)
        {
            return new AddOffWorkViewModel(serviceProvider.GetRequiredService<IOffWorkRepository
                >(), CreateOffWorkNavigationService(serviceProvider));
        }

        private DailyWorkDetailsListViewModel CreateDailyWorkDetailsViewModel(IServiceProvider serviceProvider, object parameter)
        {
            return new DailyWorkDetailsListViewModel(serviceProvider.GetRequiredService<WorkStore>(), parameter);
        }

        private AttachedWorkListViewModel CreateAttachedWorkListViewModel(IServiceProvider serviceProvider, object parameter)
        {
            return new AttachedWorkListViewModel(serviceProvider.GetRequiredService<ISnackbarService>(), serviceProvider.GetRequiredService<WorkStore>(), CreateAddWorkNavigationService(serviceProvider), parameter);
        }

        private IFileService<Work.Core.DTOs.Work> CreateWorkFileService(IServiceProvider s)
        {
            var confService = s.GetRequiredService<IConfigurationService>();
            var uri = confService.GetDatabasePath();
            return new FileRepository<Work.Core.DTOs.Work>(uri);
        }

        private IFileService<Work.Core.DTOs.Task> CreateTaskFileService(IServiceProvider s)
        {
            var confService = s.GetRequiredService<IConfigurationService>();
            var uri = confService.GetDatabasePath();
            return new FileRepository<Work.Core.DTOs.Task>(uri);
        }

        private IFileService<OffWork> CreateOffWorkFileService(IServiceProvider s)
        {
            var confService = s.GetRequiredService<IConfigurationService>();
            var uri = confService.GetDatabasePath();
            return new FileRepository<OffWork>(uri);
        }

        private IFileService<Sprint> CreateSprintFileService(IServiceProvider s)
        {
            var confService = s.GetRequiredService<IConfigurationService>();
            var uri = confService.GetDatabasePath();
            return new FileRepository<Sprint>(uri);
        }
    }
}