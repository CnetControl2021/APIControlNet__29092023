using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

///hola
namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HoseController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public HoseController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }



        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<HoseDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Hoses.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var hose = await queryable.OrderByDescending(x => x.HoseIdi).Paginar(paginacionDTO)
                .Include(x => x.LoadPosition)
                .Include(x => x.ProductStore).ThenInclude(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<HoseDTO>>(hose);
        }


        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<HoseDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Hoses.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var hose = await queryable.OrderByDescending(x => x.HoseIdi).Paginar(paginacionDTO)
                .Include(x => x.LoadPosition)
                .Include(x => x.ProductStore).ThenInclude(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<HoseDTO>>(hose);
        }


        [HttpGet("{id:int}", Name = "obtHose")]
        public async Task<ActionResult<HoseDTO>> Get(int id)
        {
            var hose = await context.Hoses.FirstOrDefaultAsync(x => x.HoseIdx == id);

            if (hose == null)
            {
                return NotFound();
            }

            return mapper.Map<HoseDTO>(hose);
        }



        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post([FromBody] HoseDTO hoseDTO, Guid storeId)
        {

            var existeid = await context.Hoses.AnyAsync(x => x.HoseIdx == hoseDTO.HoseIdx);

            var hoseMap = mapper.Map<Hose>(hoseDTO);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = hoseMap.Name;
            var storeId2 = storeId;

            if (existeid)
            {
                context.Update(hoseMap);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.Hoses.AnyAsync(x => x.HoseIdi == hoseMap.HoseIdi && x.StoreId == hoseMap.StoreId);

                if (existe)
                {
                    return BadRequest($"Ya existe {hoseMap.HoseIdi} en esa sucursal ");
                }
                else
                {
                    context.Add(hoseMap);
                    await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                    await context.SaveChangesAsync();
                }
            }
            var HoseDTO2 = mapper.Map<HoseDTO>(hoseMap);
            return CreatedAtRoute("obtHose", new { id = hoseMap.HoseIdx }, HoseDTO2);
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(HoseDTO hoseDTO, Guid storeId)
        {
            var hoseDB = await context.Hoses.FirstOrDefaultAsync(c => c.HoseIdx == hoseDTO.HoseIdx);

            if (hoseDB is null)
            {
                return NotFound();
            }
            try
            {
                hoseDB = mapper.Map(hoseDTO, hoseDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = hoseDB.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe {hoseDTO.HoseIdi} ");
            }
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Hoses.AnyAsync(x => x.HoseIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Hoses.FirstOrDefaultAsync(x => x.HoseIdx == id);
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
                var existe = await context.Hoses.AnyAsync(x => x.HoseIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.Hoses.FirstOrDefaultAsync(x => x.HoseIdx == id);
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
