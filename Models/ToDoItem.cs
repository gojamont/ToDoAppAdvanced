using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoAdvanced.Models;

//creating enums for priority level and to do status
public enum PriorityLevel {Low, Medium, High}
public enum ToDoStatus {NotStarted, InProgress, Completed}

public partial class ToDoItem : ObservableObject
{ 
    // creating properties for the ToDoItem
    [ObservableProperty] private string? _name = "New TO DO task";
    [ObservableProperty] private string? _description = "New Description";
    [ObservableProperty] private PriorityLevel _priority = PriorityLevel.Low;
    [ObservableProperty] private ToDoStatus _status = ToDoStatus.NotStarted;
    [ObservableProperty] private DateOnly? _date;
    [ObservableProperty] private TimeOnly? _time;
    
    // parameterless constructor for JSON
    public ToDoItem(){}
    
    // making a constructor for the ToDoItem
    public ToDoItem(string name, string description, PriorityLevel priority, ToDoStatus status)
    {
        _name = name;
        _description = description;
        _priority = priority;
        _status = status;
        _date = null;
        _time = null;
    }
    
    public override string ToString() =>
        $"{Name} ({Priority}) - {Status} {(Date.HasValue ? $"on {Date}" : "")}";
}