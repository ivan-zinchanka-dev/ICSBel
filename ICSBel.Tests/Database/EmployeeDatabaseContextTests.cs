using ICSBel.Domain.Database;
using ICSBel.Domain.Database.Services;
using ICSBel.Domain.Models;
using ICSBel.Tests.Utilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ICSBel.Tests.Database;

[TestFixture]
public class EmployeeDatabaseContextTests
{
    private IOptions<EmployeeDatabaseSettings> _options;
    private EmployeeDatabaseContext _databaseContext;

    [OneTimeSetUp]
    public void SetUpFixture()
    {
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
        var positionsCacheService = new PositionCacheService(LoggingUtility.CreateLogger<PositionCacheService>());
        _databaseContext = new EmployeeDatabaseContext(
            new DatabaseConnectionService(LoggingUtility.CreateLogger<DatabaseConnectionService>(), _options),
            new EmployeeQueryService(LoggingUtility.CreateLogger<EmployeeQueryService>(), positionsCacheService), 
            positionsCacheService, 
            new ReportQueryService(LoggingUtility.CreateLogger<ReportQueryService>(), positionsCacheService));
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
    
    [TearDown]
    public async Task CleanUpTestAsync()
    {
        if (_databaseContext != null)
        {
            await _databaseContext.DisposeAsync();
        }
    }
}