using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using App.Core.Entities;
using App.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using App.Application.DTO.Requests;
using App.Application.Services;

namespace App.WEB.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly CookieSettings _cookieSettings;

        public AuthController(
            IAuthService authService,
            IOptions<CookieSettings> cookieSettings)
        {
            _authService = authService;
            _cookieSettings = cookieSettings.Value;
        }

        private CookieOptions GetCookieOptions(DateTimeOffset expires)
        {
            return new CookieOptions
            {
                HttpOnly = _cookieSettings.HttpOnly,
                Secure = _cookieSettings.Secure,
                SameSite = Enum.Parse<SameSiteMode>(_cookieSettings.SameSite),
                Expires = expires
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            Response.Cookies.Append("refreshToken", response.RefreshToken, GetCookieOptions(response.RefreshTokenExpiresAt));
            return Ok(new { response.AccessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // Получаем Access Token из заголовка Authorization
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Получаем Refresh Token из кук
            var refreshToken = Request.Cookies["refreshToken"];
            if(refreshToken == null)
                throw new UnauthorizedAccessException("Refresh token not found in cookies");

            // Проверяем Refresh Token
            var response = await _authService.RefreshAsync(accessToken, refreshToken);
            Response.Cookies.Append("refreshToken", response.RefreshToken, GetCookieOptions(response.RefreshTokenExpiresAt));
            return Ok(new { response.AccessToken });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Удаление куки refreshToken
            Response.Cookies.Delete("refreshToken");
            return Ok(new { Message = "Successful logout" });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            return Ok(await _authService.GetMe());
        }

        [HttpPost("passenger/register")]
        public async Task<IActionResult> PassengerRegister(PassengerRegisterRequest request)
        {
            var response = await _authService.RegisterPassengerAsync(request);
            Response.Cookies.Append("refreshToken", response.RefreshToken, GetCookieOptions(response.RefreshTokenExpiresAt));
            return Ok(new { response.AccessToken });
        }

        /*[HttpPost("driver/register")]
        public async Task<IActionResult> DriverRegister(DriverRegisterRequest request)
        {
            var response = await _authService.RegisterDriverAsync(request);
            Response.Cookies.Append("refreshToken", response.RefreshToken, GetCookieOptions(response.RefreshTokenExpiresAt));
            return Ok(new { response.AccessToken });
        }

        [HttpPost("carrier/register")]
        public async Task<IActionResult> CarrierRegister(CarrierRegisterRequest request)
        {
            var response = await _authService.RegisterCarrierAsync(request);
            Response.Cookies.Append("refreshToken", response.RefreshToken, GetCookieOptions(response.RefreshTokenExpiresAt));
            return Ok(new { response.AccessToken });
        }*/
    }
}
