using Microsoft.AspNetCore.Mvc;
using SE07203_F1.Data;
using SE07203_F1.Models;
using System.Linq;

public class MyTaskController : Controller
{
    private readonly ApplicationDbContext _context;

    public MyTaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Create(Student model, int? accountId)
    {
        // Kiểm tra AccountId, tạo StudentId, Email/BirthDate mặc định
        if (accountId.HasValue && _context.Students.Any(s => s.AccountId == accountId.Value))
        {
            ModelState.AddModelError("", "AccountId này đã được gán cho một Student khác!");
            return View(model);
        }

        var student = new Student
        {
            Name = model.Name,
            Class = model.Class,
            Major = model.Major,
            Status = model.Status,
            Teacher = model.Teacher,
            AccountId = accountId
            // Email và BirthDate đã mặc định trong model
        };

        _context.Students.Add(student);
        _context.SaveChanges();

        student.StudentId = $"S{student.Id:0000}";
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}

namespace SE07203_F1.Models
{
    public class MyTask
    {
        public int Id { get; set; }

        public int CategoryId { get; set; } public string Name { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public int? AccountId { get; set; }
        // Add this property to fix CS1061
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
