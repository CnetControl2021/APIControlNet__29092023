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


        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<PointSaleDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
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
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var pointsale = await queryable.OrderByDescending(x => x.PointSaleIdi).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<PointSaleDTO>>(pointsale);
        }


        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<PointSaleDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.PointSales.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var pointsale = await queryable.OrderByDescending(x => x.PointSaleIdi).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<PointSaleDTO>>(pointsale);
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


        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post([FromBody] PointSaleDTO PointSaleDTO, Guid? storeId)
        {
            var existeid = await context.PointSales.AnyAsync(x => x.PointSaleIdi == PointSaleDTO.PointSaleIdi && x.StoreId == PointSaleDTO.StoreId);

            var pointsaleMap = mapper.Map<PointSale>(PointSaleDTO);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = pointsaleMap.Name;
            var storeId2 = storeId;

            if (existeid)
            {
                return BadRequest($"Ya existe {PointSaleDTO.StoreId} en esa empresa");
            }
            else
            {
                context.Add(pointsaleMap);

                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
            }
            return Ok();
            //var storeDTO2 = mapper.Map<PointSaleDTO>(employeeMap);
            //return CreatedAtRoute("getEmployee", new { id = employeeMap.EmployeeId }, storeDTO2);
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(PointSaleDTO PointSaleDTO, Guid storeId)
        {
            var pointsaleDB = await context.PointSales.FirstOrDefaultAsync(c => c.PointSaleIdx == PointSaleDTO.PointSaleIdx);

            if (pointsaleDB is null)
            {
                return NotFound();
            }
            try
            {
                pointsaleDB = mapper.Map(PointSaleDTO, pointsaleDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = pointsaleDB.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe {PointSaleDTO.Name} ");
            }
            return NoContent();
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
    }
}
