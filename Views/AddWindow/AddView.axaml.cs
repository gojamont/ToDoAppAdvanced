using Avalonia.Controls;
using ToDoAdvanced.Services.DataService;
using ToDoAdvanced.ViewModels.AddWindow;
using ToDoAdvanced.Services.ToDoManager;

namespace ToDoAdvanced.Views.AddWindow;

public partial class AddView : UserControl
{
    private readonly IDataService? _dataService;

    public AddView()
    {
        InitializeComponent();
    }

    public AddView(IDataService dataService)
        : this()
    {
        _dataService = dataService;
        DataContext = new AddViewViewModel(new ToDoManager(_dataService));
    }

    public AddView(AddViewViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}