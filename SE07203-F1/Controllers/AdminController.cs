using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using SE07203_F1.Models;

namespace SE07203_F1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ----------------- Dashboard -----------------
        public IActionResult Index()
        {
            return View();
        }

        // ----------------- Students CRUD -----------------
        public async Task<IActionResult> StudentsIndex()
        {
            var students = await _context.Students.Include(s => s.Account).ToListAsync();
            return View("Students/Index", students); // explicit path
        }

        public IActionResult StudentsCreate()
        {
            return View("Students/Create");
        }

        [HttpPost]
        public async Task<IActionResult> StudentsCreate(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(StudentsIndex));
            }
            return View("Students/Create", student);
        }

        public async Task<IActionResult> StudentsEdit(int id)
        {
            var student = await _context.Students.Include(s => s.Account).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();
            return View("Students/Edit", student);
        }

        [HttpPost]
        public async Task<IActionResult> StudentsEdit(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(StudentsIndex));
            }
            return View("Students/Edit", student);
        }

        public async Task<IActionResult> StudentsDelete(int id)
        {
            var student = await _context.Students.Include(s => s.Account).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();
            return View("Students/Delete", student);
        }

        [HttpPost]
        public async Task<IActionResult> StudentsDeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(StudentsIndex));
        }

        public async Task<IActionResult> StudentsDetails(int id)
        {
            var student = await _context.Students.Include(s => s.Account).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();
            return View("Students/Details", student);
        }

        // ----------------- Teachers CRUD -----------------
        public async Task<IActionResult> TeachersIndex()
        {
            var teachers = await _context.Teachers.Include(t => t.Account).ToListAsync();
            return View("Teachers/Index", teachers);
        }

        public IActionResult TeachersCreate()
        {
            return View("Teachers/Create");
        }

        [HttpPost]
        public async Task<IActionResult> TeachersCreate(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TeachersIndex));
            }
            return View("Teachers/Create", teacher);
        }

        public async Task<IActionResult> TeachersEdit(int id)
        {
            var teacher = await _context.Teachers.Include(t => t.Account).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return NotFound();
            return View("Teachers/Edit", teacher);
        }

        [HttpPost]
        public async Task<IActionResult> TeachersEdit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Teachers.Update(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TeachersIndex));
            }
            return View("Teachers/Edit", teacher);
        }

        public async Task<IActionResult> TeachersDelete(int id)
        {
            var teacher = await _context.Teachers.Include(t => t.Account).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return NotFound();
            return View("Teachers/Delete", teacher);
        }

        [HttpPost]
        public async Task<IActionResult> TeachersDeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(TeachersIndex));
        }

        public async Task<IActionResult> TeachersDetails(int id)
        {
            var teacher = await _context.Teachers.Include(t => t.Account).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return NotFound();
            return View("Teachers/Details", teacher);
        }

        // ----------------- Tasks CRUD -----------------
        public async Task<IActionResult> TasksIndex()
        {
            var tasks = await _context.MyTasks
                .Include(t => t.Creator).ThenInclude(c => c.Account)
                .Include(t => t.Assignee).ThenInclude(s => s.Account)
                .Include(t => t.Category)
                .ToListAsync();
            return View("Tasks/Index", tasks);
        }

        public IActionResult TasksCreate()
        {
            ViewBag.Teachers = _context.Teachers.Include(t => t.Account).ToList();
            ViewBag.Students = _context.Students.Include(s => s.Account).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View("Tasks/Create");
        }

        [HttpPost]
        public async Task<IActionResult> TasksCreate(MyTask task)
        {
            if (ModelState.IsValid)
            {
                _context.MyTasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TasksIndex));
            }
            return View("Tasks/Create", task);
        }

        public async Task<IActionResult> TasksEdit(int id)
        {
            var task = await _context.MyTasks.FindAsync(id);
            if (task == null) return NotFound();

            ViewBag.Teachers = _context.Teachers.Include(t => t.Account).ToList();
            ViewBag.Students = _context.Students.Include(s => s.Account).ToList();
            ViewBag.Categories = _context.Categories.ToList();

            return View("Tasks/Edit", task);
        }

        [HttpPost]
        public async Task<IActionResult> TasksEdit(MyTask task)
        {
            if (ModelState.IsValid)
            {
                _context.MyTasks.Update(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TasksIndex));
            }

            ViewBag.Teachers = _context.Teachers.Include(t => t.Account).ToList();
            ViewBag.Students = _context.Students.Include(s => s.Account).ToList();
            ViewBag.Categories = _context.Categories.ToList();

            return View("Tasks/Edit", task);
        }

        public async Task<IActionResult> TasksDelete(int id)
        {
            var task = await _context.MyTasks
                .Include(t => t.Creator).ThenInclude(c => c.Account)
                .Include(t => t.Assignee).ThenInclude(s => s.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound();
            return View("Tasks/Delete", task);
        }

        [HttpPost]
        public async Task<IActionResult> TasksDeleteConfirmed(int id)
        {
            var task = await _context.MyTasks.FindAsync(id);
            if (task != null)
            {
                _context.MyTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(TasksIndex));
        }

        public async Task<IActionResult> TasksDetails(int id)
        {
            var task = await _context.MyTasks
                .Include(t => t.Creator).ThenInclude(c => c.Account)
                .Include(t => t.Assignee).ThenInclude(s => s.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound();
            return View("Tasks/Details", task);
        }
    }
}
