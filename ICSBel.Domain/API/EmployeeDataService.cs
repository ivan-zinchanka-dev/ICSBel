﻿using ICSBel.Domain.API.Abstractions;
using ICSBel.Domain.API.Implementations;
using ICSBel.Domain.Database;
using ICSBel.Domain.Database.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ICSBel.Domain.API;

public class EmployeeDataService
{
    private const string AppSettingsFileName = "appsettings.json";
    private readonly IServiceProvider _serviceProvider;
    
    public IPositionRepository PositionRepository => _serviceProvider.GetRequiredService<IPositionRepository>();
    public IEmployeeRepository EmployeeRepository => _serviceProvider.GetRequiredService<IEmployeeRepository>();
    public IReportService ReportService => _serviceProvider.GetRequiredService<IReportService>();
    
    public EmployeeDataService()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    private void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(AppSettingsFileName)
            .Build();
        
        services.AddLogging(configure =>
        {
            configure.AddConsole();
            configure.SetMinimumLevel(LogLevel.Debug);
        });

        services.Configure<EmployeeDatabaseSettings>(
            configuration.GetSection(nameof(EmployeeDatabaseSettings)));

        services.AddTransient<DatabaseConnectionService>();
        services.AddTransient<PositionCacheService>();
        services.AddTransient<EmployeeQueryService>();
        services.AddTransient<ReportQueryService>();
        
        services.AddTransient<EmployeeDatabaseContext>();
        services.AddSingleton<EmployeeDatabaseContextFactory>();
        
        services.AddSingleton<IPositionRepository, PositionRepository>();
        services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
        services.AddSingleton<IReportService, ReportService>();
    }
}