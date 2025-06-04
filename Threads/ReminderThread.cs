using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using ToDoAdvanced.Models;
using ToDoAdvanced.ViewModels;

namespace ToDoAdvanced.Threads;

public class ReminderThread
{
    // setting view models
    private readonly ToDoItemViewModel _toDoItemViewModel;
    private readonly ToDoListViewModel _toDoListViewModel;
    
    // locking resources to prevent interaction with UI
    private readonly object _reminderLock = new();
    private bool _reminderStarted = false;


    public ReminderThread(ToDoItemViewModel toDoItemViewModel, ToDoListViewModel toDoListViewModel)
    {
        _toDoItemViewModel = toDoItemViewModel;
        _toDoListViewModel = toDoListViewModel;
    }
    
    // running the thread
    public void Run()
    {
        lock (_reminderLock)
        {
            if (_reminderStarted)
                return;
            _reminderStarted = true;
        }
        Thread thread = new Thread(RemindDeadline) { IsBackground = true };
        thread.Start();
    }

    private void RemindDeadline()
    {
        var date = _toDoItemViewModel.Item.Date;
        var time = _toDoItemViewModel.Item.Time;

        try
        {
            if (!date.HasValue || !time.HasValue)
                return;

            var targetDateTime = date.Value.Date + time.Value;
            var now = DateTime.Now;

            if (now < targetDateTime)
            {
                var sleepTime = (int)(targetDateTime - now).TotalMilliseconds;
                if (sleepTime > 0)
                    Thread.Sleep(sleepTime);
            }

            if (_toDoItemViewModel.Item.Status != ToDoStatus.Completed && !_toDoItemViewModel.DeadlineReached)
            {
                Notify("Deadline has come!", true);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in the thread: {e}");
        }
    }

    private void Notify(string message, bool deadlineReached = false)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (deadlineReached)
            {
                _toDoItemViewModel.DeadlineReached = true;
            }

            _toDoListViewModel.NotificationMessage = message;
        });

        Task.Run(async () =>
        {
            await Task.Delay(5000);
            ClearNotification();
        });
    }

    private void ClearNotification()
    {
        Dispatcher.UIThread.InvokeAsync(() => { _toDoListViewModel.NotificationMessage = string.Empty; });

    }
}
   