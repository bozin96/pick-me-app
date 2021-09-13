using PickMeApp.Application.Helpers;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickMeApp.Application.Interfaces
{
    public interface IRideService
    {
        Ride PrepareRideForCreation(Ride ride);

    }
}
