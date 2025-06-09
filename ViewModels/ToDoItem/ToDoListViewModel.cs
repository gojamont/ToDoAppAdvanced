using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToDoAdvanced.Models;
using ToDoAdvanced.Services;
using ToDoAdvanced.Services.DataService;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Moq;
using ToDoAdvanced.Threads;


namespace ToDoAdvanced.ViewModels;

public partial class ToDoListViewModel : ViewModelBase
{
    private readonly IToDoManager _toDoManager;
    private readonly IDataReader _dataReader;

    [ObservableProperty] private string? _notificationMessage;
    public ObservableCollection<ToDoItemViewModel> ToDoItems { get; } = new();
    public int ToDoItemsCount => ToDoItems.Count;

    [ObservableProperty] private bool _deadlineReached = false;
    
    // In ToDoListViewModel.cs
    public ToDoListViewModel()
        : this(new Mock<IToDoManager>().Object, new Mock<IDataReader>().Object)
    {
    }
    
    private ToDoItemViewModel CreateViewModel(ToDoItem item)
    {
        var vm = new ToDoItemViewModel(item, _toDoManager, this);
        vm.ItemChanged += LoadToDoItemsAsync;
        return vm;
    }
    
    public ToDoListViewModel(IToDoManager toDoManager, IDataReader dataReader)
    {
        _toDoManager = toDoManager;
        _dataReader = dataReader;
        
        ToDoItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(ToDoItemsCount));

    }

    public async Task LoadToDoItemsAsync()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoList.json");

        try
        {
            var items = await _dataReader.ReadDataAsync(filePath);
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                ToDoItems.Clear();
                foreach (var vm in items.Select(CreateViewModel))
                {
                    ToDoItems.Add(vm);
                    var reminderThread = new ReminderThread(vm, this);
                    reminderThread.Run();
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading to-do items: {ex}");
        }
    }
}
