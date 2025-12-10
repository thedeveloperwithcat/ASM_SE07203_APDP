using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE07203_F1.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public string StudentId { get; set; } // M? sinh viên (VD: HE14050)

        [Required]
        public string FullName { get; set; }  // Dùng th?ng nh?t FullName

        public string Class { get; set; }
        public string Major { get; set; }     // Chuyên ngành
        public string Status { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }

        // Ghi chú ho?c tên GV hý?ng d?n (lýu d?ng text ðõn gi?n)
        public string Teacher { get; set; }
        public string Note { get; set; }

        // --- 1. M?I QUAN H? V?I ACCOUNT (1-1) ---
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        // --- 2. M?I QUAN H? V?I TASK (1-Nhi?u) ---
        // Sinh viên là ngý?i ÐÝ?C GIAO (Assignee)
        [InverseProperty("Assignee")]
        public ICollection<MyTask> AssignedTasks { get; set; }
    }
}





//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace SE07203_F1.Models
//{
//    public class Student
//    {
//        [Key]
//        public int Id { get; set; } 

//        public string StudentId { get; set; } 
//        public string Name { get; set; }
//        public string Class { get; set; }
//        public string Major { get; set; }
//        public string Status { get; set; }
//        public string Teacher { get; set; }
//        public string Note { get; set; }

//        // Khóa ngo?i tr? v? Account
//        public int AccountId { get; set; }
//        [ForeignKey("AccountId")]
//        public virtual Account Account { get; set; }
//        public string FullName { get; set; }
//        public string Email { get; set; }
//        public DateTime BirthDate { get; set; }
//        // Danh sách Task ðý?c giao (ð? kh?p v?i .WithMany(student => student.AssignedTasks))
//        public ICollection<MyTask> AssignedTasks { get; set; }

//    }
//}





////public class Student
////{
////    public int Id { get; set; }
////    public string FullName { get; set; }
////    public string Email { get; set; }
////    public DateTime BirthDate { get; set; }
////    public string StudentId { get; internal set; }
////    public string Name { get; internal set; }
////    public string Class { get; internal set; }
////    public string Major { get; internal set; }
////    public string Status { get; internal set; }
////    public string Teacher { get; internal set; }
////    public string Note { get; internal set; }
////}