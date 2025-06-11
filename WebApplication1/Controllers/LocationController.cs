using App.Application.DTO;
using App.Application.Services;
using App.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WEB.Controllers
{
    [ApiController]
    [Route("location")]
    public class LocationController : ControllerBase
    {
        private readonly IBusLocationService _busLocationService;
        private readonly ITokenService _tokenService;
        private readonly IEtaService _etaService;

        public LocationController(IBusLocationService busLocationService, ITokenService tokenService, IEtaService etaService)
        {
            _busLocationService = busLocationService;
            _tokenService = tokenService;
            _etaService = etaService;
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
            await _etaService.CalculateEtaAsync(int.Parse(dto.TripId));
            return Ok(new { Message = "Successful update location" });
        }
    }
}
