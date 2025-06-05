using ICSBel.Domain.Database;
using ICSBel.Domain.Models;

namespace ICSBel.Domain.Services;

internal class SqlServerEmployeeRepository : IEmployeeRepository
{
    public IEnumerable<Employee> GetAllEmployees()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Position> GetAllPositions()
    {
        throw new NotImplementedException();
    }

    public bool AddEmployee(EmployeeInputData newEmployeeData)
    {
        throw new NotImplementedException();
    }
}