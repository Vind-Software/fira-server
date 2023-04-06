using FiraServer.Application.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiraServer.Infra.Dal.EntityConfigurations;

public class ApplicationResourceTypeConfiguration : IEntityTypeConfiguration<ApplicationResourceType>
{
    public void Configure(EntityTypeBuilder<ApplicationResourceType> builder)
    {
        builder.ToTable("application_resource_types", "auth");
        builder.HasKey(x => x.Id);
    }
}
