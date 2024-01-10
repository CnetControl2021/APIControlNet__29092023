using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public SettingController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IEnumerable<SettingDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, int module)
        {
            var queryable = context.Settings.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Description.ToLower().Contains(nombre));
            }
            if (module != 0)
            {
                queryable = queryable.Where(x => x.SettingModuleIdi == module);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var settings = await queryable.OrderByDescending(x => x.SettingIdx).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<SettingDTO>>(settings);
        }


        [HttpGet]
        [Route("list2")]  //sin FK
        public async Task<List<SettingDTO>> ListSettings([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            List<SettingDTO> listSetting = new List<SettingDTO>();  //Instanciar

            var queryable = context.Settings.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Description.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);

            //var islands = await queryable.Paginar(paginacionDTO).AsNoTracking()//.OrderByDescending(x => x.IslandIdx)

            listSetting = await (from Setting in context.Settings
                                 join Store in context.Stores
                                 on Setting.StoreId equals Store.StoreId
                                 select new SettingDTO
                                 {
                                     SettingIdx = Setting.SettingIdx,
                                     SettingId = Setting.SettingId,
                                     StoreId = Setting.StoreId,
                                     //StoreName = Store.Name,
                                     //ModuleId = Setting.ModuleId,
                                     Field = Setting.Field,
                                     Value = Setting.Value,
                                     Description = Setting.Description,
                                     Type=Setting.Type,
                                     Date=Setting.Date,
                                     Updated = Setting.Updated,
                                     //Active = Setting.Active,
                                     //Locked=Setting.Locked,
                                     //Deleted=Setting.Deleted,
                                     Response = Setting.Response,
                                     //IsSend = Setting.IsSend
                                 })
                                 .OrderByDescending(x => x.SettingIdx)
                                 .Paginar(paginacionDTO)
                                 .AsNoTracking()
                             .ToListAsync();
            //return listSetting;
            return mapper.Map<List<SettingDTO>>(listSetting);
        }


        //[HttpGet]
        //public async Task<ActionResult<SettingDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        //{
        //    var queryable = context.Settings.AsQueryable();
        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
        //    }
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var setting = await queryable.Paginar(paginacionDTO)
        //        .Select( x => new 
        //                { 
        //                    SettingIdx = x.SettingIdx

        //        }).Paginar(paginacionDTO)
        //                   .ToListAsync();
        //    return Ok(setting);
        //}

        
        [HttpGet("byStore/{id2}")] //por store
        public async Task<ActionResult<List<SettingDTO>>> Get2([FromRoute] Guid id2)
        {
            var RSs = await context.Settings.Where(e => e.StoreId.Equals((Guid)id2))
                //.Include(x => x.Store)
                .ToListAsync();
            return mapper.Map<List<SettingDTO>>(RSs);
        }


        [HttpGet("{id:int}", Name = "obtenerSettings")]
        public async Task<ActionResult<SettingDTO>> Get(int id)
        {
            var Setting = await context.Settings.FirstOrDefaultAsync(x => x.SettingIdx == id);

            if (Setting == null)
            {
                return NotFound();
            }

            return mapper.Map<SettingDTO>(Setting);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<SettingDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.Settings.Where(SettingDb => SettingDb.Name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<SettingDTO>>(RSs);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SettingDTO SettingDTO)
        {

            var existeid = await context.Settings.AnyAsync(x => x.SettingIdx == SettingDTO.SettingIdx);

            var Setting = mapper.Map<Setting>(SettingDTO);

            if (existeid)
            {
                context.Update(Setting);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.Settings.AnyAsync(x => x.SettingId == (SettingDTO.SettingId) && x.Name == SettingDTO.Name);

                if (existe)
                {
                    return BadRequest($"Ya existe {SettingDTO.SettingId} ");
                }
                else
                {
                    context.Add(Setting);
                    await context.SaveChangesAsync();
                }
            }
            var SettingDTO2 = mapper.Map<SettingDTO>(Setting);
            return CreatedAtRoute("obtenerSettings", new { id = Setting.SettingIdx }, SettingDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(SettingDTO SettingDTO)
        {
            var settingDB = await context.Settings.FirstOrDefaultAsync(c => c.SettingIdx == SettingDTO.SettingIdx);

            if (settingDB is null)
            {
                return NotFound();
            }

            try
            {
                settingDB = mapper.Map(SettingDTO, settingDB);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe producto {SettingDTO.SettingId} en esa sucursal ");
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.Settings.AnyAsync(x => x.SettingIdx == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Setting { SettingIdx = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
