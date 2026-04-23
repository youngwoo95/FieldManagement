using System.Windows;
using PlantManagement.Views.ViewModels.CustomerModel;

namespace PlantManagement.Views.Views.Dialogs;

public partial class AddOrderWindow : Window
{
    private readonly AddOrderViewModel _viewModel;
    
    public AddOrderWindow(AddOrderViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
        _viewModel.RequestClose += OnRequestClose;
    }

    private void OnRequestClose(bool? dialogResult)
    {
        DialogResult = dialogResult;
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        _viewModel.RequestClose -= OnRequestClose;
        Closed -= OnClosed;
    }
    
    
}