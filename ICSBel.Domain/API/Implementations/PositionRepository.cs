using ICSBel.Domain.API.Abstractions;
using ICSBel.Domain.Database;
using ICSBel.Domain.Models;

namespace ICSBel.Domain.API.Implementations;

internal class PositionRepository : IPositionRepository
{
    private readonly EmployeeDatabaseContextFactory _databaseContextFactory;

    public PositionRepository(EmployeeDatabaseContextFactory databaseContextFactory)
    {
        _databaseContextFactory = databaseContextFactory;
    }

    public async Task<IEnumerable<Position>> GetPositionsAsync()
    {
        await using var databaseContext = _databaseContextFactory.CreateContext();
        return await databaseContext.GetPositionsAsync();
    }
}