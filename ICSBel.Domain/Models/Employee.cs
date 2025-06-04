namespace ICSBel.Domain.Models;

public class Employee
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Position Position { get; private set; }
    public int BirthYear { get; private set; }
    public decimal Salary { get; private set; }

    public Employee(int id, string firstName, string lastName, Position position, int birthYear, decimal salary)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Position = position;
        BirthYear = birthYear;
        Salary = salary;
    }
}