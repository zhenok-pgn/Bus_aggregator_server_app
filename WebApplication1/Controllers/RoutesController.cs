using App.Application.DTO;
using App.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WEB.Controllers
{
    [ApiController]
    [Route("routes")]
    public class RoutesController : ControllerBase
    {
        private IRouteService _routeService { get; set; }
        private ITokenService _tokenService { get; set; }

        public RoutesController(IRouteService routeService, ITokenService tokenService)
        {
            _routeService = routeService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> GetRoutes()
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            var routes = await _routeService.GetRoutesAsync(carrierId);
            return Ok(routes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> GetRoute(int id)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            var route = await _routeService.GetRouteAsync(carrierId, id);
            return Ok(route);
        }

        [HttpPost]
        [Authorize(Roles = "Carrier")] // Только перевозчики могут добавлять
        public async Task<IActionResult> Create([FromBody] RouteDTO dto)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            if(carrierId != dto.Carrier.Id)
                throw new UnauthorizedAccessException("You do not have permission to create a route for this carrier.");

            await _routeService.CreateAsync(dto);
            return Ok(new { Message = "Successful create" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> Update([FromBody] RouteDTO dto)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            if (carrierId != dto.Carrier.Id)
                throw new UnauthorizedAccessException("You do not have permission to update a route for this carrier.");

            await _routeService.UpdateAsync(dto);
            return Ok(new { Message = "Successful update" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> Delete(int id)
        {
            var carrierId = _tokenService.GetUserIdFromContext();

            await _routeService.DeleteAsync(carrierId, id);
            return Ok(new { Message = "Successful delete" });
        }
    }
}
