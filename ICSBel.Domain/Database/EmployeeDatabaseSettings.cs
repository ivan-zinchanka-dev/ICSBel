namespace ICSBel.Domain.Database;

public class EmployeeDatabaseSettings
{
    public string ConnectionString { get; private set; }

    public EmployeeDatabaseSettings(string connectionString)
    {
        ConnectionString = connectionString;
    }
}