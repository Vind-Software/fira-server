using FiraServer.Application.Interfaces.Auth;
using FiraServer.Infra.Dal.Contexts;
using Microsoft.EntityFrameworkCore;
using FiraServer.Application.Entities.Auth;

namespace FiraServer.Infra.Dal.Repositories.Auth;

public class ApplicationResourceRepository : IApplicationResourceRepository
{
    private readonly ApplicationResourceContext _DbContext;

    public ApplicationResourceRepository(ApplicationResourceContext dbContext)
    {
        this._DbContext = dbContext;
    }

    public ApplicationResource GetByURI(string uri)
    {
        ApplicationResource applicationResource = this._DbContext.ApplicationResources
            .Include(r => r.Type)
            .Include(r => r.Scopes)
            .Where(r => r.Uri == uri)
            .OrderBy(r => r.Id)
            .First();

        return applicationResource;
    }
}
