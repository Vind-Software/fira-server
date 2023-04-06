using FiraServer.Application.Entities.Auth;

namespace FiraServer.Application.Interfaces.Auth;

public interface IApplicationClientRepository
{
    public ApplicationClient Get(Guid clientId);
    public ApplicationClient GetByCredentials(Guid clientId, Guid clientSecret);
}
