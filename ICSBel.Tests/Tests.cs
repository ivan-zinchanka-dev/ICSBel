using ICSBel.Domain.Database;
using ICSBel.Domain.Services;

namespace ICSBel.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public async Task Test1()
    {
        var a = new EmployeeDataService();

        await a.InitializeAsync();
    }
}