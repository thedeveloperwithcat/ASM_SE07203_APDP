using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using SE07203_F1.Models;
using System;

namespace SE07203_F1.Controllers
{
    public class StudentTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        // 1. XEM DANH SÁCH TASK
        // ===============================
        public async Task<IActionResult> Index()
        {
            var accountId = HttpContext.Session.GetInt32("id");
            var role = HttpContext.Session.GetString("role");

            if (accountId == null || role != "Student")
            {
                return RedirectToAction("Index", "Login");
            }

            var student = await _context.Students
                .Include(s => s.AssignedTasks)
                    .ThenInclude(t => t.Category)
                .FirstOrDefaultAsync(s => s.AccountId == accountId);

            if (student == null)
            {
                return View(new List<MyTask>());
            }

            var tasks = await _context.MyTasks
                .Include(t => t.Category)
                .Include(t => t.Creator)
                    .ThenInclude(c => c.Account)
                .Where(t => t.AssigneeId == student.Id)
                .ToListAsync();

            return View(tasks);
        }

        // ===============================
        // 2. XEM CHI TIẾT TASK
        // ===============================
        public async Task<IActionResult> Details(int id)
        {
            var accountId = HttpContext.Session.GetInt32("id");
            var role = HttpContext.Session.GetString("role");

            if (accountId == null || role != "Student")
            {
                return RedirectToAction("Index", "Login");
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.AccountId == accountId);

            if (student == null)
            {
                return NotFound();
            }

            var task = await _context.MyTasks
                .Include(t => t.Category)
                .Include(t => t.Creator)
                    .ThenInclude(c => c.Account)
                .FirstOrDefaultAsync(t =>
                    t.Id == id && t.AssigneeId == student.Id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }
    }
}
