using Microsoft.AspNetCore.Mvc;
using App.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using App.Application.Services;

namespace App.WEB.Controllers
{ 
    [ApiController]
    [Route("trips")]
    public class TripsController : ControllerBase
    {
        private ITripService _tripService { get; set; }
        private ITokenService _tokenService { get; set; }

        public TripsController(ITripService tripService, ITokenService tokenService)
        {
            _tripService = tripService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips(
            [FromQuery] int? carrierId,
            [FromQuery] int? segmentId,
            [FromQuery] DateTime? departureDateFrom,
            [FromQuery] DateTime? departureDateTo,
            [FromQuery] List<int> routes)
        {
            var trips = await _tripService.GetTripsAsync(carrierId, segmentId, departureDateFrom, departureDateTo, routes);
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
        public async Task<IActionResult> Create(
            [FromQuery] int RouteScheduleId,
            [FromQuery] int busId,
            [FromQuery] int driverId,
            [FromQuery] DateTime fromPlanning,
            [FromQuery] DateTime toPlanning

            )
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            await _tripService.CreateAsync(RouteScheduleId, fromPlanning, toPlanning, carrierId, busId, driverId);
            return Ok(new { Message = "Successful create" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> Update([FromBody] TripDTO dto)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            if (carrierId != dto.Carrier.Id)
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
    }
}
