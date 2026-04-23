using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PlantManagement.Comm;
using PlantManagement.Views.ViewModels.CustomerModel;
using PlantManagement.Views.ViewModels.DashBoardModel;
using PlantManagement.Views.ViewModels.FacilityModel;
using PlantManagement.Views.ViewModels.OrderModel;

namespace PlantManagement.Views.ViewModels.MainWindowModel;

public partial class MainWindowViewModel : BaseViewModel
{
    private readonly ThemeService _themeService;
    private readonly IServiceProvider _serviceProvider;

    public ICommand ToggleThemeCommand { get; }
    
    public MainWindowViewModel(ThemeService themeService, 
        IServiceProvider serviceProvider)
    {
        _themeService = themeService;
        _serviceProvider = serviceProvider;

        ToggleThemeCommand = new RelayCommand(_ => ToggleTheme());
        IsDashBoardSelected = true;
    }

    private void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
        _themeService.ApplyTheme(IsDarkTheme);
    }

    private void ChangeView(MenuType menuType)
    {
        CurrentViewModel = menuType switch
        {
            MenuType.DashBoard => _serviceProvider.GetRequiredService<DashBoardViewModel>(),
            MenuType.CustomerManagement => _serviceProvider.GetRequiredService<CustomerViewModel>(),
            MenuType.FacilityManagement => _serviceProvider.GetRequiredService<FacilityViewModel>(),
            MenuType.OrderManagement => _serviceProvider.GetRequiredService<OrderViewModel>()
        };
    }

}
