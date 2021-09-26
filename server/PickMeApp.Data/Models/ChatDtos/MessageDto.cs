using System;
using System.ComponentModel.DataAnnotations;

namespace PickMeApp.Application.Models.ChatDtos
{
    public class MessageDto
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public Guid ChatId { get; set; }

        [Required]
        public string ReceiverId { get; set; }
    }
}
