using FiraServer.Application.Entities.Auth;
using FiraServer.Application.Interfaces.Auth;
using FiraServer.Infra.Dal.Contexts;

namespace FiraServer.Infra.Dal.Repositories.Auth;

public class ApplicationClientRepository : IApplicationClientRepository
{
    private readonly ApplicationClientContext _DbContext;

    public ApplicationClientRepository(ApplicationClientContext dbContext)
    {
        this._DbContext = dbContext;
    }

    public ApplicationClient Get(Guid clientId)
    {
        ApplicationClient applicationClient = this._DbContext.ApplicationClients
            .Where(c => c.ClientId == clientId)
            .OrderBy(c => c.Id)
            .First();

        return applicationClient;
    }

    public ApplicationClient GetByCredentials(Guid clientId, Guid clientSecret)
    {
        ApplicationClient? applicationClient = this._DbContext.ApplicationClients
            .Where(c => c.ClientId == clientId && c.ClientSecret == clientSecret)
            .OrderBy(c => c.Id)
            .FirstOrDefault();

        return applicationClient;
    }
}
