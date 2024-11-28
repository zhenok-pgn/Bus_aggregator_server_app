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
        public ApplicationMysqlContext db { get; set; }

        public CarriersController(ApplicationMysqlContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarrierDTO>>> Get()
        {
            var result = await db.Carriers.ToListAsync();
            return result.MapToDto<CarrierDTO, Carrier>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarrierDTO>> Get(int id)
        {
            var result = await db.Carriers.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
                return NotFound();
            return new ObjectResult(result.MapToDto());
        }

        /*[HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!db.Users.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }

            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }*/
    }
}
