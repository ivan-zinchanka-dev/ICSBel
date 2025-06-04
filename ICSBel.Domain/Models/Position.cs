namespace ICSBel.Domain.Models;

public class Position
{
    public static Position All => new Position(-1, "Все");
        
    public int Id { get; private set; }
    public string Name { get; private set; }

    public Position(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}