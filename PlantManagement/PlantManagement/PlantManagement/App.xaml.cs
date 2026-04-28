using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlantManagement.Comm.Logger;
using PlantManagement.Commons.Repository;
using PlantManagement.Repository.v1.Customer;
using PlantManagement.Repository.v1.Facility;
using PlantManagement.Repository.v1.Notice;
using PlantManagement.Repository.v1.Orders;
using PlantManagement.Service.v1.Customer;
using PlantManagement.Service.v1.Facility;
using PlantManagement.Service.v1.Notice;
using PlantManagement.Service.v1.Orders;
using PlantManagement.Views;
using PlantManagement.Views.ViewModels.CustomerModel;
using PlantManagement.Views.ViewModels.CustomerModel.DialogViews;
using PlantManagement.Views.ViewModels.DashBoardModel;
using PlantManagement.Views.ViewModels.EquipmentStatusModel;
using PlantManagement.Views.ViewModels.EquipmentStatusModel.Dialog;
using PlantManagement.Views.ViewModels.FacilityModel;
using PlantManagement.Views.ViewModels.FacilityModel.DialogViews;
using PlantManagement.Views.ViewModels.MainWindowModel;
using PlantManagement.Views.ViewModels.OrderModel;
using PlantManagement.Views.ViewModels.OrderModel.Dialog;
using PlantManagement.Views.ViewModels.WorkStatusModel;
using PlantManagement.Views.ViewModels.WorkStatusModel.Dialog;
using PlantManagement.Views.Views;

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
        serviceCollection.AddTransient<DashBoardViewModel>();
        serviceCollection.AddTransient<CustomerViewModel>();
        serviceCollection.AddTransient<FacilityViewModel>();
        serviceCollection.AddSingleton<OrderViewModel>();
        serviceCollection.AddSingleton<WorkStatusViewModel>();
        serviceCollection.AddSingleton<IEquipmentDataService, EquipmentDataService>();
        serviceCollection.AddSingleton<IEquipmentEditDialogService, EquipmentEditDialogService>();
        serviceCollection.AddSingleton<EquipmentStatusViewModel>();
        serviceCollection.AddSingleton<ILogService, LogService>();
        
        serviceCollection.AddTransient<AddCustomerViewModel>();
        serviceCollection.AddTransient<AddFacilityViewModel>();        
        serviceCollection.AddTransient<AddOrderViewModel>();
        serviceCollection.AddSingleton<AddWorkStatusViewModel>();
        
        
        serviceCollection.AddTransient<ICustomerDialogService, CustomerDialogService>();
        serviceCollection.AddTransient<IFacilityDialogService, FacilityDialogService>();
        serviceCollection.AddTransient<IOrderDialogService, OrderDialogService>();
        serviceCollection.AddSingleton<IWorkDialogService, WorkDialogService>();
        
        serviceCollection.AddTransient<PlantContext>();
        serviceCollection.AddTransient<INoticeRepository, NoticeRepository>();
        serviceCollection.AddTransient<ICustomerRepository, CustomerRepository>();
        serviceCollection.AddTransient<IFacilityRepository, FacilityRepository>();
        serviceCollection.AddTransient<IOrderRepository, OrderRepository>();
        
        serviceCollection.AddTransient<INoticeService, NoticeService>();
        serviceCollection.AddTransient<ICustomerService, CustomerService>();
        serviceCollection.AddTransient<IFacilityService, FacilityService>();
        serviceCollection.AddTransient<IOrderService, OrderService>();
        
        serviceCollection.AddTransient<IDbConnection>(sp =>
            sp.GetRequiredService<PlantContext>().Database.GetDbConnection());
        
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
