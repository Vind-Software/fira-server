using FiraServer.Application.Entities.Auth;
using FiraServer.Application.Interfaces.Auth;

namespace FiraServer.Application.Services.Auth;

public class ClientAuthorizationService
{
    private readonly IClientScopeGrantRepository _ClientScopeGrantRepository;
    public ClientAuthorizationService(IClientScopeGrantRepository applicationScopeRepository) 
    {
        this._ClientScopeGrantRepository = applicationScopeRepository;
    }

    public ScopeVisibilityLevel IdentifyResourceVisibilityLevel(ApplicationResource resource)
    {
        //Flattening the list of lists that is built from selecting the VisibilityLevels attribute from every scope
        List<List<ScopeVisibilityLevel>> visibilityLevelsList = resource.Scopes.Select<ApplicationScope, List<ScopeVisibilityLevel>>(s => s.VisibilityLevels).ToList<List<ScopeVisibilityLevel>>();
        List<ScopeVisibilityLevel>  visibilityLevels = visibilityLevelsList.SelectMany(x => x).ToList();

        ScopeVisibilityLevel highestVisibilityLevel = ScopeVisibilityLevel.ADMIN;

        foreach (ScopeVisibilityLevel visibilityLevel in visibilityLevels) {
            if (visibilityLevel.CompareTo(highestVisibilityLevel) > 0)
            {
                highestVisibilityLevel= visibilityLevel;
            }
        }

        return highestVisibilityLevel;
    }

    public bool HasAccessToScopes(ApplicationClient client, List<ApplicationScope> scopes)
    {
        bool hasGrantForEveryScope = true;
        List<ClientScopeGrant> grants = this._ClientScopeGrantRepository.GetClientGrants(client, scopes);
        List<int> grantedScopesIds = grants.Select(g => g.Scope.Id).ToList();

        foreach (ApplicationScope scope in scopes)
        {
            if (!grantedScopesIds.Contains(scope.Id))
            {
                hasGrantForEveryScope = false;
                break;
            }
        }

        return hasGrantForEveryScope;
    }
}
