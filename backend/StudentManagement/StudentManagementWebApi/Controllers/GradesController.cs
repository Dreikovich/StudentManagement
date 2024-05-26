using System.Text.Json;
using MessagePublisher.Services;
using Microsoft.AspNetCore.Mvc;
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
    private readonly ISubjectTypesRepository _subjectTypesRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly NotificationHub _notificationHub;

    public GradesController(IGradeRepository gradeRepository, IStudentRepository studentRepository,
        ISubjectTypesRepository subjectTypesRepository, ISubjectRepository subjectRepository, RabbitMqPublisher publisher, NotificationHub notificationHub)
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _subjectTypesRepository = subjectTypesRepository;
        _subjectRepository = subjectRepository;
        _publisher = publisher;
        _notificationHub = notificationHub;
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
                    if(_notificationHub.IsClientConnected(gradeDto.StudentID.ToString()))
                    {
                        _notificationHub.SendMessageToStudent(gradeDto.StudentID.ToString(), messageJson);
                    }
                    else
                    {
                        _publisher.Publish(messageJson);
                    }
                    
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
            UserId = gradeDto.StudentID.ToString(),
            Content = $"Grade added for student {student.FirstName}{student.LastName} in subject {subject.SubjectName} {subject.SubjectComponents.First().TypeName} with value {gradeDto.GradeValue}"
        };
        return message;

    }
    
}

