using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;
using StudentManagementWebApi.Models;

namespace StudentManagementWebApi.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly DataHelper _dataHelper;

    public SubjectRepository(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
        _dataHelper = new DataHelper();
    }

    public List<SubjectDto> GetAllSubjects()
    {
        var query = "select * from SubjectComponents " +
                    "JOIN Subjects S on SubjectComponents.SubjectID = S.SubjectID " +
                    "JOIN Teachers T on SubjectComponents.TeacherID = T.TeacherID " +
                    "JOIN SubjectTypes ST on SubjectComponents.TypeID = ST.TypeID";
        var subjectsDataTable = _databaseHelper.ExecuteQuery(query);
        var subjectsEntities = _dataHelper.DataTableToList<SubjectEntity>(subjectsDataTable);
        var groupedSubjects = subjectsEntities.GroupBy(s => new { s.SubjectID, s.SubjectName });
        var result = groupedSubjects.Select(group =>
        {
            var subject = group.First();
            return new SubjectDto
            {
                SubjectID = subject.SubjectID,
                SubjectName = subject.SubjectName,
                SubjectComponents = group.Select(s => new TypeDto
                {
                    TypeID = s.TypeID,
                    TypeName = s.TypeName,
                    Hours = s.Hours,
                    Teacher = new Teacher
                    {
                        TeacherID = s.Teacher.TeacherID,
                        TeacherFirstName = s.Teacher.TeacherFirstName,
                        TeacherLastName = s.Teacher.TeacherLastName,
                        TeacherEmail = s.Teacher.TeacherEmail
                    }
                }).ToList()
            };
        }).ToList();
        return result;
    }

    public SubjectDto GetSubjectById(int subjectId, int typeId)
    {
        try
        {
            var query = $"select * from SubjectComponents " +
                        $"JOIN Subjects S on SubjectComponents.SubjectID = S.SubjectID " +
                        $"JOIN Teachers T on SubjectComponents.TeacherID = T.TeacherID " +
                        $"JOIN SubjectTypes ST on SubjectComponents.TypeID = ST.TypeID " +
                        $"where S.SubjectID = {subjectId}";
            var subjectsDataTable = _databaseHelper.ExecuteQuery(query);
            var subjectsEntities = _dataHelper.DataTableToList<SubjectEntity>(subjectsDataTable)
                .Where(s => s.TypeID == typeId);

            var subjectDto = new SubjectDto
            {
                SubjectName = subjectsEntities.First().SubjectName,
                SubjectID = subjectsEntities.First().SubjectID,
                SubjectComponents = subjectsEntities.Select(s => new TypeDto
                {
                    TypeID = s.TypeID,
                    TypeName = s.TypeName,
                    Hours = s.Hours,
                    Teacher = new Teacher
                    {
                        TeacherID = s.Teacher.TeacherID,
                        TeacherFirstName = s.Teacher.TeacherFirstName,
                        TeacherLastName = s.Teacher.TeacherLastName,
                        TeacherEmail = s.Teacher.TeacherEmail
                    }
                }).ToList()
            };
            return subjectDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public void CreateSubject(SubjectCreationDto subjectCreationDto)
    {
        var subjectComponents = subjectCreationDto.SubjectComponents;
        var subjectName = subjectCreationDto.SubjectName;


        var query = string.Format("insert into Subjects (SubjectName) values ('{0}')", subjectName);
        _databaseHelper.ExecuteNonQuery(query);
        var subjectId = FindSubjectId(subjectName);
        foreach (var subjectComponent in subjectComponents)
        {
            var typeId = subjectComponent.TypeID;
            var hours = subjectComponent.Hours;
            var teacherId = subjectComponent.TeacherID;
            var query2 =
                string.Format(
                    "insert into SubjectComponents (SubjectID, TypeID, Hours, TeacherID) values ({0}, {1}, {2}, {3})",
                    subjectId, typeId, hours, teacherId);
            _databaseHelper.ExecuteNonQuery(query2);
        }
    }

    private int FindSubjectId(string subjectName)
    {
        var query = $"SELECT SubjectID FROM Subjects WHERE SubjectName = '{subjectName}'";

        var subjectDataTable = _databaseHelper.ExecuteQuery(query);

        var subjectEntity = _dataHelper.DataTableToList<SubjectEntity>(subjectDataTable);

        if (subjectEntity.Any()) return subjectEntity.First().SubjectID;

        // Handle the case where the subject is not found
        throw new Exception("Subject not found.");
    }
}