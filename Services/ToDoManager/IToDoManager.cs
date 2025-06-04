using System.Threading.Tasks;
using ToDoAdvanced.Models;
namespace ToDoAdvanced.Services;

public interface IToDoManager
{
    public Task Add(ToDoItem item);
    public Task Delete(ToDoItem item);
    public void GetAll();
}
