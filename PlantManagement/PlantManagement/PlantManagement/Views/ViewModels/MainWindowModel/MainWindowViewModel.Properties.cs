using PlantManagement.Comm;

namespace PlantManagement.Views.ViewModels.MainWindowModel;

public partial class MainWindowViewModel
{
    private BaseViewModel? _currentViewModel;
    
    private bool _isDarkTheme = true;
    private MenuType _selectMenu;
    private bool _isDashBoardSelected; // 대시보드 화면 선택
    private bool _isEquipmentStatusSelected; // 설비현황판 화면 선택
    private bool _isWorkManagement; // 작업관리 화면 선택
    private bool _isOrderManagement; // 수주관리 화면 선택
    private bool _isFacilityManagement; // 설비관리 화면 선택
    private bool _isCustomerManagement; // 고객사관리 화면 선택

    public BaseViewModel? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;   
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 다크모드 선택
    /// </summary>
    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set
        {
            _isDarkTheme = value;
            OnPropertyChanged();
        } 
    }

    /// <summary>
    /// 대시보드 화면 선택
    /// </summary>
    public bool IsDashBoardSelected
    {
        get => _isDashBoardSelected;
        set
        {
            if (_isDashBoardSelected == value) return;
            _isDashBoardSelected = value;
            OnPropertyChanged();

            if (!value) return;

            if (_isCustomerManagement)
            {
                _isCustomerManagement = false;
                OnPropertyChanged(nameof(IsCustomerManagement));
            }

            SelectMenu = MenuType.DashBoard;
            ChangeView(MenuType.DashBoard);
        }
    }

    /// <summary>
    /// 설비현황판 화면 선택
    /// </summary>
    public bool IsEquipmentStatusSelected
    {
        get => _isEquipmentStatusSelected;
        set
        {
            _isEquipmentStatusSelected = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 작업관리 화면 선택
    /// </summary>
    public bool IsWorkManagement
    {
        get => _isWorkManagement;
        set
        {
            if (_isWorkManagement == value)
                return;

            _isWorkManagement = value;
            OnPropertyChanged();

            if (!value) return;

            if (_isDashBoardSelected)
            {
                _isDashBoardSelected = false;
                OnPropertyChanged(nameof(IsDashBoardSelected));
            }

            SelectMenu = MenuType.WorkManagement;
            ChangeView(MenuType.WorkManagement);
        }
    }

    /// <summary>
    /// 수주관리 화면 선택
    /// </summary>
    public bool IsOrderManagement
    {
        get => _isOrderManagement;
        set
        {
            if (_isOrderManagement == value) return;
            _isOrderManagement = value;
            OnPropertyChanged();

            if (!value) return;

            if (_isDashBoardSelected)
            {
                _isDashBoardSelected = false;
                OnPropertyChanged(nameof(IsDashBoardSelected));
            }

            SelectMenu = MenuType.OrderManagement;
            ChangeView(MenuType.OrderManagement);
        }
    }

    /// <summary>
    /// 설비관리 화면 선택
    /// </summary>
    public bool IsFacilityManagement
    {
        get => _isFacilityManagement;
        set
        {
            _isFacilityManagement = value; 
            OnPropertyChanged();
            
            if (!value) return;

            if (_isDashBoardSelected)
            {
                _isDashBoardSelected = false;
                OnPropertyChanged(nameof(IsDashBoardSelected));
            }

            SelectMenu = MenuType.FacilityManagement;
            ChangeView(MenuType.FacilityManagement);
        }
    }

    /// <summary>
    /// 고객사관리 화면 선택
    /// </summary>
    public bool IsCustomerManagement
    {
        get => _isCustomerManagement;
        set
        {
            if (_isCustomerManagement == value) return;
            _isCustomerManagement = value; 
            OnPropertyChanged();

            if (!value) return;

            if (_isDashBoardSelected)
            {
                _isDashBoardSelected = false;
                OnPropertyChanged(nameof(IsDashBoardSelected));
            }

            SelectMenu = MenuType.CustomerManagement;
            ChangeView(MenuType.CustomerManagement);
        }
    }

    public MenuType SelectMenu
    {
        get => _selectMenu;
        set
        {
            _selectMenu = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsDashBoardSelected));
            OnPropertyChanged(nameof(IsEquipmentStatusSelected));
            OnPropertyChanged(nameof(IsWorkManagement));
            OnPropertyChanged(nameof(IsOrderManagement));
            OnPropertyChanged(nameof(IsFacilityManagement));
            OnPropertyChanged(nameof(IsCustomerManagement));
            
        }
    }
    
    
}
