using ToDoAdvanced.Models;
using System.Collections.Generic;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoAdvanced.Services;
using ToDoAdvanced.Views;
using ToDoItem = ToDoAdvanced.Models.ToDoItem;

namespace ToDoAdvanced.ViewModels.AddWindow;

public partial class AddViewViewModel : ViewModelBase
{
    public IEnumerable<ToDoStatus> Statuses => Enum.GetValues<ToDoStatus>();
    public IEnumerable<PriorityLevel> Priorities => Enum.GetValues<PriorityLevel>();
    
        
    [ObservableProperty] private ToDoStatus _selectedStatus;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private string? _description;
    [ObservableProperty] private PriorityLevel _selectedPriority;
    [ObservableProperty] private DateTimeOffset _date = new DateTimeOffset(DateTime.Today);    
    [ObservableProperty] private TimeSpan _time = DateTime.Now.TimeOfDay; 

    private readonly IToDoManager _toDoManager;
    
    public AddViewViewModel(IToDoManager toDoManager)
    {
        _toDoManager = toDoManager;
    }

    [RelayCommand]
    internal void AddItem()
    {
        var newItem = new ToDoItem(Name ?? string.Empty, Description ?? string.Empty, SelectedPriority, SelectedStatus, Date,  Time);
        _toDoManager.Add(newItem);
    }
}