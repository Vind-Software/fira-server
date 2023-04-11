
using FiraServer.Application.Entities.Auth;
using FiraServer.Application.Interfaces.Auth;
using FiraServer.Infra.Dal.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FiraServer.Infra.Dal.Repositories.Auth;

public class ClientScopeGrantRepository : IClientScopeGrantRepository
{
    private readonly ApplicationScopeContext _DbContext;

    public ClientScopeGrantRepository(ApplicationScopeContext dbContext)
    {
        this._DbContext = dbContext;
    }

    public List<ClientScopeGrant> GetClientGrants(ApplicationClient client, List<ApplicationScope>? scopes = null)
    {
        IQueryable<ClientScopeGrant> query = this._DbContext.ClientScopeGrants
            .Include(g => g.Scope)
            .Where(g => g.Client.Id == client.Id);

        if (scopes != null && scopes.Count > 0)
        {
            List<int> scopesId = scopes.Select(s => s.Id).ToList();
            query = query.Where(g => scopesId.Contains(g.Scope.Id));
        }

        List<ClientScopeGrant> grants = query.ToList();

        return grants;
    }
}