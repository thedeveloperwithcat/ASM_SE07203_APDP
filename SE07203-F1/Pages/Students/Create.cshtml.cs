using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE07203_F1.Data; 
using SE07203_F1.Models; 

namespace SE07203_F1.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Student Student { get; set; }

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Students.Add(Student);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
