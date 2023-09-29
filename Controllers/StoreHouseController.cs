using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreHouseController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public StoreHouseController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<StoreHouseDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.StoreHouses.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var storesHouses = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<StoreHouseDTO>>(storesHouses);
        }



        [HttpGet("{id:int}", Name = "obtenerStoreHouse")]
        public async Task<ActionResult<StoreHouseDTO>> Get(int id)
        {
            var StoreHouse = await context.StoreHouses.FirstOrDefaultAsync(x => x.StoreHouseIdx == id);

            if (StoreHouse == null)
            {
                return NotFound();
            }

            return mapper.Map<StoreHouseDTO>(StoreHouse);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<StoreHouseDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.StoreHouses.Where(storedb => storedb.Name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<StoreHouseDTO>>(RSs);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] StoreHouseDTO StoreHouseDTO)
        {

            var existeid = await context.StoreHouses.AnyAsync(x => x.StoreHouseIdx == StoreHouseDTO.StoreHouseIdx);

            var storeHouse = mapper.Map<StoreHouse>(StoreHouseDTO);

            if (existeid)
            {
                context.Update(storeHouse);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.StoreHouses.AnyAsync(x => x.StoreHouseIdx == (StoreHouseDTO.StoreHouseIdx) | x.Name == StoreHouseDTO.Name);

                if (existe)
                {
                    return BadRequest($"Ya existe {StoreHouseDTO.StoreHouseIdx} {StoreHouseDTO.Name} ");
                }
                else
                {
                    context.Add(storeHouse);
                    await context.SaveChangesAsync();
                }
            }
            var StoreHouseDTO2 = mapper.Map<StoreHouseDTO>(storeHouse);
            return CreatedAtRoute("obtenerStoreHouse", new { id = storeHouse.StoreHouseIdx }, StoreHouseDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(StoreHouseDTO StoreHouseDTO)
        {

            var StoreH = mapper.Map<StoreHouse>(StoreHouseDTO);
            context.Update(StoreH).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.StoreHouses.AnyAsync(x => x.StoreHouseIdx == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new StoreHouse { StoreHouseIdx = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
