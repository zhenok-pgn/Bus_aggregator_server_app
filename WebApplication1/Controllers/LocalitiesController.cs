using App.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.WEB.Controllers
{
    [ApiController]
    [Route("localities")]
    public class LocalitiesController : ControllerBase
    {
        private ILocalityService _localityService { get; set; }

        public LocalitiesController(ILocalityService localityService)
        {
            _localityService = localityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocalities()
        {
            var localities = await _localityService.GetLocalitiesAsync();
            return Ok(localities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocality(int id)
        {
            var locality = await _localityService.GetLocalityAsync(id);
            return Ok(locality);
        }
    }
}
