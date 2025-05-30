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
    private readonly IViewCommand _viewCommand;
    private readonly IDataService _dataService;
    private readonly IToDoManager _toDoManager;
    public ToDoListViewModel ToDoList { get; }
    
    [ObservableProperty] private UserControl _currentView;
    
    private readonly AddView _addView;
    private readonly ToDoList _listView;
    private readonly IDataReader _dataReader;
    
    public MainWindowViewModel(IToDoManager toDoManager, IDataService dataService, IViewCommand viewCommand, IDataReader dataReader)
    {
        _viewCommand = viewCommand;
        _dataService = dataService;
        _toDoManager = toDoManager;
        _dataReader = dataReader;
        // ToDoList = new ToDoListViewModel(toDoManager, dataReader);
        _addView = new AddView { DataContext = new AddViewViewModel(_toDoManager) };
        _listView = new ToDoList { DataContext = new ToDoListViewModel(_toDoManager,  _dataReader) };
        _currentView = _listView;
    }

    [RelayCommand]
    public void SwitchToAddView(UserControl viewName)
    {
        // _viewCommand.SwitchViewModel(CurrentView, "_addView");
        CurrentView = _addView;
    }
    
    public async Task InitializeAsync()
    {
        await _dataService.FileDataLoader.LoadDataAsync();
    }
}