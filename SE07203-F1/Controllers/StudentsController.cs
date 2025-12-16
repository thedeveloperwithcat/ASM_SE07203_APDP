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
            // XỬ LÝ ACCOUNT (Tránh lỗi khóa ngoại) ---
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

            if (string.IsNullOrEmpty(student.StudentId))
            {
                student.StudentId = "SE" + DateTime.Now.Ticks.ToString().Substring(10);
            }
            student.FullName ??= student.Name; 
            student.Email ??= "student@test.com"; 

            // BỎ QUA VALIDATION KHÔNG CẦN THIẾT ---
            ModelState.Remove("Account");
            ModelState.Remove("AssignedTasks");
            ModelState.Remove("StudentId"); 
            ModelState.Remove("FullName");
            ModelState.Remove("Email");

            // LƯU DATABASE ---
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)); 
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