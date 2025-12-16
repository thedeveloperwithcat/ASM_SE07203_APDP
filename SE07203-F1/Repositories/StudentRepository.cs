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

            // --- SỬA LỖI TẠI ĐÂY ---
            // Gán giá trị tạm để vượt qua kiểm tra NOT NULL của Database
            // (Vì lúc này chưa có ID để sinh mã SV000X)
            student.StudentId = "TEMP_ID";

            // 2. Thêm vào DB (Lần này sẽ thành công vì StudentId đã có chữ "TEMP_ID")
            _context.Students.Add(student);
            _context.SaveChanges();

            // 3. Lúc này đã có student.Id (ví dụ = 5). Ta cập nhật lại mã thật.
            student.StudentId = $"SV{student.Id:D4}"; // Kết quả: SV0005

            // Gán trạng thái mặc định nếu chưa có
            if (string.IsNullOrEmpty(student.Status)) student.Status = "Active";

            // 4. Lưu lại lần cuối
            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            // Bước 1: Lấy dữ liệu cũ đang có trong Database (dùng AsNoTracking để không bị khóa record)
            var existingStudent = _context.Students.AsNoTracking().FirstOrDefault(s => s.Id == student.Id);

            if (existingStudent != null)
            {
                // Bước 2: Giữ lại AccountId cũ (vì form Sửa không gửi AccountId lên)
                student.AccountId = existingStudent.AccountId;

                // Bước 3: Đảm bảo các trường không nhập cũng không bị lỗi
                if (string.IsNullOrEmpty(student.Name)) student.Name = student.FullName;

                // Bước 4: Cập nhật
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