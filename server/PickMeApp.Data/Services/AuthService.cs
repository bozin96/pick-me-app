using iComplyICO.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PickMeApp.Application.Models;
using PickMeApp.Core.Constants;
using PickMeApp.Core.Models;
using PickMeApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PickMeApp.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthService(
                ApplicationDbContext dbContext,
                JwtSettings jwtSettings,
                TokenValidationParameters tokenValidationParameters,
                UserManager<ApplicationUser> userManager,
                RoleManager<ApplicationRole> roleManager)
        {
            _dbContext = dbContext;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AuthenticationResult> LoginUserAsync(JwtTokenRequest request)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(request.Username);
            bool userHasValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (userHasValidPassword && await _userManager.IsEmailConfirmedAsync(user))
            {

                return await GenerateAuthenticationResultForUserAsync(user);
            }

            return new AuthenticationResult()
            {
                Success = false,
                Token = null,
                RefreshToken = null,
                Errors = new List<string> { "Unable to login" }
            };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string jwtToken, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(jwtToken);

            if (validatedToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid Token" }, Success = false };
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc)
                    .AddSeconds(expiryDateUnix);

            //if (expiryDateTimeUtc > DateTime.UtcNow)
            //{
            //    return new AuthenticationResult { Errors = new[] { "This token hasn't expired yet" } };
            //}

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token does not exist" }, Success = false };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token has expired" }, Success = false };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token has been invalidated" }, Success = false };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token has been used" }, Success = false };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token does not match this JWT" }, Success = false };
            }

            storedRefreshToken.Used = true;
            _dbContext.RefreshTokens.Update(storedRefreshToken);
            await _dbContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == ClaimTypes.Name).Value);

            return await GenerateAuthenticationResultForUserAsync(user, storedRefreshToken);
        }

        public async Task<AuthenticationResult> RegisterUserAsync(RegisterUserRequest request)
        {
            AuthenticationResult result = new AuthenticationResult();

            ApplicationUser appUser = new ApplicationUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                DateCreated = DateTime.UtcNow,
                NormalizedUserName = request.Email.ToUpperInvariant(),
                NormalizedEmail = request.Email.ToUpperInvariant(),
                EmailConfirmed = true
            };

            var passwordValidator = new PasswordValidator<ApplicationUser>();
            var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, null, request.Password);

            if (!passwordValidationResult.Succeeded)
            {
                return new AuthenticationResult { Errors = passwordValidationResult.Errors.Select(e => e.Description), Success = false };
            }

            var passwordHash = _userManager.PasswordHasher.HashPassword(appUser, request.Password);
            appUser.PasswordHash = passwordHash;

            // Create User
            var creationResult = await _userManager.CreateAsync(appUser);

            // Add Client role to user
            await _userManager.AddToRoleAsync(appUser, ApplicationUserRole.Client);

            return await GenerateAuthenticationResultForUserAsync(appUser);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(ApplicationUser user, RefreshToken previousRefreshToken = null)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = await GenerateJwtTokenAsync(user);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            // Add Refresh Token
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                UserId = user.Id
            };
        }

        private async Task<SecurityToken> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(CustomClaimTypes.Id, user.Id),
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // add roles and role claims to jwt claims
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.ValidIssuer,
                Audience = _jwtSettings.ValidAudience,
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = creds
            };

            return tokenHandler.CreateToken(tokenDescriptor);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
