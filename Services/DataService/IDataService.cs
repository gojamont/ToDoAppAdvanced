using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoAdvanced.Models;

namespace ToDoAdvanced.Services.DataService;

public interface IDataService
{
     public IDataWriter FileDataWriter { get; }
     public IDataReader FileDataReader { get; }
     public IDataLoader FileDataLoader { get; }
     public IDataSaver  FileDataSaver { get; }
}

public interface IDataWriter
{
     Task WriteDataAsync(string json);
}

public interface IDataLoader
{
     Task LoadDataAsync();
}

public interface IDataSaver
{
     Task SaveDataAsync(string fileName, List<ToDoItem> items);
}

public interface IDataReader
{ 
     Task<List<ToDoItem>> ReadDataAsync(string filePath);
}