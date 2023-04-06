using FiraServer.Application.Entities.Auth;

namespace FiraServer.Application.Interfaces.Auth;

public interface IApplicationResourceRepository
{
    public ApplicationResource GetByURI(string uri);
}
