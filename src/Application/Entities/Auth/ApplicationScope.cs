namespace FiraServer.Application.Entities.Auth
{
    public class ApplicationScope
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ScopeVisibilityLevel> VisibilityLevels { get; set; }
        public List<ApplicationResource> Resources { get; set; }
    }
}