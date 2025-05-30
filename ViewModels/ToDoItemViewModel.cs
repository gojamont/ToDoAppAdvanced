using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoAdvanced.Models;
namespace ToDoAdvanced.ViewModels;

public partial class ToDoItemViewModel : ViewModelBase
{
    // adding a property for to do item
    public ToDoItem Item { get; }
    
    // making constructor for the to do item view model
    public ToDoItemViewModel(ToDoItem item) => Item = item;
    
    // public ToDoItem GetModel() => _item;
}