using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectGroupAssignmentController : ControllerBase
{
    private readonly ISubjectGroupAssignmentRepository _subjectGroupAssignmentRepository;

    public SubjectGroupAssignmentController(ISubjectGroupAssignmentRepository subjectGroupAssignmentRepository)
    {
        _subjectGroupAssignmentRepository = subjectGroupAssignmentRepository;
    }

    [HttpGet]
    public IActionResult GetSubjectGroupAssignments()
    {
        var subjectGroupAssignments = _subjectGroupAssignmentRepository.GetSubjectGroupAssignments();
        return Ok(subjectGroupAssignments);
    }

    [HttpPost]
    public IActionResult AddSubjectGroupAssignment(
        [FromBody] SubjectGroupAssignmentCreateDto subjectGroupAssignmentCreateDto)
    {
        _subjectGroupAssignmentRepository.AddSubjectGroupAssignment(subjectGroupAssignmentCreateDto);
        return Ok();
    }
}