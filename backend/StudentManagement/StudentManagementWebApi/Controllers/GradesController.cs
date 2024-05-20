using System.Text.Json;
using MessagePublisher.Services;
using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly IGradeRepository _gradeRepository;
    private readonly RabbitMqPublisher _publisher;
    
    public GradesController(IGradeRepository gradeRepository, RabbitMqPublisher publisher)
    {
        _gradeRepository = gradeRepository;
        _publisher = publisher;
    }
    
    [HttpPost]
    public IActionResult AddGrade([FromBody] GradeDto gradeDto)
    {
        try
        {
            _gradeRepository.AddGrade(gradeDto);
            Task.Run(() =>
            {
                try
                {
                    var message = new
                    {
                        UserId = gradeDto.StudentID.ToString(),
                        Content = $"Grade added for student {gradeDto.StudentID} in subject {gradeDto.SubjectID} with value {gradeDto.GradeValue}"
                    };
                    var mesaageJson = JsonSerializer.Serialize(message);
                    _publisher.Publish(mesaageJson);
                }
                catch (Exception publishEx)
                {
                    Console.WriteLine($"Failed to publish message: {publishEx.Message}");
                }
            });
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
}