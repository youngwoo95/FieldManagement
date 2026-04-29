using System;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PlantManagement.Service.v1.Works;
using PlantManagement.Views.ViewModels.CustomerModel;

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
        return new AddWorkStatusViewModel();
    }

    private async void FacilityComboBox_OnDropDownOpened(object sender, EventArgs e)
    {
        var workService = App.Services?.GetService<IWorkService>();
        if (workService is null)
        {
            return;
        }

        var rows = await workService.GetWorkFacilitiesService();
        var facilityNames = (rows ?? [])
            .Select(x => x.facilityName)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);

        _viewModel.SetFacilityNames(facilityNames);
    }
}
