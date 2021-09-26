using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PickMeApp.Application.Models
{
    public class JwtTokenRequest
    {
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
