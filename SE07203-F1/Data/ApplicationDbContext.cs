using Microsoft.EntityFrameworkCore;
using SE07203_F1.Models;

namespace SE07203_F1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MyTask> MyTasks { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Cấu hình bảng Accounts
            modelBuilder.Entity<Account>(e =>
            {
                e.ToTable("Accounts");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id");
                e.Property(x => x.Fullname).HasColumnName("Fullname").IsRequired();
                e.Property(x => x.Username).HasColumnName("Username").IsRequired();
                e.Property(x => x.Password).HasColumnName("Password").IsRequired();
            });

            // 2. Cấu hình bảng Categories
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id");
                e.Property(x => x.Name).HasColumnName("Name").IsRequired();
            });

            // 3. Cấu hình bảng Tasks (Map từ model MyTask vào bảng Tasks)
            modelBuilder.Entity<MyTask>(e =>
            {
                e.ToTable("Tasks"); // Tên bảng trong DB vẫn là Tasks
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id");
                e.Property(x => x.Name).HasColumnName("Name").IsRequired();
                e.Property(x => x.Description).HasColumnName("Description").IsRequired();
                e.Property(x => x.CategoryId).HasColumnName("CategoryId");
                e.Property(x => x.AccountId).HasColumnName("AccountId");

                // Config quan hệ
                e.HasOne(x => x.Account)
                 .WithMany(a => a.MyTasks)
                 .HasForeignKey(x => x.AccountId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Category)
                 .WithMany(c => c.MyTasks)
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // 4. Cấu hình bảng Students
            modelBuilder.Entity<Student>(e =>
            {
                e.ToTable("Students");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

















//using Microsoft.EntityFrameworkCore;
//using SE07203_F1.Models;
//using System.Security.Cryptography;

//namespace SE07203_F1.Data
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions options) : base(options)
//        {
//        }

//        protected ApplicationDbContext()
//        {
//        }

//        public DbSet<Account> Accounts { get; set; }
//        public DbSet<MyTask> MyTasks { get; set; }
//        public DbSet<Category> Categories { get; set; }
//        public DbSet<Student> Students { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Account>(e =>
//            {
//                e.ToTable("Accounts");
//                e.HasKey(e => e.Id);
//                e.Property(x  => x.Id).HasColumnName("Id");
//                e.Property(x => x.Fullname).HasColumnName("Fullname");
//                e.Property(x => x.Username).HasColumnName("Username");
//                e.Property(x => x.Password).HasColumnName("Password");
//            });

//            modelBuilder.Entity<Category>(e =>
//            {
//                e.ToTable("Categories");
//                e.HasKey(e => e.Id);
//                e.Property(x => x.Id).HasColumnName("Id");
//                e.Property(x => x.Name).HasColumnName("Name");
//            });

//            modelBuilder.Entity<MyTask>(e =>
//            {
//                e.ToTable("Tasks");
//                e.HasKey(e => e.Id);
//                e.Property(x => x.Id).HasColumnName("Id");
//                e.Property(x => x.Name).HasColumnName("Name");
//                e.Property(x => x.Description).HasColumnName("Description");
//                e.Property(x => x.CategoryId).HasColumnName("CategoryId");
//                e.Property(x => x.AccountId).HasColumnName("AccountId");
//                e.HasOne(x => x.Account)
//                 .WithMany(a => a.MyTasks)
//                 .HasForeignKey(x => x.AccountId);
//                // syntax gì đây 
//                e.HasOne(x => x.Category)
//                 .WithMany(c => c.MyTasks)
//                 .HasForeignKey(x => x.CategoryId);

//            });
//            modelBuilder.Entity<Student>(e =>
//            {
//                e.ToTable("Students");
//                e.HasKey(e => e.Id);
//                e.Property(x => x.Id).HasColumnName("Id");
//            });
//            base.OnModelCreating(modelBuilder);
//        }
//    }
//}
