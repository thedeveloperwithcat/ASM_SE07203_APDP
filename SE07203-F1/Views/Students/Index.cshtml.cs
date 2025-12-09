using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE07203_F1.Models;
using SE07203_F1.Data;
using System.Collections.Generic;
using System.Linq;

public class StudentsIndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public StudentsIndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Student> Students { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; }

    // Input model for creating a student
    [BindProperty]
    public StudentInput Input { get; set; }

    public class StudentInput
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Major { get; set; }
        public string Status { get; set; }
        public string Teacher { get; set; }
        public string Note { get; set; }
    }

    public void OnGet()
    {
        var query = _context.Students.AsQueryable();

        if (!string.IsNullOrEmpty(SearchTerm))
        {
            query = query.Where(s => (s.Name ?? "").Contains(SearchTerm) || (s.StudentId ?? "").Contains(SearchTerm));
        }

        Students = query.ToList();
    }

    // Create new student and save to DB
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            OnGet();
            return Page();
        }

        var student = new Student
        {
            StudentId = Input.StudentId,
            Name = Input.Name,
            Class = Input.Class,
            Major = Input.Major,
            Status = Input.Status,
            Teacher = Input.Teacher,
            Note = Input.Note
        };

        _context.Students.Add(student);
        _context.SaveChanges();

        // After inserting, redirect to GET to avoid double-post
        return RedirectToPage();
    }
}