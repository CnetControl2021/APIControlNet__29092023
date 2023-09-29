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
    public class SettingModuleDTOController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public SettingModuleDTOController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<SettingModuleDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.SettingModules.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Description.ToLower().Contains(nombre));
            }
            //await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var setMod = await queryable
                .AsNoTracking().ToListAsync();
            return Ok(setMod);
        }


        [HttpGet("{id:int}", Name = "obtenerSettingMod")]
        public async Task<ActionResult<SettingModuleDTO>> Get(int id)
        {
            var settingMod = await context.SettingModules.FirstOrDefaultAsync(x => x.SettingModuleIdx == id);

            if (settingMod == null)
            {
                return NotFound();
            }

            return mapper.Map<SettingModuleDTO>(settingMod);
        }

        [HttpGet("{descripcion}")]
        public async Task<ActionResult<List<SettingModuleDTO>>> Get([FromRoute] string descripcion)
        {
            var RSs = await context.SettingModules.Where(Hosedb => Hosedb.Description.Contains(descripcion)).ToListAsync();

            return mapper.Map<List<SettingModuleDTO>>(RSs);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SettingModuleDTO SettingModuleDTO)
        {
            var existeid = await context.SettingModules.AnyAsync(x => x.SettingModuleIdx == SettingModuleDTO.SettingModuleIdx);

            var settingMod = mapper.Map<SettingModule>(SettingModuleDTO);

            if (existeid)
            {
                context.Update(settingMod);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.SettingModules.AnyAsync(x => x.SettingModuleIdi == SettingModuleDTO.SettingModuleIdi);

                if (existe)
                {
                    return BadRequest($"Ya existe {SettingModuleDTO.SettingModuleIdi} ");
                }
                else
                {
                    context.Add(settingMod);
                    await context.SaveChangesAsync();
                }
            }
            var SettingModuleDTO2 = mapper.Map<SettingModuleDTO>(settingMod);
            return CreatedAtRoute("obtenerSettingMod", new { id = settingMod.SettingModuleIdx }, SettingModuleDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(SettingModuleDTO SettingModuleDTO)
        {

            var settingMod = mapper.Map<SettingModule>(SettingModuleDTO);
            context.Update(settingMod).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.SettingModules.AnyAsync(x => x.SettingModuleIdx == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new SettingModule { SettingModuleIdx = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

