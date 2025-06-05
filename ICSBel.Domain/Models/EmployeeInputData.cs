namespace ICSBel.Domain.Models;

public struct EmployeeInputData
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int PositionId { get; private set; }
    public int BirthYear { get; private set; }
    public decimal Salary { get; private set; }

    public EmployeeInputData(string firstName, string lastName, int positionId, int birthYear, decimal salary)
    {
        FirstName = firstName;
        LastName = lastName;
        PositionId = positionId;
        BirthYear = birthYear;
        Salary = salary;
    }
}