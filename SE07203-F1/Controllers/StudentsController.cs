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
            // --- BẮT ĐẦU ĐOẠN SỬA ---
            // LOGIC CŨ SAI: Lấy account admin gán cho sinh viên -> Gây lỗi Unique
            // LOGIC MỚI: Tạo account mới riêng cho sinh viên này

            //1. Tự sinh StudentId nếu chưa có (giữ nguyên logic của bạn)
            if (string.IsNullOrEmpty(student.StudentId))
            {
                student.StudentId = "SE" + DateTime.Now.Ticks.ToString().Substring(10);
            }

            //2. Tạo Account mới
            var newStudentAccount = new Account
            {
                // Lấy mã SV làm username luôn để không bị trùng
                Username = student.StudentId,
                Fullname = student.Name ?? "New Student", // Lấy tên SV sang
                Password = "123", // Pass mặc định
                Email = (student.StudentId) + "@student.fpt.edu.vn", // Email giả lập theo mã SV
                Role = "Student",
                Status = "Active"
            };

            //3. Lưu Account trước để lấy Id
            _context.Accounts.Add(newStudentAccount);
            await _context.SaveChangesAsync();

            //4. Gán Id của account vừa tạo vào sinh viên
            student.AccountId = newStudentAccount.Id;
            student.FullName = student.Name;

            // Ensure DB non-nullable columns have values to avoid constraint errors
            student.Name = student.Name ?? string.Empty;
            student.Class = student.Class ?? string.Empty;
            student.Major = student.Major ?? string.Empty;
            student.Status = string.IsNullOrEmpty(student.Status) ? "Active" : student.Status;
            student.Teacher = student.Teacher ?? string.Empty;
            student.Note = student.Note ?? string.Empty;

            // If Email not provided, use account email
            student.Email = string.IsNullOrEmpty(student.Email) ? newStudentAccount.Email : student.Email;

            // If BirthDate not provided, set a sensible default (avoid DateTime.MinValue in UI)
            if (student.BirthDate == default)
            {
                student.BirthDate = DateTime.UtcNow.Date;
            }

            // Các đoạn logic dưới (ModelState.Remove, Validation...) giữ nguyên như cũ
            ModelState.Remove("Account");
            ModelState.Remove("AssignedTasks");
            ModelState.Remove("StudentId");
            ModelState.Remove("FullName");
            ModelState.Remove("Email");

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
                    ModelState.AddModelError("", "Lỗi: " + ex.InnerException?.Message ?? ex.Message);
                }
            }
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