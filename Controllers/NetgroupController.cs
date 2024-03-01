using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> userManager;
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public NetgroupController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
            this.userManager = userManager;
        }


        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<NetgroupDTO>> Get(int skip, int take, string userName, string searchTerm = "")
        {
            var usuario = await userManager.FindByEmailAsync(userName);
            var rolesUsuario = await userManager.GetRolesAsync(usuario);
            //foreach (var rol in rolesUsuario)
            //{
            //    Console.WriteLine($"Rol: {rol}");
            //}

            var netgroupuser = await context.NetgroupUsers.FirstOrDefaultAsync(x => x.UserId == usuario.Id);
            if (usuario.Email is "usuarioPrincipalSistemaNetGroup@controlnet.com.mx")
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
                var data = await context.Netgroups.Where(x => x.NetgroupId == netgroupuser.NetgroupId).AsNoTracking().ToListAsync();
                var query = data.Skip(skip).Take(take).AsQueryable();
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(c => c.NetgroupName.ToLower().Contains(searchTerm)
                    || c.ShortDescription.ToLower().Contains(searchTerm));
                }
                var ntotal = query.Count();
                return Ok(new { query, ntotal });
            }
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NetgroupDTO netgroupDTO, Guid? storeId)
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
                //var usuarioId = obtenerUsuarioId();
                //var ipUser = obtenetIP();
                //var name = netgroupDTO.NetgroupName;
                //var storeId2 = storeId;
                //await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);

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
        public async Task<IActionResult> Delete(int id, Guid? storeId)
        {
            var existe = await context.Netgroups.FirstOrDefaultAsync(x => x.NetgroupIdx == id);
            var tabla = context.Model.FindEntityType(typeof(Netgroup)).GetTableName();
            if (existe is null) { return NotFound(); }

            var exist2 = await context.NetgroupNets.AnyAsync(x => x.NetgroupId == existe.NetgroupId);
            if (exist2) { return BadRequest("Tiene datos relacionados"); }

            var remove = await context.Netgroups.FirstOrDefaultAsync(x => x.NetgroupIdx == id);
            context.Remove(remove);

            var remove3 = await context.NetgroupStores.Where(x => x.NetgroupId == remove.NetgroupId).ToListAsync();
            foreach (var item in remove3) { context.RemoveRange(remove3); }

            var remove2 = await context.NetgroupUsers.Where(x => x.NetgroupId == remove.NetgroupId).ToListAsync();
            foreach (var item in remove2) { context.RemoveRange(remove2); }

           
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = remove.NetgroupName;
            var storeId2 = storeId;
            var Table = tabla;
            await servicioBinnacle.deleteBinnacle2(usuarioId, ipUser, name, storeId2, Table);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
