using ICSBel.Domain.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ICSBel.Domain.Services;

public class EmployeeDataService
{
    private IServiceProvider _serviceProvider;
    

    public EmployeeDataService()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure =>
        {
            configure.AddConsole();
            configure.SetMinimumLevel(LogLevel.Debug);
        });

        services.AddSingleton<EmployeeDatabaseContext>();
    }

    public IEmployeeRepository GetEmployeeRepository()
    {
        return new MockEmployeeRepository();
    }
}