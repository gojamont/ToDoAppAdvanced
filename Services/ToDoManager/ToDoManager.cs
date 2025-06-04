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
    public List<ToDoItem> items { get; set; } = [];
    
    public string filePath { get; set; }= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoList.json");
    
    // constructors for to do manager
    // public ToDoManager(){}
    
    public ToDoManager(IDataService dataService)
    {
        _dataService = dataService;
    }
    
    // function for adding a to do task into the list
    public async Task Add(ToDoItem item)
    {
        await ModifyItemsAsync(list => list.Add(new ToDoItem(item.Name, item.Description, item.Priority, item.Status, item.Date ?? DateTime.MinValue, 
            item.Time ?? TimeSpan.MinValue)));
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
    
    // function for saving the data
    public async Task Save(ToDoItem item)
    {
        await ModifyItemsAsync(list =>
        {
            var updatedItem = list.FirstOrDefault(i => i.Id == item.Id);

            if (updatedItem == null)
            {
                Console.WriteLine("Item not found for update.");
                return;
            }

            Console.WriteLine("Before update: " + updatedItem);

            updatedItem.Name = item.Name;
            updatedItem.Description = item.Description;
            updatedItem.Priority = item.Priority;
            updatedItem.Status = item.Status;
            updatedItem.Date = item.Date;
            updatedItem.Time = item.Time;

            Console.WriteLine("After update: " + updatedItem);
        });

    }
    // function for clearing the list
    public async Task ClearAll()
    {
        await ModifyItemsAsync(list =>
        {
            items.Clear();
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
        items = await _dataService.FileDataReader.ReadDataAsync(filePath);
        modifyAction(items);
        await _dataService.FileDataSaver.SaveDataAsync(filePath, items);
    }

    private async Task UpdateStatusAsync(ToDoItem item, ToDoStatus status)
    {
        items = await _dataService.FileDataReader.ReadDataAsync(filePath);

        var existingItem = items.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem == null) return;

        existingItem.Status = status;

        await _dataService.FileDataSaver.SaveDataAsync(filePath, items);
    }
    
}
