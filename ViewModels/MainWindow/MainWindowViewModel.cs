using ToDoAdvanced.Services;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoAdvanced.Services.DataService;
using ToDoAdvanced.Services.ViewService;
using ToDoAdvanced.Views.AddWindow;
using ToDoAdvanced.ViewModels.AddWindow;
using ToDoAdvanced.Views;

namespace ToDoAdvanced.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // private readonly IViewCommand _viewCommand;
    private readonly IDataService _dataService;
    private readonly IToDoManager _toDoManager;
    
    // public ToDoListViewModel ToDoList { get; }
    
    // setting a variable for a current view
    
    [ObservableProperty] private UserControl _currentView;
    
    // setting the views, add view for adding an item, and list view as default, to show to the user
    
    private readonly AddView _addView;
    private readonly ToDoList _listView;
    private readonly IDataReader _dataReader;
    
    public MainWindowViewModel(IToDoManager toDoManager, IDataService dataService, IDataReader dataReader)
    {
        // registering the services and commands, kinda annoying thing to do
        // _viewCommand = viewCommand;
        _dataService = dataService;
        _toDoManager = toDoManager;
        _dataReader = dataReader;
        
        // ToDoList = new ToDoListViewModel(toDoManager, dataReader);
        _addView = new AddView { DataContext = new AddViewViewModel(_toDoManager) };
        _listView = new ToDoList { DataContext = new ToDoListViewModel(_toDoManager,  _dataReader) };
        _currentView = _listView;
    }
    
    // a function for switching between views, for now only add view, but in future, it should be more reusable
    [RelayCommand]
    public void SwitchToAddView(UserControl viewName)
    {
        // _viewCommand.SwitchViewModel(CurrentView, "_addView");
        CurrentView = _addView;
    }
    
    // initializing async
    public async Task InitializeAsync()
    {
        await _dataService.FileDataLoader.LoadDataAsync();
    }
}