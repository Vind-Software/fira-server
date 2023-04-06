namespace FiraServer.Application.Entities.Auth
{
    public class ApplicationClient
    {
        public int Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid ClientSecret { get; set; }
        public string Name { get; set; }

        public ApplicationClient(int id, Guid clientId, Guid clientSecret, string name) 
        { 
            this.Id = id;
            this.ClientId= clientId;
            this.ClientSecret= clientSecret;
            this.Name= name;
        }

        public override string ToString()
        {
            return $"Name: {this.Name} | Client ID: {this.ClientId} | Client Secret: {this.ClientSecret}";
        }
    }
}
