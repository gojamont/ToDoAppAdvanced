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

namespace ToDoAdvanced;

public partial class App : Application
{
    private IServiceProvider Services { get; set; }

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

            // Resolve MainWindowViewModel from the container
            var mainWindowViewModel = this.Services.GetRequiredService<MainWindowViewModel>();
            var toDoItemViewModel = this.Services.GetRequiredService<ToDoItemViewModel>();
            var toDoListViewModel = this.Services.GetRequiredService<ToDoListViewModel>();
            var todoManager = this.Services.GetRequiredService<IToDoManager>();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // viewmodels
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<ToDoItemViewModel>();
        services.AddSingleton<ToDoListViewModel>();

        // services 
        services.AddSingleton<IToDoManager, ToDoManager>();
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