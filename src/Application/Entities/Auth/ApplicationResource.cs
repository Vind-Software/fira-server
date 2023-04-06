namespace FiraServer.Application.Entities.Auth
{
    public class ApplicationResource
    {
        public int Id { get; set; }
        public string? Uri { get; set; }
        public ResourceAuthLevel? AuthLevel { get; set; }
        public ApplicationResourceType? Type { get; set; }
        public List<ApplicationScope>? Scopes { get; set; }

        public ApplicationResource()
        {
        }

        public ApplicationResource(int id, string uri, ApplicationResourceType type) {
            this.Id = id;
            this.Uri = uri;
            this.Type = type;
        }
    }
}
