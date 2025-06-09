using System.Collections.ObjectModel;
using ToDoAdvanced.Services;

namespace ToDoAdvanced.ViewModels.EditWindow;

public partial class EditViewViewModel : ViewModelBase
{
    private readonly IToDoManager _toDoManager;
    public ToDoListViewModel ToDoListViewModel { get; }
    
    public ToDoItemViewModel? SelectedItem { get; set; }
    public ObservableCollection<ToDoItemViewModel> ToDoItems => ToDoListViewModel.ToDoItems;

    public EditViewViewModel(ToDoListViewModel toDoListViewModel, IToDoManager toDoManager)
    {
        ToDoListViewModel = toDoListViewModel;
        _toDoManager = toDoManager;
    }
    
}

