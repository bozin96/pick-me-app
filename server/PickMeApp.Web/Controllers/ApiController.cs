﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PickMeApp.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace PickMeApp.Web.Controllers
{
    [ApiController]
    public class ApiController : Controller
    {
        public ApiController()
        {

        }

        protected IActionResult ReturnError(int statusCode, string error)
        {
            return StatusCode(statusCode, new
            {
                success = false,
                errors = new string[] { error }
            });
        }

        protected IActionResult ReturnErrors(int statusCode, List<string> errors)
        {
            return StatusCode(statusCode, new
            {
                success = false,
                errors = errors
            });
        }

        protected IActionResult GetResponse<T>(BaseResponse<T> result = null)
        {
            if (result.Status)
            {
                return Ok(new
                {
                    success = true,
                    data = result.Data
                });
            }

            return StatusCode(result.StatusCode, new
            {
                success = false,
                errors = result.Errors
            });
        }

        protected IActionResult ResponseModelStateErrors()
        {
            List<string> errors = new List<string>();
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                errors.Add(errorMsg);
            }
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new
            {
                success = false,
                errors = errors
            });
        }
    }
}
