using FiraServer.Application.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiraServer.Infra.Dal.EntityConfigurations;

public class ClientScopeGrantConfiguration : IEntityTypeConfiguration<ClientScopeGrant>
{
    public void Configure(EntityTypeBuilder<ClientScopeGrant> builder)
    {
        builder.ToTable("application_clients_application_scopes_grants", "auth");
        builder.HasKey(g => g.Id);

        builder.HasOne(g => g.Scope)
            .WithMany();

        builder.HasOne(g => g.Client)
            .WithMany();
    }
}
