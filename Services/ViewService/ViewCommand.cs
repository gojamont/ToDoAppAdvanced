using ToDoAdvanced.ViewModels.AddWindow;
using ToDoAdvanced.ViewModels;
using System;
using Avalonia.Controls;
using ToDoAdvanced.Views.AddWindow;

namespace ToDoAdvanced.Services.ViewService;
public class ViewCommand : IViewCommand
{
    // private readonly AddView _addView = new AddView{DataContext=new AddViewViewModel()};
    public void SwitchViewModel(UserControl currentView, string targetView)
    {
        // currentView = _addView;
    }
}