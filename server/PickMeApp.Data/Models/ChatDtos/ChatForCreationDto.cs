using System.ComponentModel.DataAnnotations;

namespace PickMeApp.Application.Models.ChatDtos
{
    public class ChatForCreationDto
    {
        [Required]
        public string UserId { get; set; }
    }
}
