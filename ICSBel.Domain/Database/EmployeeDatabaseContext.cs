using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ICSBel.Domain.Database;

internal class EmployeeDatabaseContext : IAsyncDisposable
{
    private static class ParamStrings
    {
        public const string FirstName = "@FirstName";
        public const string LastName = "@LastName";
        public const string PositionId = "@PositionId";
        public const string BirthYear = "@BirthYear";
        public const string Salary = "@Salary";
    }
    
    private static class QueryStrings
    {
        public const string GetAllEmployees = "SELECT * FROM [Employees]";
        public const string GetFilteredEmployees = "SELECT * FROM [Employees] WHERE [Employees].PositionId = @PositionId";
        public const string InsertEmployee = "INSERT INTO [Employees] (FirstName, LastName, PositionId, BirthYear, Salary) VALUES (@FirstName, @LastName, @PositionId, @BirthYear, @Salary)";
        public const string DeleteEmployees = "DELETE FROM [Employees] WHERE [Id] IN ({0})";
    }
    
    private readonly ILogger<EmployeeDatabaseContext> _logger;
    private readonly SqlConnection _connection;

    public EmployeeDatabaseContext(ILogger<EmployeeDatabaseContext> logger)
    {
        _logger = logger;
        _connection = new SqlConnection("Server=localhost;Database=ics.employees;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    public async Task ConnectToDatabaseAsync()
    {
        try
        {
            await _connection.OpenAsync();
            _logger.LogInformation("The client has successfully connected to the database.");
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex.Message);
        }
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_connection.State == ConnectionState.Open)
        {
            await _connection.CloseAsync();
            _logger.LogInformation("The client has disconnected from the database.");
        }
    }
}