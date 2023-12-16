using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;
using StudentManagementWebApi.Mappers;

namespace StudentManagementWebApi.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly DataHelper _dataHelper;
    private readonly AttendanceMapper _attendanceMapper;
    private readonly StudentMapper _studentMapper;

    
    public AttendanceRepository(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
        _dataHelper = new DataHelper();
        _studentMapper = new StudentMapper();
        _attendanceMapper = new AttendanceMapper(studentMapper: _studentMapper);

    }

    public List<AttendanceDto> GetAllAttendances(string groupName, string academicYear)
    {
        string query = "SELECT * " +
                       "FROM Attendance AS A " +
                       "JOIN StudentGroups SG ON A.GroupID = SG.StudentGroupID " +
                       "JOIN Subjects S ON A.SubjectID = S.SubjectID " +
                       "JOIN Students STU ON A.StudentID = STU.StudentID " +
                       "JOIN Teachers T ON A.TeacherID = T.TeacherID " +
                       "JOIN SubjectTypes ST2 on A.TypeID = ST2.TypeID "+
                       "WHERE SG.GroupName = '" + groupName + "' AND SG.AcademicYear = '" + academicYear + "'";
        var attendancesDataTable = _databaseHelper.ExecuteQuery(query);
        var attendanceEntities = _dataHelper.DataTableToList<AttendanceEntity>(attendancesDataTable);
        var attendanceDto = new List<AttendanceDto>();
        foreach (var entity in attendanceEntities)
        {
            attendanceDto.Add(_attendanceMapper.MapAttendanceEntityToAttendanceDto(entity));
        }
        return attendanceDto;
    }
}