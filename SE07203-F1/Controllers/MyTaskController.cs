using Microsoft.AspNetCore.Mvc;
using SE07203_F1.Data;
using SE07203_F1.Models;
using Microsoft.EntityFrameworkCore;

namespace SE07203_F1.Controllers
{
    public class MyTaskController : Controller
    {
        readonly ApplicationDbContext _context;

        public MyTaskController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.IsError = false;
            var tasks = new List<MyTask>();
            try
            {
                int MyAccountId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
                tasks = _context.MyTasks.Where(e  => e.AccountId == MyAccountId).ToList();
                tasks = _context.MyTasks
                    .Include(e => e.Category)
                    .Include(e => e.Account)
                    .ToList();
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.Error = ex.Message;
            }
            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MyTask _myTask)
        {
            int MyAccountId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            _myTask.CategoryId = 1;
            _myTask.AccountId = MyAccountId;
            _context.MyTasks.Add(_myTask);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.IsError = false;
            ViewBag.Success = false;
            ViewBag.Error = string.Empty;
            return View();
        }
        [HttpDelete]
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
        public async Task<IActionResult> Edit(int id, string name, string description)
        {
            // submit đúng hay chưa 
            return RedirectToAction("Index");
        }

    }
}
