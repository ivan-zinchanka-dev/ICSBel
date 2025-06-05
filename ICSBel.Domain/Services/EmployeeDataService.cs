using ICSBel.Domain.Database;
using Microsoft.Extensions.Configuration;
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
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        services.AddLogging(configure =>
        {
            configure.AddConsole();
            configure.SetMinimumLevel(LogLevel.Debug);
        });

        services.Configure<EmployeeDatabaseSettings>(
            configuration.GetSection(nameof(EmployeeDatabaseSettings)));
        
        services.AddSingleton<EmployeeDatabaseContext>();
    }

    public IEmployeeRepository GetEmployeeRepository()
    {
        return new MockEmployeeRepository();
    }
}