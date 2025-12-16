using Microsoft.AspNetCore.Mvc;
using SE07203_F1.Models;
using SE07203_F1.Repositories;

namespace SE07203_F1.Controllers
{
    public class StudentsController : Controller
    {
        // Khai báo Interface thay vì DbContext
        private readonly IStudentRepository _studentRepo;

        public StudentsController(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        // 1. Xem danh sách + Tìm kiếm
        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var students = _studentRepo.GetStudents(searchString);
            return View(students);
        }

        // 2. Form thêm mới
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {

            ModelState.Remove("StudentId");

            ModelState.Remove("Account");


            if (ModelState.IsValid)
            {
                _studentRepo.AddStudent(student);
                return RedirectToAction(nameof(Index));
            }


            return View(student);
        }

        // 3. Form Sửa
        public IActionResult Edit(int id)
        {
            var student = _studentRepo.GetStudentById(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // Xử lý Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _studentRepo.UpdateStudent(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // 4. Xử lý Xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _studentRepo.DeleteStudent(id);
            return RedirectToAction(nameof(Index));
        }
    }
}