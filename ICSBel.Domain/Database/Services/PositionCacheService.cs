using System.Data;
using ICSBel.Domain.Database.Strings;
using ICSBel.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ICSBel.Domain.Database.Services;

internal class PositionCacheService
{
    private readonly ILogger<PositionCacheService> _logger;
    
    private Dictionary<int, Position> _cachedPositions;

    public PositionCacheService(ILogger<PositionCacheService> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Position>> GetPositionsAsync(SqlConnection connection)
    {
        await InitializePositionsAsync(connection);
        return _cachedPositions.Values;
    }
    
    public Position GetPositionById(int positionId)
    {
        return _cachedPositions.GetValueOrDefault(positionId);
    }
    
    public async Task InitializePositionsAsync(SqlConnection connection)
    {
        if (_cachedPositions != null)
        {
            return;
        }
        
        _cachedPositions = new Dictionary<int, Position>();
        
        try
        {
            var command = new SqlCommand(QueryStrings.SelectAllPositions, connection);
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
}