
namespace PickMeApp.Application.Models.RideDtos
{
    public class RideForCreationDto : RideForManipulationDto
    {

        public bool IsValid()
        {
            return StartDate > System.DateTime.UtcNow;
        }
    }
}
