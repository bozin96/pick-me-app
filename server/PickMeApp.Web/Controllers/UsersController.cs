using AutoMapper;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PickMeApp.Application.Extensions;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.NotificationDtos;
using PickMeApp.Application.Models.PassengerOnRideDtos;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Application.Models.UserDtos;
using PickMeApp.Core.Constants;
using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PickMeApp.Web.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : ApiController
    {
        private readonly IPassengerOnRideRepository _passengerOnRideRepository;
        private readonly IRideRepository _rideRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public UsersController(
            IPassengerOnRideRepository passengerOnRideRepository,
            IRideRepository rideRepository,
            INotificationRepository notificationRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _passengerOnRideRepository = passengerOnRideRepository ?? throw new ArgumentNullException(nameof(passengerOnRideRepository));
            _rideRepository = rideRepository ?? throw new ArgumentNullException(nameof(rideRepository));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));
        }

        [HttpGet("{userId}", Name = "GetUserInfo")]
        public async Task<ActionResult> GetUserInfoAsync(string userId)
        {
            var userFromRepo = await _userManager.FindByIdAsync(userId);

            if (userFromRepo == null)
            {
                return NotFound();
            }

            var friendlyResourceToReturn = _mapper.Map<UserDto>(userFromRepo);

            return Ok(friendlyResourceToReturn);
        }

        [HttpPut("{userId}", Name = "UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfoAsync(
            string userId,
            [FromBody] UpdateUserDto user)
        {
            var userFromRepo = await _userManager.FindByIdAsync(userId);
            if (userFromRepo == null)
                return ReturnError(StatusCodes.Status404NotFound, "User not found");

            _mapper.Map(user, userFromRepo);
            // create avatar picture for user
            if (userFromRepo.UserPhoto != null)
            {
                using (MagickImage image = new MagickImage(userFromRepo.UserPhoto))
                {
                    image.Format = image.Format; // Get or Set the format of the image.
                    image.Resize(40, 40); // fit the image into the requested width and height. 
                    image.Quality = 10; // This is the Compression level.
                    userFromRepo.UserAvatarPhoto = image.ToByteArray();
                }
            }
            IdentityResult result = await _userManager.UpdateAsync(userFromRepo);
            if (!result.Succeeded)
                return ReturnErrors(StatusCodes.Status409Conflict, result.Errors.Select(e => e.Description).ToList());

            return NoContent();
        }

        [HttpGet("{userId}/rides-as-driver", Name = "GetMyRidesAsDriver")]
        public async Task<ActionResult> GetRidesAsDriver(
            string userId,
            [FromQuery] MyRideResourceParameters resourceParameters)
        {

            var user = await GetUserAsync();
            if (user == null)
                Unauthorized();

            if (user.Id != userId && !await _userManager.IsInRoleAsync(user, ApplicationUserRole.Admin))
            {
                Forbid();
            }

            if (!_propertyMappingService.ValidMappingExistsFor<RideDto, Ride>(resourceParameters.OrderBy))
            {
                return BadRequest(new
                {
                    success = false,
                    errors = new string[] { "OrderBy query string has invalid value." }
                });
            }

            if (!_propertyCheckerService.TypeHasProperties<RideDto>(resourceParameters.Fields))
            {
                return BadRequest(new
                {
                    success = false,
                    errors = new string[] { "Fields query string has invalid value." }
                });
            }

            resourceParameters.DriverId = userId;
            var ridesFromRepo = await _rideRepository.ListAsync(resourceParameters);

            var paginationMetadata = new
            {
                totalCount = ridesFromRepo.TotalCount,
                pageSize = ridesFromRepo.PageSize,
                currentPage = ridesFromRepo.CurrentPage,
                totalPages = ridesFromRepo.TotalPages
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var shapedRides = _mapper.Map<IEnumerable<RideDto>>(ridesFromRepo);

            return Ok(shapedRides);
        }


        [HttpGet("{userId}/rides-as-passenger", Name = "GetMyRidesAsPassenger")]
        public async Task<ActionResult> GetRidesAsPassenger(
            string userId,
            [FromQuery] PassengerOnRideResourceParameters resourceParameters)
        {
            var user = await GetUserAsync();
            if (user == null)
                Unauthorized();

            if (user.Id != userId && !await _userManager.IsInRoleAsync(user, ApplicationUserRole.Admin))
            {
                Forbid();
            }

            if (!_propertyMappingService.ValidMappingExistsFor<PassengerOnRideDto, PassengerOnRide>(resourceParameters.OrderBy))
            {
                return BadRequest(new
                {
                    success = false,
                    errors = new string[] { "OrderBy query string has invalid value." }
                });
            }

            if (!_propertyCheckerService.TypeHasProperties<PassengerOnRideDto>(resourceParameters.Fields))
            {
                return BadRequest(new
                {
                    success = false,
                    errors = new string[] { "Fields query string has invalid value." }
                });
            }

            var passengerOnRidesFromRepo = await _passengerOnRideRepository.ListAsync(resourceParameters);

            var paginationMetadata = new
            {
                totalCount = passengerOnRidesFromRepo.TotalCount,
                pageSize = passengerOnRidesFromRepo.PageSize,
                currentPage = passengerOnRidesFromRepo.CurrentPage,
                totalPages = passengerOnRidesFromRepo.TotalPages
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var shapedRides = _mapper.Map<IEnumerable<PassengerOnRideDto>>(passengerOnRidesFromRepo);

            return Ok(shapedRides);
        }

        [HttpGet("{userId}/notifications", Name = "GetMyNotifications")]
        public async Task<ActionResult> GetNotifications(
            string userId,
            [FromQuery] PassengerOnRideResourceParameters resourceParameters)
        {
            var user = await GetUserAsync();
            if (user == null)
                Unauthorized();

            if (user.Id != userId && !await _userManager.IsInRoleAsync(user, ApplicationUserRole.Admin))
            {
                Forbid();
            }

            var notificationsFromRepo = await _notificationRepository.ListAsync(resourceParameters, user.Id);

            var paginationMetadata = new
            {
                totalCount = notificationsFromRepo.TotalCount,
                pageSize = notificationsFromRepo.PageSize,
                currentPage = notificationsFromRepo.CurrentPage,
                totalPages = notificationsFromRepo.TotalPages
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var shapedNotifications = _mapper.Map<IEnumerable<NotificationDto>>(notificationsFromRepo);
             //   .ShapeData(resourceParameters.Fields);

            return Ok(shapedNotifications);
        }

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
    }
}
