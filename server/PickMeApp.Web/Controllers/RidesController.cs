using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using PickMeApp.Application.Extensions;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.NotificationDtos;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Core.Constants;
using PickMeApp.Core.Models;
using PickMeApp.Core.Models.Notification;
using PickMeApp.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PickMeApp.Web.Controllers
{
    [Route("api/rides")]
    [Authorize(Roles = "Client")]
    public class RidesController : ApiController
    {
        private readonly IRideRepository _rideRepository;
        private readonly IPassengerOnRideRepository _passengerOnRideRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IRideService _rideService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public RidesController(
            IRideRepository rideRepository,
            IPassengerOnRideRepository passengerOnRideRepository,
            INotificationRepository notificationRepository,
            IRideService rideService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService,
            IHubContext<NotificationsHub> hubContext)
        {
            _rideRepository = rideRepository;
            _passengerOnRideRepository = passengerOnRideRepository;
            _notificationRepository = notificationRepository;
            _rideService = rideService;
            _userManager = userManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));
            _hubContext = hubContext;
        }

        #region CRUD operations

        [HttpGet(Name = "GetRides")]
        [HttpHead]
        public async Task<IActionResult> GetRidesAsync([FromQuery] RideResourceParameters resourceParameters)
        {
            if (!ModelState.IsValid)
            {
                return ResponseModelStateErrors();
            }

            if (!_propertyMappingService.ValidMappingExistsFor<RideDto, Ride>(resourceParameters.OrderBy))
            {
                return ReturnError(StatusCodes.Status400BadRequest, "OrderBy query string has invalid value.");
            }

            if (!_propertyCheckerService.TypeHasProperties<RideDto>(resourceParameters.Fields))
            {
                return ReturnError(StatusCodes.Status400BadRequest, "Fields query string has invalid value.");
            }

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var ridesFromRepo = await _rideRepository.ListAsync(resourceParameters, userId);

            var paginationMetadata = new
            {
                totalCount = ridesFromRepo.TotalCount,
                pageSize = ridesFromRepo.PageSize,
                currentPage = ridesFromRepo.CurrentPage,
                totalPages = ridesFromRepo.TotalPages
            };
            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            var shapedRides = _rideService.CheckFreeSeats(_mapper.Map<IEnumerable<RideDto>>(ridesFromRepo), resourceParameters);
            //.ShapeData(resourceParameters.Fields);

            return Ok(shapedRides);
        }

        [HttpGet("{rideId}", Name = "GetRide")]
        public async Task<IActionResult> GetRideAsync(Guid rideId, string fields,
        [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                out MediaTypeHeaderValue parsedMediaType))
            {
                return ReturnError(StatusCodes.Status400BadRequest, "Header media type has invalid value.");
            }

            if (!_propertyCheckerService.TypeHasProperties<RideDto>(fields))
            {
                return ReturnError(StatusCodes.Status400BadRequest, "Fields query string has invalid value.");
            }

            var rideFromRepo = await _rideRepository.GetByIdAsync(rideId);

            if (rideFromRepo == null)
            {
                return NotFound();
            }

            var rideDto = _mapper.Map<RideDto>(rideFromRepo);
            //.ShapeData(fields) as IDictionary<string, object>;

            return Ok(rideDto);
        }

        [HttpPost(Name = "CreateRide")]
        public async Task<IActionResult> CreateRideAsync(RideForCreationDto ride)
        {
            if (!ModelState.IsValid)
            {
                return ResponseModelStateErrors();
            }

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Forbid();

            var rideEntity = _mapper.Map<Ride>(ride);
            rideEntity.DriverId = userId;

            rideEntity = _rideService.PrepareRideForCreation(rideEntity);
            await _rideRepository.AddAsync(rideEntity);

            var rideToReturn = _mapper.Map<RideDto>(rideEntity);

            var linkedResourceToReturn = rideToReturn.ShapeData(null)
                as IDictionary<string, object>;

            return Ok(linkedResourceToReturn);
        }

        [HttpPut("{rideId}", Name = "UpdateRide")]
        public async Task<IActionResult> UpdateRideAsync(Guid rideId, RideForUpdateDto ride)
        {
            var rideFromRepo = await _rideRepository.GetByIdAsync(rideId);
            var userId = GetUserId();

            if (rideFromRepo == null)
            {
                var rideToAdd = _mapper.Map<Ride>(ride);
                rideToAdd.Id = rideId;
                rideToAdd.DriverId = userId;

                await _rideRepository.AddAsync(rideToAdd);

                var rideToReturn = _mapper.Map<RideDto>(rideToAdd);

                return CreatedAtRoute("GetRide",
                    new { rideId = rideToReturn.Id },
                    rideToReturn);
            }
            else if (rideFromRepo.StartDate <= DateTime.UtcNow)
            {
                return ReturnError(StatusCodes.Status409Conflict, "You cannot update the ride, the ride has already started or ended.");
            }
            else if (_rideService.HasPassengers(rideFromRepo))
            {
                return ReturnError(StatusCodes.Status409Conflict, "You cannot update the ride, the ride already has passengers.");
            }

            if (rideFromRepo.DriverId != userId)
                return Forbid();

            // map the entity to a RideDto
            // apply the updated field values to that dto
            // map the RideDto back to an entity
            _mapper.Map(ride, rideFromRepo);
            await _rideRepository.UpdateAsync(rideFromRepo);

            return NoContent();
        }

        [HttpPatch("{rideId}", Name = "PartiallyUpdateRide")]
        public async Task<IActionResult> PartiallyUpdateRideAsync(Guid rideId, JsonPatchDocument<RideForUpdateDto> patchDocument)
        {
            var rideFromRepo = await _rideRepository.GetByIdAsync(rideId);
            var userId = GetUserId();

            if (rideFromRepo == null)
            {
                var rideDto = new RideForUpdateDto();
                patchDocument.ApplyTo(rideDto, ModelState);

                if (!TryValidateModel(rideDto))
                {
                    return ValidationProblem(ModelState);
                }

                var rideToAdd = _mapper.Map<Ride>(rideDto);
                rideToAdd.Id = rideId;
                rideToAdd.DriverId = userId;

                await _rideRepository.AddAsync(rideToAdd);

                var rideToReturn = _mapper.Map<RideDto>(rideToAdd);

                return CreatedAtRoute("GetRide",
                    new { rideId = rideToReturn.Id },
                    rideToReturn);
            }
            else if (rideFromRepo.StartDate >= DateTime.UtcNow)
            {
                return ReturnError(StatusCodes.Status409Conflict, "You cannot update the ride, the ride has already started or ended.");
            }

            var rideToPatch = _mapper.Map<RideForUpdateDto>(rideFromRepo);
            // add validation
            patchDocument.ApplyTo(rideToPatch, ModelState);

            if (!TryValidateModel(rideToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(rideToPatch, rideFromRepo);

            await _rideRepository.UpdateAsync(rideFromRepo);

            return NoContent();
        }

        [HttpDelete("{rideId}", Name = "DeleteRide")]
        public async Task<IActionResult> DeleteRideAsync(Guid rideId)
        {
            var rideFromRepo = await _rideRepository.GetByIdAsync(rideId);
            if (rideFromRepo == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (rideFromRepo.DriverId != userId)
            {
                return ReturnError(StatusCodes.Status409Conflict, "You cannot request for your own ride.");
            }

            await _rideRepository.DeleteAsync(rideId);

            return NoContent();
        }

        #endregion

        #region Ride Actions

        [HttpPost("{rideId}/request", Name = "RequestForRide")]
        public async Task<IActionResult> RequestForRide(Guid rideId, RideRequestDto rideRequest)
        {
            if (!ModelState.IsValid)
            {
                return ResponseModelStateErrors();
            }

            var user = await GetUserAsync();
            if (user == null)
                return Unauthorized();

            // Get ride from repo.
            var rideFromRepo = await _rideRepository.GetByIdAsync(rideId);
            if (rideFromRepo == null)
                return NotFound();

            // Driver can not request own ride.
            if (rideFromRepo.DriverId == user.Id)
                return Forbid();

            // Calculate on free seats on that route leg.
            if (!_rideService.HasFreeSeats(rideFromRepo, rideRequest))
                return ReturnError(StatusCodes.Status409Conflict, "There is no enough free seats in this ride.");

            var notificationPayload = _mapper.Map<RideRequestNotificationPayload>(rideRequest);
            notificationPayload.AddUserInfo(user);

            // Create notification for driver.
            Notification notification = new Notification()
            {
                RideId = rideId,
                Type = NotificationType.RequestForRide,
                Header = NotificationConfiguration.RequestForRideHeader,
                Body = NotificationConfiguration.RequestForRideBody(
                    rideRequest.StartWaypoint,
                    rideRequest.EndWaypoint,
                    rideRequest.StartDate,
                    rideRequest.NumberOfPassengers),
                UserFromId = user.Id,
                UserToId = rideFromRepo.DriverId,
                Payload = JsonConvert.SerializeObject(notificationPayload)
            };

            // Store notification in db.
            await _notificationRepository.AddAsync(notification);

            // Send notification to driver.
            await _hubContext.Clients.User(notification.UserToId).SendAsync(
                NotificationConfiguration.RequestForRideChanell,
                notification
            );

            return NoContent();
        }

        [HttpPost("{rideId}/response", Name = "ResponseOnRideRequest")]
        public async Task<IActionResult> ResponseOnRideRequest(
            Guid rideId,
            ResponseOnRideRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ResponseModelStateErrors();
            }

            // Get current user.
            var user = await GetUserAsync();
            if (user == null)
                Unauthorized();

            // Get ride from repo.
            var rideFromRepo = await _rideRepository.GetByIdAsync(rideId);
            if (rideFromRepo == null)
                return ReturnError(StatusCodes.Status404NotFound, "There is no ride with given id.");

            // Get driver from repo.
            var driver = await _userManager.FindByIdAsync(rideFromRepo.DriverId);
            if (driver == null)
                return ReturnError(StatusCodes.Status404NotFound, "There is no ride with given id.");

            // Only driver can accept/decline request.
            if (driver.Id != user.Id)
                return Forbid();

            // Get notification from repo.
            Notification notificationFromRepo = await _notificationRepository.GetByIdAsync(request.NotificationId);
            if (notificationFromRepo == null)
                return ReturnError(StatusCodes.Status404NotFound, "There is no notification with given id.");

            RideRequestNotificationPayload notificationPayload;
            try
            {
                notificationPayload = JsonConvert.DeserializeObject<RideRequestNotificationPayload>(notificationFromRepo.Payload);
            }
            catch (Exception) { return ReturnError(StatusCodes.Status404NotFound, "Wrong notification format."); }

            // If request is accepted update ride.
            if (request.Accepted)
            {
                var rideRequestDto = _mapper.Map<RideRequestDto>(notificationPayload);
                var (status, rideAccepted) = _rideService.AcceptRideRequest(rideFromRepo, rideRequestDto);
                if (!status.Status)
                    return ReturnErrors(status.ErrorCode, status.Errors);

                await _rideRepository.UpdateAsync(rideAccepted);
            }

            // Create notification for driver.
            Notification acceptNotification = new Notification()
            {
                RideId = rideId,
                Type = NotificationType.RequestForRide,
                Header = NotificationConfiguration.ResponseOnRideRequestHeader,
                Body = NotificationConfiguration.ResponseOnRideRequestBody(
                    $"{driver.FirstName} {driver.LastName}",
                    notificationPayload.StartWaypoint,
                    notificationPayload.EndWaypoint,
                    notificationPayload.StartDate,
                   request.Accepted),
                UserFromId = user.Id,
                UserToId = rideFromRepo.DriverId
            };

            // Store response notification in db.
            await _notificationRepository.AddAsync(acceptNotification);
            var acceptNotificationDto = _mapper.Map<NotificationDto>(acceptNotification);

            // Update request notification.
            notificationFromRepo.IsVisible = false;
            notificationFromRepo.Header += request.Accepted ? " - Accepted" : " - Decline";
            await _notificationRepository.UpdateAsync(notificationFromRepo);

            // Send notification to passenger.
            await _hubContext.Clients.User(notificationFromRepo.UserFromId).SendAsync(
                NotificationConfiguration.ResponseOnRideRequestChanell,
                acceptNotificationDto
            );

            return NoContent();
        }

        [HttpPost("{rideId}/rate", Name = "RateRide")]
        public async Task<IActionResult> RateRide(Guid rideId, RideReviewDto rideReview)
        {
            if (!ModelState.IsValid)
            {
                return ResponseModelStateErrors();
            }

            // Get current user.
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Get ride from repo.
            var rideFromRepo = await _rideRepository.GetByIdAsync(rideId);
            if (rideFromRepo == null)
                return ReturnError(StatusCodes.Status404NotFound, "Ride not found.");
            if (rideFromRepo.DriverId == userId)
                return ReturnError(StatusCodes.Status409Conflict, "You can not review your own ride.");

            // Get passengerOnRide from repo.
            var passengerOnRideFromRepo = await _passengerOnRideRepository.GetByIdAsync(rideReview.Id);
            if (passengerOnRideFromRepo == null)
                return ReturnError(StatusCodes.Status404NotFound, "Wrong id.");
            if (passengerOnRideFromRepo.RideId != rideId)
                return ReturnError(StatusCodes.Status404NotFound, "Wrong ride id.");
            if (passengerOnRideFromRepo.PassengerId != userId)
                return ReturnError(StatusCodes.Status403Forbidden, "You have no permission to review this ride.");

            var result = await _passengerOnRideRepository.AddReviewAsync(rideReview.Id, rideReview.Rate);
            if (!result)
                return ReturnError(StatusCodes.Status400BadRequest, "Bad request.");

            // update driver average value.
            var driver = await _userManager.FindByIdAsync(rideFromRepo.DriverId);
            if (driver != null)
            {
                driver.AverageRate = (driver.AverageRate * driver.NumberOfRates + rideReview.Rate) / (driver.NumberOfRates + 1);
                driver.NumberOfRates++;
                await _userManager.UpdateAsync(driver);
            }
            // Create notification for driver.
            Notification notification = new Notification()
            {
                RideId = rideId,
                Type = NotificationType.RequestForRide,
                Header = NotificationConfiguration.RideReviewHeader,
                Body = NotificationConfiguration.RideReviewBody(
                        passengerOnRideFromRepo.StartWaypoint,
                        passengerOnRideFromRepo.EndWaypoint,
                        passengerOnRideFromRepo.StartDate,
                        rideReview.Rate),
                UserFromId = userId,
                UserToId = rideFromRepo.DriverId
            };
            await _notificationRepository.AddAsync(notification);

            // Send notification to driver.
            await _hubContext.Clients.User(rideFromRepo.DriverId).SendAsync(
                NotificationConfiguration.RideReviewChanell,
                notification
            );

            return NoContent();
        }

        #endregion

        #region Helpers

        [NonAction]
        private string GetUserId()
        {
            return HttpContext.User.Claims
                .Where(c => c.Type == CustomClaimTypes.Id)
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        [NonAction]
        private async Task<ApplicationUser> GetUserAsync()
        {
            return await _userManager.FindByIdAsync(HttpContext.User.Identity.Name);
        }

        #endregion
    }
}
