using Avalonia.Controls;
using ToDoAdvanced.ViewModels;

namespace ToDoAdvanced.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        // Optionally, await initialization if needed
        _ = viewModel.InitializeAsync();
    }
}