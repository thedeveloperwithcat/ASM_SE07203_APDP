using SE07203_F1.Models;
using System.Collections.Generic;

namespace SE07203_F1.Repositories
{
    public interface IStudentRepository
    {
        // Lấy danh sách (có tìm kiếm)
        IEnumerable<Student> GetStudents(string keyword);

        // Lấy chi tiết 1 sinh viên
        Student? GetStudentById(int id);

        // Thêm mới (trả về void hoặc Student đã thêm)
        void AddStudent(Student student);

        // Cập nhật
        void UpdateStudent(Student student);

        // Xóa
        void DeleteStudent(int id);
    }
}