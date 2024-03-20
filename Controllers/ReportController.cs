using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public string RPT { get; private set; }

        public ReportController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("sql")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Get([FromQuery] Guid storeId, DateTime dateIni, DateTime dateFin)
        {
            // Obtener el valor de la consulta SQL almacenada en el campo
            var report = context.Reports.FirstOrDefault(x => x.ReportIdx == 13);

            if (report == null)
            {
                return NotFound();
            }

            var consulta = report.Query;

            var resultado = await context.Reports.FromSqlRaw(consulta).IgnoreQueryFilters().ToListAsync();

            return Ok(resultado);

            //// Ejecutar la consulta SQL
            //var result = context.Set<Report>().FromSqlRaw(query).ToListAsync();
            //return Ok(result);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Get()
        {
            var cpnsulta = await context.Reports
              .FromSqlInterpolated($"Select * from report").ToListAsync();

            return Ok(cpnsulta);
        }
    }

}

