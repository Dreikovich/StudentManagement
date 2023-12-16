using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Mappers;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;
    
    public StudentsController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    
    [HttpGet]
    public IActionResult GetAllStudents()
    {
        try
        {
            var students = _studentRepository.GetAllStudents();
            return Ok(students);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpPost]
    public IActionResult AddStudent(StudentDto studentDto)
    {
        try
        {
            _studentRepository.AddStudent(studentDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
}