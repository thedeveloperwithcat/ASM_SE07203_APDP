using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE07203_F1.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        public string TeacherCode { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }

        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        public ICollection<MyTask> CreatedTasks { get; set; }
    }
}



//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace SE07203_F1.Models
//{
//    public class Teacher
//    {
//        [Key]
//        public int Id { get; set; }

//        public int AccountId { get; set; }
//        [ForeignKey("AccountId")]
//        public virtual Account Account { get; set; }

//        public string TeacherCode { get; set; } // Mã GV
//        public string Department { get; set; }  // Bộ môn

//        // Danh sách Task giáo viên này đã tạo
//        public virtual ICollection<MyTask> CreatedTasks { get; set; }
//    }
//}