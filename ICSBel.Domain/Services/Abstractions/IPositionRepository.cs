using ICSBel.Domain.Models;

namespace ICSBel.Domain.Services.Abstractions;

public interface IPositionRepository
{
    Task<IEnumerable<Position>> GetPositionsAsync();
}