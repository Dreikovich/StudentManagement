using System.Text.Json;
using MessagePublisher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NotificationService.Hubs;
using NotificationService.Models;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly IGradeRepository _gradeRepository;
    private readonly RabbitMqPublisher _publisher;
    private readonly IStudentRepository _studentRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IDistributedCache _connectedClientsCache;

    public GradesController(IGradeRepository gradeRepository, IStudentRepository studentRepository,
        ISubjectTypesRepository subjectTypesRepository, ISubjectRepository subjectRepository, RabbitMqPublisher publisher, IDistributedCache cache)
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _subjectRepository = subjectRepository;
        _publisher = publisher;
        _connectedClientsCache = cache;
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
                    var message = ProcessTheDto(gradeDto);
                    var messageJson = JsonSerializer.Serialize(message);
                    _publisher.Publish(messageJson);
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

    private Message ProcessTheDto(GradeDto gradeDto)
    {
        var student = _studentRepository.GetStudentById(gradeDto.StudentID);
        var subject = _subjectRepository.GetSubjectById(gradeDto.SubjectID, gradeDto.TypeID);
       
        
        if ( subject == null || student == null)
        {
            throw new Exception("Student or subject not found.");
        }
       
        
        var message = new Message
        {
            MessageId = Guid.NewGuid().ToString(),
            UserId = student.StudentUuid,
            Content = $"Grade added for student {student.FirstName}{student.LastName} in subject {subject.SubjectName} {subject.SubjectComponents.First().TypeName} with value {gradeDto.GradeValue}"
        };
        return message;

    }
}

