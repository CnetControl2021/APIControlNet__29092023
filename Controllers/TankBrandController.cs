using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TankBrandController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public TankBrandController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TankBrandDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.TankBrands.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var tanksbrands = await queryable.OrderByDescending(x => x.TankBrandIdx).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<TankBrandDTO>>(tanksbrands);
        }

        [HttpGet("{id:int}", Name = "obtenerTankBrand")]
        public async Task<ActionResult<TankBrandDTO>> Get(int id)
        {
            var tankBrands = await context.TankBrands.FirstOrDefaultAsync(x => x.TankBrandIdx == id);

            if (tankBrands == null)
            {
                return NotFound();
            }
            return mapper.Map<TankBrandDTO>(tankBrands);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<TankBrandDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.TankBrands.Where(TankBrandDB => TankBrandDB.Name.Contains(nombre)).ToListAsync();
            return mapper.Map<List<TankBrandDTO>>(RSs);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TankBrandDTO TankBrandDTO)
        {
            var existeid = await context.TankBrands.AnyAsync(x => x.TankBrandIdx == TankBrandDTO.TankBrandIdx);

            var tankBrand = mapper.Map<TankBrand>(TankBrandDTO);

            if (existeid)
            {
                context.Update(tankBrand);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.TankBrands.AnyAsync(x => x.TankBrandId == TankBrandDTO.TankBrandId && x.Name == TankBrandDTO.Name);
                if (existe)
                {
                    return BadRequest($"Ya existe {TankBrandDTO.TankBrandId} {TankBrandDTO.Name} ");
                }
                else
                {
                    context.Add(tankBrand);
                    await context.SaveChangesAsync();
                }
            }
            var TankBrandDTO2 = mapper.Map<TankBrandDTO>(tankBrand);
            return CreatedAtRoute("obtenerTankBrand", new { id = tankBrand.TankBrandIdx }, TankBrandDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(TankBrandDTO TankBrandDTO)
        {
            var tankBrandDB = await context.TankBrands.FirstOrDefaultAsync(c => c.TankBrandIdx == TankBrandDTO.TankBrandIdx);

            if (tankBrandDB is null)
            {
                return NotFound();
            }
            try
            {
                tankBrandDB = mapper.Map(TankBrandDTO, tankBrandDB);
                await context.SaveChangesAsync();

            }
            catch
            {
                return BadRequest($"Ya existe tankBrand {TankBrandDTO.TankBrandId} en esa sucursal ");
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.TankBrands.AnyAsync(x => x.TankBrandIdx == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new TankBrand { TankBrandIdx = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
