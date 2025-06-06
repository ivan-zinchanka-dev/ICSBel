using System.Data;
using ICSBel.Domain.Database.Strings;
using ICSBel.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ICSBel.Domain.Database.Services;

internal class ReportQueryService
{
    private readonly ILogger<ReportQueryService> _logger;
    private readonly PositionCacheService _positionCacheService;

    public ReportQueryService(
        ILogger<ReportQueryService> logger, 
        PositionCacheService positionCacheService)
    {
        _logger = logger;
        _positionCacheService = positionCacheService;
    }

    public async Task<IEnumerable<PositionSalary>> GetPositionSalaryReportAsync(SqlConnection connection)
    {
        await _positionCacheService.InitializePositionsAsync(connection);
        
        try
        {
            var command = new SqlCommand(QueryStrings.SelectPositionSalaries, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            
            var report = new List<PositionSalary>();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var entry = new PositionSalary(
                        _positionCacheService.GetPositionById(reader.GetInt32(ParamStrings.PositionId)), 
                        reader.GetDecimal(ParamStrings.AverageSalary));
                    
                    report.Add(entry);
                }
            }

            await reader.CloseAsync();
            return report;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex.Message);
            return new List<PositionSalary>();
        }
    }
}