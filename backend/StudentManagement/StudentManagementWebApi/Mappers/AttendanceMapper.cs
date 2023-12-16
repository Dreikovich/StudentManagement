using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;


namespace StudentManagementWebApi.Mappers;

public class AttendanceMapper
{
    private readonly IStudentMapper _studentMapper;
    public AttendanceMapper(IStudentMapper studentMapper)
    {
        _studentMapper = studentMapper;
    }
    public AttendanceDto MapAttendanceEntityToAttendanceDto(AttendanceEntity attendance)
    {
        return new AttendanceDto()
        {
            AttendanceID = attendance.AttendanceID,
            Student = _studentMapper.MapStudentEntityToStudentDto(attendance.StudentEntity),
            Teacher = attendance.Teacher,
            SubjectName = attendance.SubjectName,
            SessionName = attendance.TypeName,
            Date = attendance.Date,
            Time = attendance.Time,
            Status = attendance.Status,
            Comments = attendance.Comments,
            Auditorium = attendance.Auditorium,
            SubjectID = attendance.SubjectID,
            GroupID = attendance.GroupID
        };
    }
}