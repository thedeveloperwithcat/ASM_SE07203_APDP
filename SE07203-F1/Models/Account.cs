using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SE07203_F1.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; } = "Active";

        // Navigation properties (Quan trọng)
        public virtual Student? StudentProfile { get; set; }
        public virtual Teacher? TeacherProfile { get; set; }
        public ICollection<MyTask> MyTasks { get; set; }
    }
}


//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace SE07203_F1.Models
//{
//    public class Account
//    {
//        [Key]
//        public int Id { get; set; }
//        [Required]
//        [DataType(DataType.Text)]
//        public string Fullname { get; set; }
//        [Required]
//        [DataType(DataType.Text)]
//        public string Username { get; set; }
//        [Required]
//        [DataType(DataType.Text)]
//        public string Password { get; set; }
//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }
//        [Required]
//        public string Role { get; set; } // "Admin", "Teacher", "Student"
//        [Required]
//        public string Status { get; set; } = "Active";

//        public ICollection<MyTask> MyTasks { get; set; }
//        // nếu muốn dùng nhiều kiểu danh sách hơn thì dùng kiểu interface 
//        public Student Student { get; set; }
//        public virtual Student? StudentProfile { get; set; }
//        public virtual Teacher? TeacherProfile { get; set; }
//    }
//}
//// trong file này là khai báo biến 
