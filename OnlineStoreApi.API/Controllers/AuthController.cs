using Microsoft.AspNetCore.Mvc;
using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Business.Services;

namespace OnlineStoreApi.API.Controllers
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
        public async Task<ActionResult<TokenResponseDto>> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                    return BadRequest("Name, email, and password are required");

                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                    return BadRequest("Email and password are required");

                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
