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

    public List<AttendanceDto> GetAllAttendances(string groupName, string academicYear, string subjectName, string sessionType)
    {
        string query = "SELECT * " +
                       "FROM Attendance AS A " +
                       "JOIN StudentGroups SG ON A.GroupID = SG.StudentGroupID " +
                       "JOIN Subjects S ON A.SubjectID = S.SubjectID " +
                       "JOIN Students STU ON A.StudentID = STU.StudentID " +
                       "JOIN Teachers T ON A.TeacherID = T.TeacherID " +
                       "JOIN SubjectTypes ST2 on A.TypeID = ST2.TypeID "+
                       "WHERE SG.GroupName = '" + groupName + "' AND SG.AcademicYear = '" + academicYear + "' AND S.SubjectName = '" + subjectName + "' AND ST2.TypeName = '" + sessionType + "'";
        var attendancesDataTable = _databaseHelper.ExecuteQuery(query);
        var attendanceEntities = _dataHelper.DataTableToList<AttendanceEntity>(attendancesDataTable);
        var attendanceDto = new List<AttendanceDto>();
        foreach (var entity in attendanceEntities)
        {
            attendanceDto.Add(_attendanceMapper.MapAttendanceEntityToAttendanceDto(entity));
        }
        return attendanceDto;
    }

    public void AddAttendance(AttendanceCreationDto attendanceDto)
    {
        try
        {
            var attendanceEntity = _attendanceMapper.MapAttendanceDtoToAttendanceEntity(attendanceDto);
           string query = "INSERT INTO Attendance (StudentID, TeacherID, TypeID, Date, Time, Status, Comments, Auditorium, SubjectID, GroupID) " +
                          "VALUES (" + attendanceEntity.StudentID + ", " + attendanceEntity.TeacherID + ", " + attendanceEntity.TypeID + ", '" + attendanceEntity.Date + "', '" + attendanceEntity.Time + "', '" + attendanceEntity.Status + "', '" + attendanceEntity.Comments + "', '" + attendanceEntity.Auditorium + "', " + attendanceEntity.SubjectID + ", " + attendanceEntity.GroupID + ")";
            _databaseHelper.ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}