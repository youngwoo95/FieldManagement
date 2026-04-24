using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
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

    private void AttachmentFileTextBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;

        var openFileDialog = new OpenFileDialog
        {
            Title = "Select attachment",
            Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = false
        };

        var result = openFileDialog.ShowDialog(this);
        if (result != true)
            return;

        _viewModel.AttachmentFilePath = openFileDialog.FileName;
    }
}
