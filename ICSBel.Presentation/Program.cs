using System.Runtime.InteropServices;
using ICSBel.Domain.API;
using ICSBel.Presentation.ErrorHandling;
using ICSBel.Presentation.Factories;
using ICSBel.Presentation.Reporting;
using ICSBel.Presentation.ViewModels;
using ICSBel.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ICSBel.Presentation;

public static class Program
{
    [DllImport("user32.dll")]
    private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

    private const int DpiAwarenessContextPerMonitorAwareV2 = -4;
    
    [STAThread]
    public static void Main()
    {
        SetProcessDpiAwarenessContext(DpiAwarenessContextPerMonitorAwareV2);
        Application.EnableVisualStyles();
        
        var services = new ServiceCollection();
        services.ConfigureServices();
        
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var mainView = serviceProvider.GetRequiredService<ExploreEmployeesView>();
        
        Application.Run(mainView);
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddLogging(configure =>
        {
            configure.AddConsole();
            configure.SetMinimumLevel(LogLevel.Debug);
        });
        
        services.AddSingleton<EmployeeDataService>();
        services.AddSingleton<ViewFactory>();
        services.AddSingleton<ReportService>();
        services.AddTransient<ExploreEmployeesViewModel>();
        services.AddTransient<ExploreEmployeesView>();
        services.AddTransient<NewEmployeeViewModel>();
        services.AddTransient<NewEmployeeView>();
        services.AddSingleton<ErrorReporter>();
    }
}