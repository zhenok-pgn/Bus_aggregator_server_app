using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using App.BLL.Interfaces;
using App.BLL.DTO;
using App.DAL.Entities;

namespace App.WEB.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthService<PassengerDTO> PassengerAuthService { get; set; }
        public IAuthService<CarrierDTO> CarrierAuthService { get; set; }
        public IAuthService<DriverDTO> DriverAuthService { get; set; }

        public AuthController(
            IAuthService<PassengerDTO> passengerAuthService,
            IAuthService<CarrierDTO> carrierAuthService,
            IAuthService<DriverDTO> driverAuthService)
        {
            PassengerAuthService = passengerAuthService;
            CarrierAuthService = carrierAuthService;
            DriverAuthService = driverAuthService;
        }

        [Route("passenger/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> PassengerLogin(string? returnUrl)
        {
            // получаем из формы email и пароль
            var form = HttpContext.Request.Form;
            if (!await PassengerAuthService.IsDataCorrect(form))
            {
                return BadRequest();
            }

            PassengerDTO? passenger = await PassengerAuthService.FindAndCheckUser(form);
            if (passenger is null) 
            { 
                return Unauthorized();
            }

            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, passenger.Phone),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, passenger.Role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            return Redirect(returnUrl ?? "/");


            // если email и/или пароль не установлены, посылаем статусный код ошибки 400
            if (!form.ContainsKey("email") || !form.ContainsKey("password"))
                return Results.BadRequest("Email и/или пароль не установлены");
            string email = form["email"];
            string password = form["password"];

            // находим пользователя 
            Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
            // если пользователь не найден, отправляем статусный код 401
            if (person is null) return Results.Unauthorized();
            var claims = new List<Claim>
    {
        new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role.Name)
    };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await context.SignInAsync(claimsPrincipal);
            return Results.Redirect(returnUrl ?? "/");
        }

        [Route("carrier/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> CarrierLogin(string? returnUrl)
        {
            // получаем из формы email и пароль
            var form = HttpContext.Request.Form;
            if (!await CarrierAuthService.IsDataCorrect(form))
            {
                return BadRequest();
            }

            CarrierDTO? carrier = await CarrierAuthService.FindAndCheckUser(form) as CarrierDTO;
            if (carrier is null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, carrier.Phone),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, carrier.Role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            return Redirect(returnUrl ?? "/");
        }

        [Route("driver/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> DriverLogin(string? returnUrl)
        {
            // получаем из формы email и пароль
            var form = HttpContext.Request.Form;
            if (!await DriverAuthService.IsDataCorrect(form))
            {
                return BadRequest();
            }

            DriverDTO? driver = await DriverAuthService.FindAndCheckUser(form) as DriverDTO;
            if (passenger is null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, driver.Phone),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, driver.Role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            return Redirect(returnUrl ?? "/");
        }

        [Route("passenger/[controller]/signin")]
        [HttpPost]
        public async Task<IActionResult> PassengerSignin(string? returnUrl)
        {
            // получаем из формы email и пароль
            var form = HttpContext.Request.Form;
            if (!await PassengerAuthService.IsDataCorrect(form))
            {
                return BadRequest();
            }

            if (PassengerAuthService.IsUserExist(form))
            {
                return Unauthorized();
            }


            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, passenger.Phone),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, passenger.Role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            return Redirect(returnUrl ?? "/");
        }

        [Route("carrier/[controller]/signin")]
        [HttpPost]
        public async Task<IActionResult> CarrierSignin(string? returnUrl)
        {
            // получаем из формы email и пароль
            var form = HttpContext.Request.Form;
            if (!await CarrierAuthService.IsDataCorrect(form))
            {
                return BadRequest();
            }

            if (CarrierAuthService.IsUserExist(form))
            {
                return Unauthorized();
            }

            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, carrier.Phone),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, carrier.Role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            return Redirect(returnUrl ?? "/");
        }

        [Route("[controller]/logout")]
        [HttpGet]
        public async Task<string> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "Данные удалены";
        }
}
