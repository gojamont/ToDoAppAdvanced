using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoAdvanced.Models;

//creating enums for priority level and to do status
public enum PriorityLevel {Low, Medium, High}
public enum ToDoStatus {NotStarted, InProgress, Completed}

public partial class ToDoItem : ObservableObject
{ 
    // creating properties for the ToDoItem
    [ObservableProperty] private Guid _id;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private string? _description;
    [ObservableProperty] private PriorityLevel _priority;
    [ObservableProperty] private ToDoStatus _status;
    [ObservableProperty] private DateTimeOffset _date; 
    [ObservableProperty] private TimeSpan _time;
    
    // parameterless constructor for JSON
    public ToDoItem()
    {
        _id = Guid.NewGuid(); // Assign a new unique ID
    }
    
    // making a constructor for the ToDoItem
    public ToDoItem(string name, string description, PriorityLevel priority, ToDoStatus status, DateTimeOffset date, TimeSpan time)
    {
        Id = Guid.NewGuid(); // Assign a new unique ID
        Name = name;
        Description = description;
        Priority = priority;
        Status = status;
        Date = date;
        Time = time;
    }
    
   public override string ToString() =>
       $"{Name} - {Description} - ({Priority}) - {Status}" +
       (Date != default ? $" on {Date}" : "");

    public override bool Equals(object? obj)
    {
        if (obj is not ToDoItem other) return false;
        return Id == other.Id; // Use Id for equality
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id); // Use Id for hash code
    }
}
