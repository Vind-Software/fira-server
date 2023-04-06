using FiraServer.Application.Entities.Auth;
using FiraServer.Infra.Dal.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace FiraServer.Infra.Dal.Contexts;

public class ApplicationClientContext : DbContext
{
    public DbSet<ApplicationClient> ApplicationClients { get; set; }

    public ApplicationClientContext(DbContextOptions<ApplicationClientContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationClientConfiguration());
    }
}
