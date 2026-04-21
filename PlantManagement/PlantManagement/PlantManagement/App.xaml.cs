using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

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

        /* ================== DI ====================== */
        
        Services = serviceCollection.BuildServiceProvider();
        
        /*
         * 시작 윈도우 설정
         */
        var mainWindow = Services.GetRequiredService<MainWindow>();
        
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
