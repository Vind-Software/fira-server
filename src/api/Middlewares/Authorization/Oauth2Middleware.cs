using FiraServer.Application.Interfaces.Auth;
using FiraServer.Application.Entities.Auth;
using FiraServer.api.Common.Exceptions;
using System.Web;
using FiraServer.Application.Services.Auth;

namespace FiraServer.api.Middlewares.Authorization
{
    public class Oauth2Middleware
    {
        private readonly Dictionary<string, List<string>> _RequiredHeaders;
        private readonly RequestDelegate _next;
        private ClientAuthenticationService? _ClientAuthenticationService;
        private ClientAuthorizationService? _ClientAuthorizationService;
        private ApplicationResourceService? _ApplicationResourceService;

        public Oauth2Middleware(RequestDelegate nextDelegate)
        {
            this._next = nextDelegate;

            this._RequiredHeaders = new Dictionary<string, List<string>>()
            {
                {"client", new List<string>(){"clientId"}},
                {"authenticating_client", new List<string>(){"clientId", "clientSecret"}},
                {"authenticated_client", new List<string>(){"accessToken"}}
            };
        }

        public async Task InvokeAsync(HttpContext context, ApplicationResourceService applicationResourceService, ClientAuthorizationService clientAuthorizationService, ClientAuthenticationService clientAuthenticationService)
        {
            this._ApplicationResourceService = applicationResourceService;
            this._ClientAuthorizationService = clientAuthorizationService;
            this._ClientAuthenticationService = clientAuthenticationService;

            try
            {   
                ApplicationClient client = this.IdentifyClient(context);
                ApplicationResource resource = this.IdentifyApplicationResource(context);

                AuthorizationResult authResult = this.AuthorizeClient(context, client, resource);
                if (true || authResult.Status)
                {
                    await _next(context);
                } else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync($"The request was not authorized due to the following reasons: <br> {string.Join(" <br>", authResult.Messages)}");
                }
            }
            catch (MissingHeaderException Exc)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(Exc.Message);
            }
        }

        private ApplicationClient IdentifyClient(HttpContext context)
        {
            List<string> missingHeaders = this.CheckMissingRequestHeaders(context, "client");

            if (missingHeaders.Count > 0)
            {
                throw new MissingHeaderException("The following mandatory headers were not found on the request: " + string.Join(", ", missingHeaders.ToArray()));
            }

            Guid clientId = Guid.Parse(context.Request.Headers["clientId"]);
            return this._ClientAuthenticationService.GetClient(clientId);
        }
        
        private ApplicationResource IdentifyApplicationResource(HttpContext context)
        {
            string requestUrl = context.Request.Path;

            if(this._ApplicationResourceService!= null)
            {
                ApplicationResource resource = this._ApplicationResourceService.GetByURI(requestUrl);
                return resource;
            } else
            {
                throw new NullReferenceException("The Application Resources service was not available during execution.");
            }
        }

        private List<string> CheckMissingRequestHeaders(HttpContext context, string headersGroup)
        {
            List<string> requiredHeaders = this._RequiredHeaders[headersGroup];
            List<string> missingHeaders = new List<string>();

            foreach (string headerName in requiredHeaders)
            {
                string headerContent = context.Request.Headers[headerName];
                if (string.IsNullOrEmpty(headerContent)) 
                {
                    missingHeaders.Add(headerName);
                }
            }

            return missingHeaders;
        }

        private AuthorizationResult AuthorizeClient(HttpContext context, ApplicationClient client, ApplicationResource resource)
        {
            if (this._ClientAuthorizationService != null)
            {
                bool clientIsAuthorized = true;
                bool clientHasAccessToScopes = this._ClientAuthorizationService.HasAccessToScopes(client, resource.Scopes);
                bool clientSatisfyAuthLevel = true;

                switch (resource.AuthLevel)
                {
                    case ResourceAuthLevel.REAUTHENTICATE:
                        List<string> missingHeaders = this.CheckMissingRequestHeaders(context, "authenticating_client");

                        if (missingHeaders.Count > 0)
                        {
                            throw new MissingHeaderException("The following mandatory headers were not found on the request: " + string.Join(", ", missingHeaders.ToArray()));
                        }

                        Guid clientId = Guid.Parse(context.Request.Headers["clientId"]);
                        Guid clientSecret = Guid.Parse(context.Request.Headers["clientSecret"]);

                        clientSatisfyAuthLevel = this._ClientAuthenticationService.VerifyClientCredentials(clientId, clientSecret);
                        break;
                    case ResourceAuthLevel.AUTHENTICATED: 
                        break;
                }

                clientIsAuthorized = clientHasAccessToScopes && clientSatisfyAuthLevel;
                AuthorizationResult authResult = new AuthorizationResult();
                authResult.Status = clientIsAuthorized;

                if (!clientHasAccessToScopes)
                {
                    List<string> scopesNamesList = resource.Scopes.Select(s => s.Name).ToList();
                    string scopesNamesString = string.Join(", ", scopesNamesList);

                    authResult.Messages.Add($"The application client is not authorized in one of the following mandatory scopes: {scopesNamesString}");
                }

                if (!clientSatisfyAuthLevel)
                {
                    switch (resource.AuthLevel)
                    {
                        case ResourceAuthLevel.REAUTHENTICATE:
                            authResult.Messages.Add("The application client could not be authenticated with provided credentials");
                            break;
                        case ResourceAuthLevel.AUTHENTICATED:
                            authResult.Messages.Add("The provided access token was not valid");
                            break;
                    }
                }

                return authResult;
            }
            else
            {
                throw new NullReferenceException("The client authorization service was not available during execution.");
            }
        }
    }

    public static class Oauth2MiddlewareExtensions
    {
        public static IApplicationBuilder UseOauth2Middleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Oauth2Middleware>();
        }
    }

    internal class AuthorizationResult
    {
        public bool Status { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }

}