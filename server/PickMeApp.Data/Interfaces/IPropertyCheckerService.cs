
namespace PickMeApp.Application.Interfaces
{
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
