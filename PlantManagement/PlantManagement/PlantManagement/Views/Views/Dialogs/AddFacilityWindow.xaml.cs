using System.Windows;
using PlantManagement.Views.ViewModels.FacilityModel;

namespace PlantManagement.Views.Views.Dialogs;

public partial class AddFacilityWindow : Window
{
    private readonly AddFacilityViewModel _viewModel;
    
    public AddFacilityWindow(AddFacilityViewModel viewModel)
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