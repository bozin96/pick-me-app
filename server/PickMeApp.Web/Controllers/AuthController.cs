using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PickMeApp.Application.Models;
using PickMeApp.Core.Models;
using PickMeApp.Data.Interfaces;
using PickMeApp.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickMeApp.Web.Controllers
{
    [Authorize]
    [Route("api/auth")]
    public class AuthController : ApiController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(JwtTokenRequest request)
        {
            if (!string.IsNullOrEmpty(request.Username))
            {
                _logger.LogInformation($"User {request.Username} is attempting to request an API Auth Token.");
            }

            AuthenticationResult result = await _authService.LoginUserAsync(request);

            if (result == null || !result.Success)
                return BadRequest(new { error = "Could not verify username and password" });

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] JObject data)
        {
            string refreshToken = data.ContainsKey("refreshToken") ? data["refreshToken"].ToString() : "";
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { message = "There is no refresh token." });

            try
            {
                HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);
                var handler = new JwtSecurityTokenHandler();
                var formatedToken = authorizationToken.ToString().Substring(7);

                var response = await _authService.RefreshTokenAsync(formatedToken, refreshToken);

                if (response == null || !response.Success)
                    return StatusCode(StatusCodes.Status401Unauthorized, response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"You must send access token, error: {ex.Message}" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ResponseModelStateErrors();
            }

            var errors = new APIError();

            if (string.Compare(request.Password, request.ConfirmPassword) != 0)
            {
                errors.Errors.Add("Password and ConfirmPassword fields must be equal");
            }

            if (errors.Errors.Count > 0)
                return UnprocessableEntity(errors);

            AuthenticationResult result = await _authService.RegisterUserAsync(request);

            if (result == null || !result.Success)
                return BadRequest(new { error = "Could not verify username and password" });

            return Ok(result);
        }
    }
}
