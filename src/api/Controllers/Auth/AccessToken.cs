using Microsoft.AspNetCore.Mvc;

namespace FiraServer.api.Controllers.Auth;

[ApiController]
[Route("auth/access-token")]
public class AccessToken : ControllerBase {
    [HttpGet]
    public String getCurrentAccessToken() {
        return "Gets the current active access token";
    }

    [HttpPost]
    public string createNewAccessToken() {
        return "Creates a new access token revoking the previus one";
    }

    [HttpPut]
    public string renewAccessToken() {
        return "Updates the expiration date of a currently active token";
    }

    [HttpDelete]
    public string revokeAccessToken() {
        return "Revokes a currently active access token";
    }
}