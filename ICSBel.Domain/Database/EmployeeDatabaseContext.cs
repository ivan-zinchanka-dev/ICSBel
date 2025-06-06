using ICSBel.Domain.Database.Services;
using ICSBel.Domain.Models;
using Microsoft.Data.SqlClient;

namespace ICSBel.Domain.Database;

internal class EmployeeDatabaseContext : IAsyncDisposable
{
    private readonly DatabaseConnectionService _connectionService;
    private readonly PositionCacheService _positionCacheService;
    private readonly EmployeeQueryService _employeeQueryService;
    private readonly ReportQueryService _reportQueryService;
    
    public EmployeeDatabaseContext(
        DatabaseConnectionService connectionService, 
        EmployeeQueryService employeeQueryService, 
        PositionCacheService positionCacheService, 
        ReportQueryService reportQueryService)
    {
        _connectionService = connectionService;
        _employeeQueryService = employeeQueryService;
        _positionCacheService = positionCacheService;
        _reportQueryService = reportQueryService;
    }
    
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _employeeQueryService.GetEmployeesAsync(await GetConnection());
    }

    public async Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(int positionId)
    {
        return await _employeeQueryService.GetEmployeesAsync(await GetConnection(), positionId);
    }

    public async Task<bool> AddEmployeeAsync(EmployeeInputData employeeInputData)
    {
        return await _employeeQueryService.AddEmployeeAsync(await GetConnection(), employeeInputData);
    }

    public async Task<bool> RemoveEmployeesAsync(int[] employeeIds)
    {
        return await _employeeQueryService.RemoveEmployeesAsync(await GetConnection(), employeeIds);
    }

    public async Task<IEnumerable<Position>> GetPositionsAsync()
    {
        return await _positionCacheService.GetPositionsAsync(await GetConnection());
    }

    public async Task<IEnumerable<PositionSalary>> GetPositionSalaryReportAsync()
    {
        return await _reportQueryService.GetPositionSalaryReportAsync(await GetConnection());
    }

    public async ValueTask DisposeAsync()
    {
        await _connectionService.CloseConnectionAsync();
    }

    private async Task<SqlConnection> GetConnection()
    {
        return await _connectionService.ConnectToDatabaseAsync();
    }
}