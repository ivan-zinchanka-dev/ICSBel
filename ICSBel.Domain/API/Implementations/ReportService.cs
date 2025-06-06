using ICSBel.Domain.API.Abstractions;
using ICSBel.Domain.Database;
using ICSBel.Domain.Models;

namespace ICSBel.Domain.API.Implementations;

internal class ReportService : IReportService
{
    private readonly EmployeeDatabaseContextFactory _databaseContextFactory;

    public ReportService(EmployeeDatabaseContextFactory databaseContextFactory)
    {
        _databaseContextFactory = databaseContextFactory;
    }
    
    public async Task<IEnumerable<PositionSalary>> GetPositionSalaryReportAsync()
    {
        await using var databaseContext = _databaseContextFactory.CreateContext();
        return await databaseContext.GetPositionSalaryReportAsync();
    }
}