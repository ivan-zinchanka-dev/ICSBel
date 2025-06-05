using ICSBel.Domain.Database;
using ICSBel.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ICSBel.Tests;

[TestFixture]
public class EmployeeDatabaseContextTests
{
    private static ILogger<T> CreateLogger<T>() =>
        LoggerFactory.Create(b => b.AddConsole()).CreateLogger<T>();
    
    [Test]
    public async Task PrintAllEmployees()
    {
        await using (var databaseContext = new EmployeeDatabaseContext(CreateLogger<EmployeeDatabaseContext>()))
        {
            IEnumerable<Employee> employees = await databaseContext.GetAllEmployeesAsync();

            foreach (Employee employee in employees)
            {
                Console.WriteLine(JsonConvert.SerializeObject(employee));
            }
        }
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task PrintFilteredEmployees(int positionId)
    {
        await using (var databaseContext = new EmployeeDatabaseContext(CreateLogger<EmployeeDatabaseContext>()))
        {
            IEnumerable<Employee> employees = await databaseContext.GetFilteredEmployeesAsync(positionId);

            foreach (Employee employee in employees)
            {
                Console.WriteLine(JsonConvert.SerializeObject(employee));
            }
        }
    }

    [Test]
    public async Task AddEmployeeAsync()
    {
        await using (var databaseContext = new EmployeeDatabaseContext(CreateLogger<EmployeeDatabaseContext>()))
        {
            bool result = await databaseContext.AddEmployeeAsync(
                new EmployeeInputData("Иван", "Зинченко", 1, 2000, 1500));
            
            Assert.That(result, Is.True);
        }
    }
    
    [Test]
    public async Task RemoveEmployeesAsync()
    {
        var employeeIds = new int[]{ 7, 8};
        
        await using (var databaseContext = new EmployeeDatabaseContext(CreateLogger<EmployeeDatabaseContext>()))
        {
            bool result = await databaseContext.RemoveEmployeesAsync(employeeIds);
            
            Assert.That(result, Is.True);
        }
    }

}