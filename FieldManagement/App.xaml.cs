using System.Windows;
using FieldManagement.Repository.Customer;
using FieldManagement.Services;
using FieldManagement.View;
using FieldManagement.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FieldManagement;

public partial class App : Application
{
    public static ServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<ICustomerRepository, CustomerRepository>();
        serviceCollection.AddSingleton<ICustomerDialogService, CustomerDialogService>();
        serviceCollection.AddSingleton<IOrderDialogService, OrderDialogService>();
        serviceCollection.AddSingleton<IFacilityDialogService, FacilityDialogService>();
        serviceCollection.AddSingleton<IFloorEditDialogService, FloorEditDialogService>();
        serviceCollection.AddSingleton<IWorkStatusDialogService, WorkStatusDialogService>();
        serviceCollection.AddSingleton<ThemeService>();

        serviceCollection.AddSingleton<MainBoardViewModel>();
        serviceCollection.AddSingleton<InputViewModel>();
        serviceCollection.AddSingleton<OrderViewModel>();
        serviceCollection.AddSingleton<WorkStatusViewModel>();
        serviceCollection.AddSingleton<FacilityViewModel>();
        serviceCollection.AddSingleton<CustomerViewModel>();
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<MainWindow>();

        Services = serviceCollection.BuildServiceProvider();

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = Services.GetRequiredService<MainWindowViewModel>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Services?.Dispose();
        base.OnExit(e);
    }
}
