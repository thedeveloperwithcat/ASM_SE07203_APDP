using Microsoft.EntityFrameworkCore;
using SE07203_F1.Models;
using System.Security.Cryptography;

namespace SE07203_F1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<MyTask> MyTasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(e =>
            {
                e.ToTable("Accounts");
                e.HasKey(e => e.Id);
                e.Property(x  => x.Id).HasColumnName("Id");
                e.Property(x => x.Fullname).HasColumnName("Fullname");
                e.Property(x => x.Username).HasColumnName("Username");
                e.Property(x => x.Password).HasColumnName("Password");
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories");
                e.HasKey(e => e.Id);
                e.Property(x => x.Id).HasColumnName("Id");
                e.Property(x => x.Name).HasColumnName("Name");
            });

            modelBuilder.Entity<MyTask>(e =>
            {
                e.ToTable("Tasks");
                e.HasKey(e => e.Id);
                e.Property(x => x.Id).HasColumnName("Id");
                e.Property(x => x.Name).HasColumnName("Name");
                e.Property(x => x.Description).HasColumnName("Description");
                e.Property(x => x.CategoryId).HasColumnName("CategoryId");
                e.Property(x => x.AccountId).HasColumnName("AccountId");
                e.HasOne(x => x.Account)
                 .WithMany(a => a.MyTasks)
                 .HasForeignKey(x => x.AccountId);
                // syntax gì đây 
                e.HasOne(x => x.Category)
                 .WithMany(c => c.MyTasks)
                 .HasForeignKey(x => x.CategoryId);

            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
