using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface IAttendanceRepository
{
    public List<AttendanceDto> GetAllAttendances(string group_name, string academic_year);
    
    public void AddAttendance(AttendanceCreationDto attendanceDto);
    
}