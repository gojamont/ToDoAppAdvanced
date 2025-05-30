using Avalonia.Controls;
using ToDoAdvanced.ViewModels.AddWindow;
using ToDoAdvanced.Services.ToDoManager;

namespace ToDoAdvanced.Views.AddWindow;

public partial class AddView : UserControl
{
    public AddView()
    {
        InitializeComponent();
        DataContext = new AddViewViewModel(new ToDoManager());
    }

    public AddView(AddViewViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}