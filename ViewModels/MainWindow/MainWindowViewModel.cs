using System;
using System.Collections.ObjectModel;
using ToDoAdvanced.Services;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoAdvanced.Models;
using ToDoAdvanced.Services.DataService;
using ToDoAdvanced.Services.ViewService;
using ToDoAdvanced.Threads;
using ToDoAdvanced.Views.AddWindow;
using ToDoAdvanced.ViewModels.AddWindow;
using ToDoAdvanced.ViewModels.EditWindow;
using ToDoAdvanced.Views;
using ToDoAdvanced.Views.EditWindow;
using ToDoItem = ToDoAdvanced.Models.ToDoItem;

namespace ToDoAdvanced.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IDataService _dataService;
    private readonly IToDoManager _toDoManager;
    private ToDoListViewModel _toDoListViewModel;

    [ObservableProperty] private UserControl _currentView;

    private readonly AddView _addView;
    private readonly ToDoList _listView;
    private readonly EditView _editView;
    private readonly IDataReader _dataReader;

    public MainWindowViewModel(IToDoManager toDoManager, IDataService dataService, IDataReader dataReader)
    {
        _dataService = dataService;
        _toDoManager = toDoManager;
        _dataReader = dataReader;

        _toDoListViewModel = new ToDoListViewModel(_toDoManager, _dataReader);

        _addView = new AddView { DataContext = new AddViewViewModel(_toDoManager) };
        _listView = new ToDoList { DataContext = _toDoListViewModel };
        _editView = new EditView { DataContext = new EditViewViewModel(_toDoListViewModel, _toDoManager) };

        _currentView = _listView;
    }

    [RelayCommand]
    public void SwitchToAddView(UserControl viewName)
    {
        CurrentView = _addView;
    }

    [RelayCommand]
    public void SwitchToMainView(UserControl viewName)
    {
        CurrentView = _listView;
    }
    
    [RelayCommand]
    public void SwitchToEditView(ToDoItem item)
    {
       CurrentView = _editView;
    }
    

    public async Task InitializeAsync()
    {
        await _dataService.FileDataLoader.LoadDataAsync();
    }

    [RelayCommand]
    private async Task ClearAll()
    {
        await _toDoManager.ClearAll();
        _= _toDoListViewModel.LoadToDoItemsAsync();
    }

}

