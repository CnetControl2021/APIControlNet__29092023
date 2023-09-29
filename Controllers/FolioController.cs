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
    public class FolioController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public FolioController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<FolioDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.Folios.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId==storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var products = await queryable.OrderByDescending(x => x.FolioIdx).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<FolioDTO>>(products);
        }

        [HttpGet("byStore/{id2}")] //por store
        public async Task<ActionResult<List<FolioDTO>>> Get2([FromRoute] Guid id2)
        {
            var RSs = await context.Folios.Where(e => e.StoreId.Equals((Guid)id2))
                .ToListAsync();
            return mapper.Map<List<FolioDTO>>(RSs);
        }


        [HttpGet("{id:int}", Name = "obtenerFolio")]
        public async Task<ActionResult<FolioDTO>> Get(int id)
        {
            var Folio = await context.Folios.FirstOrDefaultAsync(x => x.FolioIdx == id);

            if (Folio == null)
            {
                return NotFound();
            }
            return mapper.Map<FolioDTO>(Folio);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<FolioDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.Folios.Where(FolioDB => FolioDB.Name.Contains(nombre)).ToListAsync();
            return mapper.Map<List<FolioDTO>>(RSs);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FolioDTO FolioDTO)
        {
            var existeid = await context.Folios.AnyAsync(x => x.FolioIdx == FolioDTO.FolioIdx);

            var Folio = mapper.Map<Folio>(FolioDTO);

            if (existeid)
            {
                context.Update(Folio);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.Folios.AnyAsync(x => x.Folio1 == FolioDTO.Folio1 && x.Name == FolioDTO.Type);
                if (existe)
                {
                    return BadRequest($"Ya existe {FolioDTO.Folio1} {FolioDTO.Type} ");
                }
                else
                {
                    context.Add(Folio);
                    await context.SaveChangesAsync();
                }
            }
            var FolioDTO2 = mapper.Map<FolioDTO>(Folio);
            return CreatedAtRoute("obtenerFolio", new { id = Folio.FolioIdx }, FolioDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(FolioDTO FolioDTO)
        {
            var folioDB = await context.Folios.FirstOrDefaultAsync(c => c.FolioIdx == FolioDTO.FolioIdx);

            if (folioDB is null)
            {
                return NotFound();
            }
            try
            {
                folioDB = mapper.Map(FolioDTO, folioDB);
                await context.SaveChangesAsync();

            }
            catch
            {
                return BadRequest($"Ya existe folio {FolioDTO.Folio1} en esa sucursal ");
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.Folios.AnyAsync(x => x.FolioIdx == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Folio { FolioIdx = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
