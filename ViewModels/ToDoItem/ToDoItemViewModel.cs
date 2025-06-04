using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoAdvanced.Models;
using ToDoAdvanced.Services;
using ToDoAdvanced.Services.DataService;

namespace ToDoAdvanced.ViewModels;

public partial class ToDoItemViewModel : ViewModelBase
{
    private ToDoListViewModel _listViewModel;
   public ToDoItem Item { get;}
   
   [ObservableProperty] private string _name;
   
   [ObservableProperty] private string _description;
   // [ObservableProperty] public string Description => Item.Description ?? "This is the description";
   
   [ObservableProperty] private ToDoStatus _status;
   
   [ObservableProperty] private bool _deadlineReached;

    private readonly IToDoManager _toDoManager;
    
    private readonly IDataReader _dataReader;
    
    private readonly IDataService _dataService;
    
    public ObservableCollection<ToDoStatus> Statuses = new ObservableCollection<ToDoStatus>();

    
    // Event to notify parent to reload items
    public event Func<Task>? ItemChanged;

    public ToDoItemViewModel(ToDoItem item, IToDoManager toDoManager, IDataReader dataReader, ToDoListViewModel listViewModel)
    {
        Item = item;
        _toDoManager = toDoManager;
        _dataReader = dataReader;
        
        Name = Item.Name ?? "This is the name";
        Description = Item.Description ?? "This is the description";
        Status = Item.Status;
        
        _listViewModel = listViewModel;
        
        _deadlineReached = listViewModel.DeadlineReached;
    }
    
    // a function for adding an item
    [RelayCommand]
    private Task Add() => ExecuteAndNotifyAsync(() => _toDoManager.Add(Item));
    
    // a function for deleting an item
    [RelayCommand]
    private Task Delete() => ExecuteAndNotifyAsync(() => _toDoManager.Delete(Item));
    
    [RelayCommand]
    private Task InProgress() => ExecuteAndNotifyAsync(() => _toDoManager.InProgress(Item));
    
    [RelayCommand]
    private Task IsDone() => ExecuteAndNotifyAsync(() => _toDoManager.IsDone(Item));
    
    
    // a function for notifying of changes
    private async Task ExecuteAndNotifyAsync(Func<Task> action)
    {
        await action();
        if (ItemChanged != null)
            await ItemChanged.Invoke();
    }
    
    public ToDoItem ToToDoItem()
    {
        return new ToDoItem
        {
            Id = Item.Id,
            Name = Item.Name,
            Description = Item.Description,
            Status = Item.Status,
            Priority = Item.Priority
        };
    }
    
}