using FiraServer.Application.Entities.Auth;
using FiraServer.Infra.Dal.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace FiraServer.Infra.Dal.Contexts;

public class ApplicationResourceContext : DbContext
{
    public DbSet<ApplicationResource> ApplicationResources { get; set; }
    public DbSet<ApplicationScope> ApplicationScopes { get; set; }
    public DbSet<ApplicationResourceType> ApplicationResourceTypes { get; set; }

    public ApplicationResourceContext(DbContextOptions<ApplicationResourceContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationResourceConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationResourceTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationScopeConfiguration());
    }
}
