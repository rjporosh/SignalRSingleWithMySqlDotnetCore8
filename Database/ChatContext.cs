namespace SignalRSingleWithMySql.Database
{
    using Microsoft.EntityFrameworkCore;
    using SignalRSingleWithMySql.Database.Models;

    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
