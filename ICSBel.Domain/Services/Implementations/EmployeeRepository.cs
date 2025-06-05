using ICSBel.Domain.Database;
using ICSBel.Domain.Models;
using ICSBel.Domain.Services.Abstractions;

namespace ICSBel.Domain.Services.Implementations;

internal class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDatabaseContextFactory _databaseContextFactory;

    public EmployeeRepository(EmployeeDatabaseContextFactory databaseContextFactory)
    {
        _databaseContextFactory = databaseContextFactory;
    }
    
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        await using var databaseContext = _databaseContextFactory.CreateContext();
        return await databaseContext.GetAllEmployeesAsync();
    }

    public async Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(int positionId)
    {
        await using var databaseContext = _databaseContextFactory.CreateContext();
        return await databaseContext.GetFilteredEmployeesAsync(positionId);
    }

    public async Task<bool> AddEmployeeAsync(EmployeeInputData employeeInputData)
    {
        await using var databaseContext = _databaseContextFactory.CreateContext();
        return await databaseContext.AddEmployeeAsync(employeeInputData);
    }

    public async Task<bool> RemoveEmployeeAsync(int employeeId)
    {
        return await RemoveEmployeesAsync(new[] { employeeId });
    }

    public async Task<bool> RemoveEmployeesAsync(int[] employeeIds)
    {
        await using var databaseContext = _databaseContextFactory.CreateContext();
        return await databaseContext.RemoveEmployeesAsync(employeeIds);
    }
}