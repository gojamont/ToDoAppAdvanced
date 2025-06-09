using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToDoAdvanced.Models;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoAdvanced.Services.DataService;

namespace ToDoAdvanced.Services.ToDoManager;

public class ToDoManager : IToDoManager
{
    // setting a data service for handling json files
    private readonly IDataService _dataService;
    
    public List<ToDoItem> Items { get; set; } = [];
    
    public string FilePath { get; set; }= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoList.json");
    
    // constructors for to do manager
    // public ToDoManager(){}
    
    public ToDoManager(IDataService dataService)
    {
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    }
    
    // function for adding a to do task into the list
    public async Task Add(ToDoItem item)
    {
        await ModifyItemsAsync(list => list.Add(new ToDoItem(item.Name ?? string.Empty, item.Description ?? string.Empty, item.Priority, item.Status, item.Date, 
            item.Time)));
    }
    
    // function for removing an item from the list
    public async Task Delete(ToDoItem item)
    {
        await ModifyItemsAsync(list =>
        {
            var itemToRemove = list.FirstOrDefault(i => i.Id == item.Id);
            if (itemToRemove != null)
                list.Remove(itemToRemove);
        });
    }
    
    // function for clearing the list
    public async Task ClearAll()
    {
        await ModifyItemsAsync(list =>
        {
            Items.Clear();
        });
    }
    
    // functions for updating the statuses to done or in progress

    public async Task IsDone(ToDoItem item)
    {
        await UpdateStatusAsync(item, ToDoStatus.Completed);
    }

    public async Task InProgress(ToDoItem item)
    {
        await UpdateStatusAsync(item, ToDoStatus.InProgress);
    }
    
    //HELPER FUNCTIONS
    
    // Helper method to load, modify, and save items
    private async Task ModifyItemsAsync(Action<List<ToDoItem>> modifyAction)
    {
        Items = await _dataService.FileDataReader.ReadDataAsync(FilePath);
        modifyAction(Items);
        await _dataService.FileDataSaver.SaveDataAsync(FilePath, Items);
    }

    private async Task UpdateStatusAsync(ToDoItem item, ToDoStatus status)
    {
        Items = await _dataService.FileDataReader.ReadDataAsync(FilePath);

        var existingItem = Items.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem == null) return;

        existingItem.Status = status;

        await _dataService.FileDataSaver.SaveDataAsync(FilePath, Items);
    }
    
}
