using ICSBel.Domain.Models;

namespace ICSBel.Domain.API.Abstractions;

public interface IPositionRepository
{
    Task<IEnumerable<Position>> GetPositionsAsync();
}