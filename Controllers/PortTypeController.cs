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
    public class PortTypeController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public PortTypeController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PortTypeDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.PortTypes.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var ports = await queryable.AsNoTracking().ToListAsync();
            return Ok(ports);
        }

        [HttpGet("active")]
        public async Task<ActionResult<PortTypeDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.PortTypes.AsQueryable().Where(x => x.Active==true);
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var ports = await queryable.AsNoTracking().ToListAsync();
            return Ok(ports);
        }


        [HttpGet("{id:int}", Name = "obtenerPortType")]
        public async Task<ActionResult<PortTypeDTO>> Get(int id)
        {
            var Port = await context.PortTypes.FirstOrDefaultAsync(x => x.PortTypeIdx == id);

            if (Port == null)
            {
                return NotFound();
            }
            return mapper.Map<PortTypeDTO>(Port);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PortTypeDTO PortTypeDTO)
        {
            var existeid = await context.PortTypes.AnyAsync(x => x.PortTypeIdx == PortTypeDTO.PortTypeIdx);

            var Type = mapper.Map<Models.PortType>(PortTypeDTO);

            if (existeid)
            {
                context.Update(Type);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.PortTypes.AnyAsync(x => x.PortTypeIdi == PortTypeDTO.PortTypeIdi | x.Name == PortTypeDTO.Name);

                if (existe)
                {
                    return BadRequest($"Ya existe {PortTypeDTO} {PortTypeDTO.Name} ");
                }
                else
                {
                    context.Add(Type);
                    await context.SaveChangesAsync();
                }
            }
            var PortTypeDTO2 = mapper.Map<PortTypeDTO>(Type);
            return CreatedAtRoute("obtenerPortType", new { id = Type.PortTypeIdi }, PortTypeDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(PortTypeDTO PortTypeDTO)
        {
            var Type = mapper.Map<Models.PortType>(PortTypeDTO);
            context.Update(Type).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.PortTypes.AnyAsync(x => x.PortTypeIdx == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Models.PortType { PortTypeIdx = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
