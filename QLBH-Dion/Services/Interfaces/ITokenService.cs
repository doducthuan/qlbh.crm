using QLBH_Dion.Models.ViewModel;
using System.IdentityModel.Tokens.Jwt;

namespace QLBH_Dion.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(LoginViewModel account);
        bool ValidateToken(string authToken);
        JwtSecurityToken ParseToken(string tokenString);
    }
}
