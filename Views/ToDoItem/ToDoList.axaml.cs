using System;
using Avalonia.Controls;
using ToDoAdvanced.Services.DataService;
using ToDoAdvanced.ViewModels;

namespace ToDoAdvanced.Views;

public partial class ToDoList : UserControl
{
    public ToDoList()
    {
        InitializeComponent();
        Loaded += OnViewLoaded;

    }

    public ToDoList(ToDoListViewModel viewModel): this()
    {
        DataContext = viewModel;
    }

    private async void OnViewLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is ToDoListViewModel viewModel)
        {
            try
            {
                await viewModel.LoadToDoItemsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading to-do items in ToDoList.Loaded: {ex}");
            }
        }
        else
        {
            Console.WriteLine("Warning: DataContext is not ToDoListViewModel in ToDoList.Loaded event.");
        }
    }
}