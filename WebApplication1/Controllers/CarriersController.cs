using App.BLL.DTO;
using App.DAL.EF;
using App.DAL.Entities;
using App.WEB.BLL.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarriersController : ControllerBase
    {
        public ApplicationDBContext db { get; set; }

        public CarriersController(ApplicationDBContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarrierDTO>>> Get()
        {
            var result = await db.Carriers.ToListAsync();
            return result.MapToDto<Carrier, CarrierDTO>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarrierDTO>> Get(int id)
        {
            var result = await db.Carriers.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
                return NotFound();
            return new ObjectResult(result.MapToDto<CarrierDTO>());
        }

        [HttpPut]
        public async Task<ActionResult<CarrierDTO>> Put(CarrierDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!db.Carriers.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }

            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user.MapToDto<CarrierDTO>());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CarrierDTO>> Delete(int id)
        {
            var result = db.Carriers.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            db.Carriers.Remove(result);
            await db.SaveChangesAsync();
            return Ok(result.MapToDto<CarrierDTO>());
        }
    }
}
