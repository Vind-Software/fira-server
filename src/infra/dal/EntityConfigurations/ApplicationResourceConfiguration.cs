using FiraServer.Application.Entities.Auth;
using FiraServer.Infra.Dal.EntityConfigurations.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiraServer.Infra.Dal.EntityConfigurations;

public class ApplicationResourceConfiguration : IEntityTypeConfiguration<ApplicationResource>
{
    public void Configure(EntityTypeBuilder<ApplicationResource> builder)
    {
        builder.ToTable("application_resources", "auth");
        builder.HasKey(r => r.Id);

        builder.HasOne(r => r.Type)
        .WithMany();

        builder.HasMany(r => r.Scopes)
            .WithMany(s => s.Resources)
            .UsingEntity<ApplicationResourceApplicationScopeMap>(
                j => j
                    .HasOne(m => m.ApplicationScope)
                    .WithMany()
                    .HasForeignKey(m => m.ApplicationScopeId),
                j => j
                    .HasOne(m => m.ApplicationResource)
                    .WithMany()
                    .HasForeignKey(m => m.ApplicationResourceId),
                j =>
                {
                    j.HasKey(m => m.Id);
                }
            );
    }
}
