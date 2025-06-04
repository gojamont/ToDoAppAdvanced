using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoAdvanced.Models;

//creating enums for priority level and to do status
public enum PriorityLevel {Low, Medium, High}
public enum ToDoStatus {NotStarted, InProgress, Completed}

public partial class ToDoItem : ObservableObject
{ 
    // // creating properties for the ToDoItem
    [ObservableProperty] private string _name = String.Empty;
    [ObservableProperty] private string _description = String.Empty;
    [ObservableProperty] private PriorityLevel _priority;
    [ObservableProperty] private ToDoStatus _status;
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
        $"{Name} - {Description} - ({Priority}) - {Status} {(Date.HasValue ? $"on {Date}" : "")}";
    
       public override bool Equals(object obj)
        {
            if (obj is not ToDoItem other) return false;
            return Name == other.Name && Description == other.Description;
        }
    
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description);
        }
}
