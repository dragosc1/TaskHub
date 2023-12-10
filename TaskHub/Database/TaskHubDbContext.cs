using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskHub.Models;

namespace TaskHub.Database
{
    public class TaskHubDbcontext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Comentariu> Comentarii { get; set; }
        public DbSet<Echipa> Echipe { get; set; }
        public DbSet<Proiect> Proiecte { get; set; }
        public DbSet<Utilizator> Utilizatori { get; set; }

        public TaskHubDbcontext(DbContextOptions<TaskHubDbcontext> options) : base(options) { 
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Added composite primary keys
            modelBuilder.Entity<Echipa>().HasKey(e => new { e.IdUtilizator, e.IdProiect });
            modelBuilder.Entity<Comentariu>().HasKey(c => new { c.Id, c.IdTask, c.IdUtilizator });

            // Added the asociative table relation
            modelBuilder.Entity<Echipa>()
                    .HasOne(u => u.Utilizator)
                    .WithMany(e => e.Echipe)
                    .HasForeignKey(e => e.IdUtilizator);
            modelBuilder.Entity<Echipa>()
                    .HasOne(p => p.Proiect)
                    .WithMany(e => e.Echipe)
                    .HasForeignKey(e => e.IdProiect);

            modelBuilder.Entity<Comentariu>()
                    .HasOne(u => u.Utilizator)
                    .WithMany(c => c.Comentarii)
                    .HasForeignKey(c => c.IdUtilizator);
            modelBuilder.Entity<Comentariu>()
                    .HasOne(t => t.Task)
                    .WithMany(c => c.Comentarii)
                    .HasForeignKey(c => c.IdTask);
        }
    }
}
