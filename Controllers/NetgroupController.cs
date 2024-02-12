using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NetgroupController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public NetgroupController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        //var data2 = await query.Skip(skip).Take(take).ToListAsync();
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetgroupDTO>>> Get(int skip, int take, string userName, string searchTerm = "")
        {
                if (userName is null || userName == "masterSupport@controlnet.com.mx")
                {
                    var data = await context.Netgroups.AsNoTracking().ToListAsync();

                    var query = data.Skip(skip).Take(take).AsQueryable();
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = query.Where(c => c.NetgroupName.ToLower().Contains(searchTerm)
                        || c.ShortDescription.ToLower().Contains(searchTerm));
                    }

                    var ntotal = query.Count();
                    return Ok(new { query, ntotal });
                }
                else
                {
                    var data2 = await context.NetgroupUsers.FirstOrDefaultAsync(x => x.Name == userName);
                    if (data2 != null)
                    {
                        var datauser = data2!.NetgroupId;
                        var data = await context.Netgroups.Where(x => x.NetgroupId == datauser).AsNoTracking().ToListAsync();
                    //var query = data.AsQueryable();
                    var query = data.Skip(skip).Take(take).AsQueryable();

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                        {
                            query = query.Where(c => c.NetgroupName.ToLower().Contains(searchTerm)
                            || c.ShortDescription.ToLower().Contains(searchTerm));
                        }
                        var ntotal = query.Count();
                        return Ok(new { query, ntotal });
                    }
                return NoContent();
            }           
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NetgroupDTO netgroupDTO, Guid storeId)
        {
            var dbNg = await context.Netgroups.FirstOrDefaultAsync(x => x.NetgroupId == netgroupDTO.NetgroupId && x.NetgroupIdi == netgroupDTO.NetgroupIdi);

            var netg = mapper.Map<Netgroup>(netgroupDTO);

            if (dbNg is not null)
            {
                return BadRequest($"Ya existe red {dbNg.NetgroupName} ");
            }
            else
            {
                context.Add(netg);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = netgroupDTO.NetgroupName;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);

                await context.SaveChangesAsync();
                return Ok();
            }
        }

        //[HttpGet("{id:int}", Name = "obtNetGroup")]
        //[AllowAnonymous]
        //public async Task<ActionResult<NetGroup_NetGroupStoreDTO>> Get(int id, Guid netgroupId)
        //{
        //    var data = await context.Netgroups.FirstOrDefaultAsync(x => x.NetgroupIdx == id);
        //    var data2 = await context.NetgroupStores.FirstOrDefaultAsync(x => x.NetgroupId == netgroupId);

        //    //    //chema
        //    var claseEmpaquetada = new NetGroup_NetGroupStoreDTO
        //    {
        //        NNetgroupDTO = mapper.Map<NetgroupDTO>(data),
        //        NNetgroupStoreDTO = mapper.Map<NetgroupStoreDTO>(data2)
        //    };
        //    return claseEmpaquetada;
        //}

        [HttpGet("{id:int}", Name = "obtNetGroup")]
        public async Task<ActionResult<NetgroupDTO>> Get(int id)
        {
            var data = await context.Netgroups.FirstOrDefaultAsync(x => x.NetgroupIdx == id);

            if (data == null)
            {
                return NotFound();
            }
            return mapper.Map<NetgroupDTO>(data);
        }

        [HttpPut]
        public async Task<IActionResult> Put(NetgroupDTO netgroupDTO)
        {
            var netgroupDB = await context.Netgroups.FirstOrDefaultAsync(x => x.NetgroupIdx == netgroupDTO.NetgroupIdx);

            if (netgroupDB is null)
            {
                return NotFound();
            }
            try
            {
                netgroupDB = mapper.Map(netgroupDTO, netgroupDB);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = netgroupDB.NetgroupName;
                //await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe vehiculo {netgroupDTO.NetgroupName} ");
            }
            return NoContent();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            var existe = await context.Netgroups.AnyAsync(x => x.NetgroupIdx == id);
            var tabla = context.Model.FindEntityType(typeof(Netgroup)).GetTableName();
            if (!existe) { return NotFound(); }

            var name2 = await context.Netgroups.FirstOrDefaultAsync(x => x.NetgroupIdx == id);
            context.Remove(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.NetgroupName;
            var storeId2 = storeId;
            var Table = tabla;
            await servicioBinnacle.deleteBinnacle2(usuarioId, ipUser, name, storeId2, Table);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
