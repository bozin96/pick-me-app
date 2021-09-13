using PickMeApp.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
