using System.IO;
using System.Windows.Input;

namespace PlantManagement.Views.ViewModels.WorkStatusModel;

public partial class WorkStatusViewModel : BaseViewModel
{
    private static readonly Uri BlankPdfUri = new("about:blank");
    
    public ICommand ClosePdfPanelCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand AddCommand { get; }

    public WorkStatusViewModel()
    {
        
    }
    
    
    
    
    
    private void UpdatePdfPreviewState(string? pdfPath)
    {
        if (string.IsNullOrWhiteSpace(pdfPath))
        {
            SelectedPdfUri = BlankPdfUri;
            IsPdfFallbackVisible = true;
            PdfFallbackMessage = "Select an order to preview PDF.";
            return;
        }

        var fullPath = Path.GetFullPath(pdfPath);
        if (!File.Exists(fullPath))
        {
            SelectedPdfUri = BlankPdfUri;
            IsPdfFallbackVisible = true;
            PdfFallbackMessage = $"PDF file not found.{Environment.NewLine}{fullPath}";
            return;
        }

        SelectedPdfUri = new Uri(fullPath, UriKind.Absolute);
        IsPdfFallbackVisible = false;
        PdfFallbackMessage = string.Empty;
    }

    private static string ResolvePdfPath(string pdfFileName)
    {
        var fileName = string.IsNullOrWhiteSpace(pdfFileName) ? "pdf1.pdf" : pdfFileName.Trim();

        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Comm", "Pdfs", fileName),
            Path.Combine(Directory.GetCurrentDirectory(), "Comm", "Pdfs", fileName),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Comm", "Pdfs", fileName),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Comm", "Pdfs", fileName)
        };

        foreach (var candidate in candidates)
        {
            var fullPath = Path.GetFullPath(candidate);
            if (File.Exists(fullPath))
                return fullPath;
        }

        return Path.GetFullPath(candidates[0]);
    }
}