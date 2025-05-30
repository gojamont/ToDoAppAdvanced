using System;
using System.Collections.ObjectModel;
using ToDoAdvanced.Services;

namespace ToDoAdvanced.ViewModels;

public class ToDoListViewModel : ViewModelBase
{
    public ToDoItemViewModel ToDoItemViewModel { get; }
    public ObservableCollection<ToDoItemViewModel> ToDoItems =  new();
    private readonly IToDoManager _toDoManager;

    public ToDoListViewModel(IToDoManager _toDoManager)
    {
        _toDoManager = _toDoManager;
    }
    
}