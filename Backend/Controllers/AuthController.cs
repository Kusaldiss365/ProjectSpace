using Microsoft.AspNetCore.Mvc;
using ProjectSpace.Dtos.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ProjectSpace.Services.AuthService;

namespace ProjectSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;


        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var user = await _authService.RegisterAsync(registerDto);

                return Ok(new
                {
                    message = "User registered successfully.",
                    user
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto) 
        {
            try
            {
                var user = await _authService.LoginAsync(loginDto);

                return Ok(user);
            }
            catch (Exception ex) 
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }

            
        }


        //Only authenticated users can call this endpoint.
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            try { 
                //read the user id from the token
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new
                    {
                        message = "User ID claim not found."
                    });
                }

                var user = await _authService.GetCurrentUserAsync(userId);

                if (user == null)
                {
                    return NotFound(new
                    {
                        message = "User not found."
                    });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


    }
}

