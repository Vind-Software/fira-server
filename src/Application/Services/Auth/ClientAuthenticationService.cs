using FiraServer.Application.Entities.Auth;
using FiraServer.Application.Interfaces.Auth;

namespace FiraServer.Application.Services.Auth;

public class ClientAuthenticationService
{
    private readonly IApplicationClientRepository _ApplicationClientRepository;

    public ClientAuthenticationService(IApplicationClientRepository ApplicationClientRepository)
    {
        _ApplicationClientRepository = ApplicationClientRepository;
    }

    public ApplicationClient GetClient(Guid clientId)
    {
        return this._ApplicationClientRepository.Get(clientId);
    }

    public bool VerifyClientCredentials(Guid clientId, Guid clientSecret)
    {
        ApplicationClient applicationClient = this._ApplicationClientRepository.GetByCredentials(clientId, clientSecret);
        if(applicationClient == null)
        {
            return false;
        }

        return true;
    }
}
