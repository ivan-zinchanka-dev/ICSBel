using ICSBel.Domain.Models;

namespace ICSBel.Domain.Services;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAllEmployees();
    IEnumerable<Position> GetAllPositions();
    
    bool CreateEmployee(EmployeeInputData newEmployeeData);
    
}