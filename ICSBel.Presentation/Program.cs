using ICSBel.Domain.Services;
using ICSBel.Presentation.Factories;
using ICSBel.Presentation.Reporting;
using ICSBel.Presentation.ViewModels;
using ICSBel.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ICSBel.Presentation;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
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
    }
}