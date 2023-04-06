using FiraServer.Application.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiraServer.Infra.Dal.EntityConfigurations;

public class ApplicationScopeConfiguration : IEntityTypeConfiguration<ApplicationScope>
{
    public void Configure(EntityTypeBuilder<ApplicationScope> builder)
    {
        builder.ToTable("application_scopes", "auth");
        builder.HasKey(s => s.Id);
    }
}
