using Microsoft.AspNetCore.Mvc;
using App.BLL.Interfaces;
using App.BLL.DTO;
using App.DAL.Entities;
using App.DAL.Infrastructure;

namespace App.WEB.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthService<PassengerDTO, RBKDF2PasswordHasher> PassengerAuthService { get; set; }
        public IAuthService<CarrierDTO, RBKDF2PasswordHasher> CarrierAuthService { get; set; }
        public IAuthService<DriverDTO, RBKDF2PasswordHasher> DriverAuthService { get; set; }

        public AuthController(
            IAuthService<PassengerDTO, RBKDF2PasswordHasher> passengerAuthService,
            IAuthService<CarrierDTO, RBKDF2PasswordHasher> carrierAuthService,
            IAuthService<DriverDTO, RBKDF2PasswordHasher> driverAuthService)
        {
            PassengerAuthService = passengerAuthService;
            CarrierAuthService = carrierAuthService;
            DriverAuthService = driverAuthService;
        }

        [Route("passenger/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> PassengerLogin(PassengerDTO passenger)
        {
            if (passenger is null)
            {
                return BadRequest();
            }

            if (!await PassengerAuthService.Login(passenger))
            {
                return Unauthorized();
            }

            var jwtToken = PassengerAuthService.GetJwtSecurityToken(passenger);
            var response = new
            {
                access_token = jwtToken,
                username = passenger.Phone
            };

            return Ok(response);
        }

        [Route("carrier/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> CarrierLogin(CarrierDTO carrier)
        {
            if (carrier is null)
            {
                return BadRequest();
            }

            if (!await CarrierAuthService.Login(carrier))
            {
                return Unauthorized();
            }

            var jwtToken = CarrierAuthService.GetJwtSecurityToken(carrier);
            var response = new
            {
                access_token = jwtToken,
                username = carrier.Inn
            };

            return Ok(response);
        }

        [Route("driver/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> DriverLogin(DriverDTO driver)
        {
            if (driver is null)
            {
                return BadRequest();
            }

            if (!await DriverAuthService.Login(driver))
            {
                return Unauthorized();
            }

            var jwtToken = DriverAuthService.GetJwtSecurityToken(driver);
            var response = new
            {
                access_token = jwtToken,
                username = driver.LicenseId
            };

            return Ok(response);
        }

        [Route("passenger/[controller]/signin")]
        [HttpPost]
        public async Task<IActionResult> PassengerSignin(PassengerDTO passenger)
        {
            if (passenger is null)
            {
                return BadRequest();
            }

            if (!await PassengerAuthService.Signin(passenger))
            {
                return Unauthorized();
            }

            var jwtToken = PassengerAuthService.GetJwtSecurityToken(passenger);
            var response = new
            {
                access_token = jwtToken,
                username = passenger.Phone
            };

            return Ok(response);
        }

        [Route("carrier/[controller]/signin")]
        [HttpPost]
        public async Task<IActionResult> CarrierSignin(CarrierDTO carrier)
        {
            if (carrier is null)
            {
                return BadRequest();
            }

            if (!await CarrierAuthService.Signin(carrier))
            {
                return Unauthorized();
            }

            var jwtToken = CarrierAuthService.GetJwtSecurityToken(carrier);
            var response = new
            {
                access_token = jwtToken,
                username = carrier.Inn
            };

            return Ok(response);
        }
    }
}
