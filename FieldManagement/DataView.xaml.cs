using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FieldManagement.Themes;
using Microsoft.Web.WebView2.Core;

namespace FieldManagement;

public partial class DataView : UserControl
{
    private readonly string _samplePdfPath;
    private bool _pdfViewerInitialized;

    public DataView()
    {
        InitializeComponent();
        _samplePdfPath = ResolvePdfPath();
        WorkOrderGrid.ItemsSource = new[]
        {
            new WorkOrderRow
            {
                WorkOrderNo = "WO-20260420-001",
                MachineName = "MA-0001",
                CustomerName = "장성운",
                Status = "진행중",
                WorkDate = "2026-04-20"
            }
        };
    }

    private async void WorkOrderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (WorkOrderGrid.SelectedItem == null)
            return;

        OpenPdfPanel();
        await LoadPdfPreviewAsync(_samplePdfPath);
    }

    private void ClosePdfPanel_Click(object sender, RoutedEventArgs e)
    {
        ClosePdfPanel();
        WorkOrderGrid.UnselectAll();
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

    private static string ResolvePdfPath()
    {
        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Pdfs", "Notion.pdf"),
            Path.Combine(Directory.GetCurrentDirectory(), "Pdfs", "Notion.pdf"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Pdfs", "Notion.pdf")
        };

        foreach (var candidate in candidates)
        {
            var fullPath = Path.GetFullPath(candidate);
            if (File.Exists(fullPath))
                return fullPath;
        }

        return Path.GetFullPath(candidates[0]);
    }

    private sealed class WorkOrderRow
    {
        public string WorkOrderNo { get; init; } = string.Empty;
        public string MachineName { get; init; } = string.Empty;
        public string CustomerName { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string WorkDate { get; init; } = string.Empty;
    }
}
