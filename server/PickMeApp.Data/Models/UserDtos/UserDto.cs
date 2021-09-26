
namespace PickMeApp.Application.Models.UserDtos
{
    public class UserDto
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string UserPhoto { get; set; }

        public string Email { get; set; }

        public float AverageRate { get; set; }

        public int NumberOfRates { get; set; }
    }
}
