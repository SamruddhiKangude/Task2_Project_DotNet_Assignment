using StudentManagement.Core.DTOs;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces;

namespace StudentManagement.Infrastructure.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
    {
        var students = await _repository.GetAllAsync();
        return students.Select(s => new StudentDto
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email,
            Age = s.Age,
            Course = s.Course,
            CreatedDate = s.CreatedDate
        });
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);
        if (student == null) return null;

        return new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Age = student.Age,
            Course = student.Course,
            CreatedDate = student.CreatedDate
        };
    }

    public async Task<StudentDto> AddStudentAsync(CreateStudentDto studentDto)
    {
        var student = new Student
        {
            Name = studentDto.Name,
            Email = studentDto.Email,
            Age = studentDto.Age,
            Course = studentDto.Course,
            CreatedDate = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(student);

        return new StudentDto
        {
            Id = created.Id,
            Name = created.Name,
            Email = created.Email,
            Age = created.Age,
            Course = created.Course,
            CreatedDate = created.CreatedDate
        };
    }

    public async Task<bool> UpdateStudentAsync(int id, UpdateStudentDto studentDto)
    {
        var student = await _repository.GetByIdAsync(id);
        if (student == null) return false;

        student.Name = studentDto.Name;
        student.Email = studentDto.Email;
        student.Age = studentDto.Age;
        student.Course = studentDto.Course;

        await _repository.UpdateAsync(student);
        return true;
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);
        if (student == null) return false;

        await _repository.DeleteAsync(id);
        return true;
    }
}
