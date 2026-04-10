using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
