using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using ToDoAdvanced.ViewModels;
using ToDoAdvanced.Views;
using System;
using ToDoAdvanced.Services;
using ToDoAdvanced.Services.ToDoManager;
using ToDoAdvanced.Services.DataService;
using ToDoAdvanced.Services.ViewService;

namespace ToDoAdvanced;

public partial class App : Application
{
    private IServiceProvider? Services { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Setup dependency injection container
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        this.Services = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            // Only resolve MainWindowViewModel from the container
            var mainWindowViewModel = this.Services.GetRequiredService<MainWindowViewModel>();
            // Do NOT resolve ToDoItemViewModel here
            // var toDoItemViewModel = this.Services.GetRequiredService<ToDoItemViewModel>();
            var toDoListViewModel = this.Services.GetRequiredService<ToDoListViewModel>();
            var todoManager = this.Services.GetRequiredService<IToDoManager>();
            var dataManager = this.Services.GetRequiredService<IDataService>();
            
            // REGISTERING FOR THE DATA SERVICE
            var dataWriter = dataManager.FileDataWriter;
            var dataSaver = dataManager.FileDataSaver;
            var dataLoader = dataManager.FileDataLoader;
            var dataReader = dataManager.FileDataReader;
            
            var viewManager = this.Services.GetRequiredService<IViewCommand>();

            desktop.MainWindow = new MainWindow(mainWindowViewModel)
            {
                DataContext = mainWindowViewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // viewmodels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ToDoListViewModel>();

        // services
        services.AddSingleton<IToDoManager, ToDoManager>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IViewCommand, ViewCommand>();
        
        // Register the concrete implementations for the data operation interfaces
        services.AddSingleton<IDataWriter, FileDataWriter>();
        services.AddSingleton<IDataReader, FileDataReader>();
        services.AddSingleton<IDataLoader, FileDataLoader>();
        services.AddSingleton<IDataSaver, FileDataSaver>();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}