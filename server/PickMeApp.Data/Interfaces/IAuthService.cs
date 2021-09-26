using PickMeApp.Application.Models;
using System.Threading.Tasks;

namespace PickMeApp.Data.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationResult> LoginUserAsync(JwtTokenRequest request);

        Task<AuthenticationResult> RefreshTokenAsync(string jwtToken, string refreshToken);

        Task<AuthenticationResult> RegisterUserAsync(RegisterUserRequest request);
    }
}
