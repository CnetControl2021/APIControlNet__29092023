using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PortController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public PortController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle=servicioBinnacle;
        }


        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<PortDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.Ports.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId!= Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId==storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var port = await queryable.Paginar(paginacionDTO)
                .Include(x => x.Store)
                .Include(x => x.PortTypeIdiNavigation)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<PortDTO>>(port);
        }


        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<PortDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.Ports.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId==storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var port = await queryable.Paginar(paginacionDTO)
                .Include(x => x.Store)
                .Include(x => x.PortTypeIdiNavigation)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<PortDTO>>(port);
        }


        [HttpGet("ActiveSinPag")]
        //[AllowAnonymous]
        public async Task<IEnumerable<PortDTO>> Get3([FromQuery] Guid storeId)
        {
            var queryable = context.Ports.Where(x => x.Active == true).AsQueryable();
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var port = await queryable.OrderBy(x => x.PortIdi)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<PortDTO>>(port);
        }


        [HttpGet("{id:int}", Name = "obtenerPort")]
        public async Task<ActionResult<PortDTO>> Get(int id)
        {
            var Port = await context.Ports.FirstOrDefaultAsync(x => x.PortIdx == id);

            if (Port == null)
            {
                return NotFound();
            }

            return mapper.Map<PortDTO>(Port);
        }


        [HttpGet("byStore/{id2}")] //por store
        public async Task<ActionResult<List<PortDTO>>> Get([FromRoute] Guid id2)
        {
            var RSs = await context.Ports.Where(e => e.StoreId.Equals((Guid)id2)&& e.Active ==true)
                //.Include(x => x.Store)
                .ToListAsync();
            return mapper.Map<List<PortDTO>>(RSs);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid storeId, List<PortDTO> portDTOs)
        {
            if (portDTOs == null || !portDTOs.Any())
            {
                return BadRequest("La lista está vacía o nula.");
            }

            foreach (var dto in portDTOs)
            {
                var existingEntity = await context.Ports
                    .FindAsync(dto.PortIdx);

                if (existingEntity != null)
                {

                    context.Entry(existingEntity).CurrentValues.SetValues(dto);
                    existingEntity.Updated = DateTime.Now;
                    context.Ports.Update(existingEntity);
                }
                else if (!dto.PortIdx.HasValue || dto.PortIdx == 0)
                {
                    var newEntity = new Port
                    {
                        PortIdi = dto.PortIdi,
                        StoreId = storeId,
                        Name = dto.Name,
                        NameLinux = dto.NameLinux,
                        BaudRate = dto.BaudRate,
                        PortTypeIdi = dto.PortTypeIdi,
                        Date = DateTime.Now,
                        Updated = DateTime.Now,
                        Active = true,
                        Locked = false,
                        Deleted = false
                    };
                    context.Ports.Add(newEntity);
                }
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        //[HttpPost("{storeId?}")]
        //public async Task<ActionResult> Post([FromBody] PortDTO portDTO, Guid storeId)
        //{
        //    var existeid = await context.Ports.AnyAsync(x => x.PortIdx == portDTO.PortIdx);

        //    var portMap = mapper.Map<Port>(portDTO);

        //    var usuarioId = obtenerUsuarioId();
        //    var ipUser = obtenetIP();
        //    var name = portMap.Name;
        //    var storeId2 = storeId;

        //    if (existeid)
        //    {
        //        context.Update(portMap);
        //        await context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        var existe = await context.Ports.AnyAsync(x => x.PortIdi == (portMap.PortIdi) && x.StoreId == portMap.StoreId);

        //        if (existe)
        //        {
        //            return BadRequest($"Ya existe {portMap.PortIdi} en esa sucursal ");
        //        }
        //        else
        //        {
        //            context.Add(portMap);
        //            await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    var portDTO2 = mapper.Map<Port>(portMap);
        //    return CreatedAtRoute("obtenerPort", new { id = portMap.PortIdx }, portDTO2);
        //}


        //[HttpPut("{storeId?}")]
        //public async Task<IActionResult> Put(PortDTO portDTO, Guid storeId)
        //{
        //    var portDB = await context.Ports.FirstOrDefaultAsync(c => c.PortIdx == portDTO.PortIdx);

        //    if (portDB is null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        portDB = mapper.Map(portDTO, portDB);

        //        var storeId2 = storeId;
        //        var usuarioId = obtenerUsuarioId();
        //        var ipUser = obtenetIP();
        //        var tableName = portDB.Name;
        //        await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
        //        await context.SaveChangesAsync();

        //    }
        //    catch
        //    {
        //        return BadRequest($"Ya existe {portDTO.PortIdi} ");
        //    }
        //    return NoContent();
        //}


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Ports.AnyAsync(x => x.PortIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Ports.FirstOrDefaultAsync(x => x.PortIdx == id);
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
                var existe = await context.Ports.AnyAsync(x => x.PortIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.Ports.FirstOrDefaultAsync(x => x.PortIdx == id);
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

        [HttpGet("/portType/noPage")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PortTypeDTO>>> Get5()
        {
            var pt = await context.PortTypes.ToListAsync();
            return mapper.Map<List<PortTypeDTO>>(pt);
        }

    }
}
