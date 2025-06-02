using App.Application.DTO;
using App.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WEB.Controllers
{
    [ApiController]
    [Route("buses")]
    public class BusesController : ControllerBase
    {
        private ITransportationService _transportationService { get; set; }
        private ITokenService _tokenService { get; set; }

        public BusesController(ITransportationService transportationService, ITokenService tokenService)
        {
            _transportationService = transportationService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> GetBuses()
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            var routes = await _transportationService.GetBusesAsync(carrierId);
            return Ok(routes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> GetBus(int id)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            var route = await _transportationService.GetBusAsync(carrierId, id);
            return Ok(route);
        }

        [HttpPost]
        [Authorize(Roles = "Carrier")] // Только перевозчики могут добавлять
        public async Task<IActionResult> Create([FromBody] BusDTO dto)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            if(carrierId != dto.CarrierId)
                throw new UnauthorizedAccessException("You do not have permission to create a route for this carrier.");

            await _transportationService.CreateBusAsync(dto);
            return Ok(new { Message = "Successful create" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> Update([FromBody] BusDTO dto)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            if (carrierId != dto.CarrierId)
                throw new UnauthorizedAccessException("You do not have permission to update a bus for this carrier.");

            await _transportationService.UpdateBusAsync(dto);
            return Ok(new { Message = "Successful update" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> Delete(int id)
        {
            var carrierId = _tokenService.GetUserIdFromContext();

            await _transportationService.DeleteBusAsync(carrierId, id);
            return Ok(new { Message = "Successful delete" });
        }
    }
}
