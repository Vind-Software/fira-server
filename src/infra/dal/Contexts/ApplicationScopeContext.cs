using FiraServer.Application.Entities.Auth;
using FiraServer.Infra.Dal.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace FiraServer.Infra.Dal.Contexts;

public class ApplicationScopeContext : DbContext
{
    public DbSet<ApplicationScope> ApplicationScopes { get; set; }
    public DbSet<ClientScopeGrant> ClientScopeGrants { get; set; }

    public ApplicationScopeContext(DbContextOptions<ApplicationScopeContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationScopeConfiguration());
        modelBuilder.ApplyConfiguration(new ClientScopeGrantConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationClientConfiguration());
    }
}
