using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SE07203_F1.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; } // Primary Key, auto-increment

        public string StudentId { get; set; } = null!; // sẽ tự generate
        public string? Name { get; set; }
        public string? Class { get; set; }
        public string? Major { get; set; }
        public string? Status { get; set; }
        public string? Teacher { get; set; }
        public string? Note { get; set; }

        public int? AccountId { get; set; }

        [ForeignKey("AccountId")]
        [ValidateNever]
        public virtual Account? Account { get; set; }

        public string FullName { get; set; } = "";
        public string Email { get; set; } = "unknown@example.com";
        public DateTime BirthDate { get; set; } = DateTime.Parse("2000-01-01");

        [ValidateNever]
        public ICollection<MyTask>? AssignedTasks { get; set; }

    }
}
