using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using ToDoAdvanced.ViewModels.EditWindow;

namespace ToDoAdvanced.Views.EditWindow;

public partial class EditView : UserControl
{
    public EditView()
    {
        InitializeComponent();
    }

    public EditView(EditViewViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
    
}

