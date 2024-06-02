using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Mappers;

public interface IAttendanceMapper
{
    AttendanceDto MapAttendanceEntityToAttendanceDto(AttendanceDto attendance);

    AttendanceMapper MapAttendanceDtoToAttendanceEntity(AttendanceDto attendance);
}