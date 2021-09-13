using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickMeApp.Web.Models
{
    public class APIError
    {
        public APIError()
        {
            Errors = new List<string>();
        }

        public List<string> Errors { get; set; }
    }
}
