using System.Collections.ObjectModel;
using System.ComponentModel;
using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.WorkStatusModel;

public partial class WorkStatusViewModel
{
    private ObservableCollection<WorkStatusViewItems> _workstatus = new();
    private ICollectionView _filteredWorkStatus = null!;
    
    private string _searchKeyword = string.Empty;

    public ObservableCollection<WorkStatusViewItems> WorkStatus => _workstatus;

    public ICollectionView FilteredWorkStatus => _filteredWorkStatus;

    private OrderViewItems? _selectedWorkStatus;
    private string? _selectedPdfPath;
    private Uri _selectedPdfUri = BlankPdfUri;

    private bool _isPdfFallbackVisible;
    private string _pdfFallbackMessage = "PDF preview is unavailable.";
    private double _pdfPanelWidth;
    
    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            if (_searchKeyword == value)
            {
                return;
            }

            _searchKeyword = value;
            OnPropertyChanged();
            _filteredWorkStatus.Refresh();
        }
    }

    public bool IsPdfFallbackVisible
    {
        get => _isPdfFallbackVisible;
        private set
        {
            _isPdfFallbackVisible = value;
            OnPropertyChanged();
        }
    }
    
    public OrderViewItems? SelectedWorkStatus
    {
        get => _selectedWorkStatus;
        set
        {
            _selectedWorkStatus = value;
            OnPropertyChanged();

            if (value is null)
            {
                IsPdfPanelOpen = false;
                SelectedPdfPath = null;
                PdfPanelWidth = 0;
                return;
            }

            IsPdfPanelOpen = true;
            if (PdfPanelWidth <= 0)
            {
                PdfPanelWidth = 800;
            }
            SelectedPdfPath = ResolvePdfPath(value.PdfFileName);
            
            
        }
    }

    public string? SelectedPdfPath
    {
        get => _selectedPdfPath;
        set
        {
            _selectedPdfPath = value;
            OnPropertyChanged();
            UpdatePdfPreviewState(value);
        }
    }

    public Uri SelectedPdfUri
    {
        get => _selectedPdfUri;
        private set
        {
            _selectedPdfUri = value;
            OnPropertyChanged();
        }
    }

    public string PdfFallbackMessage
    {
        get => _pdfFallbackMessage;
        private set
        {
            _pdfFallbackMessage = value;
            OnPropertyChanged();
        }
    }

    public double PdfPanelWidth
    {
        get => _pdfPanelWidth;
        set
        {
            _pdfPanelWidth = value;
            OnPropertyChanged();
        }
    }

    private bool _isPdfPanelOpen;

    public bool IsPdfPanelOpen
    {
        get => _isPdfPanelOpen;
        set
        {
            _isPdfPanelOpen = value;
            OnPropertyChanged();
        }
    }
    
}