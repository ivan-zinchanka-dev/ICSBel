using ICSBel.Domain.Services;
using ICSBel.Presentation.ViewModels;
using ICSBel.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ICSBel.Presentation;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        var services = new ServiceCollection();
        services.AddSingleton<EmployeeDataService>();
        services.AddTransient<ExploreEmployeesViewModel>();
        services.AddTransient<ExploreEmployeesView>();
        
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var mainView = serviceProvider.GetService<ExploreEmployeesView>();
        Application.Run(mainView);
    }
}