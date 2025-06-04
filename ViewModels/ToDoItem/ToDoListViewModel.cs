using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToDoAdvanced.Models;
using ToDoAdvanced.Services;
using ToDoAdvanced.Services.DataService;

namespace ToDoAdvanced.ViewModels;

public class ToDoListViewModel : ViewModelBase
{
    private readonly IToDoManager _toDoManager;
    private readonly IDataReader _dataReader;
    public ObservableCollection<ToDoItemViewModel> ToDoItems { get; } = new();
    public int ToDoItemsCount => ToDoItems.Count;
    
    private ToDoItemViewModel CreateViewModel(ToDoItem item)
    {
        var vm = new ToDoItemViewModel(item, _toDoManager, _dataReader);
        vm.ItemChanged += LoadToDoItemsAsync;
        return vm;
    }
    
    public ToDoListViewModel(IToDoManager toDoManager, IDataReader dataReader)
    {
        _toDoManager = toDoManager;
        _dataReader = dataReader;
        ToDoItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(ToDoItemsCount));

    }

    public async Task LoadToDoItemsAsync()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoList.json");

        try
        {
            var items = await _dataReader.ReadDataAsync(filePath);
            
            ToDoItems.Clear();
            
            foreach (var vm in items.Select(CreateViewModel))
                ToDoItems.Add(vm);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading to-do items: {ex}");
        }
    }
}