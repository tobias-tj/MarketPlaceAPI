using Application.Interfaces.IServices;
using Application.Wrappers;
using Domain.DTOs;
using Domain.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var response = await _authService.Register(dto);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var response = await _authService.Login(dto);
            return Ok(response);
        }

        [HttpPost("send-reset-pin")]
        public async Task<IActionResult> SendResetPin([FromBody] SendResetPassword sendResetPassword)
        {
            await _authService.GenerateAndSendResetPin(sendResetPassword);
            return Ok(new { success = true, message = "Se ha enviado un código a tu correo electrónico." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            await _authService.ResetPassword(dto);
            return Ok(new { success = true, message = "Se ha restablecido la contraseña correctamente." });
        }

        [HttpGet("valid-pin")]
        public async Task<IActionResult> ValidPin([FromQuery] ValidPin request)
        {
            var response = await _authService.ValidPin(request);
            return Ok(new Response<IsPinValid>
            {
                Success = true,
                Data = new IsPinValid { IsExist = response }
            }
           );
        }
    }
}
