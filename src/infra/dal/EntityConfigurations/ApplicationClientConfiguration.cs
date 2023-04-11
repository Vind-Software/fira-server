using FiraServer.Application.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiraServer.Infra.Dal.EntityConfigurations;

public class ApplicationClientConfiguration : IEntityTypeConfiguration<ApplicationClient>
{
    public void Configure(EntityTypeBuilder<ApplicationClient> builder)
    {
        builder.ToTable("application_clients", "auth");
    }
}
