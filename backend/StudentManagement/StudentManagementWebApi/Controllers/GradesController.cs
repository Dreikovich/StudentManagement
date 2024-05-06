using Messaging.Services;
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
    
    
    public GradesController(IGradeRepository gradeRepository, RabbitMqPublisher publisher, RabbitMqConnectionService connectionService)
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
                    _publisher.Publish("Grade added successfully.");
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