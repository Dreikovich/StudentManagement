using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface ILoginRepository
{
    void AddLogin(LoginDto loginDto);

    bool ValidateLogin(LoginDto loginDto);

    string GetStudentUuid(string login);

    void SaveTokenToDatabase(string token, DateTime expirationDate, string userUuid);

    bool IsTokenValid(string token);

    string GetUserRole(LoginDto loginDto);
}