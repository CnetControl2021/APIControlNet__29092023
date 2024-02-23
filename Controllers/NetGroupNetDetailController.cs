using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
                              join s in context.Stores on ngnd.StoreId equals s.StoreId
                              //join ngs in context.NetgroupStores on ngnd.StoreId equals ngs.StoreId
                              
                              select new
                              {
                                  ngnd.NetgroupNetDetailIdx,
                                  ngnd.NetgroupNetId,
                                  ngnd.StoreId,
                                  s.Name
                              }).AsNoTracking().ToListAsync();

            var query = data.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.ToLower().Contains(searchTerm));
            }
            var ntotal = query.Count();
            return Ok(new { query, ntotal });
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NetgroupNetDetailDTO netgrNetDetailDTO, Guid storeId)
        {
            var dbNgND = await context.NetgroupNetDetails.FirstOrDefaultAsync(
                x => x.NetgroupNetId == netgrNetDetailDTO.NetgroupNetId && x.StoreId == netgrNetDetailDTO.StoreId);
            var tabla = context.Model.FindEntityType(typeof(NetgroupNetDetail)).GetTableName();

            var netgd = mapper.Map<NetgroupNetDetail>(netgrNetDetailDTO);

            if (dbNgND is not null)
            {
                return BadRequest($"Ya existe estacion {dbNgND.StoreId} ");
            }
            else
            {
                context.Add(netgd);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = netgrNetDetailDTO.NetgroupNetId.ToString();
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
            var name = name2.StoreId.ToString();
            var storeId2 = storeId;
            var Table = tabla;
            await servicioBinnacle.deleteBinnacle2(usuarioId, ipUser, name, storeId2, Table);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
