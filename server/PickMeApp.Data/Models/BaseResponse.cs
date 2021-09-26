using System;
using System.Collections.Generic;

namespace PickMeApp.Application.Models
{
    public abstract class BaseResponse<T>
    {
        public bool Status { get; set; } = true;
        public int StatusCode { get; set; } = 200;
        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }

        public BaseResponse()
        {
        }

        public BaseResponse(Guid correlationId)
        {
            
        }

        public BaseResponse(bool status, int statusCode, List<string> errors, T data)
        {
            Status = status;
            StatusCode = statusCode;
            Errors = errors;
            Data = data;
        }
    }
}
