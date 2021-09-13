using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Application.Models
{
    public class JwtTokenRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
