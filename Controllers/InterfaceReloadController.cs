using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterfaceReloadController : ControllerBase
    {
        [HttpPost]  
        [Route("api/restart")]
        public IActionResult RestartServer()
        {
            var psi = new ProcessStartInfo("reboot");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
            return Ok();
        }

        [HttpPost]
        public IActionResult Restart()
        {
            // Aquí puedes ejecutar el comando de reinicio en el sistema Linux.
            Process.Start("sudo /sbin/reboot");

            // Puedes devolver una respuesta HTTP adecuada.
            return Ok("El sistema está reiniciando.");
        }

        [HttpGet("restart")]
        public IActionResult RestartSystem()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"reboot\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = new Process { StartInfo = psi };
            process.Start();
            process.WaitForExit();
            return Ok();
        }
    }
}
