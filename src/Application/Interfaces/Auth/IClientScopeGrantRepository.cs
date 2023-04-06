namespace FiraServer.Application.Interfaces.Auth;

using FiraServer.Application.Entities.Auth;

public interface IClientScopeGrantRepository {
    public List<ClientScopeGrant> GetClientGrants(ApplicationClient client, List<ApplicationScope>? scopes = null);
}