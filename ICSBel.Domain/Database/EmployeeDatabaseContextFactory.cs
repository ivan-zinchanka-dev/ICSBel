using Microsoft.Extensions.DependencyInjection;

namespace ICSBel.Domain.Database;

internal class EmployeeDatabaseContextFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EmployeeDatabaseContextFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public EmployeeDatabaseContext CreateContext()
    {
        return _serviceProvider.GetRequiredService<EmployeeDatabaseContext>();
    }
}