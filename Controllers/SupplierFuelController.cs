using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SupplierFuelController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public SupplierFuelController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SupplierFuelDTO>> Get2([FromQuery]
            Guid storeId, Guid id2)
        {
            var queryable = context.SupplierFuels.Where(x => x.Active == true && x.Deleted == false).AsQueryable();

            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            if (id2 != Guid.Empty)
            {
                queryable = queryable.Where(x => x.SupplierId == id2);
            }
            var suppliersFuel = await queryable.AsNoTracking().ToListAsync();
            return mapper.Map<List<SupplierFuelDTO>>(suppliersFuel);
        }

        [HttpGet("GetOK")]
        //[AllowAnonymous]
        public async Task<ActionResult<ApiRespSupplierFuelDTO>> Get5(int skip, int take, Guid storeId, string searchTerm = "")
        {
            var query = context.SupplierFuels.Where(x => x.StoreId == storeId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.ToLower().Contains(searchTerm) || c.Rfc.ToLower().Contains(searchTerm));
            }

            var nsupplierf = await query.Skip(skip).Take(take).OrderByDescending(x => x.Date).ToListAsync();
            var ntotal = await query.CountAsync();

            return new ApiRespSupplierFuelDTO
            {
                NTotal = ntotal,
                NSupplierFuelDTOs = mapper.Map<IEnumerable<SupplierFuelDTO>>(nsupplierf)
            };
        }


        [HttpGet("{id:int}/{idGuid}", Name = "newSuppFuel")]
        public async Task<ActionResult<SupplierFuelDTO>> Get(int id, Guid idGuid)
        {
            var suppliersFuel = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierFuelIdx == id);

            if (suppliersFuel == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierFuelDTO>(suppliersFuel);
        }


        [HttpGet("byguid/{id2?}/{id?}")]
        //[AllowAnonymous]
        public async Task<ActionResult<SupplierFuelDTO>> Get(Guid id2, int id)
        {
            var suppliersFuel = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierId == id2 & x.SupplierFuelIdi == id);

            if (suppliersFuel == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierFuelDTO>(suppliersFuel);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SupplierFuelDTO SupplierFuelDTO, Guid storeId)
        {
            var existe = await context.SupplierFuels.AnyAsync(x => x.SupplierId == SupplierFuelDTO.SupplierId && x.StoreId == storeId);
            var dbSupplier = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == SupplierFuelDTO.SupplierId);

            var suppFuel = mapper.Map<SupplierFuel>(SupplierFuelDTO);
            suppFuel.StoreId = storeId;
            suppFuel.Name = dbSupplier.Name;
            suppFuel.Rfc = dbSupplier.Rfc;

            if (existe)
            {
                return BadRequest($"Ya existe {dbSupplier.Name} en proveedor de combustibles ");
            }
            else
            {
                context.Add(suppFuel);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = suppFuel.Name;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);

                dbSupplier.IsSupplierOfFuel = true;
                await context.SaveChangesAsync();
                return Ok();
            }
        }


        [HttpPut("{idGuid?}")]
        public async Task<IActionResult> Put(Guid idGuid, [FromBody] SupplierFuelDTO supFuelDTO)
        {
            try
            {
                var dbSupplierFuel = await context.SupplierFuels.FirstOrDefaultAsync
                (s => s.SupplierFuelIdx == supFuelDTO.SupplierFuelIdx);
                var dbSupplier = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == supFuelDTO.SupplierId);

                dbSupplierFuel.SupplierId = dbSupplier.SupplierId;
                dbSupplierFuel.StoreId = supFuelDTO.StoreId;
                dbSupplierFuel.SupplierFuelIdi = supFuelDTO.SupplierFuelIdi++;
                dbSupplierFuel.BrandName = supFuelDTO.BrandName;
                dbSupplierFuel.Name = dbSupplier.Name;
                dbSupplierFuel.SupplierType = supFuelDTO.SupplierType;
                dbSupplierFuel.Rfc = dbSupplier.Rfc;
                dbSupplierFuel.FuelPermission = supFuelDTO.FuelPermission;
                dbSupplierFuel.StorageAndDistributionPermission = supFuelDTO.StorageAndDistributionPermission;
                dbSupplierFuel.IsConsignment = supFuelDTO.IsConsignment;
                dbSupplierFuel.Updated = DateTime.Now;

                var storeId2 = supFuelDTO.StoreId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = supFuelDTO.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);

                //reset campo de supplier
                var dbSupplierGuid = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == idGuid);
                if (dbSupplierGuid is not null) { dbSupplierGuid.IsSupplierOfFuel = false; }

                //nuevo supplierFuel true
                var dbSupplier2 = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == supFuelDTO.SupplierId);
                dbSupplier2.IsSupplierOfFuel = true;

                context.SupplierFuels.Update(dbSupplierFuel);
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                var dbSupplier2 = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == supFuelDTO.SupplierId);
                return BadRequest($"Ya existe {dbSupplier2.Name} en provedores de combustible ");
            }
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.SupplierFuels.AnyAsync(x => x.SupplierFuelIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierFuelIdx == id);
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
            var existe = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierFuelIdx == id);

            if (existe is null) { return NotFound(); }
            var dbSupplier = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == existe.SupplierId);

            var name2 = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierFuelIdx == id);
            context.Remove(name2);
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);
            dbSupplier.IsSupplierOfFuel = false;
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
