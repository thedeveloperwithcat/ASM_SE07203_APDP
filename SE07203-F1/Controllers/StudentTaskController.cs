using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using SE07203_F1.Models;

namespace SE07203_F1.Controllers
{
    public class StudentTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Kiểm tra đăng nhập
            var accountId = HttpContext.Session.GetInt32("id");
            var role = HttpContext.Session.GetString("role");

            if (accountId == null || role != "Student")
            {
                return RedirectToAction("Index", "Login");
            }

            // 2. Tìm thông tin Sinh viên dựa trên AccountId
            var student = await _context.Students.FirstOrDefaultAsync(s => s.AccountId == accountId);

            if (student == null)
            {
                // Trường hợp có tài khoản nhưng chưa có hồ sơ sinh viên
                return View(new List<MyTask>());
            }

            // 3. Lấy danh sách Task được giao cho sinh viên này (AssigneeId == student.Id)
            var tasks = await _context.MyTasks
                .Include(t => t.Category)
                .Include(t => t.Creator)              // Lấy thông tin Giáo viên
                    .ThenInclude(tr => tr.Account)    // Lấy tên Account của giáo viên
                .Where(t => t.AssigneeId == student.Id)
                .ToListAsync();

            return View(tasks);
        }
    }
}