using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ICSBel.Domain.Database.Services;

internal class DatabaseConnectionService
{
    private readonly ILogger<DatabaseConnectionService> _logger;
    private readonly IOptions<EmployeeDatabaseSettings> _options;
    private readonly SqlConnection _connection;
    
    public DatabaseConnectionService(
        ILogger<DatabaseConnectionService> logger, 
        IOptions<EmployeeDatabaseSettings> options)
    {
        _options = options;
        _logger = logger;
        _connection = new SqlConnection(_options.Value.ConnectionString);
    }
    
    public async Task<SqlConnection> ConnectToDatabaseAsync()
    {
        try
        {
            if (_connection.State == ConnectionState.Open)
            {
                _logger.LogInformation("Соединение с БД уже открыто");
            }
            else
            {
                await _connection.OpenAsync();
                _logger.LogInformation("Соединение с БД произведено успешно");
            }
            
            return _connection;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
    
    public async ValueTask CloseConnectionAsync()
    {
        if (_connection.State == ConnectionState.Open)
        {
            await _connection.CloseAsync();
            _logger.LogInformation("Приложение отключено от БД");
        }
    }
}