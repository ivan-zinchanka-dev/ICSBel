using ICSBel.Domain.Models;

namespace ICSBel.Domain.Services.Abstractions;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(int positionId);
    Task<bool> AddEmployeeAsync(EmployeeInputData employeeInputData);
    Task<bool> RemoveEmployeeAsync(int employeeId);
    Task<bool> RemoveEmployeesAsync(int[] employeeIds);
}