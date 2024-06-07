using APIControlNet.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class healthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            // Aquí puedes agregar cualquier lógica adicional para verificar la salud de otros componentes o servicios.
            return Ok("API is up and running!");
        }
    }
    
}
