using System.Collections.Generic;

namespace PickMeApp.Application.Models
{
    public class AuthenticationResult
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
