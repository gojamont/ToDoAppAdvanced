using System;
using System.Collections.Generic;
using System.IO;
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
    public ToDoManager(){}
    
    public ToDoManager(IDataService dataService)
    {
        _dataService = dataService;
    }
    
     // a function for adding a to do item
     public async Task Add(ToDoItem item)
     {
         // reading the file data to add new entries
         if (items.Count == 0) 
             items = await _dataService.FileDataReader.ReadDataAsync(filePath);
         
        // adding new entries to the read list
         items.Add(new ToDoItem(item.Name, item.Description, item.Priority, item.Status));
         
         // saving data
         await _dataService.FileDataSaver.SaveDataAsync(filePath, items);
     }
     
    // a function for deleting a to do item
    public async Task Delete(ToDoItem item)
    {
        items = await _dataService.FileDataReader.ReadDataAsync(filePath);
        
        items.Remove(item);
        
        Console.WriteLine($"Item to be removed {item}");
        
        await _dataService.FileDataSaver.SaveDataAsync(filePath, items);
    }

    // function for getting all the entries
    public void GetAll()
    {
        Console.WriteLine("GetAll");
    }
    
}
