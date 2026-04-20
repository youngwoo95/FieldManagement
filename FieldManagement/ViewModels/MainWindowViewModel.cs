using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Services;

namespace FieldManagement.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
  private readonly ThemeService _themeService;

    private BaseViewModel? _currentViewModel;
    public BaseViewModel? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    private MenuType _selectedMenu;
    public MenuType SelectedMenu
    {
        get => _selectedMenu;
        set
        {
            if (_selectedMenu == value)
                return;

            _selectedMenu = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsHomeSelected));
            OnPropertyChanged(nameof(IsInputSelected));
            OnPropertyChanged(nameof(IsWorkersSelected));
            OnPropertyChanged(nameof(IsDataSelected));
            OnPropertyChanged(nameof(IsFacilitySelected));
            OnPropertyChanged(nameof(IsCustomerSelected));
            ChangeView(value);
        }
    }

    private bool _isDarkTheme;
    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set
        {
            if (_isDarkTheme == value)
                return;

            _isDarkTheme = value;
            OnPropertyChanged();
        }
    }

    public bool IsHomeSelected
    {
        get => SelectedMenu == MenuType.Home;
        set
        {
            if (value)
                SelectedMenu = MenuType.Home;
        }
    }

    public bool IsInputSelected
    {
        get => SelectedMenu == MenuType.Input;
        set
        {
            if (value)
                SelectedMenu = MenuType.Input;
        }
    }

    public bool IsWorkersSelected
    {
        get => SelectedMenu == MenuType.Workers;
        set
        {
            if (value)
                SelectedMenu = MenuType.Workers;
        }
    }

    public bool IsDataSelected
    {
        get => SelectedMenu == MenuType.Data;
        set
        {
            if (value)
                SelectedMenu = MenuType.Data;
        }
    }

    public bool IsFacilitySelected
    {
        get => SelectedMenu == MenuType.Facility;
        set
        {
            if (value)
                SelectedMenu = MenuType.Facility;
        }
    }

    public bool IsCustomerSelected
    {
        get => SelectedMenu == MenuType.Customer;
        set
        {
            if(value)
                SelectedMenu = MenuType.Customer;
        }
    }

    public ICommand ToggleThemeCommand { get; }

    public MainWindowViewModel()
    {
        _themeService = new ThemeService();

        _isDarkTheme = _themeService.IsDarkThemeActive();
        ToggleThemeCommand = new RelayCommand(_ => ToggleTheme());

        _selectedMenu = MenuType.Home;
        ChangeView(MenuType.Home);
    }

    private void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
        _themeService.ApplyTheme(IsDarkTheme);
    }

    private void ChangeView(MenuType menu)
    {
        CurrentViewModel = menu switch
        {
            MenuType.Home => new MainBoardViewModel(),
            MenuType.Input => new InputViewModel(),
            MenuType.Workers => new WorkerViewModel(),
            MenuType.Data => new DataViewModel(),
            MenuType.Facility => new FacilityViewModel(),
            MenuType.Customer => new CustomerViewModel(),
            _ => new MainBoardViewModel()
        };
    }
}