using Avalonia.Controls;
using ToDoAdvanced.ViewModels;

namespace ToDoAdvanced.Views;

public partial class ToDoItem : UserControl
{
    public ToDoItem()
    {
        InitializeComponent();
    }

    public ToDoItem(ToDoItemViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}