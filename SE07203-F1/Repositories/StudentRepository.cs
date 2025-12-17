using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using SE07203_F1.Models;
using System.Linq;

namespace SE07203_F1.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Student> GetStudents(string keyword)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => s.FullName.Contains(keyword)
                                      || s.StudentId.Contains(keyword)
                                      || s.Email.Contains(keyword));
            }

            return query.ToList();
        }

        public Student? GetStudentById(int id)
        {
            return _context.Students.Find(id);
        }

        public void AddStudent(Student student)
        {
            // 1. Logic đồng bộ tên (như đã làm trước đó)
            if (!string.IsNullOrEmpty(student.FullName) && string.IsNullOrEmpty(student.Name))
            {
                student.Name = student.FullName;
            }
            else if (!string.IsNullOrEmpty(student.Name) && string.IsNullOrEmpty(student.FullName))
            {
                student.FullName = student.Name;
            }

            student.StudentId = "TEMP_ID";


            _context.Students.Add(student);
            _context.SaveChanges();

           
            student.StudentId = $"SV{student.Id:D4}"; 

            if (string.IsNullOrEmpty(student.Status)) student.Status = "Active";


            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {

            var existingStudent = _context.Students.AsNoTracking().FirstOrDefault(s => s.Id == student.Id);

            if (existingStudent != null)
            {

                if (!string.IsNullOrEmpty(student.FullName) && string.IsNullOrEmpty(student.Name))
                {
                    student.Name = student.FullName;
                }
                // Ngược lại, nếu người dùng nhập Name mà quên FullName
                else if (!string.IsNullOrEmpty(student.Name) && string.IsNullOrEmpty(student.FullName))
                {
                    student.FullName = student.Name;
                }

                // 3. Giữ lại các thông tin quan trọng từ bản ghi cũ nếu form Edit không gửi lên
                student.AccountId = existingStudent.AccountId; // Giữ liên kết tài khoản
                if (string.IsNullOrEmpty(student.StudentId))
                {
                    student.StudentId = existingStudent.StudentId; // Giữ mã SV cũ nếu bị mất
                }

                // 4. Cập nhật vào Database
                _context.Students.Update(student);
                _context.SaveChanges();
            }
        }

        public void DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

    }
}