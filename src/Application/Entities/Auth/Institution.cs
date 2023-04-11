namespace FiraServer.Application.Entities.Auth;

public class Institution
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OwnerName { get; set; } = string.Empty;

    public Institution(string name)
    {
        this.Name = name;
    }
}
