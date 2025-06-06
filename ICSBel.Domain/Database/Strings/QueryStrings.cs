namespace ICSBel.Domain.Database.Strings;

internal static class QueryStrings
{
    public const string SelectAllPositions = "SELECT * FROM [Positions]";
    public const string SelectAllEmployees = "SELECT * FROM [Employees]";
    public const string SelectFilteredEmployees = "SELECT * FROM [Employees] WHERE [Employees].PositionId = @PositionId";
    public const string InsertEmployee = "INSERT INTO [Employees] (FirstName, LastName, PositionId, BirthYear, Salary) VALUES (@FirstName, @LastName, @PositionId, @BirthYear, @Salary)";
    public const string DeleteEmployees = "DELETE FROM [Employees] WHERE [Id] IN ({0})";
    public const string SelectPositionSalaries = "SELECT p.Id AS PositionId, AVG(e.Salary) AS AverageSalary FROM Positions p INNER JOIN Employees e ON p.Id = e.PositionId GROUP BY p.Id ORDER BY p.Id";
}