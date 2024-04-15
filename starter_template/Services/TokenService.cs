using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using starter_template.Dtos;
using starter_template.Interfaces;
using starter_template.Services;

namespace starter_template.Services;

//This called primary constructor, a short hand for the normal constructor
public class TokenService(IConfiguration configuration) : ITokenService
{
    //Encryption key of JWT token
    private readonly SymmetricSecurityKey _encryptionKey = new(Encoding.UTF8.GetBytes(configuration["JwtKey"]));


    //Assign encryption key from app settings file

    public string CreateJwtToken(UserDto user)
    {
        //Claims are what inside the body of the JWT
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id),
            new Claim("Username", user.Username)
        };

        //This will what key to use and what algorithm will be used for encryption
        var credentials = new SigningCredentials(_encryptionKey, SecurityAlgorithms.HmacSha512Signature);

        //Contains the data to be used when creating a token like data, the signing credentials and the expiry date
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            //You can edit this expiration time. Currently expires after 1 hour
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = credentials
        };

        //This object will handle the the creation of token 
        var tokenHandler = new JwtSecurityTokenHandler();


        //Create the token
        var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

        //This function serialize JWT into a readable form as string
        return tokenHandler.WriteToken(jwtToken);
    }
}