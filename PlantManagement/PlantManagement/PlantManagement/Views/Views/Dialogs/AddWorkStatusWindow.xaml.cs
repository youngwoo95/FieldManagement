using System;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PlantManagement.Views.ViewModels.CustomerModel;
using PlantManagement.Views.ViewModels.FacilityModel;

namespace PlantManagement.Views.Views.Dialogs;

public partial class AddWorkStatusWindow : Window
{
    private readonly AddWorkStatusViewModel _viewModel;

    public AddWorkStatusWindow() : this(CreateDefaultViewModel())
    {
    }

    public AddWorkStatusWindow(AddWorkStatusViewModel viewModel)
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

    private static AddWorkStatusViewModel CreateDefaultViewModel()
    {
        var viewModel = new AddWorkStatusViewModel();
        var customerViewModel = App.Services?.GetService<CustomerViewModel>();
        if (customerViewModel is not null)
        {
            var customerNames = customerViewModel.Customers
                .Select(x => x.Name)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
            viewModel.SetCustomerNames(customerNames);
        }

        var facilityViewModel = App.Services?.GetService<FacilityViewModel>();
        if (facilityViewModel is not null)
        {
            var facilityNames = facilityViewModel.Facilitys
                .Select(x => x.Name)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
            viewModel.SetFacilityNames(facilityNames);
        }

        return viewModel;
    }
}
