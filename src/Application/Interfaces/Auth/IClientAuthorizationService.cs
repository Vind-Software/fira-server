using FiraServer.Application.Entities.Auth;

namespace FiraServer.Application.Interfaces.Auth;

public interface IClientAuthorizationService
{
    public ScopeVisibilityLevel IdentifyResourceVisibilityLevel(ApplicationResource resource);
}
