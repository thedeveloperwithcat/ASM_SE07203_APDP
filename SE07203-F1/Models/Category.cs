using System.ComponentModel.DataAnnotations;

namespace SE07203_F1.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
    }
}
