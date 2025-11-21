using System.ComponentModel.DataAnnotations;

namespace SE07203_F1.Models
{
    public class MyTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Account? Account { get; set; }
        // chấm hỏi vì không phải dữ liệu lúc nào cũng phải có 
        public Category? Category { get; set; }
    }
}
