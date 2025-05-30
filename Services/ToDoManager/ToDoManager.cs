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
    
    // constructors for to do manager
    public ToDoManager(){}
    
    public ToDoManager(IDataService dataService)
    {
        _dataService = dataService;
    }
    
     // a function for adding a to do item
     public async Task Add(ToDoItem item)
     {
         // setting the filepath for the to do tasks json file
         var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoList.json");
         
         // reading the file data to add new entries
         items = await _dataService.FileDataReader.ReadDataAsync(filePath);
         
        // adding new entries to the read list
         items.Add(new ToDoItem(item.Name, item.Description, item.Priority, item.Status));
         
         // writing everything into the updated json file
         var updatedJson = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
         
         await File.WriteAllTextAsync(filePath, updatedJson);
     }
     
    // a function for deleting a to do item
    public void Delete(ToDoItem item)
    {
        Console.WriteLine("Delete");
    }

    // function for getting all the entries
    public void GetAll()
    {
        Console.WriteLine("GetAll");
    }
    
}
