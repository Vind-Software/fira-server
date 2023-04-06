namespace FiraServer.Application.Entities.Auth;

public class ClientScopeGrant
{
    public int Id { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ExpirationDate { get; set; }
    public ScopeVisibilityLevel VisibilityLevel { get; set; }
    public ApplicationScope? Scope { get; set; }
    public ApplicationClient? Client { get; set; }
}
