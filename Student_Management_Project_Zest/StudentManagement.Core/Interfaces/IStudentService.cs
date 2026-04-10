using StudentManagement.Core.DTOs;

namespace StudentManagement.Core.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<StudentDto?> GetStudentByIdAsync(int id);
    Task<StudentDto> AddStudentAsync(CreateStudentDto studentDto);
    Task<bool> UpdateStudentAsync(int id, UpdateStudentDto studentDto);
    Task<bool> DeleteStudentAsync(int id);
}
