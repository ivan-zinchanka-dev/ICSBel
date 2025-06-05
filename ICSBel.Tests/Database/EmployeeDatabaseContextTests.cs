using ICSBel.Domain.Database;
using ICSBel.Domain.Models;
using ICSBel.Tests.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ICSBel.Tests.Database;

[TestFixture]
public class EmployeeDatabaseContextTests
{
    private ILogger<EmployeeDatabaseContext> _logger;
    private IOptions<EmployeeDatabaseSettings> _options;

    private EmployeeDatabaseContext _databaseContext;

    [OneTimeSetUp]
    public void SetUpFixture()
    {
        _logger = LoggingUtility.CreateLogger<EmployeeDatabaseContext>();
        var settings = new EmployeeDatabaseSettings()
        {
            ConnectionString = 
                "Server=localhost;Database=ics.employees;Trusted_Connection=True;TrustServerCertificate=True;",
        };
        
        _options = Options.Create(settings);
    }

    [SetUp]
    public void SetUpTest()
    {
        _databaseContext = new EmployeeDatabaseContext(_options, _logger);
    }

    [Test]
    public async Task PrintAllEmployees()
    {
        IEnumerable<Employee> employees = await _databaseContext.GetAllEmployeesAsync();

        foreach (Employee employee in employees)
        {
            Console.WriteLine(JsonConvert.SerializeObject(employee));
        }
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task PrintFilteredEmployees(int positionId)
    {
        IEnumerable<Employee> employees = await _databaseContext.GetFilteredEmployeesAsync(positionId);

        foreach (Employee employee in employees)
        {
            Console.WriteLine(JsonConvert.SerializeObject(employee));
        }
    }

    [Test]
    public async Task AddEmployeeAsync()
    {
        bool result = await _databaseContext.AddEmployeeAsync(
            new EmployeeInputData("Иван", "Зинченко", 1, 2000, 1500));
            
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task RemoveEmployeesAsync()
    {
        var employeeIds = new int[]{ 7, 8};
        bool result = await _databaseContext.RemoveEmployeesAsync(employeeIds);
        Assert.That(result, Is.True);
    }
    
    [TearDown]
    public async Task CleanUpTestAsync()
    {
        if (_databaseContext != null)
        {
            await _databaseContext.DisposeAsync();
        }
    }
}