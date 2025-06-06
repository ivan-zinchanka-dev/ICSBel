using System.Data;
using ICSBel.Domain.Database.Strings;
using ICSBel.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ICSBel.Domain.Database.Services;

internal class EmployeeQueryService
{
    private readonly ILogger<EmployeeQueryService> _logger;
    private readonly PositionCacheService _positionCacheService;

    public EmployeeQueryService(
        ILogger<EmployeeQueryService> logger, 
        PositionCacheService positionCacheService)
    {
        _logger = logger;
        _positionCacheService = positionCacheService;
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(SqlConnection connection, int? positionId = null)
    {
        await _positionCacheService.InitializePositionsAsync(connection);
        
        try
        {
            SqlCommand command;
            
            if (positionId.HasValue)
            {
                command = new SqlCommand(QueryStrings.SelectFilteredEmployees, connection);
                command.Parameters.Add(new SqlParameter(QueryParamStrings.PositionId, positionId));
            }
            else
            {
                command = new SqlCommand(QueryStrings.SelectAllEmployees, connection);
            }
            
            SqlDataReader reader = await command.ExecuteReaderAsync();
            
            var employees = new List<Employee>();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var employee = new Employee(
                        reader.GetInt32(nameof(Employee.Id)),
                        reader.GetString(nameof(Employee.FirstName)),
                        reader.GetString(nameof(Employee.LastName)),
                        _positionCacheService.GetPositionById(reader.GetInt32(ParamStrings.PositionId)),
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
    
    public async Task<bool> AddEmployeeAsync(SqlConnection connection, EmployeeInputData employeeInputData)
    {
        try
        {
            var command = new SqlCommand(QueryStrings.InsertEmployee, connection);
            command.Parameters.Add(new SqlParameter(QueryParamStrings.FirstName, employeeInputData.FirstName));
            command.Parameters.Add(new SqlParameter(QueryParamStrings.LastName, employeeInputData.LastName));
            command.Parameters.Add(new SqlParameter(QueryParamStrings.PositionId, employeeInputData.PositionId));
            command.Parameters.Add(new SqlParameter(QueryParamStrings.BirthYear, employeeInputData.BirthYear));
            command.Parameters.Add(new SqlParameter(QueryParamStrings.Salary, employeeInputData.Salary));
            
            int affectedRows = await command.ExecuteNonQueryAsync();
            return affectedRows > 0;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }
    
    public async Task<bool> RemoveEmployeesAsync(SqlConnection connection, int[] employeeIds)
    {
        if (employeeIds == null || employeeIds.Length == 0)
        {
            return false;
        }
        
        try
        {
            SqlParameter[] parameters = employeeIds
                .Select((id, index) => new SqlParameter(string.Format(QueryParamStrings.Id, index), id))
                .ToArray();

            IEnumerable<string> paramNames = parameters.Select(param => param.ParameterName);
            string inClause = string.Join(", ", paramNames);
            
            var command = new SqlCommand(string.Format(QueryStrings.DeleteEmployees, inClause), connection);
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
}