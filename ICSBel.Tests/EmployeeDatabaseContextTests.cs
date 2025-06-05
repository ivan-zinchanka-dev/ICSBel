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
}