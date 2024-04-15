using starter_template.Dtos;

namespace starter_template.Interfaces;

public interface ITokenService
{
    string CreateJwtToken(UserDto user);
}