using Avalonia.Controls;
using ToDoAdvanced.ViewModels;

namespace ToDoAdvanced.Views;

public partial class ToDoItem : UserControl
{ 
    public ToDoItem()
    {
        InitializeComponent();
        DataContext = new ToDoItemViewModel(new Models.ToDoItem());   
    }
}