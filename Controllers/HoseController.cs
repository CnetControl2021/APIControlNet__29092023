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
        [AllowAnonymous]
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


        [HttpGet("notPage")]
        [AllowAnonymous]
        public async Task<IEnumerable<HoseDTO>> Get3([FromQuery] Guid storeId)  
        {
            var queryable = context.Hoses.Where(x => x.Active == true).AsQueryable();
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var h = await queryable.OrderBy(x => x.HoseIdi)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<HoseDTO>>(h);
        }


        [HttpGet("/hose/productIsFuel")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductDTO>> Product()
        {
            var queryable = context.Products.Where(x => x.Active == true && x.IsFuel == true).AsQueryable();

            var p = await queryable.OrderBy(x => x.ProductIdx)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductDTO>>(p);
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

        [HttpPost]
        public async Task<IActionResult> Post(Guid storeId, List<HoseDTO> hoseDTOs)
        {
            if (hoseDTOs == null || !hoseDTOs.Any())
            {
                return BadRequest("Sin datos");
            }
            foreach (var dto in hoseDTOs)
            {
                var existingEntity = await context.Hoses
                    .FindAsync(dto.HoseIdx);

                if (existingEntity != null)
                {

                    context.Entry(existingEntity).CurrentValues.SetValues(dto);
                    existingEntity.Updated = DateTime.Now;
                    context.Hoses.Update(existingEntity);
                }
                else if (!dto.HoseIdx.HasValue || dto.HoseIdx == 0)
                {
                    var newEntity = new Hose
                    {
                        StoreId = storeId,
                        HoseIdi = dto.HoseIdi,
                        Name = dto.Name,
                        LoadPositionIdi = dto.LoadPositionIdi,
                        ProductId = dto.ProductId,
                        CpuAddressHose = dto.CpuAddressHose,
                        Position = dto.Position,
                        SlowFlow = dto.SlowFlow,
                        Date = DateTime.Now,
                        Updated = DateTime.Now,
                        Active = true,
                        Locked = false,
                        Deleted = false
                    };
                    context.Hoses.Add(newEntity);
                }
            }
            await context.SaveChangesAsync();
            return Ok();
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
