using ICSBel.Domain.Models;

namespace ICSBel.Domain.Services.Abstractions;

public interface IReportService
{
    Task<IEnumerable<PositionSalary>> GetPositionSalaryReportAsync();
}