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
        // BỔ SUNG: Tham số SecretKey để nhận mã bí mật
        public async Task<IActionResult> RegisterAccount(string Username, string Fullname, string Password, string Role, string SecretKey)
        {
            ViewBag.IsError = false;
            try
            {
                // --- 1. LOGIC BẢO MẬT ADMIN (Kiểm tra đầu tiên) ---
                if (Role == "Admin")
                {
                    // Mã bí mật quy định trước (nên khớp với hướng dẫn View)
                    string requiredKey = "SE07203_ADMIN_KEY";

                    if (SecretKey != requiredKey)
                    {
                        ViewBag.IsError = true;
                        ViewBag.Error = "Sai mã bí mật! Bạn không có quyền đăng ký Admin.";
                        return View("Index");
                    }
                }

                // --- 2. Kiểm tra tài khoản tồn tại ---
                var existingAccount = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == Username);
                if (existingAccount != null)
                {
                    ViewBag.IsError = true;
                    ViewBag.Error = "Tên đăng nhập đã tồn tại.";
                    return View("Index");
                }

                // --- 3. Tạo tài khoản ---
                Account account = new Account();
                account.Username = Username;
                account.Fullname = Fullname;
                account.Password = HashPassword(Password);
                account.Role = Role; // Lưu Role (Student/Teacher/Admin)
                account.Status = "Active";
                account.Email = Username + "@gmail.com";

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                ViewBag.Success = true;
                ViewBag.Error = null;

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.Error = "Lỗi hệ thống: " + ex.Message;
                return View("Index");
            }
        }
    }
}