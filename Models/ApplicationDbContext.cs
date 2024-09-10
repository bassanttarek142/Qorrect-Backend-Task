using Microsoft.EntityFrameworkCore;

namespace Qorrect_Backend_Task.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRoles"));

            // Seed roles into the database
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Manager" },
                new Role { Id = 2, Name = "Reviewer" },
                new Role { Id = 3, Name = "Admin" },
                new Role { Id = 4, Name = "Supervisor" },
                new Role { Id = 5, Name = "SubjectCreator" },
                new Role { Id = 6, Name = "Teacher" },
                new Role { Id = 7, Name = "Writer" },
                new Role { Id = 8, Name = "Examinee" }
            );
        }
    }
}
