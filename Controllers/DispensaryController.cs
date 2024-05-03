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


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DispensaryController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public DispensaryController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("{storeId?}")]
        public async Task<IEnumerable<DispensaryDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Dispensaries.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var dispensary = await queryable.Paginar(paginacionDTO)
                .Include(x => x.Store)
                .Include(x => x.DispensaryBrand)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<DispensaryDTO>>(dispensary);
        }

        //[HttpGet("activeSinPag")]
        //[AllowAnonymous]
        //public async Task<ActionResult> Get3()
        //{
        //    var datos = await context.Dispensaries.ToListAsync();
        //    return Ok(datos);
        //}



        [HttpGet("ActiveSinPag")]
        [AllowAnonymous]
        public async Task<IEnumerable<DispensaryDTO>> Get3([FromQuery] Guid storeId)
        {
            var queryable = context.Dispensaries.Where(x => x.Active == true && x.Deleted == false).AsQueryable();

            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var dp = await queryable.OrderBy(x => x.DispensaryIdi)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<DispensaryDTO>>(dp);
        }


        [HttpGet("{id:int}", Name = "obtenerDispensary")]
        public async Task<ActionResult<DispensaryDTO>> Get(int id)
        {
            var Dispensary = await context.Dispensaries.FirstOrDefaultAsync(x => x.DispensaryIdx == id);

            if (Dispensary == null)
            {
                return NotFound();
            }

            return mapper.Map<DispensaryDTO>(Dispensary);
        }


        [HttpGet("byStore/{id2}")] //por store
        public async Task<ActionResult<List<DispensaryDTO>>> Get([FromRoute] Guid id2)
        {
            var RSs = await context.Dispensaries.Where(e => e.StoreId.Equals((Guid)id2))
                .Include(x => x.Store)
                .ToListAsync();
            return mapper.Map<List<DispensaryDTO>>(RSs);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid storeId, List<DispensaryDTO> dispensaries)
        {
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = dispensaries.LastOrDefault().Name;
            var storeId2 = storeId;
            var tabla = "Dispensary";

            if (dispensaries == null || !dispensaries.Any())
            {
                return BadRequest("La lista está vacía o nula.");
            }

            foreach (var dto in dispensaries)
            {
                var existingEntity = await context.Dispensaries
                    .FindAsync(dto.DispensaryIdx);

                if (existingEntity != null)
                {
                    
                    context.Entry(existingEntity).CurrentValues.SetValues(dto);
                    existingEntity.Updated = DateTime.Now;
                    context.Dispensaries.Update(existingEntity);
                    await servicioBinnacle.EditBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
                }
                else if (!dto.DispensaryIdx.HasValue || dto.DispensaryIdx == 0)
                {
                    var newEntity = new Dispensary
                    {
                        DispensaryIdi = dto.DispensaryIdi,
                        StoreId = storeId,
                        Name = dto.Name,
                        SatMeasurementType = dto.SatMeasurementType,
                        SatMeasurementPercentageUncertainty = dto.SatMeasurementPercentageUncertainty,
                        SatCalibrationDate = dto.SatCalibrationDate,
                        DispensaryBrandId = dto.DispensaryBrandId,
                        Date = DateTime.Now,
                        Updated = DateTime.Now,
                        Active = true,
                        Locked = false,
                        Deleted = false,
                        UniqueId = Guid.NewGuid().ToString()
                    };
                    context.Dispensaries.Add(newEntity);
                    await servicioBinnacle.AddBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
                }
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        //[HttpPost("{storeId?}")]
        //public async Task<ActionResult> Post([FromBody] DispensaryDTO dispensaryDTO, Guid storeId)
        //{

        //    var existeid = await context.Dispensaries.AnyAsync(x => x.DispensaryIdx == dispensaryDTO.DispensaryIdx);

        //    var Dispensary = mapper.Map<Dispensary>(dispensaryDTO);

        //    var usuarioId = obtenerUsuarioId();
        //    var ipUser = obtenetIP();
        //    var name = Dispensary.Name;
        //    var storeId2 = storeId;

        //    if (existeid)
        //    {
        //        context.Update(Dispensary);
        //        await context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        var existe = await context.Dispensaries.AnyAsync(x => x.DispensaryIdi == (dispensaryDTO.DispensaryIdi) && x.StoreId == dispensaryDTO.StoreId);

        //        if (existe)
        //        {
        //            return BadRequest($"Ya existe {dispensaryDTO.DispensaryIdi} en esa sucursal ");
        //        }
        //        else
        //        {
        //            context.Add(Dispensary);
        //            await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    var DispensaryDTO2 = mapper.Map<DispensaryDTO>(Dispensary);
        //    return CreatedAtRoute("obtenerDispensary", new { id = Dispensary.DispensaryIdx }, DispensaryDTO2);
        //}


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(DispensaryDTO dispensaryDTO, Guid storeId)
        {
            var dispensaryDB = await context.Dispensaries.FirstOrDefaultAsync(c => c.DispensaryIdx == dispensaryDTO.DispensaryIdx);

            if (dispensaryDB is null)
            {
                return NotFound();
            }
            try
            {
                dispensaryDB = mapper.Map(dispensaryDTO, dispensaryDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = dispensaryDB.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
                return Ok(dispensaryDB);

            }
            catch
            {
                return BadRequest($"Ya existe dispensario {dispensaryDTO.DispensaryIdi} en esa sucursal ");
            }

        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Dispensaries.AnyAsync(x => x.DispensaryIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Dispensaries.FirstOrDefaultAsync(x => x.DispensaryIdx == id);
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
                var existe = await context.Dispensaries.FirstOrDefaultAsync(x => x.DispensaryIdx == id);
                if (existe is null) { return NotFound(); }

                var lp = await context.LoadPositions.FirstOrDefaultAsync(x => x.DispensaryIdi == existe.DispensaryIdi && x.StoreId == storeId);

                if (lp is not null) { return BadRequest("Posicion de carga relacionada"); }
                else
                {
                    var name2 = await context.Dispensaries.FirstOrDefaultAsync(x => x.DispensaryIdx == id);
                    context.Remove(name2);

                    var usuarioId = obtenerUsuarioId();
                    var ipUser = obtenetIP();
                    var name = name2.Name;
                    var storeId2 = storeId;
                    var tabla = "Dispensaries";
                    await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);

                    await context.SaveChangesAsync();
                    return NoContent();
                }
            }
            catch
            {
                return BadRequest("ERROR DE DATOS RELACIONADOS");
            }

        }

        [HttpGet("/typeSistemMedition/noPage")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<JsonTipoSistemaMedicion>>> Get5()
        {
            var tipoSistemaMedicion = await context.JsonTipoSistemaMedicions.ToListAsync();
            return Ok(tipoSistemaMedicion);
        }
    }
}
