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
    public class NetGroupNetDetailController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public NetGroupNetDetailController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NetgroupNetDetailDTO>>> Get(int skip, int take, Guid netgroupnetId, string searchTerm = "")
        {
            var data = await (from ngnd in context.NetgroupNetDetails where ngnd.NetgroupNetId == netgroupnetId
                              join ngn in context.NetgroupNets on ngnd.NetgroupNetId equals ngn.NetgroupNetId
                              join ngs in context.NetgroupStores on ngnd.NetgroupNetStore equals ngs.StoreId
                              
                              select new
                              {
                                  ngnd.NetgroupNetDetailIdx,
                                  ngnd.NetgroupNetId,
                                  ngnd.NetgroupNetStore,
                                  ngn.NetgroupNetName,
                                  ngs.Name
                              }).AsNoTracking().ToListAsync();

            var query = data.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.NetgroupNetName.ToLower().Contains(searchTerm));
            }
            var ntotal = query.Count();
            return Ok(new { query, ntotal });
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NetgroupNetDetailDTO netgrNetDetailDTO, Guid storeId)
        {
            var dbNgND = await context.NetgroupNetDetails.FirstOrDefaultAsync(
                x => x.NetgroupNetId == netgrNetDetailDTO.NetgroupNetId && x.NetgroupNetStore == netgrNetDetailDTO.NetgroupNetStore);
            var tabla = context.Model.FindEntityType(typeof(NetgroupNetDetail)).GetTableName();

            var netgd = mapper.Map<NetgroupNetDetail>(netgrNetDetailDTO);

            if (dbNgND is not null)
            {
                return BadRequest($"Ya existe estacion {dbNgND.NetgroupNetStore} ");
            }
            else
            {
                context.Add(netgd);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = netgrNetDetailDTO.NetgroupNetName;
                var storeId2 = storeId;
                var Table = tabla;
                await servicioBinnacle.AddBinnacle2(usuarioId, ipUser, name, storeId2, tabla);

                await context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            var existe = await context.NetgroupNetDetails.AnyAsync(x => x.NetgroupNetDetailIdx == id);
            var tabla = context.Model.FindEntityType(typeof(NetgroupNetDetail)).GetTableName();
            if (!existe) { return NotFound(); }

            var name2 = await context.NetgroupNetDetails.FirstOrDefaultAsync(x => x.NetgroupNetDetailIdx == id);
            context.Remove(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.NetgroupNetStore.ToString();
            var storeId2 = storeId;
            var Table = tabla;
            await servicioBinnacle.deleteBinnacle2(usuarioId, ipUser, name, storeId2, Table);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
