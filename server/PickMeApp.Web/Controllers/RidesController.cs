using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using PickMeApp.Application.Extensions;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PickMeApp.Web.Controllers
{
    [Route("api/rides")]
    //[AllowAnonymous]
    //[Authorize]
    //[Authorize(Roles ="Client")]
    public class RidesController : ApiController
    {
        private readonly IRideRepository _rideRepository;
        private readonly IRideService _rideService;
        private readonly ILogger<RidesController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public RidesController(
            IRideRepository rideRepository,
            IRideService rideService,
            ILogger<RidesController> logger,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _rideRepository = rideRepository;
            _rideService = rideService;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));
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

            string username = HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

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

            var ridesFromRepo = await _rideRepository.ListAsync(resourceParameters);

            var paginationMetadata = new
            {
                totalCount = ridesFromRepo.TotalCount,
                pageSize = ridesFromRepo.PageSize,
                currentPage = ridesFromRepo.CurrentPage,
                totalPages = ridesFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var shapedRides = _mapper.Map<IEnumerable<RideDto>>(ridesFromRepo).ShapeData(resourceParameters.Fields);

            return Ok(shapedRides);
        }

        [HttpGet("{rideId}", Name = "GetRide")]
        public async Task<ActionResult<RideDto>> GetRideAsync(Guid rideId, string fields,
        [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest(new
                {
                    success = false,
                    errors = new string[] { "Header media type has invalid value." }
                });
            }

            if (!_propertyCheckerService.TypeHasProperties<RideDto>(fields))
            {
                return BadRequest(new
                {
                    success = false,
                    errors = new string[] { "Fields query string has invalid value." }
                });
            }

            var rideFromRepoo = await _rideRepository.GetByIdAsync(rideId);

            if (rideFromRepoo == null)
            {
                return NotFound();
            }

            var friendlyResourceToReturn = _mapper.Map<RideDto>(rideFromRepoo)
                .ShapeData(fields) as IDictionary<string, object>;


            return Ok(friendlyResourceToReturn);
        }

        [HttpPost(Name = "CreateRide")]
        public async Task<IActionResult> CreateRideAsync(RideForCreationDto ride)
        {
            if (!ModelState.IsValid)
            {
                return ResponseModelStateErrors();
            }

            var rideEntity = _mapper.Map<Ride>(ride);

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

            if (rideFromRepo == null)
            {
                var rideToAdd = _mapper.Map<Ride>(ride);
                rideToAdd.Id = rideId;

                await _rideRepository.AddAsync(rideToAdd);


                var rideToReturn = _mapper.Map<RideDto>(rideToAdd);

                return CreatedAtRoute("GetRide",
                    new { rideId = rideToReturn.Id },
                    rideToReturn);
            }

            // map the entity to a RideDto
            // apply the updated field values to that dto
            // map the RideDto back to an entity
            _mapper.Map(ride, rideFromRepo);

            await _rideRepository.UpdateAsync(rideFromRepo);

            return NoContent();
        }

        [HttpPatch("{rideId}", Name = "PartiallyUpdateRide")]
        public async Task<ActionResult> PartiallyUpdateRideAsync(Guid rideId, JsonPatchDocument<RideForUpdateDto> patchDocument)
        {
            var rideFromRepo = await _rideRepository.GetByIdAsync(rideId);

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

                await _rideRepository.AddAsync(rideToAdd);

                var rideToReturn = _mapper.Map<RideDto>(rideToAdd);

                return CreatedAtRoute("GetRide",
                    new { rideId = rideToReturn.Id },
                    rideToReturn);
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
        public async Task<ActionResult> DeleteRideAsync(Guid rideId)
        {
            if (!await _rideRepository.ExistsAsync(rideId))
            {
                return NotFound();
            }

            await _rideRepository.DeleteAsync(rideId);

            return NoContent();
        }

        #endregion

        [HttpPut("{rideId}/check-in", Name = "CheckInForRide")]
        public async Task<IActionResult> CheckInForRide(Guid rideId, RideForUpdateDto ride)
        {
            if (!ModelState.IsValid)
            {
                return ResponseModelStateErrors();
            }

            var rideFromRepo = await _rideRepository.GetByIdAsync(rideId);

            if (rideFromRepo == null)
            {
                var rideToAdd = _mapper.Map<Ride>(ride);
                rideToAdd.Id = rideId;

                await _rideRepository.AddAsync(rideToAdd);


                var rideToReturn = _mapper.Map<RideDto>(rideToAdd);

                return CreatedAtRoute("GetRide",
                    new { rideId = rideToReturn.Id },
                    rideToReturn);
            }

            // map the entity to a RideDto
            // apply the updated field values to that dto
            // map the RideDto back to an entity
            _mapper.Map(ride, rideFromRepo);

            await _rideRepository.UpdateAsync(rideFromRepo);

            return NoContent();
        }
    }
}
