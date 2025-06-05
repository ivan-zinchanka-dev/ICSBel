using System.Data;
using ICSBel.Domain.Models;
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
        public const string SelectAllPositions = "SELECT * FROM [Positions]";
        public const string SelectAllEmployees = "SELECT * FROM [Employees]";
        public const string SelectFilteredEmployees = "SELECT * FROM [Employees] WHERE [Employees].PositionId = @PositionId";
        public const string InsertEmployee = "INSERT INTO [Employees] (FirstName, LastName, PositionId, BirthYear, Salary) VALUES (@FirstName, @LastName, @PositionId, @BirthYear, @Salary)";
        public const string DeleteEmployees = "DELETE FROM [Employees] WHERE [Id] IN ({0})";
    }
    
    private readonly ILogger<EmployeeDatabaseContext> _logger;
    private readonly SqlConnection _connection;
    private Dictionary<int, Position> _cachedPositions;
    
    public EmployeeDatabaseContext(ILogger<EmployeeDatabaseContext> logger)
    {
        _logger = logger;
        _connection = new SqlConnection("Server=localhost;Database=ics.employees;Trusted_Connection=True;TrustServerCertificate=True;");
    }

   
    

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        await ConnectToDatabaseAsync();
        await InitializePositionsIfNeedAsync();
        
        try
        {
            var command = new SqlCommand(QueryStrings.SelectAllEmployees, _connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            
            var employees = new List<Employee>();
            const string positionIdParam = "PositionId";
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var employee = new Employee(
                        reader.GetInt32(nameof(Employee.Id)),
                        reader.GetString(nameof(Employee.FirstName)),
                        reader.GetString(nameof(Employee.LastName)),
                        GetPositionById(reader.GetInt32(positionIdParam)),
                        reader.GetInt32(nameof(Employee.BirthYear)),
                        reader.GetDecimal(nameof(Employee.Salary)));
                    
                    employees.Add(employee);
                }
            }

            await reader.CloseAsync();
            return employees;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex.Message);
            return new List<Employee>();
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
    
    private async Task ConnectToDatabaseAsync()
    {
        try
        {
            if (_connection.State == ConnectionState.Open)
            {
                _logger.LogInformation("The client connection is already opened.");
            }
            else
            {
                await _connection.OpenAsync();
                _logger.LogInformation("The client has successfully connected to the database.");
            }
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    private async Task InitializePositionsIfNeedAsync()
    {
        if (_cachedPositions == null)
        {
            await InitializePositionsAsync();
        }
    }

    private async Task InitializePositionsAsync()
    {
        _cachedPositions = new Dictionary<int, Position>();
        
        try
        {
            var command = new SqlCommand(QueryStrings.SelectAllPositions, _connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var position = new Position(
                        reader.GetInt32(nameof(Position.Id)),
                        reader.GetString(nameof(Position.Name)));
                    
                    _cachedPositions.Add(position.Id, position);
                }
            }

            await reader.CloseAsync();
      
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex.Message);
        }
    }
    
    private Position GetPositionById(int positionId)
    {
        return _cachedPositions.GetValueOrDefault(positionId);
    }
}