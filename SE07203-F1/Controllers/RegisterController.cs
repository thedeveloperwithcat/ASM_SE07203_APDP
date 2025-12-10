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
        // Thêm tham số Role vào đây
        public async Task<IActionResult> RegisterAccount(string Username, string Fullname, string Password, string Role)
        {
            ViewBag.IsError = false;
            try
            {
                var existingAccount = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == Username);
                if (existingAccount != null)
                {
                    ViewBag.IsError = true;
                    ViewBag.Error = "Tên đăng nhập đã tồn tại.";
                    return View("Index");
                }

                Account account = new Account();
                account.Username = Username;
                account.Fullname = Fullname;
                account.Password = HashPassword(Password);

                // Lấy Role từ form gửi lên (Student hoặc Teacher)
                account.Role = Role;

                account.Status = "Active";
                account.Email = Username + "@gmail.com"; // Logic cũ của bạn

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                ViewBag.Success = true;
                ViewBag.Error = null; // Xóa lỗi cũ nếu có

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View("Index");
            }
        }
    }
}