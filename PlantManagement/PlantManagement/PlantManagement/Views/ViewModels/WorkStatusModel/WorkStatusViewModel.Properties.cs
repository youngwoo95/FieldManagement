using System.Collections.ObjectModel;
using System.ComponentModel;
using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.WorkStatusModel;

public partial class WorkStatusViewModel
{
    private ObservableCollection<WorkStatusViewItems> _workstatus = new();
    private ICollectionView _filteredWorkStatus = null!;
    
    private string _searchKeyword = string.Empty;
    private string _machineKeyword = string.Empty;
    private string _customerKeyword = string.Empty;
    private DateTime? _startDateFilter;
    private DateTime? _endDateFilter;
    private string _selectedStatusFilter = string.Empty;

    public ObservableCollection<WorkStatusViewItems> WorkStatus => _workstatus;

    public ICollectionView FilteredWorkStatus => _filteredWorkStatus;

    private WorkStatusViewItems? _selectedWorkStatus;
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
        }
    }

    public string MachineKeyword
    {
        get => _machineKeyword;
        set
        {
            if (_machineKeyword == value)
            {
                return;
            }

            _machineKeyword = value;
            OnPropertyChanged();
        }
    }

    public string CustomerKeyword
    {
        get => _customerKeyword;
        set
        {
            if (_customerKeyword == value)
            {
                return;
            }

            _customerKeyword = value;
            OnPropertyChanged();
        }
    }

    public DateTime? StartDateFilter
    {
        get => _startDateFilter;
        set
        {
            if (_startDateFilter == value)
            {
                return;
            }

            _startDateFilter = value;
            OnPropertyChanged();
        }
    }

    public DateTime? EndDateFilter
    {
        get => _endDateFilter;
        set
        {
            if (_endDateFilter == value)
            {
                return;
            }

            _endDateFilter = value;
            OnPropertyChanged();
        }
    }

    public string SelectedStatusFilter
    {
        get => _selectedStatusFilter;
        set
        {
            if (_selectedStatusFilter == value)
            {
                return;
            }

            _selectedStatusFilter = value;
            OnPropertyChanged();
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
    
    public WorkStatusViewItems? SelectedWorkStatus
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
