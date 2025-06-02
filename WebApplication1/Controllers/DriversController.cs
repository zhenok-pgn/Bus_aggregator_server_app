using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WEB.Controllers
{
    [ApiController]
    [Route("drivers")]
    public class DriversController : ControllerBase
    {
        private ITransportationService _transportationService { get; set; }
        private ITokenService _tokenService { get; set; }

        public DriversController(ITransportationService transportationService, ITokenService tokenService)
        {
            _transportationService = transportationService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> GetDrivers()
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            var res = await _transportationService.GetDriversAsync(carrierId);
            return Ok(res);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> GetDriver(int id)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            var res = await _transportationService.GetDriverAsync(carrierId, id);
            return Ok(res);
        }

        [HttpPost]
        [Authorize(Roles = "Carrier")] // Только перевозчики могут добавлять
        public async Task<IActionResult> Create([FromBody] DriverRegisterRequest dto)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            if(carrierId != dto.CarrierId)
                throw new UnauthorizedAccessException("You do not have permission to create a driver for this carrier.");

            await _transportationService.CreateDriverAsync(dto);
            return Ok(new { Message = "Successful create" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> Update([FromBody] DriverDTO dto)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            if (carrierId != dto.CarrierId)
                throw new UnauthorizedAccessException("You do not have permission to update a driver for this carrier.");

            await _transportationService.UpdateDriverAsync(dto);
            return Ok(new { Message = "Successful update" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> Delete(int id)
        {
            var carrierId = _tokenService.GetUserIdFromContext();

            await _transportationService.DeleteDriverAsync(carrierId, id);
            return Ok(new { Message = "Successful delete" });
        }
    }
}
