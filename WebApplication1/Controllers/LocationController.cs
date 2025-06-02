using App.Application.DTO;
using App.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WEB.Controllers
{
    [ApiController]
    [Route("location")]
    public class LocationController : ControllerBase
    {
        private IBusLocationService _busLocationService { get; set; }
        private ITokenService _tokenService { get; set; }

        public LocationController(IBusLocationService busLocationService, ITokenService tokenService)
        {
            _busLocationService = busLocationService;
            _tokenService = tokenService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetLatestLocation(int tripId)
        {
            var location = await _busLocationService.GetLatestBusLocationAsync(tripId);
            return Ok(location);
        }

        [HttpPost]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> UpdateLocation([FromBody] BusLocationDTO dto)
        {
            var driverId = _tokenService.GetUserIdFromContext();
            await _busLocationService.UpdateBusLocationAsync(driverId, dto);
            return Ok(new { Message = "Successful update location" });
        }
    }
}
