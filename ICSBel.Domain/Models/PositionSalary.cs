namespace ICSBel.Domain.Models;

public class PositionSalary
{
    public string Position { get; set; }
    public decimal AverageSalary { get; set; }

    public PositionSalary()
    {
        
    }
    
    public PositionSalary(string position, decimal averageSalary)
    {
        Position = position;
        AverageSalary = averageSalary;
    }
}