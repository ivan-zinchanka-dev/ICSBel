using ICSBel.Domain.Models;

namespace ICSBel.Domain.API.Abstractions;

public interface IReportService
{
    Task<IEnumerable<PositionSalary>> GetPositionSalaryReportAsync();
}