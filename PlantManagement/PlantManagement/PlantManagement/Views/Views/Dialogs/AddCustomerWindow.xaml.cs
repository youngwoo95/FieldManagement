using System;
using System.Windows;
using PlantManagement.Views.ViewModels.CustomerModel;

namespace PlantManagement.Views.Views.Dialogs;

public partial class AddCustomerWindow : Window
{
    private readonly AddCustomerViewModel _viewModel;

    public AddCustomerWindow(AddCustomerViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
        _viewModel.RequestClose += OnRequestClose;
        Closed += OnClosed;
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
