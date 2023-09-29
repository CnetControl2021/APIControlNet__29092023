using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIControlNet.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class CustomBaseController : ControllerBase
    {
        protected string obtenerUsuarioId()
        {
            var usuarioClaim = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            var usuarioId = usuarioClaim.Value;
            return usuarioId;
        }

        protected string obtenetIP()
        {
            var Ip = HttpContext.Connection.RemoteIpAddress.ToString();
            return Ip;
        }
    }
}
