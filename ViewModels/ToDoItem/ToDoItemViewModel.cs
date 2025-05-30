using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ToDoAdvanced.Models;
using ToDoAdvanced.Services;
using ToDoAdvanced.Services.DataService;

namespace ToDoAdvanced.ViewModels;

public partial class ToDoItemViewModel : ViewModelBase
{
    private ToDoItem Item { get; }
    public string Name => Item.Name ?? "This is the name";
    public string Description => Item.Description ?? "This is the description";
    public ToDoStatus Status => Item.Status;

    private readonly IToDoManager _toDoManager;
    
    private readonly IDataReader _dataReader;

    public ToDoItemViewModel(ToDoItem item, IToDoManager toDoManager, IDataReader dataReader)
    {
        Item = item;
        _toDoManager = toDoManager;
        _dataReader = dataReader;
    }

    [RelayCommand]
    private Task Add()
    {
        return _toDoManager.Add(Item);
    }

    [RelayCommand]
    public void Delete()
    {
        _toDoManager.Delete(Item);
    }
}