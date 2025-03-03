using Microsoft.AspNetCore.Mvc;
using App.BLL.Interfaces;
using App.BLL.DTO;
using App.DAL.Entities;
using App.DAL.Infrastructure;
using App.WEB.BLL.Interfaces;
using App.WEB.BLL.DTO.Requests;
using App.WEB.BLL.Infrastructure;
using Microsoft.Extensions.Options;

namespace App.WEB.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService<Passenger> _passengerAuthService;
        private readonly IAuthService<Carrier> _carrierAuthService;
        private readonly IAuthService<Driver> _driverAuthService;
        private readonly CookieSettings _cookieSettings;

        public AuthController(
            IAuthService<Passenger> passengerAuthService,
            IAuthService<Carrier> carrierAuthService,
            IAuthService<Driver> driverAuthService,
            IOptions<CookieSettings> cookieSettings)
        {
            _passengerAuthService = passengerAuthService;
            _carrierAuthService = carrierAuthService;
            _driverAuthService = driverAuthService;
            _cookieSettings = cookieSettings.Value;
        }

        private CookieOptions GetCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = _cookieSettings.HttpOnly,
                Secure = _cookieSettings.Secure,
                SameSite = Enum.Parse<SameSiteMode>(_cookieSettings.SameSite),
                Expires = DateTime.UtcNow.AddDays(_cookieSettings.ExpiresInDays)
            };
        }

        [Route("passenger/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> PassengerLogin(AuthRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var response = await _passengerAuthService.GetLoginResponse(request);

            if (response is null)
            {
                return Unauthorized();
            }
            else
            {
                Response.Cookies.Append("refreshToken", response.RefreshToken.Token, GetCookieOptions());
                return Ok(response.AccessToken);
            }



            /*if (!await _passengerAuthService.Login(passenger))
            {
                return Unauthorized();
            }

            var claims = _passengerAuthService.GetClaims(passenger);
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();


            var response = new
            {
                access_token = jwtToken,
                username = passenger.Phone
            };

            return Ok(response);*/
        }

        [Route("passenger/[controller]/signup")]
        [HttpPost]
        public async Task<IActionResult> PassengerSignup(AuthRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var response = await _passengerAuthService.GetSignupResponse(request);

            if (response is null)
            {
                return Unauthorized();
            }
            else
            {
                Response.Cookies.Append("refreshToken", response.RefreshToken.Token, GetCookieOptions());
                return Ok(response.AccessToken);
            }

            /*if (!await _passengerAuthService.Signin(passenger))
            {
                return Unauthorized();
            }

            var jwtToken = _passengerAuthService.GetJwtSecurityToken(passenger);
            var response = new
            {
                access_token = jwtToken,
                username = passenger.Phone
            };

            return Ok(response);*/
        }

        [Route("passenger/[controller]/refresh")]
        [HttpPost]
        public async Task<IActionResult> PassengerRefreshToken()
        {
            // Получаем Refresh Token из кук
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh Token отсутствует.");
            }

            // Проверяем Refresh Token
            var response = await _passengerAuthService.GetRefreshResponse(new() { RefreshToken = refreshToken });
            if (response is null)
            {
                return Unauthorized();
            }
            else
            {
                Response.Cookies.Append("refreshToken", response.RefreshToken.Token, GetCookieOptions());
                return Ok(response.AccessToken);
            }
        }

        [HttpPost("passenger/[controller]/logout")]
        public IActionResult Logout()
        {
            // Удаление куки refreshToken
            Response.Cookies.Delete("refreshToken", GetCookieOptions());

            return Ok(new { Message = "Успешный выход." });
        }

        // TODO: переделать 
        /*[Route("carrier/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> CarrierLogin(CarrierDTO carrier)
        {
            if (carrier is null)
            {
                return BadRequest();
            }

            if (!await _carrierAuthService.Login(carrier))
            {
                return Unauthorized();
            }

            var jwtToken = _carrierAuthService.GetJwtSecurityToken(carrier);
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

            if (!await _driverAuthService.Login(driver))
            {
                return Unauthorized();
            }

            var jwtToken = _driverAuthService.GetJwtSecurityToken(driver);
            var response = new
            {
                access_token = jwtToken,
                username = driver.LicenseId
            };

            return Ok(response);
        }*/


        // TODO: аналогично
        /*[Route("carrier/[controller]/signin")]
        [HttpPost]
        public async Task<IActionResult> CarrierSignin(CarrierDTO carrier)
        {
            if (carrier is null)
            {
                return BadRequest();
            }

            if (!await _carrierAuthService.Signin(carrier))
            {
                return Unauthorized();
            }

            var jwtToken = _carrierAuthService.GetJwtSecurityToken(carrier);
            var response = new
            {
                access_token = jwtToken,
                username = carrier.Inn
            };

            return Ok(response);
        }*/
    }
}
