using FiraServer.Application.Entities.Auth;

namespace FiraServer.Application.Interfaces.Auth;

public interface IApplicationResourceService
{
    public ApplicationResource GetByURI(string uri);
}
