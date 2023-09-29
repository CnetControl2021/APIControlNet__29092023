using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class StoreSatController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public StoreSatController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IEnumerable<StoreSatDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.StoreSats.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.SatPermission.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var storeSat = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<StoreSatDTO>>(storeSat);           
        }



        [HttpGet("{id:int}", Name = "obtenerStoreSat")]
        public async Task<ActionResult<StoreSatDTO>> Get(int id)
        {
            var storeSat = await context.StoreSats.FirstOrDefaultAsync(x => x.StoreSatIdx == id);

            if (storeSat == null)
            {
                return NotFound();
            }
            return mapper.Map<StoreSatDTO>(storeSat);
        }

        [HttpGet("byStore{storeId}")] //por store
        ////[AllowAnonymous]
        public async Task<ActionResult<StoreSatDTO>> Get4(Guid storeId)
        {
            var storesat = await context.StoreSats.FirstOrDefaultAsync(x => x.StoreId == storeId);

            if (storesat == null)
            {
                return NotFound();
            }
            return mapper.Map<StoreSatDTO>(storesat);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] StoreSatDTO storeSatDTO, Guid storeId)
        {

            var existe = await context.StoreSats.AnyAsync(x => x.StoreSatIdx == storeSatDTO.StoreSatIdx);
            var storeSat = mapper.Map<StoreSat>(storeSatDTO);

            //var storeAddrdDB = await context.StoreAddresses.FirstOrDefaultAsync(x => x.StoreId == storeId);
            storeSat.StoreId = storeId;

            if (existe)
            {
                return BadRequest($"Ya existe {storeSatDTO.SatPermission} ");
            }
            else
            {
                context.Add(storeSat);

                //var queryable = context.StoreSats.AsQueryable();

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = storeSatDTO.SatPermission;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
                var storeAddressDTO2 = mapper.Map<StoreSatDTO>(storeSat);
                return CreatedAtRoute("obtenerStoreSat", new { id = storeSatDTO.StoreSatIdx }, storeAddressDTO2);

            }
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(StoreSatDTO storeSatDTO, Guid storeId)
        {
            var storeSatDB = await context.StoreSats.FirstOrDefaultAsync(c => c.StoreSatIdx == storeSatDTO.StoreSatIdx);

            if (storeSatDB is null)
            {
                return NotFound();
            }
            storeSatDB = mapper.Map(storeSatDTO, storeSatDB);

            var storeId2 = storeId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = storeSatDB.SatPermission;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.StoreSats.AnyAsync(x => x.StoreSatIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.StoreSats.FirstOrDefaultAsync(x => x.StoreSatIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.SatPermission;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid? storeId)
        {
            var existe = await context.StoreSats.AnyAsync(x => x.StoreSatIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.StoreSats.FirstOrDefaultAsync(x => x.StoreSatIdx == id);
            context.Remove(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.SatPermission;
            var storeId2 = storeId;
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
