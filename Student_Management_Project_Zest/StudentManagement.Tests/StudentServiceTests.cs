using FluentAssertions;
using Moq;
using StudentManagement.Core.DTOs;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces;
using StudentManagement.Infrastructure.Services;
using Xunit;

namespace StudentManagement.Tests;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _mockRepo;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _mockRepo = new Mock<IStudentRepository>();
        _service = new StudentService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new Student { Id = 2, Name = "Jane Doe", Email = "jane@example.com" }
        };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(students);

        // Act
        var result = await _service.GetAllStudentsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Select(s => s.Name).Should().Contain("John Doe");
    }

    [Fact]
    public async Task GetStudentByIdAsync_ShouldReturnStudent_WhenStudentExists()
    {
        // Arrange
        var student = new Student { Id = 1, Name = "John Doe", Email = "john@example.com" };
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(student);

        // Act
        var result = await _service.GetStudentByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetStudentByIdAsync_ShouldReturnNull_WhenStudentDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Student)null!);

        // Act
        var result = await _service.GetStudentByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddStudentAsync_ShouldAddStudentAndReturnDto()
    {
        // Arrange
        var createDto = new CreateStudentDto { Name = "John", Email = "john@example.com" };
        var createdStudent = new Student { Id = 1, Name = "John", Email = "john@example.com" };
        
        _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Student>())).ReturnsAsync(createdStudent);

        // Act
        var result = await _service.AddStudentAsync(createDto);

        // Assert
        result.Id.Should().Be(1);
        result.Name.Should().Be("John");
        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public async Task UpdateStudentAsync_ShouldReturnTrue_WhenStudentUpdated()
    {
        // Arrange
        var student = new Student { Id = 1, Name = "Old Name" };
        var updateDto = new UpdateStudentDto { Name = "New Name" };
        
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(student);
        _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateStudentAsync(1, updateDto);

        // Assert
        result.Should().BeTrue();
        student.Name.Should().Be("New Name");
        _mockRepo.Verify(repo => repo.UpdateAsync(student), Times.Once);
    }
}
