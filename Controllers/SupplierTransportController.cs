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
using System;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SupplierTransportController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public SupplierTransportController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("GetOK")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiRespSupplierTransportDTO>> Get5(int skip, int take, Guid storeId, string searchTerm = "")
        {
            var query = context.SupplierTransports.Where(x => x.StoreId == storeId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.ToLower().Contains(searchTerm) || c.Rfc.ToLower().Contains(searchTerm));
            }

            var nsupplierT = await query.Skip(skip).Take(take).OrderByDescending(x => x.Date).ToListAsync();
            var ntotal = await query.CountAsync();

            return new ApiRespSupplierTransportDTO
            {
                NTotal = ntotal,
                NSupplierTransportDTOs = mapper.Map<IEnumerable<SupplierTransportDTO>>(nsupplierT)
            };
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SupplierTransportDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre,
            Guid storeId, Guid id2)
        {
            var queryable = context.SupplierTransports.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            if (id2 != Guid.Empty)
            {
                queryable = queryable.Where(x => x.SupplierId == id2);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var suppliersTrans = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<SupplierTransportDTO>>(suppliersTrans);
        }


        [HttpGet("{id:int}/{idGuid}", Name = "newSuppTrans")]
        public async Task<ActionResult<SupplierTransportDTO>> Get(int id, Guid idGuid)
        {
            var suppliersTrans = await context.SupplierTransports.FirstOrDefaultAsync(x => x.SupplierTransportIdx == id);

            if (suppliersTrans == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierTransportDTO>(suppliersTrans);
        }


        [HttpGet("byguid/{id2?}/{id?}")]
        //[AllowAnonymous]
        public async Task<ActionResult<SupplierTransportDTO>> Get(Guid id2, int id)
        {
            var suppliersTrans = await context.SupplierTransports.FirstOrDefaultAsync(x => x.SupplierId == id2 & x.SupplierTransportIdi == id);

            if (suppliersTrans == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierTransportDTO>(suppliersTrans);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SupplierTransportDTO supTransDTO, Guid storeId)
        {
            var existe = await context.SupplierTransports.AnyAsync(x => x.SupplierId == supTransDTO.SupplierId && x.StoreId == storeId);
            var dbSupplier = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == supTransDTO.SupplierId);

            var suppTransp = mapper.Map<SupplierTransport>(supTransDTO);
            suppTransp.StoreId = storeId;
            suppTransp.Name = dbSupplier.Name;
            suppTransp.Rfc = dbSupplier.Rfc;

            if (existe)
            {
                return BadRequest($"Ya existe {dbSupplier.Name} en proveedor de combustibles ");
            }
            else
            {
                context.Add(suppTransp);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = suppTransp.Name;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);

                dbSupplier.IsSupplierOfTransport = true;
                await context.SaveChangesAsync();
                return Ok();
            }
        }


        [HttpPut("{idGuid?}")]
        public async Task<IActionResult> Put(Guid idGuid, [FromBody] SupplierTransportDTO suptransDTO)
        {
            try
            {
                var dbSupTransp = await context.SupplierTransports.FirstOrDefaultAsync
                (s => s.SupplierTransportIdx == suptransDTO.SupplierTransportIdx);
                var dbSupplier = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == suptransDTO.SupplierId);

                dbSupTransp.SupplierId = dbSupplier.SupplierId;
                dbSupTransp.StoreId = suptransDTO.StoreId;
                dbSupTransp.SupplierTransportIdi = suptransDTO.SupplierTransportIdi++;
                dbSupTransp.BrandName = suptransDTO.BrandName;
                dbSupTransp.Name = dbSupplier.Name;
                dbSupTransp.Rfc = dbSupplier.Rfc;
                dbSupTransp.TransportPermission = suptransDTO.TransportPermission;
                dbSupTransp.Updated = DateTime.Now;

                var storeId2 = suptransDTO.StoreId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = suptransDTO.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);

                //reset campo de supplier
                var dbSupplierGuid = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == idGuid);
                if (dbSupplierGuid is not null) { dbSupplierGuid.IsSupplierOfFuel = false; }

                //nuevo supplierFuel true
                var dbSupplier2 = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == suptransDTO.SupplierId);
                dbSupplier2.IsSupplierOfFuel = true;

                context.SupplierTransports.Update(dbSupTransp);
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                var dbSupplier2 = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == suptransDTO.SupplierId);
                return BadRequest($"Ya existe {dbSupplier2.Name} en provedores de combustible ");
            }
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.SupplierTransports.AnyAsync(x => x.SupplierTransportIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierTransports.FirstOrDefaultAsync(x => x.SupplierTransportIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid? storeId)
        {
            var existe = await context.SupplierTransports.FirstOrDefaultAsync(x => x.SupplierTransportIdx == id);

            if (existe is null) { return NotFound(); }
            var dbSupplier = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == existe.SupplierId);

            var name2 = await context.SupplierTransports.FirstOrDefaultAsync(x => x.SupplierTransportIdx == id);
            context.Remove(name2);
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            var tabla = "SupplierTransports";
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);
            dbSupplier.IsSupplierOfTransport = false;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
