using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Application.Interfaces
{
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
