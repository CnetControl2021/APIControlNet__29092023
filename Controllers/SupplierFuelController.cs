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
        [AllowAnonymous]
        public async Task<IEnumerable<SupplierFuelDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, 
            Guid storeId, Guid id2)
        {
            var queryable = context.SupplierFuels.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
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
            var suppliersFuel = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<SupplierFuelDTO>>(suppliersFuel);
        }


        [HttpGet("{id:int}", Name = "newSuppFuel")]
        public async Task<ActionResult<SupplierFuelDTO>> Get(int id)
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
            var existe = await context.SupplierFuels.AnyAsync(x => x.SupplierId == SupplierFuelDTO.SupplierId && SupplierFuelDTO.StoreId == storeId);
            var dbSupplier = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == SupplierFuelDTO.SupplierId);
            
            var suppFuel = mapper.Map<SupplierFuel>(SupplierFuelDTO);
            suppFuel.StoreId = storeId;
            suppFuel.Name = dbSupplier.Name;

            if (existe)
            {
                return BadRequest($"Ya existe {SupplierFuelDTO.SupplierFuelIdi} y {SupplierFuelDTO.Name} ");
            }
            else
            {
                context.Add(suppFuel);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = suppFuel.Name;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
                var storeAddressDTO2 = mapper.Map<SupplierFuelDTO>(suppFuel);
                return CreatedAtRoute("newSuppFuel", new { id = SupplierFuelDTO.SupplierFuelIdx }, storeAddressDTO2);
            }
        }

        //[HttpPost("{storeId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult> Post([FromBody] SupplierFuel_SupplierDTO supplierFuel_SupplierDTO, Guid storeId)
        //{
        //    if (storeId == Guid.Empty)
        //    {
        //        return BadRequest("Sucursal no valida");
        //    }
        //    try
        //    {
        //        using (var transaccion = await context.Database.BeginTransactionAsync())
        //        {
        //            var supplierFuel = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierFuelIdi == supplierFuel_SupplierDTO.NSupplierFuelDTO.SupplierFuelIdi);
        //            //var dataDB = await context.Invoices.FirstOrDefaultAsync(x => x.Uuid == saleSubOrder_Invoice.NInvoiceDTO.Uuid);

        //            if (supplierFuel is null || Guid.Empty)
        //            {
        //                SupplierFuel oSupFuel = new();
        //                oSupFuel.SupplierId = supplierFuel_SupplierDTO.NSupplierFuelDTO.SupplierId;
        //                oSupFuel.StoreId = storeId;
        //                oSupFuel.SupplierFuelIdi = supplierFuel_SupplierDTO.NSupplierFuelDTO.SupplierFuelIdi;
        //                oSupFuel.BrandName = supplierFuel_SupplierDTO.NSupplierFuelDTO.BrandName;
        //                oSupFuel.Name = supplierFuel_SupplierDTO.NSupplierFuelDTO.Name;
        //                oSupFuel.FuelPermission = supplierFuel_SupplierDTO.NSupplierFuelDTO.FuelPermission;
        //                oSupFuel.StorageAndDistributionPermission = supplierFuel_SupplierDTO.NSupplierFuelDTO.StorageAndDistributionPermission;
        //                oSupFuel.Date = DateTime.Now;
        //                oSupFuel.Updated = DateTime.Now;
        //                oSupFuel.Active = true;
        //                oSupFuel.Locked = false;
        //                oSupFuel.Deleted = false;

        //                context.Invoices.Add(oSupFuel);


        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Revisar datos");
        //    }
        //    return Ok();

        //}



        [HttpPut]
        public async Task<IActionResult> Put(SupplierFuelDTO SupplierFuelDTO)
        {
            var supFuelDB = await context.SupplierFuels.FirstOrDefaultAsync(c => c.SupplierFuelIdx == SupplierFuelDTO.SupplierFuelIdx);
            if (supFuelDB is null)
            {
                return NotFound();
            }
            supFuelDB = mapper.Map(SupplierFuelDTO, supFuelDB);

            var storeId2 = supFuelDB.StoreId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = supFuelDB.Name;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
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
            var existe = await context.SupplierFuels.AnyAsync(x => x.SupplierFuelIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierFuelIdx == id);
            context.Remove(name2);
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
