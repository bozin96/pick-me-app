using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PickMeApp.Application.Models.ChatDtos
{
    public class MessageDto
    {
        [Required]
        public string Text { get; set; }
    }
}
