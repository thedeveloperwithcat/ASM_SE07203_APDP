using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE07203_F1.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Fullname { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Password { get; set; }
        // trong file này là khai báo biến 
        public ICollection<MyTask> MyTasks { get; set; }
        // nếu muốn dùng nhiều kiểu danh sách hơn thì dùng kiểu interface 
    }
}
