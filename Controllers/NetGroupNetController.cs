﻿using APIControlNet.DTOs;
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
    public class NetGroupNetController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public NetGroupNetController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NetgroupNetDTO>>> Get(int skip, int take, string searchTerm = "")
        {
            var data = await context.NetgroupNets.AsNoTracking().ToListAsync();

            var query = data.Skip(skip).Take(take).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.ToLower().Contains(searchTerm));
            }
            var ntotal = query.Count();
            return Ok(new { query, ntotal });
        }

        [HttpGet("Notpage")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NetgroupNetDTO>>> Get(string userName)
        {
            var netGroup = context.NetgroupUsers.FirstOrDefault(x => x.Name == userName);

            var data = await context.NetgroupNets.Where(x => x.NetgroupId == netGroup.NetgroupId).AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NetgroupNetDTO ngnDTO, Guid? storeId, string userName)
        {
            try
            {
                var dbNgn = await context.NetgroupNets.FirstOrDefaultAsync(x => x.NetgroupId == ngnDTO.NetgroupId && x.NetgroupNetId == ngnDTO.NetgroupNetId);
                if (dbNgn is not null)
                {
                    return BadRequest($"Ya existe {dbNgn.Name} ");
                }

                var data2 = await context.NetgroupUsers.Where(x => x.Name == userName).FirstOrDefaultAsync();

                var tabla = context.Model.FindEntityType(typeof(NetgroupNet)).GetTableName();

                NetgroupNet ngn = new();
                ngn.NetgroupNetId = Guid.NewGuid();
                ngn.Name = ngnDTO.Name;
                ngn.NetgroupId = data2.NetgroupId;
                ngn.Date = DateTime.Now;
                ngn.Updated = DateTime.Now;
                ngn.Active = true;
                ngn.Locked = false;
                ngn.Deleted = false;

                context.NetgroupNets.Add(ngn);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = ngnDTO.Name;
                var storeId2 = storeId;
                var Table = tabla;
                await servicioBinnacle.AddBinnacle2(usuarioId, ipUser, name, storeId2, tabla);

                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            var existe = await context.NetgroupNets.FirstOrDefaultAsync(x => x.NetgroupNetIdx == id);

            var tabla = context.Model.FindEntityType(typeof(NetgroupNet)).GetTableName();
            if (existe is null) { return NotFound(); }

            var exist2 = await context.Odrs.AnyAsync(x => x.NetgroupNetId == existe.NetgroupNetId);
            if (exist2) { return BadRequest("Tiene datos relacionados"); }

            var remove = await context.NetgroupNets.FirstOrDefaultAsync(x => x.NetgroupNetIdx == id);
            context.Remove(remove);

            var removenetgroDet = await context.NetgroupNetDetails.Where(x => x.NetgroupNetId == remove.NetgroupNetId).ToListAsync();
            foreach (var removeStore in removenetgroDet) { context.RemoveRange(removenetgroDet); }

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = remove.Name;
            var storeId2 = storeId;
            var Table = tabla;
            await servicioBinnacle.deleteBinnacle2(usuarioId, ipUser, name, storeId2, Table);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
