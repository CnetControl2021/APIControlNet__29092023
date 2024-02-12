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
    public class NetGroupUserController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public NetGroupUserController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NetgroupUserDTO>>> Get(int skip, int take, Guid netgroupId)
        {
            var data = await context.NetgroupUsers.Where(x => x.NetgroupId == netgroupId).AsNoTracking().ToListAsync();
            var query = data.AsQueryable();
            var ntotal = query.Count();
            return Ok(new { query, ntotal });
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NetgroupUserDTO netgroupUserDTO, Guid storeId)
        {
            var dbNguser = await context.NetgroupUsers.FirstOrDefaultAsync(x => x.NetgroupId == netgroupUserDTO.NetgroupId && x.UserId == netgroupUserDTO.UserId);
            var tabla = context.Model.FindEntityType(typeof(NetgroupUser)).GetTableName();

            var netgu = mapper.Map<NetgroupUser>(netgroupUserDTO);

            if (dbNguser is not null)
            {
                return BadRequest($"Ya existe usuario {dbNguser.Name} ");
            }
            else
            {
                context.Add(netgu);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = netgroupUserDTO.Name;
                var storeId2 = storeId;
                var Table = tabla;
                await servicioBinnacle.AddBinnacle2(usuarioId, ipUser, name, storeId2, tabla);

                await context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, Guid storeId, Guid netgroupId)
        {
            var existe = await context.NetgroupUsers.FirstOrDefaultAsync(x => x.NetgroupUserIdx == id 
            && x.NetgroupId == netgroupId);
            var tabla = context.Model.FindEntityType(typeof(NetgroupUser)).GetTableName();
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
