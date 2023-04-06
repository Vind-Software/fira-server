namespace FiraServer.Infra.Dal.Options
{
    public class RelationalDBOptions
    {
        public const string Section = "DB:Relational";
        public string Host { get; set; } = String.Empty;
        public string Database { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
