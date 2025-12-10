using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using SE07203_F1.Models;

namespace SE07203_F1.Controllers
{
    public class MyTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyTask
        public async Task<IActionResult> Index()
        {
            // Kiểm tra đăng nhập
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Login");
            }

            var role = HttpContext.Session.GetString("role");
            var accountId = HttpContext.Session.GetInt32("id");

            // Khởi tạo truy vấn cơ bản (Load kèm Category và Assignee để hiển thị tên)
            var tasksQuery = _context.MyTasks
                .Include(m => m.Category)
                .Include(m => m.Assignee) // Load thông tin Sinh viên được giao
                .AsQueryable();

            // --- PHÂN QUYỀN DỮ LIỆU ---
            if (role == "Teacher")
            {
                // Giáo viên: Chỉ xem task mà mình (AccountId) là người tạo
                // Giả sử trong Model MyTask bạn dùng AccountId để lưu người tạo
                tasksQuery = tasksQuery.Where(t => t.AccountId == accountId);
            }
            else if (role == "Student")
            {
                // Sinh viên: Chỉ xem task được giao cho mình
                // Cần tìm StudentId tương ứng với AccountId đang đăng nhập
                var studentProfile = await _context.Students.FirstOrDefaultAsync(s => s.AccountId == accountId);

                if (studentProfile != null)
                {
                    tasksQuery = tasksQuery.Where(t => t.AssigneeId == studentProfile.Id);
                }
                else
                {
                    // Nếu là Student nhưng chưa có hồ sơ Student thì không thấy task nào
                    tasksQuery = tasksQuery.Where(t => false);
                }
            }
            // Admin: Xem hết (không lọc)

            return View(await tasksQuery.ToListAsync());
        }

        // GET: MyTask/Create
        public IActionResult Create()
        {
            // Chỉ Admin hoặc Teacher được tạo Task
            var role = HttpContext.Session.GetString("role");
            if (role != "Admin" && role != "Teacher")
            {
                return RedirectToAction("Index", "Home"); // Hoặc trang báo lỗi quyền
            }

            // Load danh sách Sinh viên để chọn người giao (Assignee)
            ViewBag.StudentList = _context.Students.ToList();
            // Load danh sách Category
            ViewBag.CategoryList = _context.Categories.ToList();

            return View();
        }

        // ... Các hàm Create [HttpPost], Edit, Delete bạn giữ nguyên hoặc bổ sung tương tự ...
    }
}




//using Microsoft.AspNetCore.Mvc;
//using SE07203_F1.Data;
//using SE07203_F1.Models;
//using Microsoft.EntityFrameworkCore;

//namespace SE07203_F1.Controllers
//{
//    public class MyTaskController : Controller
//    {
//        readonly ApplicationDbContext _context;

//        public MyTaskController(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public IActionResult Index()
//        {
//            ViewBag.IsError = false;
//            var tasks = new List<MyTask>();
//            try
//            {
//                int MyAccountId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
//                tasks = _context.MyTasks.Where(e  => e.AccountId == MyAccountId).ToList();
//                tasks = _context.MyTasks
//                    .Include(e => e.Category)
//                    .Include(e => e.Account)
//                    .ToList();
//            }
//            catch (Exception ex)
//            {
//                ViewBag.IsError = true;
//                ViewBag.Error = ex.Message;
//            }
//            return View(tasks);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(MyTask _myTask)
//        {
//            int MyAccountId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
//            _myTask.CategoryId = 1;
//            _myTask.AccountId = MyAccountId;
//            _context.MyTasks.Add(_myTask);
//            await _context.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

//        [HttpGet]
//        public async Task<IActionResult> Create()
//        {
//            ViewBag.IsError = false;
//            ViewBag.Success = false;
//            ViewBag.Error = string.Empty;
//            return View();
//        }
//        [HttpDelete]
//        public async Task<IActionResult> Remove(int id)
//        {
//            ViewBag.IsError = false;
//            ViewBag.Success = false;
//            ViewBag.Error = string.Empty;
//            var task = await _context.MyTasks.FirstOrDefaultAsync(t => t.Id == id);
//            //MyTask myTask = new MyTask();
//            //myTask.Id = id;
//            if (task != null)
//            {
//                _context.MyTasks.Remove(task);  // 202511191537 
//                // sau khi xóa thì lưu nó lại 
//                await _context.SaveChangesAsync();
//                // _context.SaveChanges();
//            }
//            return RedirectToAction("Index");

//        }
//        public async Task<IActionResult> Edit(int id, string name, string description)
//        {
//            // submit đúng hay chưa 
//            return RedirectToAction("Index");
//        }

//    }
//}
