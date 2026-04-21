using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FieldManagement.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly ThemeService _themeService;
    private readonly IServiceProvider _serviceProvider;
    public ICommand ToggleThemeCommand { get; }

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


    public MainWindowViewModel(ThemeService themeService, IServiceProvider serviceProvider)
    {
        _themeService = themeService;
        _serviceProvider = serviceProvider;

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
            MenuType.Home => _serviceProvider.GetRequiredService<MainBoardViewModel>(),
            MenuType.Input => _serviceProvider.GetRequiredService<InputViewModel>(),
            MenuType.Workers => _serviceProvider.GetRequiredService<OrderViewModel>(),
            MenuType.Data => _serviceProvider.GetRequiredService<WorkStatusViewModel>(),
            MenuType.Facility => _serviceProvider.GetRequiredService<FacilityViewModel>(),
            MenuType.Customer => _serviceProvider.GetRequiredService<CustomerViewModel>(),
            _ => _serviceProvider.GetRequiredService<MainBoardViewModel>()
        };
    }
}
