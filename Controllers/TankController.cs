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
    public class TankController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public TankController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<TankDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.Tanks.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId==storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var tanks = await queryable.OrderByDescending(x => x.TankIdx).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<TankDTO>>(tanks);
        }

        [HttpGet("byStore/{id2}")] //por store
        //[AllowAnonymous]
        public async Task<ActionResult<List<TankDTO>>> Get2([FromRoute] Guid id2)
        {
            var RSs = await context.Tanks.Where(e => e.StoreId.Equals((Guid)id2))
                .ToListAsync();
            return mapper.Map<List<TankDTO>>(RSs);
        }


        [HttpGet("{id:int}", Name = "obtenerTank")]
        public async Task<ActionResult<TankDTO>> Get(int id)
        {
            var tank = await context.Tanks.FirstOrDefaultAsync(x => x.TankIdx == id);

            if (tank == null)
            {
                return NotFound();
            }
            return mapper.Map<TankDTO>(tank);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<TankDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.Tanks.Where(TankDB => TankDB.Name.Contains(nombre)).ToListAsync();
            return mapper.Map<List<TankDTO>>(RSs);
        }


        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post([FromBody] TankDTO tankDTO, Guid storeId)
        {
            var existeid = await context.Tanks.AnyAsync(x => x.TankIdi == tankDTO.TankIdi);

            var tankMap = mapper.Map<Tank>(tankDTO);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = tankMap.Name;
            var storeId2 = storeId;

            if (existeid)
            {
                context.Update(tankMap);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.Tanks.AnyAsync(x => x.TankIdi == (tankMap.TankIdi) && x.StoreId == tankMap.StoreId);

                if (existe)
                {
                    return BadRequest($"Ya existe {tankMap.TankIdi} en esa sucursal ");
                }
                else
                {
                    context.Add(tankMap);
                    await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                    await context.SaveChangesAsync();
                }
            }
            var tankDTO2 = mapper.Map<Tank>(tankMap);
            return CreatedAtRoute("obtenerTank", new { id = tankMap.TankIdx }, tankDTO2);
        }

        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(TankDTO tankDTO, Guid storeId)
        {
            var tankDB = await context.Tanks.FirstOrDefaultAsync(c => c.TankIdx == tankDTO.TankIdx);

            if (tankDB is null)
            {
                return NotFound();
            }
            try
            {
                tankDB = mapper.Map(tankDTO, tankDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = tankDB.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
                return Ok();

            }
            catch(Exception)
            {
                return BadRequest($"Ya existe {tankDTO.TankIdi} ");
            }
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Tanks.AnyAsync(x => x.TankIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Tanks.FirstOrDefaultAsync(x => x.TankIdx == id);
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
                var existe = await context.Tanks.AnyAsync(x => x.TankIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.Tanks.FirstOrDefaultAsync(x => x.TankIdx == id);
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
