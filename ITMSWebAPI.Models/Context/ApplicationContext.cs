using Microsoft.EntityFrameworkCore;

namespace ITMSWebAPI.Models.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Debit> Debits { get; set; }
        public DbSet<SessionInfo> SessionInfos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
