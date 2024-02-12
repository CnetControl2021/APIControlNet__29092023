using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NetGroupStoreController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public NetGroupStoreController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetgroupStoreDTO>>> Get(int skip, int take, Guid netgroupId)
        {
            var data = await context.NetgroupStores.Where(x => x.NetgroupId == netgroupId).AsNoTracking().ToListAsync();
            var query = data.AsQueryable();
            var ntotal = query.Count();
            return Ok(new { query, ntotal });
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NetgroupStoreDTO netgroupStoreDTO)
        {
            var dbNgSt = await context.NetgroupStores.FirstOrDefaultAsync(x => x.NetgroupId == netgroupStoreDTO.NetgroupId && x.StoreId == netgroupStoreDTO.StoreId);
            var tabla = context.Model.FindEntityType(typeof(NetgroupStore)).GetTableName();

            var netg = mapper.Map<NetgroupStore>(netgroupStoreDTO);

            if (dbNgSt is not null)
            {
                return BadRequest($"Ya existe estacion {dbNgSt.Name} ");
            }
            else
            {
                context.Add(netg);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = netgroupStoreDTO.Name;
                var storeId2 = netgroupStoreDTO.StoreId;
                var Table = tabla;
                await servicioBinnacle.AddBinnacle2(usuarioId, ipUser, name, storeId2, tabla);

                await context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, Guid storeId, Guid netgroupId)
        {
            var existe = await context.NetgroupStores.FirstOrDefaultAsync(x => x.NetgroupStoreIdx == id && x.StoreId == storeId
            && x.NetgroupId == netgroupId);
            var tabla = context.Model.FindEntityType(typeof(NetgroupStore)).GetTableName();
            if (existe is null) { return NotFound(); }
            
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = existe.Name;
            var storeId2 = storeId;
            var Table = tabla;
            await servicioBinnacle.deleteBinnacle2(usuarioId, ipUser, name, storeId2, Table);

            context.Remove(existe);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
