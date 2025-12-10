using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using SE07203_F1.Models;

namespace SE07203_F1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string search)
        {
            var query = _context.Students.Include(s => s.Account).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Name.Contains(search) || s.Class.Contains(search));
                ViewData["CurrentFilter"] = search;
            }

            return View(await query.ToListAsync());
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            // --- 1. XỬ LÝ ACCOUNT (Tránh lỗi khóa ngoại) ---
            var defaultAccount = await _context.Accounts.FirstOrDefaultAsync();
            if (defaultAccount == null)
            {
                defaultAccount = new Account
                {
                    Fullname = "Admin System",
                    Username = "admin",
                    Password = "123",
                    Email = "admin@test.com",
                    Role = "Admin",
                    Status = "Active"
                };
                _context.Accounts.Add(defaultAccount);
                await _context.SaveChangesAsync();
            }
            student.AccountId = defaultAccount.Id;

            // --- 2. XỬ LÝ DỮ LIỆU THIẾU (Tránh lỗi Validation) ---
            // Nếu người dùng không nhập StudentId (Mã SV dạng chuỗi), tự sinh ngẫu nhiên
            if (string.IsNullOrEmpty(student.StudentId))
            {
                student.StudentId = "SE" + DateTime.Now.Ticks.ToString().Substring(10);
            }
            // Mấy trường này trong Model không cho null, nên nếu form gửi lên null phải gán giá trị mặc định
            student.FullName ??= student.Name; // Gán FullName bằng Name nếu thiếu
            student.Email ??= "student@test.com"; // Email tạm

            // --- 3. BỎ QUA VALIDATION KHÔNG CẦN THIẾT ---
            // Báo cho hệ thống biết: "Đừng kiểm tra Account và AssignedTasks, tôi tự lo được"
            ModelState.Remove("Account");
            ModelState.Remove("AssignedTasks");
            ModelState.Remove("StudentId"); // Đã xử lý ở trên
            ModelState.Remove("FullName");
            ModelState.Remove("Email");

            // --- 4. LƯU DATABASE ---
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)); // Lưu xong tải lại trang
                }
                catch (Exception ex)
                {
                    // Nếu lỗi SQL, hiện ra màn hình để biết đường sửa
                    ModelState.AddModelError("", "Lỗi Database: " + ex.InnerException?.Message ?? ex.Message);
                }
            }
            else
            {
                // Nếu lỗi Validation, liệt kê lỗi ra (debug)
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var err in errors)
                {
                    ModelState.AddModelError("", "Lỗi nhập liệu: " + err);
                }
            }

            // Nếu thất bại, load lại danh sách cũ và hiện lỗi
            return View("Index", await _context.Students.ToListAsync());
        }
    }
}



//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SE07203_F1.Data; // Thay ProjectName bằng namespace của bạn (thường là tên project)
//using SE07203_F1.Models; // Thay ProjectName bằng namespace của bạn

//namespace SE07203_F1.Controllers
//{
//    public class StudentsController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public StudentsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // Action hiển thị danh sách sinh viên
//        public async Task<IActionResult> Index()
//        {
//            // Lấy danh sách sinh viên từ database
//            var students = await _context.Students.ToListAsync();
//            return View(students);
//        }
//    }
//}