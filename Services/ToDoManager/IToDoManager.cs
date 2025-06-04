using System.Threading.Tasks;
using ToDoAdvanced.Models;
namespace ToDoAdvanced.Services;

public interface IToDoManager
{
    public Task Add(ToDoItem item);
    public Task Delete(ToDoItem item);
    public Task IsDone(ToDoItem item);
    public Task InProgress(ToDoItem item);
    public Task Save(ToDoItem item);
    public Task ClearAll();
}
