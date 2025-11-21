using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;

namespace SE07203_F1.Controllers
{
    public class LoginController : Controller
    {
        readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
                
                if (account == null)
                {
                    return View("Error");
                }
                else
                {
                    HttpContext.Session.SetString("username", username);
                    HttpContext.Session.SetString("fullname", account.Fullname);
                    HttpContext.Session.SetInt32("id", account.Id);
                    return View("Success");
                }
                
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            
        }
    }
}
