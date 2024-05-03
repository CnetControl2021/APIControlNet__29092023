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
    public class StoreAddressController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public StoreAddressController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<StoreAddressDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.StoreAddresses.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Street.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var storeAddr = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<StoreAddressDTO>>(storeAddr);
        }



        [HttpGet("{id:int}", Name = "obtenerStoreAddress")]
        public async Task<ActionResult<StoreAddressDTO>> Get(int id)
        {
            var storeAddr = await context.StoreAddresses.FirstOrDefaultAsync(x => x.StoreAddressIdx == id);

            if (storeAddr == null)
            {
                return NotFound();
            }
            return mapper.Map<StoreAddressDTO>(storeAddr);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] StoreAddressDTO storeAddressDTO, Guid storeId)
        {

            var existe = await context.StoreAddresses.AnyAsync(x => x.StoreAddressIdx == storeAddressDTO.StoreAddressIdx);
            
            var storeAddress = mapper.Map<StoreAddress>(storeAddressDTO);

            //var storeAddrdDB = await context.StoreAddresses.FirstOrDefaultAsync(x => x.StoreId == storeId);
            storeAddress.StoreId = storeId;

            if (existe)
            {
                return BadRequest($"Ya existe {storeAddressDTO.Street} ");
            }
            else
            {
                context.Add(storeAddress);

                var queryable = context.StoreAddresses.AsQueryable();

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = storeAddress.Street;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
                var storeAddressDTO2 = mapper.Map<StoreAddressDTO>(storeAddress);
                return CreatedAtRoute("obtenerStoreAddress", new { id = storeAddressDTO.StoreAddressIdx }, storeAddressDTO2);

            }
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(StoreAddressDTO storeAddressDTO, Guid storeId)
        {
            var storeAddrDB = await context.StoreAddresses.FirstOrDefaultAsync(c => c.StoreAddressIdx == storeAddressDTO.StoreAddressIdx);

            if (storeAddrDB is null)
            {
                return NotFound();
            }
            storeAddrDB = mapper.Map(storeAddressDTO, storeAddrDB);

            var storeId2 = storeId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = storeAddrDB.Street;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        ////[AllowAnonymous]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.StoreAddresses.AnyAsync(x => x.StoreAddressIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.StoreAddresses.FirstOrDefaultAsync(x => x.StoreAddressIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Street;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        ////[AllowAnonymous]
        public async Task<IActionResult> Delete(int id, Guid? storeId)
        {
            var existe = await context.StoreAddresses.AnyAsync(x => x.StoreAddressIdx == id);
            if (!existe) { return NotFound(); }

            //var queryable = context.Stores.AsQueryable();
            //if (storeId != Guid.Empty || storeId is not null)
            //{
            //    queryable = queryable.Where(x => x.StoreId == storeId);
            //}

            var name2 = await context.StoreAddresses.FirstOrDefaultAsync(x => x.StoreAddressIdx == id);
            context.Remove(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Street;
            var storeId2 = storeId;
            var tabla = "StoreAddresses";
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
