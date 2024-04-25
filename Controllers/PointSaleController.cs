using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PointSaleController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public PointSaleController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("sinPag/{nombre}")]
        ////[AllowAnonymous]
        public async Task<IEnumerable<PointSaleDTO>> Get(Guid storeId, string nombre)
        {
            var queryable = context.PointSales.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var pointsale = await queryable
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<PointSaleDTO>>(pointsale);
        }


        [HttpGet("{id:int}", Name = "getPointsale")]
        public async Task<ActionResult<PointSaleDTO>> Get(int id)
        {
            var pointsale = await context.PointSales.FirstOrDefaultAsync(x => x.PointSaleIdx == id);

            if (pointsale == null)
            {
                return NotFound();
            }

            return mapper.Map<PointSaleDTO>(pointsale);
        }


        [HttpPost]
        public async Task<IActionResult> Post(Guid storeId, List<PointSaleDTO> pointSaleDTOs)
        {
            if (pointSaleDTOs == null || !pointSaleDTOs.Any())
            {
                return BadRequest("Sin datos");
            }
            foreach (var dto in pointSaleDTOs)
            {
                var existingEntity = await context.PointSales
                    .FindAsync(dto.PointSaleIdx);

                if (existingEntity != null)
                {
                    context.Entry(existingEntity).CurrentValues.SetValues(dto);
                    existingEntity.Updated = DateTime.Now;
                    context.PointSales.Update(existingEntity);
                }
                else if (!dto.PointSaleIdx.HasValue || dto.PointSaleIdx == 0)
                {
                    var newEntity = new PointSale
                    {
                        StoreId = storeId,
                        PointSaleIdi = dto.PointSaleIdi,
                        PortIdi = dto.PortIdi,
                        Name = dto.Name,
                        Type = dto.Type,
                        Subtype = dto.Subtype,
                        Address = dto.Address,
                        PrinterBaudRate = dto.PrinterBaudRate,
                        PrinterBrandId = dto.PrinterBrandId,
                        PrinterIdi = dto.PrinterIdi,
                        TypeAuthorization = dto.TypeAuthorization,
                        PointSaleUnique = dto.PointSaleUnique,
                        InvoiceSerieId = dto.InvoiceSerieId,
                        IsEnabledPrintToPrinterIdi = dto.IsEnabledPrintToPrinterIdi,
                        Active = true,
                        Locked = false,
                        Deleted = false
                    };
                    context.PointSales.Add(newEntity);
                }
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("notPage")]
        public async Task<IEnumerable<PointSaleDTO>> Get3([FromQuery] Guid storeId)
        {
            var queryable = context.PointSales.Where(x => x.Active == true).AsQueryable();
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var tps = await queryable.OrderBy(x => x.PointSaleIdi)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<PointSaleDTO>>(tps);
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.PointSales.AnyAsync(x => x.PointSaleIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.PointSales.FirstOrDefaultAsync(x => x.PointSaleIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            try
            {
                var existe = await context.PointSales.AnyAsync(x => x.PointSaleIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.PointSales.FirstOrDefaultAsync(x => x.PointSaleIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.Name;
                var storeId2 = storeId;
                await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return BadRequest("ERROR DE DATOS RELACIONADOS");
            }
        }

        [HttpGet("/printerBrand/noPage")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PrinterBrand>>> Get6()
        {
            var pb = await context.PrinterBrands.ToListAsync();
            return Ok(pb);
        }
    }
}
