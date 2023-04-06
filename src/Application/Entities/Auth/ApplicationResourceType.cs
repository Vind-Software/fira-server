namespace FiraServer.Application.Entities.Auth;

public class ApplicationResourceType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ApplicationResourceType (int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}
