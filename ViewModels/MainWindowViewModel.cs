namespace ToDoAdvanced.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ToDoItemViewModel? ToDoItemViewModel { get; set; }
    public ToDoListViewModel? ToDoListViewModel { get; set; }
    public string Greeting { get; } = "Welcome to Avalonia!";
}