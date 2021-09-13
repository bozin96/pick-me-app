using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Application.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> Errors { get; set; }

        //public void AddError(string error)
        //{
        //    Errors.Add(error);
        //    Success = false;
        //}

        //public void AddErrors(IEnumerable<string> errors)
        //{
        //    Errors.AddRange(errors);
        //    Success = false;
        //}
    }
}
