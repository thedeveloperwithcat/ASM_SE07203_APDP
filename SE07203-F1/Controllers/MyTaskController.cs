using Microsoft.AspNetCore.Mvc;
using SE07203_F1.Data;
using SE07203_F1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SE07203_F1.Controllers
{
    public class MyTaskController : Controller
    {
        readonly ApplicationDbContext _context;

        public MyTaskController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Kiểm tra đăng nhập và Role
            var accountId = HttpContext.Session.GetInt32("id");
            var role = HttpContext.Session.GetString("role");

            if (accountId == null || role != "Teacher")
            {
                return RedirectToAction("Index", "Login");
            }

            // Tìm Teacher dựa trên AccountId
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.AccountId == accountId);
            if (teacher == null)
            {
                // Nếu là Teacher nhưng chưa có dữ liệu trong bảng Teachers
                return View(new List<MyTask>());
            }

            // Lấy danh sách Task mà CreatorId == Teacher.Id
            var tasks = await _context.MyTasks
                .Include(t => t.Category)
                .Include(t => t.Assignee) // Include để hiện tên sinh viên
                .Where(t => t.CreatorId == teacher.Id)
                .ToListAsync();

            return View(tasks);
        }



        [HttpPost]
        public async Task<IActionResult> Create(MyTask _myTask) // Dùng tên biến _myTask như bạn yêu cầu
        {
            // 1. Lấy Account ID từ session
            int? MyAccountId = HttpContext.Session.GetInt32("id");

            // 2. Tìm Teacher tương ứng để lấy CreatorId (QUAN TRỌNG)
            // Nếu không có bước này, logic lọc ở trang Index sẽ không thấy task
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.AccountId == MyAccountId);

            if (teacher != null)
            {
                _myTask.AccountId = MyAccountId;
                _myTask.CreatorId = teacher.Id; // Gán ID giáo viên vào người tạo
                _myTask.CategoryId = 1;         // Mặc định Category
                _myTask.AssigneeId = null;      // Mới tạo chưa giao cho ai

                _context.MyTasks.Add(_myTask);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Nếu lỗi hoặc không tìm thấy giáo viên
            ViewBag.StudentList = new SelectList(_context.Students, "Id", "Name");
            return View(_myTask);
        }

        public async Task<IActionResult> Assign(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.MyTasks.FindAsync(id);
            if (task == null) return NotFound();

            // ViewBag cho Dropdown list sinh viên
            ViewBag.StudentId = new SelectList(_context.Students, "Id", "Name");
            return View(task);
        }
        [HttpPost]
        public async Task<IActionResult> Assign(int id, int AssigneeId)
        {
            var task = await _context.MyTasks.FindAsync(id);
            if (task != null)
            {
                task.AssigneeId = AssigneeId;
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.StudentList = new SelectList(_context.Students, "Id", "Name");
            ViewBag.IsError = false;
            ViewBag.Success = false;
            ViewBag.Error = string.Empty;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            ViewBag.IsError = false;
            ViewBag.Success = false;
            ViewBag.Error = string.Empty;
            var task = await _context.MyTasks.FirstOrDefaultAsync(t => t.Id == id);
            //MyTask myTask = new MyTask();
            //myTask.Id = id;
            if (task != null)
            {
                _context.MyTasks.Remove(task);  // 202511191537 
                // sau khi xóa thì lưu nó lại 
                await _context.SaveChangesAsync();
                // _context.SaveChanges();
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string description)
        {
            var task = await _context.MyTasks.FindAsync(id);
            if (task == null) return NotFound();

            // Update fields
            task.Name = name;
            task.Description = description;

            _context.Update(task);
            await _context.SaveChangesAsync();

            // Return status code 200 (OK)
            return Ok(new { success = true });
        }

    }
}
