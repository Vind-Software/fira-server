using FiraServer.Application.Entities.Auth;
using FiraServer.Application.Interfaces.Auth;

namespace FiraServer.Application.Services.Auth;

public class ClientAuthenticationService
{
    private readonly IApplicationClientRepository _applicationClientRepository;

    public ClientAuthenticationService(IApplicationClientRepository ApplicationClientRepository)
    {
        _applicationClientRepository = ApplicationClientRepository;
    }

    public ApplicationClient GetClient(Guid clientId)
    {
        return this._applicationClientRepository.Get(clientId);
    }

    public bool VerifyClientCredentials(Guid clientId, Guid clientSecret)
    {
        ApplicationClient applicationClient = this._applicationClientRepository.GetByCredentials(clientId, clientSecret);
        if(applicationClient == null)
        {
            return false;
        }

        return true;
    }
}
