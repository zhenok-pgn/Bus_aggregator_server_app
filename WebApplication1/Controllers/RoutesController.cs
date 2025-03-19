using App.Application.DTO;
using App.Application.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.WEB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        /*public ApplicationDBContext db { get; set; }

        public RoutesController(ApplicationDBContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteDTO>>> Get()
        {
            var result = await db.Routes.ToListAsync();
            return result.MapToDto<DAL.Entities.Route, RouteDTO>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RouteDTO>> Get(int id)
        {
            var result = await db.Routes.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
                return NotFound();
            return new ObjectResult(result.MapToDto<RouteDTO>());
        }

        [HttpPost]
        public async Task<ActionResult<RouteDTO>> Post(RouteDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            await db.Routes.AddAsync(new()
            {
                Id = user.Id,
            });
            await db.SaveChangesAsync();
            return Ok(user.MapToDto<RouteDTO>());
        }

        [HttpPut]
        public async Task<ActionResult<RouteDTO>> Put(RouteDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!db.Routes.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }

            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user.MapToDto<RouteDTO>());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RouteDTO>> Delete(int id)
        {
            var result = db.Routes.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            db.Routes.Remove(result);
            await db.SaveChangesAsync();
            return Ok(result.MapToDto<RouteDTO>());
        }*/
    }
}
