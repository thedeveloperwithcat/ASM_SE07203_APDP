using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using SE07203_F1.Models;
using System;

namespace SE07203_F1.Controllers
{
    public class MyTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        // Index - danh sách task của Teacher
        // ===============================
        public async Task<IActionResult> Index()
        {
            var accountId = HttpContext.Session.GetInt32("id");
            var role = HttpContext.Session.GetString("role");

            if (accountId == null || role != "Teacher")
            {
                return RedirectToAction("Index", "Login");
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.AccountId == accountId);

            if (teacher == null)
            {
                // Nếu chưa có teacher tương ứng → redirect login hoặc thông báo
                ViewBag.Error = "Teacher không tồn tại trong hệ thống.";
                return RedirectToAction("Index", "Login");
            }

            var tasks = await _context.MyTasks
                .Include(t => t.Assignee)
                .Include(t => t.Category)
                .Where(t => t.CreatorId == teacher.Id)
                .ToListAsync();

            return View(tasks);
        }

        // ===============================
        // Details
        // ===============================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.MyTasks
                .Include(t => t.Assignee)
                .Include(t => t.Category)
                .Include(t => t.Creator)
                    .ThenInclude(c => c.Account)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null) return NotFound();

            return View(task);
        }

        // ===============================
        // Create
        // ===============================
        public IActionResult Create()
        {
            ViewBag.Students = _context.Students.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MyTask task)
        {
            var accountId = HttpContext.Session.GetInt32("id");
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.AccountId == accountId);

            if (teacher == null) return RedirectToAction("Index", "Login");

            task.CreatorId = teacher.Id;

            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(task);
        }

        // ===============================
        // Edit
        // ===============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.MyTasks.FindAsync(id);
            if (task == null) return NotFound();

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MyTask task)
        {
            if (id != task.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MyTasks.Any(e => e.Id == task.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(task);
        }

        // ===============================
        // Delete
        // ===============================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.MyTasks
                .Include(t => t.Assignee)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.MyTasks.FindAsync(id);
            _context.MyTasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
