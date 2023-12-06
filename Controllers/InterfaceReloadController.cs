using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterfaceReloadController : ControllerBase
    {
        [HttpPost("restart")]
        public IActionResult RestartSystem()
        {
            try
            {
                // Ejecutar el comando de reinicio del sistema
                var processInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"sudo shutdown -r now\"", // Comando para reiniciar el sistema
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();
                    process.WaitForExit();

                    // Puedes manejar la salida del proceso si es necesario
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();

                    return Ok(new { Message = "Reinicio del sistema iniciado." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Error al reiniciar el sistema: {ex.Message}" });
            }
        }
    }
}
