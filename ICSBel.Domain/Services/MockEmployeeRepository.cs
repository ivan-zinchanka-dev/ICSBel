using ICSBel.Domain.Models;

namespace ICSBel.Domain.Services;

internal class MockEmployeeRepository : IEmployeeRepository
{
    private static readonly List<Position> Positions = new List<Position>()
    {
        Position.All,
        new Position(1, "Разработчик"),
        new Position(2, "Тестировщик"),
        new Position(3, "Аналитик"),
        new Position(4, "HR"),
    };
        
    private static readonly List<Employee> Employees = new List<Employee>()
    {
        new Employee(1, "Иван", "Иванов", Positions[1], 1990, 2000m),
        new Employee(2, "Елена", "Сидорова", Positions[2], 1992, 1700m),
        new Employee(3, "Пётр", "Петров", Positions[3], 1985, 2200m),
        new Employee(4, "Анна", "Кузнецова", Positions[1], 1995, 2100m),
        new Employee(5, "Максим", "Смирнов", Positions[4], 1988, 1500m),
        new Employee(6, "Ольга", "Николаева", Positions[2], 1991, 1650m)
    };
        
    public IEnumerable<Employee> GetAllEmployees()
    {
        return Employees;
    }

    public IEnumerable<Position> GetAllPositions()
    {
        return Positions;
    }

    public bool CreateEmployee(EmployeeInputData newEmployeeData)
    {
        Employee newEmployee = Map(newEmployeeData);
        Employees.Add(newEmployee);
        return true;
    }

    private Employee Map(EmployeeInputData inputData)
    {
        return new Employee(
            Employees.Last().Id + 1,
            inputData.FirstName,
            inputData.LastName,
            Positions.Find(position => position.Id == inputData.PositionId),
            inputData.BirthYear,
            inputData.Salary);
    }
}