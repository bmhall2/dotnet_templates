namespace WebApi_Postgres_Docker_GraphQL.Domain
{
    public class Group
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        
        public required List<User> Users { get; set; }
    }
}