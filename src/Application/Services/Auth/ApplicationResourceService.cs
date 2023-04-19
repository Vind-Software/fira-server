using FiraServer.Application.Entities.Auth;
using FiraServer.Application.Interfaces.Auth;

namespace FiraServer.Application.Services.Auth;

public class ApplicationResourceService
{
    private IApplicationResourceRepository _ApplicationResourceRepository;

    public ApplicationResourceService(IApplicationResourceRepository applicationResourceRepository)
    {
        this._ApplicationResourceRepository = applicationResourceRepository;
    }

    public ApplicationResource GetByURI(string uri)
    {
        return this._ApplicationResourceRepository.GetByURI(uri);
    }
}
