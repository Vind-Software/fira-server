using FiraServer.Application.Entities.Auth;

namespace FiraServer.Application.Interfaces.Auth;

public interface IClientAuthenticationService
{
    public ApplicationClient GetClient(Guid clientId);
}
