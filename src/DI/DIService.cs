using FiraServer.Application.Entities.Auth;
using FiraServer.Application.Interfaces.Auth;
using FiraServer.Application.Services.Auth;
using FiraServer.Infra.Dal.Contexts;
using FiraServer.Infra.Dal.Options;
using FiraServer.Infra.Dal.Repositories.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Npgsql.NameTranslation;
using System.Net;

namespace FiraServer.DI;

public static class DIService
{
    public static void RegisterDependencies(WebApplicationBuilder builder)
    {
        RegisterConfigurationProviders(builder.Configuration);
        RegisterConfigurationObjects(builder.Services, builder.Configuration);
        RegisterControllers(builder.Services);
        RegisterServices(builder.Services);
        RegisterRepositories(builder.Services);
        RegisterDbContexts(builder.Services, builder.Configuration);
    }

    private static void RegisterConfigurationProviders(ConfigurationManager configurationManager)
    {
        configurationManager.AddEnvironmentVariables(prefix: "FIRASERVER_");
    }

    private static void RegisterConfigurationObjects(IServiceCollection service, ConfigurationManager configurationManager)
    {
        service.Configure<RelationalDBOptions>(configurationManager.GetSection(RelationalDBOptions.Section));

        service.Configure<ForwardedHeadersOptions>(options =>
        {
            options.KnownProxies.Add(Dns.GetHostEntry("reverse-proxy").AddressList[0]);
        });
    }

    private static void RegisterControllers(IServiceCollection services)
    {
        services.AddControllers();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ClientAuthenticationService, ClientAuthenticationService>();
        services.AddScoped<ApplicationResourceService, ApplicationResourceService>();
        services.AddScoped<ClientAuthorizationService, ClientAuthorizationService>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IApplicationClientRepository, ApplicationClientRepository>();
        services.AddScoped<IApplicationResourceRepository, ApplicationResourceRepository>();
        services.AddScoped<IClientScopeGrantRepository, ClientScopeGrantRepository>();
    }

    private static void RegisterDbContexts(IServiceCollection services, ConfigurationManager configurationManager)
    {
        //This kind of mapping is flagged as deprecated because is not recomended to be used with npgsql datasource approach anymore.
        //At this time (04/04/23) it is still the proposed approach to use with EF Core by the official docs.
#pragma warning disable CS0618
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ScopeVisibilityLevel>("auth.scope_visibility_level", new NpgsqlNullNameTranslator());
        //NpgsqlConnection.GlobalTypeMapper.MapEnum<ResourceAction>("auth.resource_action");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ResourceAuthLevel>("auth.resource_auth_level", new NpgsqlNullNameTranslator());
#pragma warning restore CS0618

        RelationalDBOptions dbOptions = (configurationManager.GetSection(RelationalDBOptions.Section)).Get<RelationalDBOptions>();

        services.AddDbContext<ApplicationResourceContext>(ctx =>
        {
            ctx.UseNpgsql($"Host={dbOptions.Host};Database={dbOptions.Database};Username={dbOptions.Username};Password={dbOptions.Password}")
                .UseSnakeCaseNamingConvention();


        });

        services.AddDbContext<ApplicationClientContext>(ctx =>
        {
            ctx.UseNpgsql($"Host={dbOptions.Host};Database={dbOptions.Database};Username={dbOptions.Username};Password={dbOptions.Password}")
                .UseSnakeCaseNamingConvention();
        });

        services.AddDbContext<ApplicationScopeContext>(ctx =>
        {
            ctx.UseNpgsql($"Host={dbOptions.Host};Database={dbOptions.Database};Username={dbOptions.Username};Password={dbOptions.Password}")
                .UseSnakeCaseNamingConvention();
        });
    }
}
