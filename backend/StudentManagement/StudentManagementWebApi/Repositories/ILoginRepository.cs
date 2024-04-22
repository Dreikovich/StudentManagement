using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface ILoginRepository
{
    void AddLogin(LoginDto loginDto);
    
    bool ValidateLogin(LoginDto loginDto);
    
    void SaveTokenToDatabase(string token, DateTime expirationDate, string studentUuid);
    
    bool IsTokenValid(string token);
}