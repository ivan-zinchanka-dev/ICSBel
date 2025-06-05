using System.Data;
using ICSBel.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
    
    private readonly IOptions<EmployeeDatabaseSettings> _options;
    private readonly ILogger<EmployeeDatabaseContext> _logger;
    private readonly SqlConnection _connection;
    private Dictionary<int, Position> _cachedPositions;
    
    public EmployeeDatabaseContext(
        IOptions<EmployeeDatabaseSettings> options, 
        ILogger<EmployeeDatabaseContext> logger)
    {
        _options = options;
        _logger = logger;
        _connection = new SqlConnection(_options.Value.ConnectionString);
    }

    public async Task<IEnumerable<Position>> GetPositionsAsync()
    {
        await ConnectToDatabaseAsync();
        await InitializePositionsAsync();

        return _cachedPositions.Values;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await GetEmployeesAsync();
    }

    public async Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(int positionId)
    {
        return await GetEmployeesAsync(positionId);
    }

    public async Task<bool> AddEmployeeAsync(EmployeeInputData employeeInputData)
    {
        await ConnectToDatabaseAsync();
        
        try
        {
            var command = new SqlCommand(QueryStrings.InsertEmployee, _connection);
            command.Parameters.Add(new SqlParameter(ParamStrings.FirstName, employeeInputData.FirstName));
            command.Parameters.Add(new SqlParameter(ParamStrings.LastName, employeeInputData.LastName));
            command.Parameters.Add(new SqlParameter(ParamStrings.PositionId, employeeInputData.PositionId));
            command.Parameters.Add(new SqlParameter(ParamStrings.BirthYear, employeeInputData.BirthYear));
            command.Parameters.Add(new SqlParameter(ParamStrings.Salary, employeeInputData.Salary));
            
            int affectedRows = await command.ExecuteNonQueryAsync();
            return affectedRows > 0;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }
    
    public async Task<bool> RemoveEmployeesAsync(int[] employeeIds)
    {
        if (employeeIds == null || employeeIds.Length == 0)
        {
            return false;
        }
        
        await ConnectToDatabaseAsync();
        
        try
        {
            SqlParameter[] parameters = employeeIds
                .Select((id, index) => new SqlParameter($"@id{index}", id))
                .ToArray();

            IEnumerable<string> paramNames = parameters.Select(param => param.ParameterName);
            string inClause = string.Join(", ", paramNames);
            
            var command = new SqlCommand(string.Format(QueryStrings.DeleteEmployees, inClause), _connection);
            command.Parameters.AddRange(parameters);
            
            int affectedRows = await command.ExecuteNonQueryAsync();
            return affectedRows > 0;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }
    
    private async Task<IEnumerable<Employee>> GetEmployeesAsync(int? positionId = null)
    {
        await ConnectToDatabaseAsync();
        await InitializePositionsAsync();
        
        try
        {
            SqlCommand command;
            
            if (positionId.HasValue)
            {
                command = new SqlCommand(QueryStrings.SelectFilteredEmployees, _connection);
                command.Parameters.Add(new SqlParameter(ParamStrings.PositionId, positionId));
            }
            else
            {
                command = new SqlCommand(QueryStrings.SelectAllEmployees, _connection);
            }
            
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

    private async Task InitializePositionsAsync()
    {
        if (_cachedPositions != null)
        {
            return;
        }
        
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