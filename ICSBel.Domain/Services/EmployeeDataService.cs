using ICSBel.Domain.Database;
using ICSBel.Domain.Services.Abstractions;
using ICSBel.Domain.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ICSBel.Domain.Services;

public class EmployeeDataService
{
    private readonly IServiceProvider _serviceProvider;
    
    public IPositionRepository PositionRepository => _serviceProvider.GetRequiredService<IPositionRepository>();
    public IEmployeeRepository EmployeeRepository => _serviceProvider.GetRequiredService<IEmployeeRepository>();
    
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
        
        services.AddTransient<EmployeeDatabaseContext>();
        services.AddSingleton<EmployeeDatabaseContextFactory>();
        services.AddSingleton<IPositionRepository, PositionRepository>();
        services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
    }
}