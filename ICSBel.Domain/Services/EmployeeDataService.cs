namespace ICSBel.Domain.Services;

public class EmployeeDataService
{
    public IEmployeeRepository GetEmployeeRepository()
    {
        return new MockEmployeeRepository();
    }
}