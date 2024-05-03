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
    public class LoadPositionController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public LoadPositionController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle=servicioBinnacle;
        }


        [HttpGet("{storeId?}")]
        public async Task<IEnumerable<LoadPositionDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.LoadPositions.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId!= Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId==storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var loadPosition = await queryable.OrderByDescending(x => x.LoadPositionIdi).Paginar(paginacionDTO)
                .Include(x => x.Dispensary)
                .Include(x => x.Port)
                //.Include(x => x.Island)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<LoadPositionDTO>>(loadPosition);
        }


        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<LoadPositionDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.LoadPositions.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId==storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var loadPosition = await queryable.OrderByDescending(x => x.LoadPositionIdi).Paginar(paginacionDTO)
                .Include(x => x.Dispensary)
                .Include(x => x.Port)
                //.Include(x => x.Island)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<LoadPositionDTO>>(loadPosition);
        }


        [HttpGet("notPage")]
        [AllowAnonymous]
        public async Task<IEnumerable<LoadPositionDTO>> Get3([FromQuery] Guid storeId)
        {
            var queryable = context.LoadPositions.Where(x => x.Active == true).AsQueryable();
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var loadPosition = await queryable.OrderBy(x => x.LoadPositionIdi)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<LoadPositionDTO>>(loadPosition);
        }


        [HttpPost]
        public async Task<IActionResult> Post(Guid storeId, List<LoadPositionDTO> loadPositionDTOs)
        {
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = loadPositionDTOs.LastOrDefault().Name;
            var storeId2 = storeId;
            var tabla = "LoadPosition";

            if (loadPositionDTOs == null || !loadPositionDTOs.Any())
            {
                return BadRequest("La lista está vacía o nula.");
            }

            foreach (var dto in loadPositionDTOs)
            {
                var existingEntity = await context.LoadPositions
                    .FindAsync(dto.LoadPositionIdx);

                if (existingEntity != null)
                {

                    context.Entry(existingEntity).CurrentValues.SetValues(dto);
                    existingEntity.Updated = DateTime.Now;
                    context.LoadPositions.Update(existingEntity);
                    await servicioBinnacle.EditBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
                }
                else if (!dto.LoadPositionIdx.HasValue || dto.LoadPositionIdx == 0)
                {
                    var newEntity = new LoadPosition
                    {
                        StoreId = storeId,
                        LoadPositionIdi = dto.LoadPositionIdi,
                        Name = dto.Name,
                        DispensaryIdi = dto.DispensaryIdi,
                        PortIdi = dto.PortIdi,
                        IslandIdi = dto.IslandIdi,
                        CpuAddress = dto.CpuAddress,
                        CpuNumberLoop = dto.CpuNumberLoop,
                        FactorSetQuantity = dto.FactorSetQuantity,
                        FactorSetCurrency = dto.FactorSetCurrency,
                        FactorSetPrice = dto.FactorSetPrice,
                        FactorGetQuantity = dto.FactorGetQuantity,
                        FactorGetCurrency = dto.FactorGetCurrency,
                        FactorGetPrice = dto.FactorGetPrice,
                        FactorGetTotalQuantity = dto.FactorGetTotalQuantity,
                        FactorGetTotalCurrency = dto.FactorGetTotalCurrency,
                        FactorPulseWayne = dto.FactorPulseWayne,
                        DispensingMode = dto.DispensingMode,
                        IsSecurityEnabled = dto.IsSecurityEnabled,
                        PriceLevel = dto.PriceLevel,
                        MaximumQuantity = dto.MaximumQuantity,
                        MaximumAmount = dto.MaximumAmount,
                        DefaultQuantity = dto.DefaultQuantity,
                        MinimumQuantity = dto.MinimumQuantity,
                        MinimumAmount = dto.MinimumAmount,
                        QuantityPrefix = dto.QuantityPrefix,
                        IsStopCancelled = dto.IsStopCancelled,
                        IsEnableSaveToZero = dto.IsEnableSaveToZero,
                        AutomaticPrintingIsEnabled = dto.AutomaticPrintingIsEnabled,
                        PointSaleIdi = dto.PointSaleIdi,
                        Date = DateTime.Now,
                        Updated = DateTime.Now,
                        Active = true,
                        Locked = false,
                        Deleted = false
                    };
                    context.LoadPositions.Add(newEntity);
                    await servicioBinnacle.AddBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
                }
            }
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("{id:int}", Name = "obtenerLoadPosition")]
        public async Task<ActionResult<LoadPositionDTO>> Get(int id)
        {
            var loadPosition = await context.LoadPositions.FirstOrDefaultAsync(x => x.LoadPositionIdx == id);

            if (loadPosition == null)
            {
                return NotFound();
            }

            return mapper.Map<LoadPositionDTO>(loadPosition);
        }


        [HttpGet("byStore/{id2}")] //por store
        public async Task<ActionResult<List<LoadPositionDTO>>> Get([FromRoute] Guid id2)
        {
            var RSs = await context.LoadPositions.Where(e => e.StoreId.Equals((Guid)id2))
                .Include(x => x.Store)
                .ToListAsync();
            return mapper.Map<List<LoadPositionDTO>>(RSs);
        }


        //[HttpPost("{storeId?}")]
        //public async Task<ActionResult> Post([FromBody] LoadPositionDTO loadPositionDTO, Guid storeId)
        //{

        //    var existeid = await context.LoadPositions.AnyAsync(x => x.LoadPositionIdx == loadPositionDTO.LoadPositionIdx);

        //    var loadPosition = mapper.Map<LoadPosition>(loadPositionDTO);

        //    var usuarioId = obtenerUsuarioId();
        //    var ipUser = obtenetIP();
        //    var name = loadPosition.Name;
        //    var storeId2 = storeId;

        //    if (existeid)
        //    {
        //        context.Update(loadPosition);
        //        await context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        var existe = await context.LoadPositions.AnyAsync(x => x.LoadPositionIdi == (loadPosition.LoadPositionIdi) && x.StoreId == loadPosition.StoreId);

        //        if (existe)
        //        {
        //            return BadRequest($"Ya existe {loadPosition.LoadPositionIdi} en esa sucursal ");
        //        }
        //        else
        //        {
        //            context.Add(loadPosition);
        //            await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    var LoadPositionDTO2 = mapper.Map<LoadPositionDTO>(loadPosition);
        //    return CreatedAtRoute("obtenerPort", new { id = loadPositionDTO.LoadPositionIdx }, LoadPositionDTO2);
        //}


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(LoadPositionDTO loadPositionDTO, Guid storeId)
        {
            var loadPoistionDB = await context.LoadPositions.FirstOrDefaultAsync(c => c.LoadPositionIdx == loadPositionDTO.LoadPositionIdx);

            if (loadPoistionDB is null)
            {
                return NotFound();
            }
            try
            {
                loadPoistionDB = mapper.Map(loadPositionDTO, loadPoistionDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = loadPoistionDB.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe {loadPositionDTO.LoadPositionIdi} ");
            }
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.LoadPositions.AnyAsync(x => x.LoadPositionIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.LoadPositions.FirstOrDefaultAsync(x => x.LoadPositionIdx == id);
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
                var existe = await context.LoadPositions.FirstOrDefaultAsync(x => x.LoadPositionIdx == id);
                if (existe is null) { return NotFound(); }

                var exist = await context.Hoses.FirstOrDefaultAsync(x => x.LoadPositionIdi == existe.LoadPositionIdi);
                if (exist is not null) { return BadRequest("Manguera relacionada"); }   

                var name2 = await context.LoadPositions.FirstOrDefaultAsync(x => x.LoadPositionIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.Name;
                var storeId2 = storeId;
                var tabla = "LoadPositions";
                await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);

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
