using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Application.Mapping;
using App.Application.DTO;

namespace App.WEB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        /*public ApplicationDBContext db { get; set; }

        public TripsController(ApplicationDBContext db)
        {
            this.db = db;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TripDTO>> Get(int id)
        {
            var user = await db.Trips.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            return new ObjectResult(user.MapToDto<TripDTO>());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDTO>>> Get()
        {
            var result = await db.Trips.ToListAsync();
            return result.MapToDto<Trip, TripDTO>();
        }

        [HttpPost]
        public async Task<ActionResult<TripDTO>> Post(TripDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            await db.Trips.AddAsync(new()
            {
                Id = user.Id,
            });
            await db.SaveChangesAsync();
            return Ok(user.MapToDto<TripDTO>());
        }

        [HttpPut]
        public async Task<ActionResult<TripDTO>> Put(TripDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!db.Trips.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }

            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user.MapToDto<TripDTO>());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TripDTO>> Delete(int id)
        {
            var result = db.Trips.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            db.Trips.Remove(result);
            await db.SaveChangesAsync();
            return Ok(result.MapToDto<TripDTO>());
        }*/
    }
}
