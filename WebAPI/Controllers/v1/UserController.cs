using Application.Interfaces.IServices;
using Application.Wrappers;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace WebAPI.Controllers.v1
{
    /// <summary>
    /// UserController
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }


        [HttpGet("getDetailsUser")]
        [SwaggerOperation(
            Summary = "Obtener todos los detalles del usuario",
            Description = "Tener en cuenta IDs cada usuario")]
        public async Task<IActionResult> GetDetailsUser([FromQuery][Description("Id del usuario")] int userId)
        {
            var resultado = await _service.GetDetailsUser(userId);

            return Ok(new Response<UserDetailsDTO>
            {
                Success = true,
                Data = resultado
            });

        }

    }
}
