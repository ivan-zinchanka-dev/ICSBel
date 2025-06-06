namespace ICSBel.Domain.Models;

public class PositionSalary
{
    public Position Position { get; private set; }
    public decimal AverageSalary { get; private set; }
    
    public PositionSalary(Position position, decimal averageSalary)
    {
        Position = position;
        AverageSalary = averageSalary;
    }
}