using Microsoft.AspNetCore.Mvc;
using SE07203_F1.Data;
using SE07203_F1.Models;

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

        [HttpPost]
        public async Task<IActionResult> RegisterAccount(string Username, string Fullname, string Password)
        {
            ViewBag.IsError = false;
            try
            {
                Account account = new Account();
                account.Username = Username;
                account.Fullname = Fullname;
                account.Password = Password;
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                ViewBag.Success = true;
                ViewBag.Error = string.Empty;
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.Error = ex.Message;
                return View("Index");
            }
        }
    }
}
