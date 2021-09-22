using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
