﻿using System;

namespace PickMeApp.Application.Models
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public TimeSpan TokenLifetime { get; set; }

        public string ValidIssuer { get; set; }

        public string ValidAudience { get; set; }
    }
}
