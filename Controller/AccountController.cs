using DemoApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Okta.AspNetCore;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace DemoApplication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private HttpClient client = new HttpClient();
        private readonly ITokenService _tokenService;

        public AccountController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("GetToken")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAndGetToken(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
                return BadRequest("Please enter User Credentials");

            var token = await _tokenService.GetToken(username, password);
            
            return Ok(token);
        }        
    }
}
