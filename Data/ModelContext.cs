using Microsoft.EntityFrameworkCore;

namespace CoreAdminLTE.Data {
    public class ModelContext : DbContext {
        public ModelContext() { }
        public ModelContext(DbContextOptions<ModelContext> options) : base (options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;userid=root;password=toor1212;database=AdminLTEdb;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleRel> UserRoleRels { get; set; }

    }
}
