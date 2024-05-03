using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IslandController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public IslandController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        //[HttpGet]
        //public async Task<ActionResult<IslandDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        //{
        //    var queryable = context.Islands.AsQueryable();
        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
        //    }
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var islands = await queryable.Paginar(paginacionDTO).AsNoTracking()//.OrderByDescending(x => x.IslandIdx)
        //        .Select(x => new 
        //        {
        //            x.IslandIdx,
        //            x.IslandIdi,
        //            x.Name,
        //            x.StoreId,
        //            StoreName = x.Store.Name,
        //            x.Description
        //        }).Paginar(paginacionDTO).AsNoTracking()
        //        .ToListAsync();
        //    return Ok(islands);
        //}


        [HttpGet("{storeId?}")]
        //[AllowAnonymous]
        public async Task<IEnumerable<IslandDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Islands.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var island = await queryable.OrderByDescending(x => x.IslandIdx).Paginar(paginacionDTO)
                .Include(x => x.Store)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<IslandDTO>>(island);
        }

        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<IslandDTO>> Get2([FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.Islands.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var data = await queryable.OrderBy(x => x.IslandIdi)
                //.Include(x => x.Store)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<IslandDTO>>(data);
        }


        [HttpGet("skipTake")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTake(int skip, int take, Guid? storeId, string searchTerm = "")
        {
            IQueryable<Island> query = context.Islands.AsNoTracking();

            if (storeId.HasValue && storeId != Guid.Empty)
            {
                query = query.Where(x => x.StoreId == storeId);
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(searchTerm) || (c.Description != null && c.Description.ToLower().Contains(searchTerm)));
            }

            var ntotal = await query.CountAsync();
            var result = await query.Skip(skip).Take(take).OrderBy(x => x.IslandIdi).ToListAsync(); // Aplica paginado después de todos los filtros

            return Ok(new
            {
                Total = ntotal,
                Data = mapper.Map<List<IslandDTO>>(result)
            });
        }


        [HttpGet("activeSinPag")]
        //[AllowAnonymous]
        public async Task<IEnumerable<IslandDTO>> Get3([FromQuery] Guid storeId)
        {
            var queryable = context.Islands.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var island = await queryable.OrderByDescending(x => x.IslandIdx)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<IslandDTO>>(island);
        }


        [HttpGet("{id:int}", Name = "obtenerIsla")]
        public async Task<ActionResult<IslandDTO>> Get(int id)
        {
            var Isla = await context.Islands.FirstOrDefaultAsync(x => x.IslandIdx == id);
            if (Isla == null)
            {
                return NotFound();
            }
            return mapper.Map<IslandDTO>(Isla);
        }


        //[HttpPost("{storeId?}")]
        //public async Task<ActionResult> Post(IslandDTO IslandDTO, Guid storeId)
        //{
        //    var existeIDYNombre = await context.Islands.AnyAsync(x => x.IslandIdi == IslandDTO.IslandIdi && x.StoreId == IslandDTO.StoreId);

        //    var island = mapper.Map<Island>(IslandDTO);

        //    var usuarioId = obtenerUsuarioId();
        //    var ipUser = obtenetIP();
        //    var name = island.Name;
        //    var storeId2 = storeId;

        //    if (existeIDYNombre)
        //    {
        //        return BadRequest($"Ya existe {IslandDTO.IslandIdi} en esa sucursal ");
        //    }
        //    else
        //    {
        //        context.Add(island);
        //        await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
        //        await context.SaveChangesAsync();
        //    }

        //    var IslandDTO2 = mapper.Map<IslandDTO>(island);
        //    return CreatedAtRoute("obtenerIsla", new { id = island.IslandIdx }, IslandDTO2);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Post(Guid storeId, List<IslandDTO> islands)
        //{
        //    if (islands == null || !islands.Any())
        //    {
        //        return BadRequest("La lista de islas está vacía o nula.");
        //    }

        //    foreach (var dto in islands)
        //    {
        //        if (dto.IslandIdx.HasValue)
        //        {
        //            var entity = await context.Islands.FindAsync(dto.IslandIdx.Value);

        //            if (entity != null)
        //            {
        //                entity.IslandIdi = dto.IslandIdi;
        //                entity.StoreId = dto.StoreId;
        //                entity.Name = dto.Name;
        //                entity.Description = dto.Description;
        //                entity.Date = dto.Date ?? DateTime.Now;
        //                entity.Updated = DateTime.Now;
        //                entity.Active = dto.Active ?? true;
        //                entity.Locked = dto.Locked ?? false;
        //                entity.Deleted = dto.Deleted ?? false;
        //            }
        //        }
        //        if (dto.IslandIdx == 0)
        //        {
        //            // Lógica para añadir nuevos registros
        //            var newEntity = new Island
        //            {
        //                IslandIdi = dto.IslandIdi,
        //                StoreId = storeId,
        //                Name = dto.Name,
        //                Description = dto.Description,
        //                Date = dto.Date ?? DateTime.Now,
        //                Updated = DateTime.Now,
        //                Active = dto.Active ?? true,
        //                Locked = dto.Locked ?? false,
        //                Deleted = dto.Deleted ?? false
        //            };
        //            context.Islands.Add(newEntity);
        //        }
        //    }

        //    await context.SaveChangesAsync();
        //    return Ok();
        //}


        [HttpPost]
        public async Task<IActionResult> Post(Guid storeId, List<IslandDTO> islandDTOs)
        {

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = islandDTOs.LastOrDefault().Name;
            var storeId2 = storeId;
            var tabla = "Island";

            if (islandDTOs == null || !islandDTOs.Any())
            {
                return BadRequest("La lista está vacía o nula.");
            }

            foreach (var dto in islandDTOs)
            {
                var existingEntity = await context.Islands
                    .FindAsync(dto.IslandIdx);

                if (existingEntity != null)
                {

                    context.Entry(existingEntity).CurrentValues.SetValues(dto);
                    existingEntity.Updated = DateTime.Now;
                    context.Islands.Update(existingEntity);
                    await servicioBinnacle.EditBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
                }
                else if (!dto.IslandIdx.HasValue || dto.IslandIdx == 0)
                {
                    var newEntity = new Island
                    {
                        IslandIdi = dto.IslandIdi,
                        StoreId = storeId,
                        Name = dto.Name,
                        Description = dto.Description,
                        Date = dto.Date ?? DateTime.Now,
                        Updated = DateTime.Now,
                        Active = dto.Active ?? true,
                        Locked = dto.Locked ?? false,
                        Deleted = dto.Deleted ?? false

                    };
                    context.Islands.Add(newEntity);
                    await servicioBinnacle.AddBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
                }
            }
            await context.SaveChangesAsync();
            return Ok();
        }


        //[HttpPost]
        //public async Task<IActionResult> Post(Guid storeId, List<IslandDTO> islands)
        //{
        //    if (islands == null || !islands.Any())
        //    {
        //        return BadRequest("La lista de islas está vacía o nula.");
        //    }

        //    foreach (var dto in islands)
        //    {
        //        var existingEntity = await context.Islands
        //            .FirstOrDefaultAsync(i => i.IslandIdx == dto.IslandIdx);

        //        if (existingEntity != null)
        //        {
        //            context.Islands.Remove(existingEntity);
        //            await context.SaveChangesAsync(); 

        //            var newEntity = new Island
        //            {
        //                IslandIdi = dto.IslandIdi,
        //                StoreId = storeId, 
        //                Name = dto.Name,
        //                Description = dto.Description,
        //                Date = dto.Date ?? DateTime.Now,
        //                Updated = DateTime.Now,
        //                Active = dto.Active ?? true,
        //                Locked = dto.Locked ?? false,
        //                Deleted = dto.Deleted ?? false
        //            };
        //            context.Islands.Add(newEntity);
        //        }
        //        else if (!dto.IslandIdx.HasValue || dto.IslandIdx == 0)
        //        {
        //            var newEntity = new Island
        //            {
        //                IslandIdi = dto.IslandIdi,
        //                StoreId = storeId,
        //                Name = dto.Name,
        //                Description = dto.Description,
        //                Date = dto.Date ?? DateTime.Now,
        //                Updated = DateTime.Now,
        //                Active = dto.Active ?? true,
        //                Locked = dto.Locked ?? false,
        //                Deleted = dto.Deleted ?? false
        //            };
        //            context.Islands.Add(newEntity);
        //        }
        //    }
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}


        [HttpPut("{storeId?}")]
        public async Task<ActionResult> Put(IslandDTO islandDTO, Guid storeId)
        {
            var islanDB = await context.Islands.FirstOrDefaultAsync(c => c.IslandIdx == islandDTO.IslandIdx);

            if (islanDB is null)
            {
                return NotFound();
            }
            try
            {
                islanDB = mapper.Map(islandDTO, islanDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = islanDB.Name;

                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest($"Ya existe isla {islandDTO.IslandIdi} en esa sucursal ");
            }

        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Islands.AnyAsync(x => x.IslandIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Islands.FirstOrDefaultAsync(x => x.IslandIdx == id);
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
                var existe = await context.Islands.FirstOrDefaultAsync(x => x.IslandIdx == id);
                if (existe is null) { return NotFound(); }

                var lp = await context.LoadPositions.FirstOrDefaultAsync(x => x.IslandIdi == existe.IslandIdi && x.StoreId == storeId);
                
                if (lp is not null) { return BadRequest("Posicion de carga relacionada"); }
                else
                {
                    var name2 = await context.Islands.FirstOrDefaultAsync(x => x.IslandIdx == id);
                    context.Remove(name2);

                    var usuarioId = obtenerUsuarioId();
                    var ipUser = obtenetIP();
                    var name = name2.Name;
                    var storeId2 = storeId;
                    var tabla = "Island";
                    await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);

                    await context.SaveChangesAsync();
                    return NoContent();
                }
                
                
            }
            catch
            {
                //return NotFound("ERROR DE DATOS RELACIONADOS");
                return BadRequest("ERROR DE DATOS RELACIONADOS");
            }

        }
    }
}
