using FiraServer.Application.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiraServer.Infra.Dal.EntityConfigurations.Maps;

[Table("map__application_resources__application_scopes", Schema = "auth")]
public class ApplicationResourceApplicationScopeMap
{
    [Key]
    public int Id { get; set; }

    public int ApplicationResourceId { get; set; }
    public ApplicationResource? ApplicationResource { get; set; }

    public int ApplicationScopeId { get; set; }
    public ApplicationScope? ApplicationScope { get; set; }
}
