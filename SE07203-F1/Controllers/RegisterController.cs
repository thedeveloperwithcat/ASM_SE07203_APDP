using Microsoft.AspNetCore.Mvc;
using SE07203_F1.Data;
using SE07203_F1.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SE07203_F1.Controllers
{
    public class RegisterController : Controller
    {
        readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.IsError = false;
            return View();
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAccount(string Username, string Fullname, string Password)
        {
            ViewBag.IsError = false;
            try
            {
                // THÊM: Kiểm tra tên đăng nhập đã tồn tại chưa
                var existingAccount = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == Username);
                if (existingAccount != null)
                {
                    ViewBag.IsError = true;
                    ViewBag.Error = "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.";
                    return View("Index");
                }

                Account account = new Account();
                account.Username = Username;
                account.Fullname = Fullname;
                // Băm mật khẩu trước khi lưu
                account.Password = HashPassword(Password);

                // --- BẮT BUỘC PHẢI THÊM ĐOẠN NÀY ---
                account.Role = "Student";     
                account.Status = "Active";   
                account.Email = Username + "@gmail.com"; 

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                ViewBag.Success = true;
                ViewBag.Error = string.Empty;

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.Error = "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại.";
                return View("Index");
            }
        }
    }
}
