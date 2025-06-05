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
}