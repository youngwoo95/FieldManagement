using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PlantManagement.Views;
using PlantManagement.Views.ViewModels.CustomerModel;
using PlantManagement.Views.ViewModels.CustomerModel.DialogViews;
using PlantManagement.Views.ViewModels.DashBoardModel;
using PlantManagement.Views.ViewModels.FacilityModel;
using PlantManagement.Views.ViewModels.FacilityModel.DialogViews;
using PlantManagement.Views.ViewModels.MainWindowModel;

namespace PlantManagement;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ServiceProvider Services { get; private set; } = null!;
    
    /// <summary>
    /// 시작 시 호출
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceCollection = new ServiceCollection();

        /* ================== DI ====================== */
        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddSingleton<ThemeService>();
        
        /* ================== DI ====================== */
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<DashBoardViewModel>();
        serviceCollection.AddSingleton<CustomerViewModel>();
        serviceCollection.AddSingleton<FacilityViewModel>();
        
        serviceCollection.AddSingleton<AddCustomerViewModel>();
        serviceCollection.AddSingleton<AddFacilityViewModel>();        
        
        serviceCollection.AddSingleton<ICustomerDialogService, CustomerDialogService>();
        serviceCollection.AddSingleton<IFacilityDialogService, FacilityDialogService>();
        
        Services = serviceCollection.BuildServiceProvider();
        
        /*
         * 시작 윈도우 설정
         */
        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = Services.GetRequiredService<MainWindowViewModel>();
        mainWindow.Show();
        
    }

    /// <summary>
    /// 종료 시 호출
    /// </summary>
    protected override void OnExit(ExitEventArgs e)
    {
        Services?.Dispose();
        base.OnExit(e);
    }
}
