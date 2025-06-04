using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManagment.Models
{
    public class TaskDBContext:IdentityDbContext<AppUser>
    {
       public virtual DbSet<Tasks> Tasks { get; set; }
        public TaskDBContext(DbContextOptions<TaskDBContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
