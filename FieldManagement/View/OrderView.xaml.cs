using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using FieldManagement.Themes;
using FieldManagement.ViewModels;
using Microsoft.Web.WebView2.Core;

namespace FieldManagement.View;

public partial class OrderView : UserControl
{
    private bool _pdfViewerInitialized;
    public OrderView()
    {
        InitializeComponent();
        DataContext = new OrderViewModel();
        Loaded += DataView_Loaded;
    }
    
      private void DataView_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is OrderViewModel vm)
        {
            vm.PropertyChanged -= ViewModel_PropertyChanged;
            vm.PropertyChanged += ViewModel_PropertyChanged;
        }
    }
    
   private async void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not OrderViewModel vm)
            return;

        if (e.PropertyName == nameof(OrderViewModel.IsPdfPanelOpen))
        {
            if (vm.IsPdfPanelOpen)
                OpenPdfPanel();
            else
                ClosePdfPanel();
        }

        if (e.PropertyName == nameof(OrderViewModel.SelectedPdfPath) && !string.IsNullOrWhiteSpace(vm.SelectedPdfPath))
        {
            await LoadPdfPreviewAsync(vm.SelectedPdfPath);
        }
    }

    private void OpenPdfPanel()
    {
        var animation = new GridLengthAnimation
        {
            From = new GridLength(0),
            To = new GridLength(500),
            Duration = new Duration(TimeSpan.FromMilliseconds(250))
        };

        PdfPanelColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
    }

    private void ClosePdfPanel()
    {
        var animation = new GridLengthAnimation
        {
            From = PdfPanelColumn.Width,
            To = new GridLength(0),
            Duration = new Duration(TimeSpan.FromMilliseconds(200))
        };

        PdfPanelColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
    }

    private async Task LoadPdfPreviewAsync(string pdfPath)
    {
        if (!File.Exists(pdfPath))
        {
            ShowPdfFallback($"PDF file not found.\n{pdfPath}");
            return;
        }

        try
        {
            await EnsurePdfViewerAsync();
            PdfFallbackText.Visibility = Visibility.Collapsed;
            PdfViewer.Visibility = Visibility.Visible;
            PdfViewer.Source = new Uri(pdfPath);
        }
        catch (Exception ex)
        {
            ShowPdfFallback($"Unable to open PDF preview.\n{ex.Message}");
        }
    }

    private async Task EnsurePdfViewerAsync()
    {
        if (_pdfViewerInitialized)
            return;

        try
        {
            await PdfViewer.EnsureCoreWebView2Async();
        }
        catch (WebView2RuntimeNotFoundException ex)
        {
            throw new InvalidOperationException(
                "WebView2 Runtime is missing. Install: https://developer.microsoft.com/microsoft-edge/webview2/",
                ex);
        }

        PdfViewer.CoreWebView2.Settings.AreDevToolsEnabled = false;
        _pdfViewerInitialized = true;
    }

    private void ShowPdfFallback(string message)
    {
        PdfViewer.Visibility = Visibility.Collapsed;
        PdfFallbackText.Text = message;
        PdfFallbackText.Visibility = Visibility.Visible;
    }
}
