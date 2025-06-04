using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ToDoAdvanced.Models;
using ToDoAdvanced.Services;
using ToDoAdvanced.Services.DataService;

namespace ToDoAdvanced.ViewModels;

public partial class ToDoItemViewModel : ViewModelBase
{
    private ToDoItem Item { get;}
    public string Name => Item.Name ?? "This is the name";
    public string Description => Item.Description ?? "This is the description";
    public ToDoStatus Status => Item.Status;

    private readonly IToDoManager _toDoManager;
    
    private readonly IDataReader _dataReader;
    
    // Event to notify parent to reload items
    public event Func<Task>? ItemChanged;

    public ToDoItemViewModel(ToDoItem item, IToDoManager toDoManager, IDataReader dataReader)
    {
        Item = item;
        _toDoManager = toDoManager;
        _dataReader = dataReader;
    }
    
    // a function for adding an item
    [RelayCommand]
    private Task Add() => ExecuteAndNotifyAsync(() => _toDoManager.Add(Item));
    
    // a function for deleting an item
    [RelayCommand]
    private Task Delete() => ExecuteAndNotifyAsync(() => _toDoManager.Delete(Item));
    
    // a function for notifying of changes
    private async Task ExecuteAndNotifyAsync(Func<Task> action)
    {
        await action();
        if (ItemChanged != null)
            await ItemChanged.Invoke();
    }

}