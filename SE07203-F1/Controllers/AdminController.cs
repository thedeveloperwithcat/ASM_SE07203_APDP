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

        // Hàm kiểm tra quyền Admin tập trung
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("role");
            return role == "Admin";
        }

        // --- QUẢN LÝ SINH VIÊN ---
        public async Task<IActionResult> Students()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var students = await _context.Students
                                         .Include(s => s.Account) // Load thông tin tài khoản
                                         .ToListAsync();
            return View(students);
        }

        // --- QUẢN LÝ GIÁO VIÊN ---
        public async Task<IActionResult> Teachers()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var teachers = await _context.Teachers
                                         .Include(t => t.Account)
                                         .ToListAsync();
            return View(teachers);
        }

        // --- QUẢN LÝ TASK (Góc nhìn toàn cục của Admin) ---
        public async Task<IActionResult> Tasks()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var tasks = await _context.MyTasks
                                      .Include(t => t.Category)
                                      .Include(t => t.Account) // Người tạo
                                      .Include(t => t.Assignee) // Người được giao
                                      .ToListAsync();
            return View(tasks);
        }
    }
}