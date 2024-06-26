﻿using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Data;
using System.Globalization;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;
        private readonly IConfiguration _configuration;

        public TestController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager,
                ServicioBinnacle servicioBinnacle, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
            this._configuration = configuration;
        }

        private const int batchSize = 1000;  // Define el tamaño del lote

        [HttpPost("importar")]
        [AllowAnonymous]
        public async Task<IActionResult> Importar(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length <= 0)
            {
                return BadRequest("Sube un archivo válido.");
            }

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    var dataTests = new List<Datatest>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        dataTests.Add(new Datatest
                        {
                            //Idx = int.Parse(worksheet.Cells[row, 1].Value?.ToString().Trim() ?? "0"),
                            //DatatestId = Guid.TryParse(worksheet.Cells[row, 2].Value?.ToString().Trim(), out Guid resultGuid) ? resultGuid : null,
                            DatatestId = Guid.NewGuid(),
                            DatatestNumber = int.TryParse(worksheet.Cells[row, 1].Value?.ToString().Trim(), out int resultNumber) ? resultNumber : null,
                            Name = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                            Descripcion = worksheet.Cells[row, 3].Value?.ToString().Trim(),
                            // Intenta convertir el valor de la sexta columna (Date) en un DateTime. Si la conversión es exitosa, asigna el valor, de lo contrario, asigna null.
                            //Date = DateTime.TryParse(worksheet.Cells[row, 4].Value?.ToString().Trim(), out DateTime resultDate) ? resultDate : null,
                            // Extrae y convierte la fecha de la celda a DateTime, asumiendo el formato D/M/YYYY
                            Date = worksheet.Cells[row, 4].Value is double oleDate1 ? DateTime.FromOADate(oleDate1) : (worksheet.Cells[row, 4].Value is DateTime dt1 ? dt1 : (DateTime?)null),
                            // Extrae y convierte la fecha y hora de la celda a DateTime, asumiendo el formato YYYY-MM-DD HH:MM:SS
                            Updated = worksheet.Cells[row, 5].Value is double oleDate2 ? DateTime.FromOADate(oleDate2) : (worksheet.Cells[row, 5].Value is DateTime dt2 ? dt2 : (DateTime?)null),
                            Active = bool.TryParse(worksheet.Cells[row, 6].Value?.ToString().Trim(), out bool resultActive) ? resultActive : null,
                            Locked = bool.TryParse(worksheet.Cells[row, 7].Value?.ToString().Trim(), out bool resultLocked) ? resultLocked : null,
                            Deleted = bool.TryParse(worksheet.Cells[row, 8].Value?.ToString().Trim(), out bool resultDeleted) ? resultDeleted : null,
                        });

                        // Guardar en lotes cada 1000 registros
                        if (dataTests.Count >= batchSize)
                        {
                            context.Datatests.AddRange(dataTests);
                            await context.SaveChangesAsync();
                            dataTests.Clear(); // Limpiar la lista para el siguiente lote
                        }
                    }

                    // Guardar los últimos registros si quedan algunos
                    if (dataTests.Count > 0)
                    {
                        context.Datatests.AddRange(dataTests);
                        await context.SaveChangesAsync();
                    }
                }
            }

            return Ok();
        }

    }
}
