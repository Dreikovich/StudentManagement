using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;
using StudentManagementWebApi.Mappers;

namespace StudentManagementWebApi.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly DataHelper _dataHelper;
    private readonly IStudentMapper _studentMapper;

    public StudentRepository(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
        _dataHelper = new DataHelper();
        _studentMapper = new StudentMapper();
    }

    public List<StudentDto> GetAllStudents(string search, List<string> filters)
    {
        try
        {
            var query =
                "Select S.StudentID, S.FirstName, S.LastName, S.Email, S.Gender, S.Status, SG.GroupName, S.StudentUuid from students as S" +
                " Left Join StudentGroupAssignment SGA on S.StudentID = SGA.StudentID" +
                " Left Join StudentGroups SG on SGA.StudentGroupID = SG.StudentGroupID";
            if (!string.IsNullOrEmpty(search))
                query += $" WHERE S.FirstName LIKE '%{search}%' OR S.LastName LIKE '%{search}%'";
            var studentsDataTable = _databaseHelper.ExecuteQuery(query);
            var studentEntities = _dataHelper.DataTableToList<StudentEntity>(studentsDataTable);
            var studentDtos = new List<StudentDto>();
            foreach (var entity in studentEntities)
                studentDtos.Add(_studentMapper.MapStudentEntityToStudentDto(entity));

            return studentDtos;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public StudentDto GetStudentById(int studentId)
    {
        try
        {
            var query =
                $"Select S.StudentID, S.FirstName, S.LastName, S.Email, S.StudentUuid from students as S WHERE S.StudentID = {studentId}";
            var studentDataTable = _databaseHelper.ExecuteQuery(query);
            var studentEntity = _dataHelper.DataTableToList<StudentEntity>(studentDataTable).FirstOrDefault();
            return _studentMapper.MapStudentEntityToStudentDto(studentEntity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void AddStudent(StudentDto studentDto)
    {
        try
        {
            var studentEntity = _studentMapper.MapStudentDtoToStudentEntity(studentDto);
            var query = $"INSERT INTO Students (FirstName, LastName, Email, Gender, Status, StudentUuid) VALUES " +
                        $"('{studentEntity.FirstName}', " +
                        $"'{studentEntity.LastName}', " +
                        $"'{studentEntity.Email}', " +
                        $"'{studentEntity.Gender}', " +
                        $"'{studentEntity.Status}', " +
                        $"'{studentEntity.StudentUuid}')";

            _databaseHelper.ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}