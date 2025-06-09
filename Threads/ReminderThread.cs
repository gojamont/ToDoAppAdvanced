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

    internal bool DeadlineReached { get; set; } = false; 
    
    // locking resources to prevent interaction with UI
    private readonly object _reminderLock = new();
    
    internal bool ReminderStarted = false;


    public ReminderThread(ToDoItemViewModel toDoItemViewModel, ToDoListViewModel toDoListViewModel)
    {
        _toDoItemViewModel = toDoItemViewModel ?? throw new ArgumentNullException(nameof(toDoItemViewModel));
        _toDoListViewModel = toDoListViewModel ?? throw new ArgumentNullException(nameof(toDoListViewModel));
    }
    
    // running the thread
    public void Run()
    {
        lock (_reminderLock)
        {
            if (ReminderStarted)
                return;
            ReminderStarted = true;
        }
        Thread thread = new Thread(RemindDeadline) { IsBackground = true };
        thread.Start();
    }

    internal void RemindDeadline()
    {
        var date = _toDoItemViewModel.Item.Date;
        var time = _toDoItemViewModel.Item.Time;

        try
        {
            var targetDateTime = date + time;
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

    internal void Notify(string message, bool deadlineReached)
    {
        DeadlineReached = deadlineReached;
        try
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
        catch (Exception e)
        {
            Console.WriteLine($"Error in the thread: {e}");
        }
        
    }

    internal void ClearNotification()
    {
        Dispatcher.UIThread.InvokeAsync(() => { _toDoListViewModel.NotificationMessage = string.Empty; });

    }
}
   