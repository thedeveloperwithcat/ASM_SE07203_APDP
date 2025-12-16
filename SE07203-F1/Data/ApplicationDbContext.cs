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
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<MyTask> MyTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Cấu hình Account (Unique Username/Email)
            modelBuilder.Entity<Account>()
                .HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Account>()
                .HasIndex(u => u.Email).IsUnique();

            // 2. Quan hệ 1-1: Account - Student
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Account)
                .WithOne(a => a.StudentProfile)
                .HasForeignKey<Student>(s => s.AccountId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Account thì xóa luôn Profile Student

            // 3. Quan hệ 1-1: Account - Teacher
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Account)
                .WithOne(a => a.TeacherProfile)
                .HasForeignKey<Teacher>(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);



            // 5. Seed Data (Tạo sẵn tài khoản Admin mặc định)
            // Vì Admin không đăng ký công khai để bảo mật
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    Username = "admin",
                    // Lưu ý: Password này cần hash trước khi đưa vào đây. 
                    // Ví dụ hash của "123456" (SHA256 đơn giản như code cũ của bạn)
                    Password = "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=",
                    Fullname = "System Administrator",
                    Email = "admin@system.com",
                    Role = "Admin",
                    Status = "Active"
                }
            );
        }
    }
}




//using Microsoft.EntityFrameworkCore;
//using SE07203_F1.Models;

//namespace SE07203_F1.Data
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
//        {
//        }

//        public DbSet<Account> Accounts { get; set; }
//        public DbSet<Category> Categories { get; set; }
//        public DbSet<MyTask> MyTasks { get; set; }
//        public DbSet<Student> Students { get; set; }


//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            // 1. Cấu hình bảng Accounts (Login/Register)
//            modelBuilder.Entity<Account>(e =>
//            {
//                e.ToTable("Accounts");
//                e.HasKey(x => x.Id);
//                e.Property(x => x.Id).HasColumnName("Id");
//                e.Property(x => x.Fullname).HasColumnName("Fullname").IsRequired();
//                e.Property(x => x.Username).HasColumnName("Username").IsRequired();
//                e.Property(x => x.Password).HasColumnName("Password").IsRequired();

//                // Bổ sung role và status
//                e.Property(x => x.Role).HasColumnName("Role").IsRequired(); // admin / teacher / student
//                e.Property(x => x.Status).HasColumnName("Status").IsRequired(); // active / inactive
//            });

//            // 2. Cấu hình bảng Categories
//            modelBuilder.Entity<Category>(e =>
//            {
//                e.ToTable("Categories");
//                e.HasKey(x => x.Id);
//                e.Property(x => x.Id).HasColumnName("Id");
//                e.Property(x => x.Name).HasColumnName("Name").IsRequired();
//            });

//            // 3. Cấu hình bảng Tasks (MyTask)
//            modelBuilder.Entity<MyTask>(e =>
//            {
//                e.ToTable("Tasks");
//                e.HasKey(x => x.Id);
//                e.Property(x => x.Id).HasColumnName("Id");
//                e.Property(x => x.Name).HasColumnName("Name").IsRequired();
//                e.Property(x => x.Description).HasColumnName("Description").IsRequired();
//                e.Property(x => x.CategoryId).HasColumnName("CategoryId");
//                e.Property(x => x.AccountId).HasColumnName("AccountId");

//                // Thêm các cột quản lý Task
//                e.Property(x => x.Status).HasColumnName("Status").HasDefaultValue("new"); // new / in_progress / completed / overdue
//                e.Property(x => x.Priority).HasColumnName("Priority").HasDefaultValue("medium"); // low / medium / high
//                e.Property(x => x.DueDate).HasColumnName("DueDate").IsRequired(false);

//                // Config quan hệ
//                e.HasOne(x => x.Account)
//                 .WithMany(a => a.MyTasks)
//                 .HasForeignKey(x => x.AccountId)
//                 .OnDelete(DeleteBehavior.Cascade);

//                e.HasOne(x => x.Category)
//                 .WithMany(c => c.MyTasks)
//                 .HasForeignKey(x => x.CategoryId)
//                 .OnDelete(DeleteBehavior.Cascade);
//            });

//            // 4. Cấu hình bảng Students
//            modelBuilder.Entity<Student>(e =>
//            {
//                e.ToTable("Students");
//                e.HasKey(x => x.Id);
//                e.Property(x => x.Id).HasColumnName("Id");
//                e.Property(x => x.FullName).HasColumnName("FullName").IsRequired();
//                e.Property(x => x.Email).HasColumnName("Email").IsRequired();
//                e.Property(x => x.BirthDate).HasColumnName("BirthDate").IsRequired();
//                e.Property(x => x.StudentId).HasColumnName("StudentId").IsRequired(false);
//                e.Property(x => x.Name).HasColumnName("Name").IsRequired(false);
//                e.Property(x => x.Class).HasColumnName("Class").IsRequired(false);
//                e.Property(x => x.Major).HasColumnName("Major").IsRequired(false);
//                e.Property(x => x.Status).HasColumnName("Status").IsRequired(false);
//                e.Property(x => x.Teacher).HasColumnName("Teacher").IsRequired(false);
//                e.Property(x => x.Note).HasColumnName("Note").IsRequired(false);


//            });

//            base.OnModelCreating(modelBuilder);
//        }
//    }
//}

















////using Microsoft.EntityFrameworkCore;
////using SE07203_F1.Models;
////using System.Security.Cryptography;

////namespace SE07203_F1.Data
////{
////    public class ApplicationDbContext : DbContext
////    {
////        public ApplicationDbContext(DbContextOptions options) : base(options)
////        {
////        }

////        protected ApplicationDbContext()
////        {
////        }

////        public DbSet<Account> Accounts { get; set; }
////        public DbSet<MyTask> MyTasks { get; set; }
////        public DbSet<Category> Categories { get; set; }
////        public DbSet<Student> Students { get; set; }

////        protected override void OnModelCreating(ModelBuilder modelBuilder)
////        {
////            modelBuilder.Entity<Account>(e =>
////            {
////                e.ToTable("Accounts");
////                e.HasKey(e => e.Id);
////                e.Property(x  => x.Id).HasColumnName("Id");
////                e.Property(x => x.Fullname).HasColumnName("Fullname");
////                e.Property(x => x.Username).HasColumnName("Username");
////                e.Property(x => x.Password).HasColumnName("Password");
////            });

////            modelBuilder.Entity<Category>(e =>
////            {
////                e.ToTable("Categories");
////                e.HasKey(e => e.Id);
////                e.Property(x => x.Id).HasColumnName("Id");
////                e.Property(x => x.Name).HasColumnName("Name");
////            });

////            modelBuilder.Entity<MyTask>(e =>
////            {
////                e.ToTable("Tasks");
////                e.HasKey(e => e.Id);
////                e.Property(x => x.Id).HasColumnName("Id");
////                e.Property(x => x.Name).HasColumnName("Name");
////                e.Property(x => x.Description).HasColumnName("Description");
////                e.Property(x => x.CategoryId).HasColumnName("CategoryId");
////                e.Property(x => x.AccountId).HasColumnName("AccountId");
////                e.HasOne(x => x.Account)
////                 .WithMany(a => a.MyTasks)
////                 .HasForeignKey(x => x.AccountId);
////                // syntax gì đây 
////                e.HasOne(x => x.Category)
////                 .WithMany(c => c.MyTasks)
////                 .HasForeignKey(x => x.CategoryId);

////            });
////            modelBuilder.Entity<Student>(e =>
////            {
////                e.ToTable("Students");
////                e.HasKey(e => e.Id);
////                e.Property(x => x.Id).HasColumnName("Id");
////            });
////            base.OnModelCreating(modelBuilder);
////        }
////    }
////}
