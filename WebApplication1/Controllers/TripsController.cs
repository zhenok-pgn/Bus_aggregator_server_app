using Microsoft.AspNetCore.Mvc;
using App.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using App.Application.Services;
using App.Application.DTO.Requests;
using App.Core.Entities;

namespace App.WEB.Controllers
{ 
    [ApiController]
    [Route("trips")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly ITokenService _tokenService;

        public TripsController(ITripService tripService, ITokenService tokenService)
        {
            _tripService = tripService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips(
            [FromQuery] int? carrierId,
            [FromQuery] int? localityFromId,
            [FromQuery] int? localityToId,
            [FromQuery] DateOnly? departureDateFrom,
            [FromQuery] DateOnly? departureDateTo,
            [FromQuery] List<int> routes)
        {
            var trips = await _tripService.GetTripsAsync(carrierId, localityFromId, localityToId, departureDateFrom, departureDateTo, routes);
            return Ok(trips);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip([FromQuery] int? segmentId, int id)
        {
            var trip = await _tripService.GetTripAsync(segmentId, id);
            return Ok(trip);
        }

        [HttpPost]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> Create([FromBody] TripPlanRequest planRequest)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            await _tripService.CreateAsync(planRequest, carrierId);
            return Ok(new { Message = "Successful create" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> Update([FromBody] TripDTO dto)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            if (carrierId != dto.Route.CarrierId)
                throw new UnauthorizedAccessException("You do not have permission to update a trip for this carrier.");

            await _tripService.UpdateAsync(dto);
            return Ok(new { Message = "Successful update" });
        }

        [HttpGet("driver")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> DriverTrips()
        {
            var driverId = _tokenService.GetUserIdFromContext();
            var trips = await _tripService.GetDriversTripsAsync(driverId);
            return Ok(trips);
        }

        [HttpGet("{id}/segments")]
        public async Task<IActionResult> GetTripSegments(int id)
        {
            var execution = await _tripService.GetTripExecutionAsync(id);
            return Ok(execution);
        }

        [Authorize(Roles = "Driver")]
        [HttpPost("{tripId}/segments/{segmentId}/confirm-arrival")]
        public async Task<IActionResult> ConfirmArrival(int tripId, int segmentId, [FromBody] TimestampDTO timestamp)
        {
            var driverId = _tokenService.GetUserIdFromContext();
            await _tripService.ConfirmArrivalAsync(driverId, tripId, segmentId, timestamp.Timestamp);
            return Ok(new { Message = "arrival confirmed" });
        }

        [Authorize(Roles = "Driver")]
        [HttpPost("{tripId}/segments/{segmentId}/confirm-departure")]
        public async Task<IActionResult> ConfirmDeparture(int tripId, int segmentId, [FromBody] TimestampDTO timestamp)
        {
            var driverId = _tokenService.GetUserIdFromContext();
            await _tripService.ConfirmDepartureAsync(driverId, tripId, segmentId, timestamp.Timestamp);
            return Ok(new { Message = "departure confirmed" });
        }
    }
}
