using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoAdvanced.Models;

namespace ToDoAdvanced.Services.DataService;

public class DataService(IDataWriter fileDataWriter, IDataLoader fileDataLoader, IDataReader fileDataReader, IDataSaver fileDataSaver) : IDataService
{
    // class for writing data
    public IDataWriter FileDataWriter { get; } = fileDataWriter;
    
    // class for loading data
    public IDataLoader FileDataLoader { get; } = fileDataLoader;
    
    // class for reading data
    public IDataReader FileDataReader { get; } = fileDataReader;
    
    // class for saving data
    public IDataSaver FileDataSaver { get; } = fileDataSaver;
}

// function for writing data async
public partial class FileDataWriter : IDataWriter
{
    public async Task WriteDataAsync(string json)
    {
        Console.WriteLine("Writing data...");
        await File.WriteAllTextAsync(json, "");
    }
}

// function for loading data async
public partial class FileDataLoader(IDataWriter fileDataWriter, IDataReader fileDataReader) : IDataLoader
{
    public async Task LoadDataAsync()
    {
        Console.WriteLine("Loading data...");
        await Task.Delay(500);

        if (!File.Exists("ToDoList.json"))
        {
            await fileDataWriter.WriteDataAsync("ToDoList.json");
        }

        await fileDataReader.ReadDataAsync("ToDoList.json");
    }
}

// function for reading data async
public partial class FileDataReader : IDataReader
{
    private List<ToDoItem> _items = new List<ToDoItem>();
    public async Task<List<ToDoItem>> ReadDataAsync(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                if (!string.IsNullOrWhiteSpace(json)) 
                    _items = JsonSerializer.Deserialize<List<ToDoItem>>(json) 
                             ?? new List<ToDoItem>();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while reading a file {e}");
        }
        return _items;
    }
}

// function for saving data async
public partial class FileDataSaver : IDataSaver
{
    public async Task SaveDataAsync(string fileName, List<ToDoItem> items)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        var json = JsonSerializer.Serialize(items, jsonOptions);
        
        await File.WriteAllTextAsync(fileName, json);
    }
}