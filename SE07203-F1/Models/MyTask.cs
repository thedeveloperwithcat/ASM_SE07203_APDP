using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE07203_F1.Models
{
    public class MyTask
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } // Required for .Include(e => e.Category)
                                                       // Người tạo Task (Teacher)
        public int? CreatorId { get; set; } // Có thể null nếu cần, hoặc int nếu bắt buộc
        public Teacher Creator { get; set; }

        // Người được giao Task (Student)
        public int? AssigneeId { get; set; }
        public Student Assignee { get; set; }

        public int? AccountId { get; set; } // Required for e.AccountId

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } // Required for .Include(e => e.Account)
    }
}



//using System.ComponentModel.DataAnnotations;

//namespace SE07203_F1.Models
//{
//    public class MyTask
//    {
//        [Key]
//        public int Id { get; set; }
//        [Required]
//        [DataType(DataType.Text)]
//        public string Name { get; set; }
//        [Required]
//        [DataType(DataType.Text)]
//        public string Description { get; set; }
//        [Required]
//        public int? AccountId { get; set; }
//        [Required]
//        public int? CategoryId { get; set; }
//        public string Status { get; set; } = "new"; // new / in_progress / completed / overdue
//        public string Priority { get; set; } = "medium"; // low / medium / high
//        public DateTime? DueDate { get; set; }
//        public Account? Account { get; set; }
//        // chấm hỏi vì không phải dữ liệu lúc nào cũng phải có 
//        public Category? Category { get; set; }
//    }
//}
