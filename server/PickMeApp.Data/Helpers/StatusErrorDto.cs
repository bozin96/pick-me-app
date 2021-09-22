using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Application.Helpers
{
    public class StatusErrorDto
    {
        public bool Status { get; set; }
        public List<string> Errors { get; set; }
        public int ErrorCode { get; set; }

        public StatusErrorDto()
        {
            Status = true;
            Errors = new List<string>();
            ErrorCode = 200;
        }

        public StatusErrorDto(string error, int errorCode)
        {
            Status = false;
            Errors = new List<string>() { error };
            ErrorCode = errorCode;
        }

        public void AddErrors(int errorCode, List<string> errors)
        {
            Status = false;
            ErrorCode = errorCode;
            Errors.AddRange(errors);
        }

        public void AddError(int errorCode, string error)
        {
            Status = false;
            ErrorCode = errorCode;
            Errors.Add(error);
        }
    }
}
